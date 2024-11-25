using Shared.Helpers;
using System.Threading.Tasks;
using UnityEngine;

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

        private float restingClawY;
        private const float threshold = 0.01f;
        private const float autoAxis = 0.75f;

        #endregion

        protected override void Awake()
        {
            base.Awake();
            restingClawY = clawBlock.Find("Claw").localPosition.y;
        }

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
                    0,
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
            var railPosition = rails.localPosition + (axis.y * forwardMovementSpeed * Time.smoothDeltaTime * rails.right);
            railPosition.x = Mathf.Clamp(railPosition.x, railBoundaries.x, railBoundaries.y);
            rails.localPosition = railPosition;
            
            //move claw block
            var blockPosition = clawBlock.localPosition + (axis.x * horizontalMovementSpeed * Time.smoothDeltaTime * -clawBlock.forward);
            blockPosition.z = Mathf.Clamp(blockPosition.z, blockBoundaries.x, blockBoundaries.y);
            clawBlock.localPosition = blockPosition;
        }

        /// <summary>
        ///     Animates the dropping of the claw.
        /// </summary>
        /// <returns>Completed dropping animation</returns>
        public async Task AnimateClawDrop()
        {
            PressButton(0);
            SetBool("Down", true);

            await Timer.WaitForSeconds(1f);
            ReleaseButton(0);
            
            SetBool("Down", false);
            var claw = clawBlock.Find("Claw");
            await Timer.WaitUntil(() => claw.localPosition.y >= restingClawY);
            await Timer.WaitForSeconds(.25f);
        }

        /// <summary>
        ///     Animates the claw moving to the drop zone of the machine.
        /// </summary>
        /// <returns>Completed drop animation</returns>
        public async Task AnimatePrizeDrop()
        {
            await AnimateTowards(new Vector2(.308f , - .423f));
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

            while (this && (Mathf.Abs(clawBlock.localPosition.z - position.x) > threshold) || (Mathf.Abs(rails.localPosition.x - position.y) > threshold))
            {
                xInput = Mathf.Abs(clawBlock.localPosition.z - position.x) <= threshold ? 0 : clawBlock.localPosition.z < position.x ? -autoAxis : autoAxis;
                yInput = Mathf.Abs(rails.localPosition.x - position.y) <= threshold ? 0 : rails.localPosition.x < position.y ? autoAxis : -autoAxis;
                AnimateRails(new Vector2(xInput, yInput));
                await Timer.WaitForFrame();
            }
        }

        #endregion
    }
}