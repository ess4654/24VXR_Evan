using ArcadeGame.Controllers;
using UnityEngine;

namespace ArcadeGame.Views
{
    /// <summary>
    ///     Handles the world canvases that enter/exit an arcade machine game.
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    public class WorldCanvas : Shared.Behaviour
    {
        [SerializeField] private InteractionArea interactionRegion;

        private Canvas canvas;

        #region SETUP

        private void Awake()
        {
            canvas = GetComponent<Canvas>();
            canvas.enabled = false;
            canvas.worldCamera = Camera.main;
        }

        private void OnEnable()
        {
            if (interactionRegion)
            {
                interactionRegion.OnEnter += RegionEntered;
                interactionRegion.OnExit += RegionExited;
            }
        }

        private void OnDisable()
        {
            if (interactionRegion)
            {
                interactionRegion.OnEnter -= RegionEntered;
                interactionRegion.OnExit -= RegionExited;
            }
        }

        #endregion

        private void RegionEntered()
        {
            canvas.enabled = true;
        }

        private void RegionExited()
        {
            canvas.enabled = false;
        }
    }
}