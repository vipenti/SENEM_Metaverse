using TMPro;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject target;
    private Vector3 height = new Vector3(0, 0.8f, 0);
    private Vector3 startPosition = new Vector3(0, 1, -9.99f);
    private Vector3 offset = Vector3.zero;

    private bool showNames = true;

    public Vector3 start;
    public Vector3 end;
    public float movementSpeed = 5f;
    private float scrollValue;

    void LateUpdate()
    {
        if (target == null && this.transform.position == startPosition)
        {
            return;
        }

        else if (target == null && this.transform.position != startPosition) {
            transform.position = startPosition;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            return;
        }

        transform.position = target.transform.position + height + offset;
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, target.transform.rotation.eulerAngles.y, transform.eulerAngles.z);

        if(Input.GetKeyUp(KeyCode.LeftShift) && showNames == true)
        {
            foreach (GameObject name in GameObject.FindGameObjectsWithTag("PlayerName"))
                name.GetComponent<TMP_Text>().enabled = false;            
            showNames = false;
        }

        else if (Input.GetKeyUp(KeyCode.LeftShift) && showNames == false)
        {
            foreach (GameObject name in GameObject.FindGameObjectsWithTag("PlayerName"))
                name.GetComponent<TMP_Text>().enabled = true;
            showNames = true;
        }

        scrollValue = Input.GetAxis("Mouse ScrollWheel") * movementSpeed;
        if (scrollValue != 0)
        {
            // Debug.Log("scrool value " + scrollValue);
            Camera camera = GetComponent<Camera>();

            float currentFOV = camera.fieldOfView;
            float targetFOV = Mathf.Clamp(currentFOV - (scrollValue), 10f, 60f);

            float lerpValue = Mathf.InverseLerp(10f, 60f, targetFOV);
            float interpolatedFOV = Mathf.Lerp(10f, 60f, lerpValue);

            camera.fieldOfView = interpolatedFOV;
        }

        /*if(Input.GetKeyUp(KeyCode.F1) && offset.Equals(Vector3.zero))
        {
            offset = new Vector3(0, 0, -2);
        }

        else if (Input.GetKeyUp(KeyCode.F1) && !offset.Equals(Vector3.zero))
        {
            offset = Vector3.zero;
        }*/
    }
}
