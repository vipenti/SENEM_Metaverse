using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon;

public static class SerializableColor
{
    public static byte[] Serialize(object obj)
    {
        Color32 color = (Color32)obj;
        byte[] bytes = new byte[4];
        bytes[0] = color.r;
        bytes[1] = color.g;
        bytes[2] = color.b;
        bytes[3] = color.a;
        return bytes;
    }

    public static object Deserialize(byte[] bytes)
    {
        Color32 color = new Color32();
        color.r = bytes[0];
        color.g = bytes[1];
        color.b = bytes[2];
        color.a = bytes[3];
        return color;
    }
}

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    public TMP_InputField nameInputField;
    public TMP_InputField passwordInputField;
    public GameObject initialGUI;
    public GameObject loggedGUI;

    public Button hostButton;
    public Button clientButton;
    public Button quitButton;
    public Button leaveButton;
    public Button editButton;

    public AudioSource buttonClick;

    public PhotonView player;
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        loggedGUI.SetActive(false);
        //clientButton.enabled = false;
        //hostButton.enabled = false;

        hostButton.onClick.AddListener(() => {
            buttonClick.Play();
            if (nameInputField.text == "")
            {
                Logger.Instance.LogError("You must enter a name!");
                nameInputField.placeholder.GetComponent<TMP_Text>().text = "You must enter a name!";
                return;
            }
            else if(passwordInputField.text == "")
            {
                Logger.Instance.LogError("You must enter a password!");
                passwordInputField.placeholder.GetComponent<TMP_Text>().text = "You must enter a password!";
                return;
            }

            else 
                CreateRoom(passwordInputField.text);
        });

        clientButton.onClick.AddListener(() => {
            buttonClick.Play();

            if (nameInputField.text == "")
            {
                Logger.Instance.LogError("You must enter a name!");
                nameInputField.placeholder.GetComponent<TMP_Text>().text = "You must enter a name!";
                return;
            }
            else if (passwordInputField.text == "")
            {
                Logger.Instance.LogError("You must enter a password!");
                passwordInputField.placeholder.GetComponent<TMP_Text>().text = "You must enter a password!";
                return;
            }
            else 
                JoinRoom(passwordInputField.text);
        });

        quitButton.onClick.AddListener(() => {
            buttonClick.Play();
            Application.Quit();
        });

        leaveButton.onClick.AddListener(() => {
            buttonClick.Play();
            Leave();
        });

        editButton.onClick.AddListener(() => {
            buttonClick.Play();
            SceneManager.LoadScene("CharacterEditor");
        });

    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        photonView.RPC("NotifyNewMaster", RpcTarget.All, newMasterClient.NickName);
    }    

    public override void OnConnectedToMaster()
    {
        Logger.Instance.LogInfo("Connected to Master");
        hostButton.enabled = true;
        clientButton.enabled = true;

    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.NickName = nameInputField.text;
        SetCustomProperties();

        initialGUI.SetActive(false);
        loggedGUI.SetActive(true);
        PhotonNetwork.Instantiate(player.name, Vector3.zero, Quaternion.identity);

    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        string msg = $"{otherPlayer.NickName} left the classroom.";
        //photonView.RPC("NotifyRpc", RpcTarget.All, msg);
        LogManager.Instance.LogInfo($"{otherPlayer.NickName} left the room");

    }

    private void CreateRoom(string name)
    {
        PhotonNetwork.CreateRoom(name, new RoomOptions() { BroadcastPropsChangeToAll = true, EmptyRoomTtl = 0, CleanupCacheOnLeave = true});

        LogManager.Instance.LogInfo($"{nameInputField.text} created room {name}");
        Presenter.Instance.presenterID = PhotonNetwork.LocalPlayer.UserId;
    }

    private void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }


    private void Leave()
    {
        PhotonNetwork.LeaveRoom(becomeInactive: false);

        initialGUI.SetActive(true);
        loggedGUI.SetActive(false);

        nameInputField.placeholder.GetComponent<TMP_Text>().text = "Enter player name...";
        passwordInputField.placeholder.GetComponent<TMP_Text>().text = "Enter password...";
        Cursor.lockState = CursorLockMode.None;
    }

    private void SetCustomProperties()
    {
        PhotonPeer.RegisterType(typeof(Color32), (byte)'C', SerializableColor.Serialize, SerializableColor.Deserialize);
    }

    [PunRPC]
    public void NotifyRpc(string msg)
    {
        Logger.Instance.LogInfo(msg);
    }

    [PunRPC]
    public void NotifyNewMaster(string name)
    {
        Logger.Instance.LogInfo("Old room owner disconnected. New owner is now <color=yellow>" + name + "</color>.");
    }
}
