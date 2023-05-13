using UnityEngine;

public class CameraFacer : MonoBehaviour
{
    private void LateUpdate()
    {
        // Makes the player name always face the active camera
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
    }
}
