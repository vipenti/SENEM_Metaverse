using UnityEngine;

public class ZoomAndRotation : MonoBehaviour
{
    public float rotationSpeed = 100.0f;
    public Camera editorCamera;

    public Vector3 start;
    public Vector3 end;
    public float movementSpeed = 1.0f;

    private float totalDistance;
    private float currentDistance;
    private void Start()
    {
        start = new Vector3(-1.370778f, -1.37f, 6.154f);
        end = new Vector3(-2.18f, -0.52f, 9.2f);
        totalDistance = Vector3.Distance(start, end);
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(Vector3.up * -rotationSpeed * Time.deltaTime);
        }
    }

    void LateUpdate()
    {
        float scrollValue = Input.GetAxis("Mouse ScrollWheel");
        if (scrollValue != 0)
        {
            currentDistance += scrollValue * movementSpeed;
            float lerpValue = Mathf.Clamp01(currentDistance / totalDistance);
            editorCamera.transform.position = Vector3.Lerp(start, end, lerpValue);
        }
    }
}
