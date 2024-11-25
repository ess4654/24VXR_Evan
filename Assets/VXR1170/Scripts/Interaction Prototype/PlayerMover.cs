using Shared;
using Shared.Editor;
using Shared.Helpers;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace ArcadeGame.Controllers
{
    /// <summary>
    ///     Moves the player across the nav mesh.
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class PlayerMover : Singleton<PlayerMover>
    {
        [SerializeField, ReadOnly] private bool isMoving;
        [SerializeField, ReadOnly] private float movingVelocity;

        private NavMeshAgent ai;

        protected override void Awake()
        {
            ai = GetComponent<NavMeshAgent>();
            ai.enabled = false;
            base.Awake();
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

            isMoving = true;
            ai.enabled = true;
            ai.updateRotation = false;
            var distance = Vector3.Distance(position, transform.position);
            var startingRotation = transform.rotation;
            ai.SetDestination(position);

            //wait for movement to stop
            movingVelocity = float.MaxValue;
            while (this && movingVelocity > 0.1f)
            {
                await Timer.WaitForFrame();
                if (this == null) return;

                movingVelocity = ai.velocity.magnitude;

                //rotate player
                if (rotation.HasValue) 
                {
                    var remainingDistance = Vector3.Distance(position, transform.position);
                    var t = 1f - (remainingDistance / distance);
                    transform.rotation = Quaternion.Lerp(startingRotation, rotation.Value, t);
                }
            }
            if (this == null) return;

            ai.isStopped = true;
            ai.enabled = false;
            isMoving = false;
        }
    }
}