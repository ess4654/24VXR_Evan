using ArcadeGame.Controllers.Games;
using ArcadeGame.Data;
using ArcadeGame.Views.Machines;
using Shared.Editor;
using Shared.Helpers.Extensions;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace ArcadeGame.Controllers.Machines
{
    /// <summary>
    ///     Controls the behaviour of the shooter arcade cabinet.
    /// </summary>
    public class ShooterMachine : ShooterArcadeCabinetBase
    {
        #region VARIABLE DECLARATIONS

        /// <summary>
        ///     Called when the machine is interacted with passing the player standing position.
        /// </summary>
        public static event Action<int> OnInteraction;

        [Header("Cabinet Settings")]
        [SerializeField] private string gameScene;
        [SerializeField] private XRRayInteractor gunRay;
        [SerializeField] private MeshCollider screenCollider;

        [SerializeField, ReadOnly] private bool targetingScreen;
        [SerializeField, ReadOnly] private Vector2 uvScreenCoord;

        private new ShooterMachineAnimator animator => base.animator as ShooterMachineAnimator;

        /// <summary>
        ///     The position the player is standing at the machine.
        /// </summary>
        private int playerPosition = -1;
        private bool triggerDown;
        private float cacheStartLineWidth;
        private float cacheEndLineWidth;

        #endregion

        #region SETUP

        protected override void Awake()
        {
            base.Awake();
            
            //load arcade machine scene as additive
            if(!gameScene.IsNullOrWhiteSpace())
                SceneManager.LoadSceneAsync(gameScene, LoadSceneMode.Additive);
        }

        private void OnEnable()
        {
            XRInputManager.OnControllerTrigger += HandleControllerTrigger;
            XRInputManager.OnControllerTriggerUp += HandleControllerTriggerUp;
        }

        private void OnDisable()
        {
            XRInputManager.OnControllerTrigger -= HandleControllerTrigger;
            XRInputManager.OnControllerTriggerUp -= HandleControllerTriggerUp;
        }

        private void HandleControllerTrigger(InputDevice device)
        {
            if (triggerDown) return;

            if(device.characteristics.HasFlag(InputDeviceCharacteristics.Right) && GameData.State == gameStateOnPlay && targetingScreen)
            {
                triggerDown = true;

                var tex = screenCollider.GetComponent<MeshRenderer>().material.mainTexture;
                var screenPointHit = new Vector2(uvScreenCoord.x * tex.width, uvScreenCoord.y * tex.height);
                //Log($"Hit Screen ({tex.width} x {tex.height}): {uvScreenCoord} ({screenPointHit})");
                ShooterGame.Instance.FireShot(screenPointHit);
            }
        }

        private void HandleControllerTriggerUp(InputDevice device)
        {
            if (device.characteristics.HasFlag(InputDeviceCharacteristics.Right))
                triggerDown = false;
        }

        protected override void OnGameStart()
        {
            if(gunRay.TryGetComponent(out LineRenderer line))
            {
                cacheStartLineWidth = line.startWidth;
                cacheEndLineWidth = line.endWidth;

                line.startWidth = 0; //hide the raycast line
                line.endWidth = 0; //hide the raycast line
            }
        }

        protected override void OnGameStop()
        {
            animator.ToggleGun(0, true);
            animator.ToggleGun(1, true);

            if (gunRay.TryGetComponent(out LineRenderer line) && (cacheStartLineWidth > 0 || cacheEndLineWidth > 0))
            {
                line.startWidth = cacheStartLineWidth; //hide the raycast line
                line.endWidth = cacheEndLineWidth; //hide the raycast line
            }
        }

        #endregion

        /// <summary>
        ///     Interact with the shooter game.
        /// </summary>
        /// <param name="standingPosition">Standing position of the player.</param>
        public void Interact(int standingPosition)
        {
            playerPosition = standingPosition;
            OnInteraction?.Invoke(playerPosition); //subscribed event
            animator.ToggleGun(playerPosition, false); //toggle the gun model at the position off
            base.Interact();
        }

        protected override void OnGameActive()
        {
            UpdateUVCoord(gunRay);
        }

        /// <summary>
        ///     The ray passed in from the right controller of the player.
        /// </summary>
        /// <param name="ray">Reference to the ray interactor object.</param>
        private void UpdateUVCoord(XRRayInteractor ray)
        {
            if(ray != null && ray.TryGetCurrent3DRaycastHit(out RaycastHit hit) && hit.collider == screenCollider)
            {
                targetingScreen = true;
                uvScreenCoord = hit.textureCoord;
            }
            else
                targetingScreen = false;
        }
    }
}