using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject target;
    private Vector3 offset = Vector3.zero;
    private Vector3 height = new Vector3(0, 0.8f, 0);

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.transform.position + height;
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, target.transform.rotation.eulerAngles.y, transform.eulerAngles.z);
        }
    }
}
