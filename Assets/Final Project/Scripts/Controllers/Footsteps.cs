using UnityEngine;

namespace ArcadeGame.Controllers
{
    [RequireComponent(typeof(AudioSource))]
    public class Footsteps : MonoBehaviour
    {
        private AudioSource footsteps;

        private void Awake()
        {
            footsteps = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            XRInputManager.OnControllerLeftAxis += HandleMovementAxis;
        }

        private void OnDisable()
        {
            XRInputManager.OnControllerLeftAxis -= HandleMovementAxis;
        }

        private void HandleMovementAxis(Vector2 axis)
        {
            footsteps.mute = axis == Vector2.zero;
        }
    }
}