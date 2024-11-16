using Shared;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ArcadeGame.Views
{
    /// <summary>
    ///     Manages the HUD for the arcade game.
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    public class HUD : Singleton<HUD>
    {
        #region VARIABLE DECLARATIONS

        /// <summary>
        ///     Container used to tie a UI panel with a string key.
        /// </summary>
        [System.Serializable]
        public struct HUDPanelField
        {
            public string key;
            public CanvasRenderer panel;
        }

        [SerializeField] private List<HUDPanelField> panels;

        [Header("Camera Settings")]
        [SerializeField] private Transform xrCameraPivot;
        [SerializeField] private float distanceFromCamera = 10;

        #endregion

        #region SETUP

        protected override void Awake()
        {
            base.Awake();

            HideAllPanels();
        }

        #endregion

        #region UPDATES

        private void Update()
        {
            //move the HUD to match the HMD position and rotation
            if(xrCameraPivot != null)
            {
                transform.position = xrCameraPivot.position + (xrCameraPivot.forward * distanceFromCamera);
                transform.forward = xrCameraPivot.forward;
            }
        }

        #endregion

        #region METHODS

        /// <summary>
        ///     Hides all panels from view.
        /// </summary>
        public static void HideAllPanels()
        {
            foreach (var panel in Instance.panels)
                ToggleUI(panel.key, false);
        }

        /// <summary>
        ///     Toggles the UI panel on/off.
        /// </summary>
        /// <param name="key">Key of the panel to toggle.</param>
        /// <param name="active">Whether the panel should be turned on or off.</param>
        public static void ToggleUI(string key, bool active)
        {
            var panelParent = Instance.panels.FirstOrDefault(x => x.key == key);
            if(panelParent.panel != null)
                panelParent.panel.gameObject.SetActive(active);
        }

        #endregion
    }
}