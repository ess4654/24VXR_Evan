using FryingPanGame.Controllers;
using Shared.Helpers;
using System.Collections.Generic;
using UnityEngine;

namespace FryingPanGame.Views
{
    /// <summary>
    ///     Randomizes the material of a mesh renderer when new available ingredients are chosen.
    /// </summary>
    [RequireComponent(typeof(MeshRenderer))]
    public class RandomMaterialLoader : MonoBehaviour
    {
        [SerializeField] private Material[] materials;

        #region SETUP

        private void OnEnable()
        {
            GameEventBroadcaster.OnAvailableIngredientsSelected += SelectedNewIngredients;
        }

        private void OnDisable()
        {
            GameEventBroadcaster.OnAvailableIngredientsSelected -= SelectedNewIngredients;
        }

        #endregion

        private void SelectedNewIngredients(HashSet<int> doughIDs, HashSet<int> glazeIDs, HashSet<int> sprinkleIDs)
        {
            ResetMaterials();
        }

        private void ResetMaterials()
        {
            GetComponent<MeshRenderer>().material = materials.SelectRandom();
        }
    }
}