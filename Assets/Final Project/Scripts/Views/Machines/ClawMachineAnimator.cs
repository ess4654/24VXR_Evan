using ArcadeGame.Controllers;
using ArcadeGame.Data;
using Shared.Helpers;
using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace ArcadeGame.Views.Machines
{
    /// <summary>
    ///     Controls the animations for the claw machine game.
    /// </summary>
    public class ClawMachineAnimator : ArcadeMachineAnimator
    {
        public const string Filename = "Claw Machine Animator";

        #region VARIABLE DECLARATIONS

        [Header("Joystick Settings")]
        [SerializeField] private Transform joystickPivot;
        [SerializeField, Range(0f, 90f)] private float joystickRange = 30f;
        [SerializeField] private float joystickDeadZone = .1f;

        [Header("Rails Settings")]
        [SerializeField] private Transform rails;
        [SerializeField] private float forwardMovementSpeed = 1f;
        [SerializeField] private Vector2 railBoundaries;

        [Header("Block Settings")]
        [SerializeField] private Transform clawBlock;
        [SerializeField] private float horizontalMovementSpeed = 1f;
        [SerializeField] private Vector2 blockBoundaries;
        [SerializeField] private Vector2 dropZone;

        [Header("Claw Settings")]
        [SerializeField] private CharacterJoint claw;
        [SerializeField] private Vector2 clawExtensionRange;
        [SerializeField] private float clawExtensionTime = 1.5f;
        [SerializeField] private float bladeClosingTime = 1f;

        [Header("UI")]
        [SerializeField] private Canvas uiCanvas;
        [SerializeField] private Image countdownClock;
        [SerializeField] private TextMeshProUGUI countdownDisplay;

        private float restingClawY;
        private const float threshold = 0.01f;
        private const float autoAxis = 0.75f;

        #endregion

        #region SETUP

        protected override void Awake()
        {
            base.Awake();
            restingClawY = clawBlock.Find("Claw").localPosition.y;
            if (uiCanvas)
                uiCanvas.enabled = false;
        }

        private void OnEnable()
        {
            GameEventBroadcaster.OnGameStateChanged += GameStateChanged;
        }

        private void OnDisable()
        {
            GameEventBroadcaster.OnGameStateChanged -= GameStateChanged;
        }

        private void GameStateChanged(GameState state)
        {
            if (uiCanvas)
                uiCanvas.enabled = state == GameState.ClawMachine;
        }

        #endregion

        #region METHODS

        /// <summary>
        ///     Animates the movement of the joystick on the claw machine.
        /// </summary>
        /// <param name="axis">Axis of the input.</param>
        public void AnimateJoystick(in Vector2 axis)
        {
            if (joystickPivot != null)
            {
                joystickPivot.localEulerAngles = new Vector3
                (
                    -axis.x * joystickRange,
                    90,
                    -axis.y * joystickRange
                );
            }

            var clampedAxis = axis;
            clampedAxis.x = Mathf.Abs(axis.x) < joystickDeadZone ? 0 : axis.x;
            clampedAxis.y = Mathf.Abs(axis.y) < joystickDeadZone ? 0 : axis.y;
            AnimateRails(in clampedAxis);
        }

        /// <summary>
        ///     Animates the movement of the claw machine rails and claw block.
        /// </summary>
        /// <param name="axis">Axis of the input.</param>
        private void AnimateRails(in Vector2 axis)
        {
            //move railing
            var railPosition = rails.localPosition + (axis.y * forwardMovementSpeed * Time.smoothDeltaTime * -rails.right);
            railPosition.z = Mathf.Clamp(railPosition.z, railBoundaries.x, railBoundaries.y);
            rails.localPosition = railPosition;
            
            //move claw block
            var blockPosition = clawBlock.localPosition + (axis.x * horizontalMovementSpeed * Time.smoothDeltaTime * clawBlock.forward);
            blockPosition.x = Mathf.Clamp(blockPosition.x, blockBoundaries.x, blockBoundaries.y);
            clawBlock.localPosition = blockPosition;
        }

        /// <summary>
        ///     Animates the dropping of the claw.
        /// </summary>
        /// <returns>Completed dropping animation</returns>
        public async Task AnimateClawDrop()
        {
            ExtendAndRetractClaw(); //start the claw extension and retraction method asynchronously
            
            //animate the button press down
            PressButton(0);
            SetBool("Down", true);

            await Timer.WaitForSeconds(1f); //wait 1 second to animate the button back up
            if (this == null) return;

            //animate the button press up down
            ReleaseButton(0);
            
            SetBool("Down", false);
            
            await Timer.WaitUntil(() => this == null || clawExtensionCompete);
        }

        bool clawExtensionCompete;

        /// <summary>
        ///     Controls the animation of the claw extension downwards, then closing the blades, and retracting back up.
        /// </summary>
        /// <returns>Completed claw extension method.</returns>
        private async Task ExtendAndRetractClaw()
        {
            clawExtensionCompete = false;
            var startY = clawExtensionRange.x;
            var endY = clawExtensionRange.y;

            //animate claw dropping down
            LeanTween
                .value(startY, endY, clawExtensionTime)
                .setOnUpdate(UpdateClawAnchor)
                .setEaseLinear();
            
            await Timer.WaitForSeconds(clawExtensionTime);
            if (this == null) return;

            //animate the claw blades closing
            await AnimateClawBlades(false);
            if (this == null) return;

            //animate claw pulling up
            LeanTween
                .value(endY, startY, clawExtensionTime)
                .setOnUpdate(UpdateClawAnchor)
                .setEaseLinear();

            await Timer.WaitForSeconds(clawExtensionTime);
            if (this == null) return;

            clawExtensionCompete = true;
        }

        /// <summary>
        ///     Updates the value of the claw anchor.
        /// </summary>
        /// <param name="yValue">Y Value of the claw anchor point.</param>
        private void UpdateClawAnchor(float yValue)
        {
            if (claw != null)
                claw.anchor = yValue * Vector3.up;
        }

        /// <summary>
        ///     Animates the opening and closing of the claw blades.
        /// </summary>
        /// <param name="open">Whether the blades are to be opened or not.</param>
        /// <returns>Completed animation routine</returns>
        private async Task AnimateClawBlades(bool open)
        {
            //animate the claw blades opening or closing
            var start = open ? 1 : 0;
            var end = open ? 0: 1;
            LeanTween
                .value(start, end, bladeClosingTime)
                .setOnUpdate((float v) => controller.SetFloat("Claw Blade Rotation", v))
                .setEaseLinear();

            await Timer.WaitForSeconds(bladeClosingTime);
        }

        /// <summary>
        ///     Animates the claw moving to the drop zone of the machine.
        /// </summary>
        /// <returns>Completed drop animation</returns>
        public async Task AnimatePrizeDrop()
        {
            await AnimateTowards(dropZone);
            await Timer.WaitForSeconds(.5f);

            //animate the claw blades opening
            await AnimateClawBlades(true);
            
            await Timer.WaitForSeconds(1);
        }

        /// <summary>
        ///     Animates the claw returning to the center point of the machine.
        /// </summary>
        /// <returns>Completed return animation</returns>
        public Task AnimateClawReturn() => AnimateTowards(Vector2.zero);

        /// <summary>
        ///     Animates the movement mechanism towards the desired position.
        /// </summary>
        /// <remarks>
        ///     x = block position
        ///     y = rails position
        /// </remarks>
        /// <param name="position">Position of the movement mechanism to move towards.</param>
        /// <returns>Completed movement animation</returns>
        private async Task AnimateTowards(Vector2 position)
        {
            float xInput;
            float yInput;

            while (this && (Mathf.Abs(clawBlock.localPosition.x - position.x) > threshold) || (Mathf.Abs(rails.localPosition.z - position.y) > threshold))
            {
                //Log($"Moving Towards: {position}");
                //Log($"Distance Remaining: {new Vector2(Mathf.Abs(clawBlock.localPosition.x - position.x), Mathf.Abs(rails.localPosition.z - position.y)).magnitude}");

                xInput = Mathf.Abs(clawBlock.localPosition.x - position.x) <= threshold ? 0 : clawBlock.localPosition.x < position.x ? -autoAxis : autoAxis;
                yInput = Mathf.Abs(rails.localPosition.z - position.y) <= threshold ? 0 : rails.localPosition.z < position.y ? -autoAxis : autoAxis;
                AnimateRails(new Vector2(xInput, yInput));
                await Timer.WaitForFrame();
            }

            //Log($"Finished Moving Claw");
        }

        /// <summary>
        ///     Updates the countdown clock for the UI Canvas with the amount of remaining time left.
        /// </summary>
        /// <param name="remainingTime">Time remaining before the claw automatically drops.</param>
        /// <param name="startingTime">The time on the clock when the countdown first starts.</param>
        public void UpdateCountDown(int remainingTime, int startingTime)
        {
            if(countdownDisplay)
                countdownDisplay.text = remainingTime.ToString();
            if(countdownClock)
                countdownClock.fillAmount = ((float)remainingTime / startingTime);
        }

        #endregion
    }
}