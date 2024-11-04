using FryingPanGame.Data;
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

        #endregion

        #region METHODS

        private void Awake()
        {
            collider = GetComponent<Collider>();
            renderers = GetComponentsInChildren<MeshRenderer>();
        }

        private void OnEnable()
        {
            GameEventBroadcaster.OnAvailableIngredientsSelected += NewIngredientsSelected;
        }

        private void OnDisable()
        {
            GameEventBroadcaster.OnAvailableIngredientsSelected -= NewIngredientsSelected;
        }

        private void OnMouseDown()
        {
            GameEventBroadcaster.BroadcastIngredientAdded(Category, ID);
            ActivateIngredient(false);
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
                    RenderIngredient(doughIDs.ElementAt(ID));
                    break;

                case IngedientType.Glaze:
                    RenderIngredient(glazeIDs.ElementAt(ID));
                    break;

                case IngedientType.Sprinkles:
                    RenderIngredient(sprinkleIDs.ElementAt(ID));
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

            var material = materialOptions[materialIndex];
            materialRender.material = material;
        }

        #endregion
    }
}