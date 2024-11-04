using FryingPanGame.Data;
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

        /// <summary>
        ///     The cook time of this individual ingedient.
        /// </summary>
        public int CookTime => cookTime;
        [SerializeField] private int cookTime;

        /// <summary>
        ///     The score for cooking this ingredient.
        /// </summary>
        public int Score => score;
        [SerializeField] private int score;

        [SerializeField] private MeshRenderer materialRender;
        [SerializeField] private Material[] materialOptions;

        private Collider collider;
        private MeshRenderer[] renderers;
        private GameObject highlight;

        #endregion

        #region METHODS

        #region ENGINE

        private void Awake()
        {
            collider = GetComponent<Collider>();
            renderers = GetComponentsInChildren<MeshRenderer>();
            highlight = transform.Find("Highlight").gameObject;
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
            if (GameManager.Instance.GameOn)
                HighlightIngredient(true);
        }

        private void OnMouseExit()
        {
            if (GameManager.Instance.GameOn)
                HighlightIngredient(false);
        }

        private void OnMouseDown()
        {
            if (GameManager.Instance.GameOn)
            {
                GameEventBroadcaster.BroadcastIngredientAdded(Category, ID); //invoke global event
                ActivateIngredient(false);
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
                highlight.SetActive(on);
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

            //Move the camera over this ingredient if this ingredient is the required one for the new recipe.
            switch(category)
            {
                case IngredientType.Dough:
                    if(recipe.doughID == _ID)
                        await CameraMover.Instance.MoveCamera(category, transform.position.z);
                    break;

                case IngredientType.Glaze:
                    if (recipe.glazeID == _ID)
                        await CameraMover.Instance.MoveCamera(category, transform.position.z);
                    break;

                case IngredientType.Sprinkles:
                    if (recipe.sprinkleID == _ID)
                        await CameraMover.Instance.MoveCamera(category, transform.position.z);
                    break;
            }
            if (this == null) return;

            ActivateIngredient(true); //reactivate the ingredient and it's collider
        }

        #endregion
    }
}