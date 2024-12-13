using Shared;
using Shared.Editor;
using Shared.Helpers;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR.Interaction.Toolkit;

namespace ArcadeGame.Controllers
{
    /// <summary>
    ///     Moves the player across the nav mesh.
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class PlayerMover : Singleton<PlayerMover>
    {
        #region VARIABLE DECLARATIONS

        [SerializeField, ReadOnly] private bool isMoving;
        [SerializeField, ReadOnly] private float movingVelocity;

        private NavMeshAgent ai;
        private ContinuousMoveProviderBase moveProvider;
        private ContinuousTurnProviderBase turnProvider;

        #endregion

        #region SETUP

        protected override void Awake()
        {
            ai = GetComponent<NavMeshAgent>();
            moveProvider = GetComponent<ContinuousMoveProviderBase>();
            turnProvider = GetComponent<ContinuousTurnProviderBase>();

            ActivatePlayerInput(true);
            base.Awake();
        }

        #endregion

        /// <summary>
        ///     Activates/De-Activates the input of the player's devices.
        /// </summary>
        /// <param name="active">Is the movement to be player controlled (true) or AI controlled (false)?</param>
        public void ActivatePlayerInput(bool active)
        {
            ai.enabled = !active;
            moveProvider.enabled = active;
            //turnProvider.enabled = active;
        }

        /// <summary>
        ///     Moves the AI nav agent to the destination in global space.
        /// </summary>
        /// <param name="position">Position to move the agent.</param>
        /// <param name="callback">(Optional) The rotation to lerp towards.</param>
        /// <returns>Completed movement task</returns>
        public async Task MoveToPosition(Vector3 position, Quaternion? rotation = null)
        {
            if (isMoving) return;

            ActivatePlayerInput(false);
            isMoving = true;
            ai.updateRotation = false;
            var startingRotation = transform.rotation;
            ai.SetDestination(position);
            var distance = Vector3.Distance(ai.destination, transform.position);

            //wait for movement to stop
            movingVelocity = float.MaxValue;
            while (this && Vector3.Distance(ai.destination, transform.position) > 0.1f)
            {
                await Timer.WaitForFrame();
                if (this == null) return;

                movingVelocity = ai.velocity.magnitude;

                //rotate player
                //if (rotation.HasValue) 
                //{
                //    var remainingDistance = Vector3.Distance(ai.destination, transform.position);
                //    var t = 1f - (remainingDistance / distance);
                //    transform.rotation = Quaternion.Lerp(startingRotation, rotation.Value, t);
                //}
            }
            if (this == null) return;

            if(rotation.HasValue)
                transform.rotation = rotation.Value;
            ai.isStopped = true;
            isMoving = false;
            ActivatePlayerInput(true);
        }
    }
}