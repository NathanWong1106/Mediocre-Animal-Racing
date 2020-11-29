using UnityEngine;

public class CameraVehicleTracker : MonoBehaviour
{
    private Camera mainCamera;
    private Vector3 CameraPositionOffset = new Vector3(0, 4, -3f);
    private float RotationX = 18f;
    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        mainCamera = Camera.main;
        mainCamera.transform.position = transform.TransformPoint(CameraPositionOffset);
        mainCamera.transform.rotation = Quaternion.Euler(RotationX, transform.rotation.eulerAngles.y, mainCamera.transform.rotation.eulerAngles.z);
    }

    //Normally we use LateUpdate() to handle camera movement
    //Since we're following a rigidbody updating with FixedUpdate() we have to use this to avoid jitter
    private void FixedUpdate()
    {
        Vector3 desiredPosition = transform.TransformPoint(CameraPositionOffset);
        Vector3 smoothedPosition = Vector3.SmoothDamp(mainCamera.transform.position, desiredPosition, ref velocity, 0.023f);

        mainCamera.transform.position = smoothedPosition;
        mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, Quaternion.Euler(RotationX, transform.rotation.eulerAngles.y, mainCamera.transform.rotation.eulerAngles.z), Time.deltaTime * 20f);
    }
}
