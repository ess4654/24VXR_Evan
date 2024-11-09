using HorrorHouse.Data;
using UnityEngine;

namespace HorrorHouse.Controllers
{
    /// <summary>
    ///     A trigger to the outside/inside of the house.
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    public class PlayerLocationBroadcaster : MonoBehaviour
    {
        [SerializeField] private PlayerLocation location;

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag(Constants.TAG_PLAYER))
                GameEventBroadcaster.BroadcastPlayerLocation(location);
        }
    }
}