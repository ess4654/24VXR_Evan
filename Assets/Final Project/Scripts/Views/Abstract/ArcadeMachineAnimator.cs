using UnityEngine;

namespace ArcadeGame.Views.Machines
{
    /// <summary>
    ///     Base class used by all arcade machine animators.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public abstract class ArcadeMachineAnimator : MonoBehaviour
    {
        #region VARIABLE DECLARATIONS
        
        private Animator controller;

        #endregion

        protected virtual void Awake()
        {
            controller = GetComponent<Animator>();
        }
    }
}