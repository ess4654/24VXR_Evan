using HorrorHouse.Controllers;
using HorrorHouse.Data;
using UnityEngine;

namespace HorrorHouse.Helpers
{
    /// <summary>
    ///     Handles the swapping of footsteps from inside to out.
    /// </summary>
    public class Footsteps : MonoBehaviour
    {
        [SerializeField] private AudioSource outsideSteps;
        [SerializeField] private AudioSource insideSteps;

        private PlayerLocation playerLocation;

        private void OnEnable()
        {
            GameEventBroadcaster.OnPlayerLocationChanged += OnChangeLocation;            
            GameEventBroadcaster.OnPlayerMoving += OnMoving;            
        }

        private void OnDisable()
        {
            GameEventBroadcaster.OnPlayerLocationChanged -= OnChangeLocation;            
            GameEventBroadcaster.OnPlayerMoving -= OnMoving;            
        }

        private void OnChangeLocation(PlayerLocation location)
        {
            playerLocation = location == PlayerLocation.Outside ? PlayerLocation.Outside : PlayerLocation.Inside;
        }
        
        private void OnMoving(bool isMoving)
        {
            switch(playerLocation)
            {
                case PlayerLocation.Outside:
                    if (outsideSteps)
                        outsideSteps.mute = !isMoving;
                    if (insideSteps)
                        insideSteps.mute = true;
                    break;

                case PlayerLocation.Inside:
                    if (insideSteps)
                        insideSteps.mute = !isMoving;
                    if (outsideSteps)
                        outsideSteps.mute = true;
                    break;
            }
        }
    }
}