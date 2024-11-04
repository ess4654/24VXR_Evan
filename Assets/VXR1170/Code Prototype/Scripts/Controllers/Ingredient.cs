using FryingPanGame.Data;
using FryingPanGame.Helpers;
using FryingPanGame.Views;
using Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FryingPanGame.Controllers
{
    /// <summary>
    ///     Handles the player input of clicking an ingredient.
    /// </summary>
    public class Ingredient : MonoBehaviour
    {
        #region VARIABLE DECLARATIONS

        [SerializeField] private int index;

        /// <summary>
        ///     Unique ID of this ingredient.
        /// </summary>
        /// 
        public int ID => _ID;
        [SerializeField, Min(0)] private int _ID;

        /// <summary>
        ///     Type of category this ingredient belongs to.
        /// </summary>
        public IngredientType Category => category;
        [SerializeField] private IngredientType category;

        [SerializeField] private MeshRenderer materialRender;
        [SerializeField] private Material[] materialOptions;
        [SerializeField] private AudioClip hoverSound;

        private Collider collider;
        private MeshRenderer[] renderers;
        private Transform highlight;
        private bool active;

        #endregion

        #region METHODS

        #region ENGINE

        private void Awake()
        {
            collider = GetComponent<Collider>();
            renderers = GetComponentsInChildren<MeshRenderer>();
            highlight = transform.Find("Highlight");
            HighlightIngredient(false);
        }

        private void OnEnable()
        {
            GameEventBroadcaster.OnAvailableIngredientsSelected += NewIngredientsSelected;
            GameEventBroadcaster.OnNewRecipe += NewRecipe;
        }

        private void OnDisable()
        {
            GameEventBroadcaster.OnAvailableIngredientsSelected -= NewIngredientsSelected;
            GameEventBroadcaster.OnNewRecipe -= NewRecipe;
        }

        private void OnMouseEnter()
        {
            if (GameManager.Instance.GameOn && active)
            {
                SoundManager.Instance.PlayClip(hoverSound);
                HighlightIngredient(true);
            }
        }

        private void OnMouseExit()
        {
            if (GameManager.Instance.GameOn && active)
                HighlightIngredient(false);
        }

        private void OnMouseDown()
        {
            if (GameManager.Instance.GameOn && active && !CookTimer.IsCooking) //prevent adding new ingredients to the pan if we are cooking
            {
                ActivateIngredient(false);
                GameEventBroadcaster.BroadcastIngredientAdded(Category, ID); //invoke global event
            }
        }

        #endregion

        /// <summary>
        ///     Highlights the ingredient when hovered over.
        /// </summary>
        /// <param name="on">Is the highlight on?</param>
        private void HighlightIngredient(bool on)
        {
            if(highlight)
                highlight.gameObject.SetActive(on);
        }

        /// <summary>
        ///     Enables/Disables this ingredient.
        /// </summary>
        /// <param name="active">Is the ingredient to be activated or deactivated?</param>
        public void ActivateIngredient(bool active)
        {
            if (collider != null)
                collider.enabled = active;

            if (renderers != null)
            {
                foreach (var renderer in renderers)
                    renderer.enabled = active;
            }

            HighlightIngredient(false);
            this.active = active;
        }

        /// <summary>
        ///     Called when a new game is started and the list of available recipe ingredients is reset.
        /// </summary>
        /// <param name="doughIDs">IDs for the available dough options.</param>
        /// <param name="glazeIDs">IDs of the available glaze options.</param>
        /// <param name="sprinkleIDs">IDs of the available sprinkle selection.</param>
        private void NewIngredientsSelected(HashSet<int> doughIDs, HashSet<int> glazeIDs, HashSet<int> sprinkleIDs)
        {
            switch (category)
            {
                case IngredientType.Dough:
                    RenderIngredient(doughIDs.ElementAt(index));
                    break;

                case IngredientType.Glaze:
                    RenderIngredient(glazeIDs.ElementAt(index));
                    break;

                case IngredientType.Sprinkles:
                    RenderIngredient(sprinkleIDs.ElementAt(index));
                    break;
            }
        }

        /// <summary>
        ///     Renders the material for the ingredient.
        /// </summary>
        /// <param name="materialIndex">Index of the material to render on this ingredient.</param>
        /// <exception cref="IndexOutOfRangeException">If the index of the material is out of range of the list of available materials</exception>
        /// <exception cref="Exception">If the material render for this ingredient is null</exception>
        public void RenderIngredient(int materialIndex)
        {
            if (materialIndex < 0 || materialIndex >= materialOptions.Length)
            {
                throw new IndexOutOfRangeException("materialIndex");
            }

            if(materialRender == null)
            {
                throw new Exception("You must define the material renderer for this ingredient");
            }

            _ID = materialIndex;
            var material = materialOptions[materialIndex];
            materialRender.material = material;
        }
        
        /// <summary>
        ///     Called when a new recipe is generated.
        /// </summary>
        /// <param name="recipe">IDs for the required item of the recipe.</param>
        private async void NewRecipe(Recipe recipe)
        {
            //Handles case where recipe is generated prior to the camera mover being initialized
            await Timer.WaitUntil(() => this == null || CameraMover.Instance != null);
            if (this == null) return;

            ActivateIngredient(true); //reactivate the ingredient and it's collider

            //Move the camera over this ingredient if this ingredient is the required one for the new recipe.
            var zPosition = transform.position.z;
            switch (category)
            {
                case IngredientType.Dough:
                    if(recipe.doughID == _ID)
                        await CameraMover.Instance.MoveCamera(category, zPosition);
                    break;

                case IngredientType.Glaze:
                    if (recipe.glazeID == _ID)
                        await CameraMover.Instance.MoveCamera(category, zPosition);
                    break;

                case IngredientType.Sprinkles:
                    if (recipe.sprinkleID == _ID)
                        await CameraMover.Instance.MoveCamera(category, zPosition);
                    break;
            }
        }

        #endregion
    }
}