using FryingPanGame.Data;
using FryingPanGame.Views;
using Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FryingPanGame.Controllers
{
    /// <summary>
    ///     Handles all game logic for the frying pan game.
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {
        #region VARIABLE DECLARATIONS

        [SerializeField] private bool gameOn;
        [SerializeField] private FryingPan fryingPan;
        [SerializeField] private HUD hud;
        [SerializeField] private Recipe currentRecipe;

        /// <summary>
        ///     Is an active game currently running?
        /// </summary>
        public bool GameOn => gameOn;

        /// <summary>
        ///     The current score earned.
        /// </summary>
        public int Score => score;
        private int score;

        /// <summary>
        ///     The score earned if this recipe is completed successfully
        /// </summary>
        public int RecipeScore => currentRecipe.scoreForRecipe;

        /// <summary>
        ///     The cook time for this recipe.
        /// </summary>
        public int CookTime => currentRecipe.cookTime;

        private RecipeBuilder recipeBuilder;
        private int timer;
        private Coroutine countdownRoutine;

        #endregion

        #region METHODS

        #region ENGINE

        private void Update()
        {
            if (gameOn && timer <= 0)
                GameOver();
        }

        #endregion

        /// <summary>
        ///     Called by buttons to start a new game or reset one on game over.
        /// </summary>
        public void StartNewGame()
        {
            StopAllCoroutines();
            score = 0;
            ResetRecipe();
            gameOn = true;
            countdownRoutine = StartCoroutine(Countdown());
        }

        /// <summary>
        ///     While the game is running, count down the timer clock.
        /// </summary>
        /// <returns>Reference to the countdown Corotuine</returns>
        private IEnumerator Countdown()
        {
            timer = Constants.CountdownTime;
            hud.UpdateTimer(timer);
            
            while (this && gameOn && timer > 0)
            {
                yield return new WaitForSeconds(1.0f); //wait for 1 second
                timer--;
                hud.UpdateTimer(timer);
            }
        }

        /// <summary>
        ///     Resets the recipe on new game or when the recipe is cooked or failed.
        /// </summary>
        public void ResetRecipe()
        {
            recipeBuilder ??= new RecipeBuilder();
            currentRecipe = recipeBuilder.GenerateRecipe();
            fryingPan.ClearPan();
            GameEventBroadcaster.BroacastNewRecipe(currentRecipe);
        }

        /// <summary>
        ///     Checks the current selection of ingredients in the frying pan to see if it matches the recipe.
        /// </summary>
        /// <param name="panIngredients">Ingredients currently in the pan.</param>
        /// <returns>True if the ingredients in the pan matches the recipe.</returns>
        public bool CheckRecipe(List<Tuple<IngredientType, int>> panIngredients)
        {
            if(panIngredients.Count != Constants.RequiredIngredients) //handles the case of an empty pan
                return false;

            panIngredients.Remove(new Tuple<IngredientType, int>(IngredientType.Dough, currentRecipe.doughID));
            panIngredients.Remove(new Tuple<IngredientType, int>(IngredientType.Glaze, currentRecipe.glazeID));
            panIngredients.Remove(new Tuple<IngredientType, int>(IngredientType.Sprinkles, currentRecipe.sprinkleID));

            return GameOn && panIngredients.Count == 0; //all ingredients are a match and there is no additional ingredients in the pan
        }

        /// <summary>
        ///     Updates the score in the game manager.
        /// </summary>
        /// <param name="score">Value of the score.</param>
        public void UpdateScore(int score)
        {
            this.score = score;
            HUD.Instance.UpdateScore(score);
        }

        /// <summary>
        ///     Triggered when the timer reaches 0 and the game is running.
        /// </summary>
        private void GameOver()
        {
            Debug.Log("Game Over");
            if(countdownRoutine!= null)
                StopCoroutine(countdownRoutine);
            hud.ShowGameOver(true);
        }

        #endregion
    }
}