using FryingPanGame.Data;
using FryingPanGame.Helpers;
using FryingPanGame.Views;
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
        
        [Header("Final Product")]
        [SerializeField] private GameObject finalProduct;
        [SerializeField] private MeshRenderer doughRender;
        [SerializeField] private MeshRenderer glazeRender;
        [SerializeField] private Material[] doughMaterials;
        [SerializeField] private Material[] glazeMaterials;
        [SerializeField] private Material[] spinkleMaterials;

        [Header("Audio")]
        [SerializeField] private AudioClip errorSound;

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
            if (!GameManager.Instance.GameOn) //prevent adding ingredients to the pan if the game is not running
                return;

            if(currentIngredients.Count >= Constants.RequiredIngredients) //prevent additional ingredients from being added to the pan
                return;

            //add to the list of current ingredients in the pan
            currentIngredients.Add(new Tuple<IngredientType, int>(category, ID));
            
            //show the ingredient in the pan
            var prefab = ingredientPrefabs[(int)category - 1];
            var ingredient = Instantiate(prefab, spawnPoints[currentIngredients.Count - 1]);
            ingredient.RenderIngredient(ID);
            Destroy(ingredient.GetComponent<Collider>()); //prevent any interaction with this ingredient
            Destroy(ingredient);

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

                point.gameObject.SetActive(true);
            }

            if (finalProduct)
                finalProduct.SetActive(false); //hide the final product

            //ensure that we are not cooking
            isCooking = false;
            addBonus = false;

            if(cookingRoutine != null)
                StopCoroutine(cookingRoutine);
        }

        /// <summary>
        ///     Submits a recipe to the game manager.
        /// </summary>
        private void SubmitRecipe()
        {
            bool matchingRecipe = GameManager.Instance.CheckRecipe(new List<Tuple<IngredientType, int>>(currentIngredients)); //ensure that game manager does not edit the current items in pan

            if (matchingRecipe) 
            {
                Debug.Log("Recipe is a match");
                cookingRoutine = StartCoroutine(CookDoughnut());
            }
            else //wrong recipe
            {
                Debug.Log("Wrong ingredients for this recipe");
                SoundManager.Instance.PlayClip(errorSound);
                GameManager.Instance.UpdateScore(GameManager.Instance.Score - penaltyScore); //penalize player for incorrect recipe
                GameManager.Instance.ResetRecipe();
            }
        }

        private IEnumerator CookDoughnut()
        {
            //Wait for cooking to complete
            var cookTime = GameManager.Instance.CookTime;
            CookTimer.Instance.RunTimer(cookTime);
            isCooking = true;
            yield return new WaitForSeconds(cookTime / 2f);

            //render the final product after half the cooking time
            CompleteDoughnut();

            yield return new WaitForSeconds(cookTime / 2f);
            isCooking = false;

            if (GameManager.Instance.GameOn)
            {
                //calculate bonus and update the player score
                int baseScore = GameManager.Instance.Score;
                float ingredientScore = GameManager.Instance.RecipeScore * (1f + (addBonus ? bonusModifier : 0)); //adds a 20% bonus to the score for clicking the pan while we are cooking
                int newScore = (int)(baseScore + ingredientScore);

                Debug.Log(addBonus ?
                    $"Added 20% Bonus to the score for this recipe: {ingredientScore}" :
                    $"Score for cooking this recipe: {ingredientScore}");

                //update the score and reset the recipe
                GameManager.Instance.UpdateScore(newScore); //submit the new score to the game manager
                GameManager.Instance.ResetRecipe();
            }
        }

        /// <summary>
        ///     Renders the final product in the pan.
        /// </summary>
        private void CompleteDoughnut()
        {
            //set the materials for the final render
            foreach(var item in currentIngredients)
            {
                switch(item.Item1)
                {
                    case IngredientType.Dough:
                        if(doughRender)
                            doughRender.material = doughMaterials[item.Item2];
                        break;

                    case IngredientType.Glaze:
                        if(glazeRender)
                        {
                            var materials = glazeRender.materials;
                            materials[0] = glazeMaterials[item.Item2];
                            glazeRender.materials = materials;
                        }
                        break;

                    case IngredientType.Sprinkles:
                        if (glazeRender)
                        {
                            var materials = glazeRender.materials;
                            materials[1] = spinkleMaterials[item.Item2];
                            glazeRender.materials = materials;
                        }
                        break;
                }
            }

            //hide individual ingredients
            foreach (var point in spawnPoints)
                point.gameObject.SetActive(false);

            //show the final product
            if (finalProduct)
                finalProduct.SetActive(true);
        }

        #endregion
    }
}