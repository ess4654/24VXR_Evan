using FryingPanGame.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FryingPanGame.Controllers
{
    /// <summary>
    ///     Controls the behaviour of the frying pan.
    /// </summary>
    public class FryingPan : MonoBehaviour
    {
        #region VARIABLE DECLARATIONS

        [SerializeField] private bool isCooking;
        [SerializeField] private Ingredient[] ingredientPrefabs;
        [SerializeField] private Transform[] spawnPoints;

        /// <summary>
        ///     Are the ingredients in the pan being cooked?
        /// </summary>
        public bool IsCooking => isCooking;

        private List<Tuple<IngredientType, int>> currentIngredients = new List<Tuple<IngredientType, int>>();
        private Coroutine cookingRoutine;
        private bool addBonus;
        
        private const int penaltyScore = 5;
        private const float bonusModifier = .2f;

        #endregion

        #region METHODS

        #region ENGINE

        private void OnEnable()
        {
            GameEventBroadcaster.OnIngredientAdded += AddIngredient;
        }

        private void OnDisable()
        {
            GameEventBroadcaster.OnIngredientAdded -= AddIngredient;
        }

        private void OnMouseDown()
        {
            if (GameManager.Instance.GameOn)
            {
                //submit the recipe if not cooking
                if (!isCooking)
                    SubmitRecipe();
                else
                    addBonus = true; //adds bonus if we click the pan while ingredients are cooking
            }
        }

        #endregion

        /// <summary>
        ///     Adds the selected ingredient to the frying pan.
        /// </summary>
        /// <param name="category">Category of the ingredient selected.</param>
        /// <param name="ID">ID of the ingredient selected.</param>
        private void AddIngredient(IngredientType category, int ID)
        {
            if(currentIngredients.Count >= Constants.RequiredIngredients) //prevent additional ingredients from being added to the pan
            {
                return;
            }

            //add to the list of current ingredients in the pan
            currentIngredients.Add(new Tuple<IngredientType, int>(category, ID));
            
            //show the ingredient in the pan
            var prefab = ingredientPrefabs[(int)category - 1];
            var ingredient = Instantiate(prefab, spawnPoints[currentIngredients.Count]);
            ingredient.ActivateIngredient(false);
            ingredient.RenderIngredient(ID);

            //pan is full of ingredients, check with the manager if the recipe is a match
            if (currentIngredients.Count >= Constants.RequiredIngredients)
                SubmitRecipe();
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
                for (int i = point.childCount - 1; i >= 0; i--)
                {
                    var child = point.GetChild(i);
                    Destroy(child.gameObject);
                }
            }

            //ensure that we are not cooking
            isCooking = false;
            addBonus = false;
            StopCoroutine(cookingRoutine);
        }

        /// <summary>
        ///     Submits a recipe to the game manager.
        /// </summary>
        private void SubmitRecipe()
        {
            bool matchingRecipe = GameManager.Instance.CheckRecipe(currentIngredients);

            if (matchingRecipe) 
            {
                Debug.Log("Recipe is a match");
                cookingRoutine = StartCoroutine(CookDoughnut());
            }
            else //wrong recipe
            {
                Debug.Log("Wrong ingredients for this recipe");
                GameManager.Instance.UpdateScore(GameManager.Instance.Score - penaltyScore); //penalize player for incorrect recipe
                GameManager.Instance.ResetRecipe();
            }
        }

        private IEnumerator CookDoughnut()
        {
            //Wait for cooking to complete
            var cookTime = GameManager.Instance.CookTime;
            isCooking = true;
            yield return new WaitForSeconds(cookTime);
            isCooking = false;

            //calculate bonus and update the player score
            int baseScore = GameManager.Instance.Score;
            float ingredientScore = GameManager.Instance.RecipeScore * (1f + (addBonus ? bonusModifier : 0)); //adds a 20% bonus to the score for clicking the pan while we are cooking
            int newScore = (int)(baseScore + ingredientScore);

            Debug.Log(addBonus ?
                $"Added 20% Bonus to the score for this recipe: {ingredientScore}":
                $"Score for cooking this recipe: {ingredientScore}");

            //update the score and reset the recipe
            GameManager.Instance.UpdateScore(newScore); //submit the new score to the game manager
            GameManager.Instance.ResetRecipe();
        }

        #endregion
    }
}