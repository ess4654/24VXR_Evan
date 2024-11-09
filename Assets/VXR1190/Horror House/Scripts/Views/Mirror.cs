using HorrorHouse.Data;
using UnityEngine;

namespace HorrorHouse.Views
{
    /// <summary>
    ///     Activates/Deactivates the mirror cameras for better performance.
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    public class Mirror : MonoBehaviour
    {
        private Camera mirrorCam;

        private void Awake()
        {
            mirrorCam = GetComponentInChildren<Camera>(true);
        }

        private void Start()
        {
            if(mirrorCam)
                mirrorCam.gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(mirrorCam != null && other.CompareTag(Constants.TAG_PLAYER))
                mirrorCam.gameObject.SetActive(true);
        }

        private void OnTriggerExit(Collider other)
        {
            if (mirrorCam != null && other.CompareTag(Constants.TAG_PLAYER))
                mirrorCam.gameObject.SetActive(false);
        }
    }
}