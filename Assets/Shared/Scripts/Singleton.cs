namespace Shared
{
    /// <summary>
    ///     Base class used by all singleton manager scripts.
    /// </summary>
    /// <typeparam name="TClass">Type of class inherited by this singleton.</typeparam>
    public abstract class Singleton<TClass> : Behaviour
    where TClass : Singleton<TClass>
    {
        /// <summary>
        ///     Reference to the instance of the singleton.
        /// </summary>
        public static TClass Instance { get; private set; }

        /// <summary>
        ///     Does the singleton object exist in the 'Dont Destroy On Load' scene?
        /// </summary>
        protected virtual bool DontDestroy => false;

        protected virtual void Awake()
        {
            if(Instance != null && Instance != GetComponent<TClass>())
            {
                Destroy(GetComponent<TClass>()); //ensure that no duplicates of the singleton can exist
                return;
            }

            Instance = GetComponent<TClass>();

            //set this object to not destroy
            if(DontDestroy)
            {
                Instance.transform.parent = null; //set the parent to the root
                DontDestroyOnLoad(Instance.gameObject);
            }
        }
    }
}