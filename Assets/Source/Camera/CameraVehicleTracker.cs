using UnityEngine;

namespace Racing.Camera
{
    /// <summary>
    /// Translates the camera along with the player's vehicle and applies damping
    /// </summary>
    /// <remarks>Place this script on player vehicle bodies</remarks>
    public class CameraVehicleTracker : MonoBehaviour
    {
        private UnityEngine.Camera mainCamera;
        private Vector3 CameraPositionOffset = new Vector3(0, 4, -3f);
        private float RotationX = 18f;
        private Vector3 velocity = Vector3.zero;

        private float damping = 20f;
        private float smoothDampTime = 0.023f;

        private void Start()
        {
            mainCamera = UnityEngine.Camera.main;
            mainCamera.transform.position = transform.TransformPoint(CameraPositionOffset);
            mainCamera.transform.rotation = Quaternion.Euler(RotationX, transform.rotation.eulerAngles.y, mainCamera.transform.rotation.eulerAngles.z);
        }

        //Normally we use LateUpdate() to handle camera movement
        //Since we're following a rigidbody updating with FixedUpdate() we have to use this to avoid jitter
        private void FixedUpdate()
        {
            Vector3 targetPos = transform.TransformPoint(CameraPositionOffset); 
            Vector3 smoothedPos = Vector3.SmoothDamp(mainCamera.transform.position, targetPos, ref velocity, smoothDampTime);

            mainCamera.transform.position = smoothedPos;
            mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, 
                Quaternion.Euler(RotationX, transform.rotation.eulerAngles.y, mainCamera.transform.rotation.eulerAngles.z), 
                Time.deltaTime * damping);
        }
    }
}
