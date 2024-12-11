using ArcadeGame.Helpers.Audio;
using Shared.Editor;
using System;
using UnityEngine;

namespace ArcadeGame.Views.Machines
{
    /// <summary>
    ///     Base class used by all arcade machine animators.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Animator))]
    public abstract class ArcadeMachineAnimator : Shared.Behaviour
    {
        #region VARIABLE DECLARATIONS

        [Header("Buttons")]
        [SerializeField] private bool hasButtons;
        [SerializeField, DependsUpon("hasButtons")] private string buttonAnimatorToggle = "Pressed";
        [SerializeField, DependsUpon("hasButtons")] private Animator[] buttonAnimators;
        [SerializeField, DependsUpon("hasButtons")] private LightFlasher buttonsFlasher;
        [SerializeField, DependsUpon("hasButtons")] private string buttonPressAudioKey;
        [SerializeField, DependsUpon("hasButtons")] private string buttonReleaseAudioKey;

        protected Animator controller;

        #endregion

        #region SETUP

        protected virtual void Awake()
        {
            controller = GetComponent<Animator>();
        }

        #endregion

        #region METHODS

        /// <summary>
        ///     Animates the button press of an arcade machine, & plays the audio feedback.
        /// </summary>
        /// <param name="buttonIndex">Index of the button to animate.</param>
        /// <param name="down">If the button is being pressed down or up.</param>
        /// <exception cref="Exception">If the index is out of range, or the button animators are not properly configured.</exception>
        private void AnimateButton(int buttonIndex, bool down)
        {
            if (!hasButtons)
                throw new Exception($"{name} does not use buttons.");

            if (buttonAnimators.Length == 0)
                throw new Exception($"{name} needs to configure it's button animators.");

            if (buttonIndex < 0 || buttonIndex >= buttonAnimators.Length)
                throw new IndexOutOfRangeException($"{buttonIndex}");

            buttonsFlasher.TestFlash();
            buttonAnimators[buttonIndex].SetBool(buttonAnimatorToggle, down);
            SoundManager.PlayAudioClip(down ? buttonPressAudioKey: buttonReleaseAudioKey);
        }

        /// <summary>
        ///     Press the button at the given index down.
        /// </summary>
        /// <param name="buttonIndex"></param>
        public void PressButton(int buttonIndex) => AnimateButton(buttonIndex, true);

        /// <summary>
        ///     Release the button at the given index up.
        /// </summary>
        /// <param name="buttonIndex"></param>
        public void ReleaseButton(int buttonIndex) => AnimateButton(buttonIndex, false);

        /// <summary>
        ///     Sets the boolean value of the animator.
        /// </summary>
        /// <param name="name">Name of the boolean variable.</param>
        /// <param name="value">Value to set the boolean.</param>
        protected void SetBool(string name, bool value) => controller.SetBool(name, value);

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