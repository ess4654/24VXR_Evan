using HorrorHouse.Controllers;
using HorrorHouse.Data;
using UnityEngine;
using UnityEngine.Audio;

namespace HorrorHouse.Helpers
{
    /// <summary>
    ///     Used to mix the background sounds.
    /// </summary>
    public class BackgroundMixer : MonoBehaviour
    {
        [SerializeField] private float transitionTime = 2;
        [SerializeField] private AudioMixerSnapshot outside;
        [SerializeField] private AudioMixerSnapshot inside;
        [SerializeField] private AudioMixerSnapshot bathroom;

        #region METHODS

        private void OnEnable()
        {
            GameEventBroadcaster.OnPlayerLocationChanged += LocationChanged;
        }

        private void OnDisable()
        {
            GameEventBroadcaster.OnPlayerLocationChanged -= LocationChanged;
        }

        private void TransitionToInside()
        {
            if(inside)
                inside.TransitionTo(transitionTime);
        }

        private void TransitionToOutside()
        {
            if(outside)
                outside.TransitionTo(transitionTime);
        }

        private void TransitionToBathroom()
        {
            if (bathroom)
                bathroom.TransitionTo(transitionTime / 2f);
        }

        private void LocationChanged(PlayerLocation location)
        {
            if (location == PlayerLocation.Inside)
                TransitionToInside();
            else if(location == PlayerLocation.Outside)
                TransitionToOutside();
            else if(location == PlayerLocation.Bathroom)
                TransitionToBathroom();
        }

        #endregion
    }
}