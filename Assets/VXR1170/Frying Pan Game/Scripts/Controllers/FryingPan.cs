using FryingPanGame.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Controllers
{
    /// <summary>
    ///     Controls the behaviour of the frying pan.
    /// </summary>
    public class FryingPan : MonoBehaviour
    {
        [SerializeField] private List<Ingredient> ingredientPrefabs;
        [SerializeField] private List<Transform> spawnPoints;
        [SerializeField] private GameManager game;

        private List<int> currentIngredients = new List<int>();

        public void AddIngredient(int ID)
        {
            if(currentIngredients.Count >= Constants.RequiredIngredients)
            {
                return;
            }

            var prefab = ingredientPrefabs[ID];
            var parent = spawnPoints[currentIngredients.Count];
            currentIngredients.Add(ID);
            Instantiate(prefab, parent);
        }

        /// <summary>
        ///     Remove all ingredients from the frying pan.
        /// </summary>
        public void ClearPan()
        {
            currentIngredients?.Clear();

            //Remove all ingredients from the frying pan
            foreach(var point in spawnPoints)
            {
                for(int i = point.childCount; i >= 0; i--)
                {
                    var child = point.GetChild(i);
                    Destroy(child.gameObject);
                }
            }
        }

        //Submits a recipe to the game mamnager.
        private void SubmitRecipe()
        {
            game.CheckRecipe(currentIngredients);
        }

        private void OnMouseDown()
        {
            if (currentIngredients.Count >= Constants.RequiredIngredients)
                SubmitRecipe();
        }
    }
}