using UnityEngine;

namespace ArcadeGame.Views.Machines
{
    /// <summary>
    ///     Controls the animations for the claw machine game.
    /// </summary>
    public class ClawMachineAnimator : ArcadeMachineAnimator
    {
        public const string Filename = "Claw Machine Animator";

        [SerializeField] private Transform joystickPivot;
        [SerializeField, Range(0f, 90f)] private float joystickRange = 30f;

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
        }
    }
}