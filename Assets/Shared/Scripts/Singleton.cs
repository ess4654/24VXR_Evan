using UnityEngine;

namespace Shared
{
    /// <summary>
    ///     Base class used by all singleton manager scripts.
    /// </summary>
    /// <typeparam name="TClass">Type of class inherited by this singleton.</typeparam>
    public abstract class Singleton<TClass> : MonoBehaviour
    where TClass : Singleton<TClass>
    {
        /// <summary>
        ///     Reference to the instance of the singleton.
        /// </summary>
        public static TClass Instance { get; private set; }

        protected virtual void Awake()
        {
            if(Instance != null && Instance != GetComponent<TClass>())
            {
                Destroy(GetComponent<TClass>()); //ensure that no duplicates of the singleton can exist
                return;
            }

            Instance = GetComponent<TClass>();
        }
    }
}