using UnityEngine;

namespace HorrorHouse
{
    public class FaceCamera : MonoBehaviour
    {
        private Camera xrCamera;

        private void Awake()
        {
            xrCamera = Camera.main;
        }
    
        private void LateUpdate()
        {
            if (xrCamera != null)
            {
                transform.LookAt(xrCamera.transform.position, Vector3.forward);
            }
        }
    }
}