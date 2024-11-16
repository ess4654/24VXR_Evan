using UnityEngine;

namespace Shared
{
    /// <summary>
    ///     Base class used by all game behaviours.
    /// </summary>
    public abstract class Behaviour : MonoBehaviour
    {
        /// <summary>
        ///     Destroys the game object this behaviour is attached to.
        /// </summary>
        public void Destroy()
        {
            if(gameObject)
                Destroy(gameObject);
        }
    }
}