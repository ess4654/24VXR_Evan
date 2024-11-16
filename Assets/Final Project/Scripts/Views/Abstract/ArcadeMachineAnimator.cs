using UnityEngine;

namespace ArcadeGame.Views.Machines
{
    /// <summary>
    ///     Base class used by all arcade machine animators.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public abstract class ArcadeMachineAnimator : Shared.Behaviour
    {
        #region VARIABLE DECLARATIONS

        [SerializeField] private bool hasButtons;
        [SerializeField] private string buttonAnimatorToggle = "Pressed";
        [SerializeField] private Animator[] buttonAnimators;

        private Animator controller;

        #endregion

        #region SETUP

        protected virtual void Awake()
        {
            controller = GetComponent<Animator>();
        }

        #endregion

        #region METHODS

        /// <summary>
        ///     Press the button at the given index down.
        /// </summary>
        /// <param name="buttonIndex"></param>
        public void PressButton(int buttonIndex)
        {
            if (!hasButtons)
                throw new System.Exception($"{name} does not use buttons.");

            buttonAnimators[buttonIndex].SetBool(buttonAnimatorToggle, true);
        }

        /// <summary>
        ///     Release the button at the given index up.
        /// </summary>
        /// <param name="buttonIndex"></param>
        public void ReleaseButton(int buttonIndex)
        {
            if (!hasButtons)
                throw new System.Exception($"{name} does not use buttons.");

            buttonAnimators[buttonIndex].SetBool(buttonAnimatorToggle, false);
        }

        #endregion

        #region DEBUGGING
        [SerializeField, Min(0)] int m_buttonIndex;
        
        [ContextMenu("Press Button")]
        private void TestButtonDown() => PressButton(m_buttonIndex);
        
        [ContextMenu("Release Button")]
        private void TestButtonUp() => ReleaseButton(m_buttonIndex);
        #endregion
    }
}