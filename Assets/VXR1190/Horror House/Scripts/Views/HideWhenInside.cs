using HorrorHouse.Controllers;
using HorrorHouse.Data;
using UnityEngine;

namespace HorrorHouse.Views
{
    /// <summary>
    ///     Hides objects when inside.
    ///     Shows them when outside.
    /// </summary>
    public class HideWhenInside : MonoBehaviour
    {
        [SerializeField] private GameObject[] objectsToHide;

        #region METHODS

        private void OnEnable()
        {
            GameEventBroadcaster.OnPlayerLocationChanged += LocationChanged;
        }

        private void OnDisable()
        {
            GameEventBroadcaster.OnPlayerLocationChanged -= LocationChanged;
        }

        /// <summary>
        ///     Called when the player changes their location.
        /// </summary>
        /// <param name="location">New location of the player.</param>
        private void LocationChanged(PlayerLocation location)
        {
            foreach(var obj in objectsToHide)
                if(obj)
                    obj.SetActive(location == PlayerLocation.Outside);
        }

        #endregion
    }
}