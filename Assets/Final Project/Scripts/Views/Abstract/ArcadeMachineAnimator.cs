using Shared.Editor;
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
        [SerializeField, DependsUpon("hasButtons")] private string buttonAnimatorToggle = "Pressed";
        [SerializeField, DependsUpon("hasButtons")] private Animator[] buttonAnimators;
        [SerializeField, DependsUpon("hasButtons")] private LightFlasher buttonsFlasher;

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
        
        [Debugging]
        [SerializeField, DependsUpon("hasButtons"), Min(0)] int m_buttonIndex;
        [SerializeField, InspectorButton("TestButtonDown")] bool m_PressButton;
        [SerializeField, InspectorButton("TestButtonUp")] bool m_ReleaseButton;
        
        [ContextMenu("Press Button")]
        public void TestButtonDown() => PressButton(m_buttonIndex);
        
        [ContextMenu("Release Button")]
        public void TestButtonUp() => ReleaseButton(m_buttonIndex);
        
        #endregion
    }
}