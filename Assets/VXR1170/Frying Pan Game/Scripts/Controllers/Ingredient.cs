using FryingPanGame.Data;
using Helpers;
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
        public IngedientType Category => category;
        [SerializeField] private IngedientType category;

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
        private MaterialPropertyBlock highlight;

        #endregion

        #region METHODS

        #region ENGINE

        private void Awake()
        {
            collider = GetComponent<Collider>();
            renderers = GetComponentsInChildren<MeshRenderer>();
            highlight = new MaterialPropertyBlock();
            transform.Find("Higlight").GetComponent<MeshRenderer>().GetPropertyBlock(highlight);
            HighlightIngredient(false);
        }

        private void OnEnable()
        {
            GameEventBroadcaster.OnAvailableIngredientsSelected += NewIngredientsSelected;
        }

        private void OnDisable()
        {
            GameEventBroadcaster.OnAvailableIngredientsSelected -= NewIngredientsSelected;
        }

        private void OnMouseEnter()
        {
            HighlightIngredient(true);
        }

        private void OnMouseExit()
        {
            HighlightIngredient(false);
        }

        private void OnMouseDown()
        {
            GameEventBroadcaster.BroadcastIngredientAdded(Category, ID); //invoke global event
            ActivateIngredient(false);
        }

        #endregion

        /// <summary>
        ///     Highlights the ingredient when hovered over.
        /// </summary>
        /// <param name="on">Is the highlight on?</param>
        private void HighlightIngredient(bool on)
        {
            highlight?.SetColor("_Color", highlight.GetColor("_Color").ChangeAlpha(on ? .35f : .0f));
        }

        /// <summary>
        ///     Enables/Disables this ingredient.
        /// </summary>
        /// <param name="active">Is the ingredient to be activated or deactivated?</param>
        private void ActivateIngredient(bool active)
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
                case IngedientType.Dough:
                    RenderIngredient(doughIDs.ElementAt(index));
                    break;

                case IngedientType.Glaze:
                    RenderIngredient(glazeIDs.ElementAt(index));
                    break;

                case IngedientType.Sprinkles:
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
        private void RenderIngredient(int materialIndex)
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

        #endregion
    }
}