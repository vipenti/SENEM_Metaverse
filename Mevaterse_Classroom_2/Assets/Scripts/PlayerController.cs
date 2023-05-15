using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using TMPro;
public class PlayerController : MonoBehaviourPunCallbacks
{
    // Controls the camera movement
    [Header("Camera")]
    public Transform playerRoot;
    public Transform playerCam;
    public float cameraSensitivity;
    private float rotX;
    private float rotY;

    [Header("Movement")]
    public CharacterController controller;
    public float speed;
    public float gravity;
    public Transform feet;
    public bool isGrounded;
    Vector3 velocity;

    [Header("Input")]
    public InputAction move;
    public InputAction mouseX;
    public InputAction mouseY;

    // Controls forward and backward movement speed
    private float originalSpeed;
    private float backwardSpeed;

    // Variables for animation control
    private bool isMoving;
    private bool isBackwardMoving;
    private bool handRaised;
    private bool isWaving;
    public bool isTyping;
    private TextChat textChat;
    public TMP_Text volumeIcon;
    public TMP_Text playerName;
    private float idleTime;
    private float handRaiseCooldown;

    // Variables for the sitting control
    private GameObject chair;
    private bool isSitting;
    private Vector3 originalPosition;
    private float originalFov;
    private int originalCullingMask;

    public Animator animatorController;
    private TMP_Text interactionInfo;
    private Vector3 spawnPosition;

    public override void OnEnable()
    {
        move.Enable();
        mouseX.Enable();
        mouseY.Enable();
        textChat = GameObject.Find("TextChat").GetComponent<TextChat>();
    }

    public override void OnDisable()
    {
        move.Disable();
        mouseX.Disable();
        mouseY.Disable();
    }

    private void Start()
    {
        playerName.text = GetComponent<PhotonView>().Controller.NickName;
        volumeIcon.text = "";

        // Make the local player camera the only active one for each plater
        if (!photonView.IsMine) {

            playerCam.gameObject.SetActive(false);
            return;
        }

        photonView.RPC("NotifySpawnRPC", RpcTarget.All);
        GameObject.Find("WelcomeAudioSource").GetComponent<AudioSource>().Play();
        GameObject.Find("BgAudioSource").GetComponent<AudioSource>().enabled = false;

        Cursor.lockState = CursorLockMode.Locked;

        // Variables inizialitazion
        controller = GetComponent<CharacterController>();
        idleTime = 0;
        handRaiseCooldown = 10;
        originalSpeed = speed;
        originalFov = playerCam.GetComponent<Camera>().fieldOfView;
        originalCullingMask = playerCam.GetComponent<Camera>().cullingMask;
        backwardSpeed = originalSpeed - 1.3f;
        handRaised = false;
        isSitting = false;

        interactionInfo = GameObject.Find("InteractionInfo").GetComponent<TMP_Text>();
        interactionInfo.text = "";
        spawnPosition = GameObject.Find("SpawnPosition").GetComponent<Transform>().position;

        gameObject.SetActive(false);
        transform.position = spawnPosition + new Vector3((float)(-photonView.ViewID)/10000f, 0, 0);        
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (!photonView.IsMine) { return; }

        controller.Move(velocity * Time.deltaTime);

        // Camera Movement
        Vector2 mouseInput = new Vector2(mouseX.ReadValue<float>() * cameraSensitivity, mouseY.ReadValue<float>() * cameraSensitivity);
        rotX -= mouseInput.y;
        rotX = Mathf.Clamp(rotX, -60, +50);

        if(!isSitting)
            rotY += mouseInput.x;

        else if(isSitting & !isTyping)
        {
            rotY += mouseInput.x;
            rotY = Mathf.Clamp(rotY, -90, +90);
        }

        else if(isSitting & isTyping)
        {
            rotY += mouseInput.x;
            rotY = Mathf.Clamp(rotY, -10, +10);
        }

        playerRoot.rotation = Quaternion.Euler(0f, rotY, 0f);
        playerCam.localRotation = Quaternion.Euler(rotX, 0f, 0f);

        // Player Movement
        Vector2 moveInput = move.ReadValue<Vector2>();
        Vector3 moveVelocity = playerRoot.forward * moveInput.y + playerRoot.right * moveInput.x;        
        
        controller.Move(moveVelocity * speed * Time.deltaTime);

        isGrounded = Physics.Raycast(feet.position, feet.TransformDirection(Vector3.down), 0.50f);

        if (isGrounded)
        {
            velocity = new Vector3(0f, -3f, 0f);
        }
        else
        {
            velocity -= gravity * Time.deltaTime * Vector3.up;
        }

        // Player sitting 
        if (Input.GetKeyUp(KeyCode.C) && chair != null && !chair.GetComponent<ChairController>().IsBusy() && !isSitting && !isMoving && !isBackwardMoving && !textChat.isSelected)
        {
            Seat();
        }

        else if (Input.GetKeyUp(KeyCode.C) && isSitting && !Input.GetKey(KeyCode.W) && 
            !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D) && !textChat.isSelected && !isTyping)
        {
            GetUp();
        }

        if(handRaiseCooldown > 0)
            handRaiseCooldown -= Time.deltaTime;

        // If the player presses M, the character raises their hand
        if (Input.GetKeyUp(KeyCode.M) && handRaiseCooldown <= 0 && !textChat.isSelected && !isTyping)
        {
            RaiseHand();
            handRaiseCooldown = 10;
        }

        if(textChat.isSelected || isTyping)
        {
            controller.enabled = false;
        }
        else if(!textChat.isSelected && !isSitting && !isTyping && controller.enabled == false)
        {
            controller.enabled = true;
        }

        AnimatorChecker(moveVelocity);
        InteractionInfoUpdate();

    }

    private void LateUpdate()
    {
        // Locks and unlocks the mouse if the player press ESC or the right mouse button
        if (Input.GetKey(KeyCode.Escape))
            Cursor.lockState = CursorLockMode.None;

        if ((Cursor.lockState == CursorLockMode.None) && Input.GetMouseButton(1))
            Cursor.lockState = CursorLockMode.Locked;

        GetComponent<AudioSource>().enabled = (isMoving || isBackwardMoving) && !isSitting;
    }

    void AnimatorChecker(Vector3 moveVelocity)
    {
        isMoving = ((moveVelocity.x != 0 || moveVelocity.y != 0 || moveVelocity.z != 0) && !textChat.isSelected && !isTyping);
        isBackwardMoving = false;
        handRaised = false;
        isWaving = false;
        isTyping = (GetComponent<TabletSpawner>().tablet.GetComponent<TabletManager>().isBeingEdited) || (PhotonNetwork.LocalPlayer.UserId == Presenter.Instance.writerID);

        // If the player is walking backward, this changes the animation and slows down the speed
        if (Input.GetKey(KeyCode.S) && !textChat.isSelected && !isTyping)
        {
            isBackwardMoving = true;
            isMoving = false;
            animatorController.SetBool("IsMovingBackward", true);
            speed = backwardSpeed;

        }

        // When the backward walking is done, it brings the original values back
        else
        {
            isBackwardMoving = false;
            speed = originalSpeed;
            animatorController.SetBool("IsMovingBackward", false);
        }

        if (Input.GetKey(KeyCode.M) && handRaiseCooldown <= 0 && !textChat.isSelected && !isTyping)
        {
            handRaised = true;
        }

        if (Input.GetKey(KeyCode.N) && !textChat.isSelected && !isTyping)
        {
            isWaving = true;
        }

        if(isTyping)
        {
            textChat.inputField.enabled = false;
        }

        else if(!isTyping)
        {
            textChat.inputField.enabled = true;
        }

        // If the player doesn't move for 6 seconds, perform an idle animation
        idleTime += Time.deltaTime;
        if (isMoving || isBackwardMoving)
        {
            idleTime = 0;
            animatorController.SetBool("LongPause", false);
        }

        animatorController.SetBool("LongPause", idleTime >= 30);

        if (idleTime >= 30)
        {
            idleTime = 0;
        }

        animatorController.SetBool("IsMoving", isMoving);        
        animatorController.SetBool("HandRaised", handRaised);
        animatorController.SetBool("IsWaving", isWaving);
        animatorController.SetBool("IsTalking", GetComponent<PlayerVoiceController>().isTalking);
        animatorController.SetBool("IsWriting", isTyping);

        if (photonView.GetComponent<PlayerVoiceController>().isTalking)
            photonView.RPC("NotifyTalkRPC", RpcTarget.All, "<sprite index=0>");
        else photonView.RPC("NotifyTalkRPC", RpcTarget.All, "");
    }

    private void Seat()
    {
        GameObject.Find("SitAudioSource").GetComponent<AudioSource>().Play();

        // The chair is set to busy and the player who is occupying it is saved
        GetComponent<PhotonView>().RPC("NotifySitting", RpcTarget.All, true, GetComponent<PhotonView>().Controller.NickName);

        // Saves original player position for when they get up
        originalPosition = transform.position;

        // Makes the player position move on the chair
        gameObject.SetActive(false);
        transform.position = chair.transform.position + new Vector3(0, +0.65f, +0.1f);
        playerCam.GetComponent<Camera>().fieldOfView -= 10f;
        GetComponent<CharacterController>().enabled = false;
        gameObject.SetActive(true);

        // Starts the sitting animation for the player
        isSitting = true;
        animatorController.SetBool("IsSitting", true);

        //Tablet spawn
        GetComponent<TabletSpawner>().SetTabletActive(true, transform.position + new Vector3(-0.05f, 0, 0.5f));

    }

    private void GetUp()
    {
        GameObject.Find("SitAudioSource").GetComponent<AudioSource>().Play();

        // The chair is set to free and the player who was occupying it is deleted
        GetComponent<PhotonView>().RPC("NotifySitting", RpcTarget.All, false, "");

        // Makes the player position the original one
        gameObject.SetActive(false);
        transform.position = originalPosition;
        playerCam.GetComponent<Camera>().fieldOfView = originalFov;
        GetComponent<CharacterController>().enabled = true;
        gameObject.SetActive(true);

        // Stops the sitting animation for the player
        isSitting = false;
        animatorController.SetBool("IsSitting", false);
        chair = null;

        //Tablet despawn
        GetComponent<TabletSpawner>().SetTabletActive(false, transform.position);

    }

    private void RaiseHand()
    {
        GetComponent<PhotonView>().RPC("NotifyHandRaisedRPC", RpcTarget.All);
    }

    // Checks if the player is near to a chair to sit on
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Chair"))
        {
            chair = collision.gameObject;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Chair"))
        {
            chair = null;
        }
    }

    private void InteractionInfoUpdate()
    {
        if (chair != null && !isSitting && !chair.GetComponent<ChairController>().IsBusy())
            interactionInfo.text = "Press C to sit";

        else if (isSitting && !isTyping)
            interactionInfo.text = "Press SPACE to edit your note \n Press C to stand up";

        else if(isSitting && isTyping)
            interactionInfo.text = "Press ESC to stop writing";

        else if (chair != null && !isSitting && chair.GetComponent<ChairController>().IsBusy())
            interactionInfo.text = "Chair is occupied";

        else if(chair == null)
            interactionInfo.text = "";
    }

    [PunRPC]
    public void NotifySitting(bool value, string playerName)
    {
        if (chair != null)
        {
            chair.GetComponent<ChairController>().SetBusy(value);
            chair.GetComponent<ChairController>().playerName = playerName;
        }
            
    }

    [PunRPC]
    public void NotifyHandRaisedRPC()
    {
        string msg = GetComponent<PhotonView>().Controller.NickName + " raised a hand!";
        Logger.Instance.LogInfo(msg);
        LogManager.Instance.LogInfo(msg);

    }

    [PunRPC]
    public void NotifySpawnRPC()
    {
        Logger.Instance.LogInfo($"<color=yellow>{GetComponent<PhotonView>().Controller.NickName}</color> just joined the class!");
        LogManager.Instance.LogInfo($"{GetComponent<PhotonView>().Controller.NickName} joined the room");
        GameObject.Find("SpawnAudioSource").GetComponent<AudioSource>().Play();
    }

    [PunRPC]
    public void NotifyTalkRPC(string msg)
    {
        volumeIcon.text = msg;
    }




}
