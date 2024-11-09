using UnityEngine;

namespace HorrorHouse.Views
{
    /// <summary>
    ///     Rotates the forward axis of a transform to face the camera.
    /// </summary>
    public class FaceCamera : MonoBehaviour
    {
        [SerializeField] private bool yOnly;

        private Camera xrCamera;

        private void Awake()
        {
            xrCamera = Camera.main;
        }
    
        private void LateUpdate()
        {
            if (xrCamera != null)
            {
                if(yOnly)
                {
                    Vector3 v = xrCamera.transform.position - transform.position;
                    v.x = v.z = 0.0f;
                    transform.LookAt(xrCamera.transform.position - v);
                }
                else
                    transform.LookAt(xrCamera.transform.position, Vector3.forward);
            }
        }
    }
}