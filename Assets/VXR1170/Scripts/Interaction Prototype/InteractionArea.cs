using ArcadeGame.Controllers;
using ArcadeGame.Controllers.Machines;
using Shared.Editor;
using System;
using UnityEngine;

namespace Assets.Final_Project.Scripts.Controllers
{
    /// <summary>
    ///     Handles interaction areas the player can enter/exit.
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    public class InteractionArea : Shared.Behaviour
    {
        #region VARIABLE DECLARATIONS

        /// <summary>
        ///     Types of interactions this area can handle.
        /// </summary>
        private enum InteractionEvents
        {
            None,
            TokenCup,
            ArcadeMachine
        }

        /// <summary>
        ///     Called when this interaction region is entered.
        /// </summary>
        public Action OnEnter;

        /// <summary>
        ///     Called when this interaction region is exited.
        /// </summary>
        public Action OnExit;

        [SerializeField, ReadOnly] private bool insideRegion;
        [SerializeField] private string playerTag;
        [SerializeField] private InteractionEvents interactionEvent;
        [SerializeField, DependsUpon("interactionEvent", InteractionEvents.ArcadeMachine)] private TokenMachineBase machine;
        [SerializeField, DependsUpon("interactionEvent", InteractionEvents.ArcadeMachine, "machine", true)] private int playerPosition;
        [SerializeField, DependsUpon("interactionEvent", InteractionEvents.TokenCup)] private TokenCup cup;

        private BoxCollider boxTrigger;

        #endregion

        #region EDITOR

        //ensure the box collider is always a trigger
        private void OnValidate()
        {
            GetComponent<BoxCollider>().isTrigger = true;
        }

        #endregion

        #region SETUP

        private void Awake()
        {
            boxTrigger = GetComponent<BoxCollider>();
        }

        private void OnEnable()
        {
            GameEventBroadcaster.OnPlayerEnteredRegion += HandlePlayerEnterRegion;
        }

        private void OnDisable()
        {
            GameEventBroadcaster.OnPlayerEnteredRegion -= HandlePlayerEnterRegion;
        }

        /// <summary>
        ///     Handles the event broadcast when player enters regions.
        /// </summary>
        /// <param name="area">The area that was entered.</param>
        /// <remarks>
        ///     Prevents the player from being in multiple regions at once.
        /// </remarks>
        private void HandlePlayerEnterRegion(InteractionArea area)
        {
            if (area != this && insideRegion) //we have entered a new region we are no longer in this one
                ExitRegion();
        }

        private void HandleInput()
        {
            if(insideRegion)
            {
                switch(interactionEvent)
                {
                    case InteractionEvents.TokenCup:
                        cup.Collect();
                        break;

                    case InteractionEvents.ArcadeMachine:
                        if(machine != null)
                        {
                            if(machine is Cyclone cyclone)
                                cyclone.Interact(playerPosition);
                            else if(machine is ShooterMachine shooter)
                                shooter.Interact(playerPosition);
                            else
                                machine.Interact();
                        }
                        break;
                }
            }
        }
        
        #endregion

        #region METHODS

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag(playerTag))
                EnterRegion();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(playerTag))
                ExitRegion();
        }

        /// <summary>
        ///     Handles the player enter region logic.
        /// </summary>
        private void EnterRegion()
        {
            insideRegion = true;
            OnEnter?.Invoke(); //subscribed event
            GameEventBroadcaster.BroadcastPlayerEnteredRegion(this); //informs other regions that we are in this one
        }

        /// <summary>
        ///     Handles the player exit region logic.
        /// </summary>
        private void ExitRegion()
        {
            insideRegion = false;
            OnExit?.Invoke(); //subscribed event
        }

        /// <summary>
        ///     Moves the player to the interaction region and locks the player movement.
        /// </summary>
        public async void MoveToInteraction()
        {
            var worldPosition = transform.TransformPoint(boxTrigger.center + (boxTrigger.size.x / 2f * Vector3.right)); //position at the boundary of the box trigger
            await PlayerMover.Instance.MoveToPosition(worldPosition, Quaternion.LookRotation(-transform.right));
            PlayerMover.Instance.ActivatePlayerInput(false);

            EnterRegion();
            HandleInput();
        }

        #endregion

        #region DEBUGGING
        [Debugging]
        [SerializeField, InspectorButton("MoveToInteraction")] bool m_MoveToInteraction;
        #endregion
    }
}