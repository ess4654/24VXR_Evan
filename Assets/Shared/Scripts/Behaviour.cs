using UnityEngine;

namespace Shared
{
    /// <summary>
    ///     Base class used by all game behaviours.
    /// </summary>
    public abstract class Behaviour : MonoBehaviour
    {
        /// <summary>
        ///     Logs a message to the console.
        /// </summary>
        /// <param name="message">Message to log.</param>
        protected void Log(object message) => Debug.Log(message);

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