using ArcadeGame.Controllers;
using ArcadeGame.Controllers.Machines;
using ArcadeGame.Data;
using Shared.Editor;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace ArcadeGame.Views
{
    /// <summary>
    ///     Swaps the model of the controller on the attached XR Hand.
    /// </summary>
    public class XRHandModelSwapper : Shared.Behaviour
    {
        #region VARIABLE DECLARATIONS

        [SerializeField] private List<GameObject> handPrefabs;

        /// <summary>
        ///     Parent of the controller model in 3D space.
        /// </summary>
        private Transform HandParent => xrController.modelParent;

        /// <summary>
        ///     Default prefab for the XR controller.
        /// </summary>
        private Transform DefaultHandPrefab => xrController.modelPrefab;
        
        private XRBaseController xrController;
        private int handIndex = -1;

        #endregion

        #region SETUP

        private void Awake()
        {
            xrController = GetComponent<XRBaseController>();
        }

        private void OnEnable()
        {
            ShooterMachine.OnInteraction += SwapHand;
            GameEventBroadcaster.OnGameStateChanged += HandleStateChanged;
        }

        private void OnDisable()
        {
            ShooterMachine.OnInteraction -= SwapHand;
            GameEventBroadcaster.OnGameStateChanged -= HandleStateChanged;
        }

        private void HandleStateChanged(GameState state)
        {
            if (state == GameState.Arcade)
                ReturnToDefaultHand();
        }

        #endregion

        #region METHODS

        /// <summary>
        ///     Returns the model to the default hand model.
        /// </summary>
        public void ReturnToDefaultHand() => SwapHand(-1);

        /// <summary>
        ///     Swaps the model to the one provided at the given index.
        /// </summary>
        /// <param name="prefabIndex">Index of the model from the prefab list.</param>
        public void SwapHand(int prefabIndex)
        {
            handIndex = prefabIndex;
            SwapModel(handIndex == -1 ? DefaultHandPrefab.gameObject : handPrefabs[handIndex]);
        }

        /// <summary>
        ///     Swaps the model on the hand with the injected prefab for the new hand.
        /// </summary>
        /// <param name="handPrefab">Prefab to be instantiated.</param>
        private void SwapModel(GameObject handPrefab)
        {
            Destroy(HandParent.GetChild(0).gameObject);
            Instantiate(handPrefab, HandParent);
        }

        #endregion

        #region DEBUGGING

        [Debugging]
        [SerializeField] int _index;
        [SerializeField, InspectorButton("TestSwapHand")] bool m_SwapHand;
        void TestSwapHand() => SwapHand(_index);

        #endregion
    }
}