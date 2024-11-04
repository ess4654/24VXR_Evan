using FryingPanGame.Data;
using FryingPanGame.Views;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FryingPanGame.Controllers
{
    /// <summary>
    ///     Handles all game logic for the frying pan game.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private FryingPan fryingPan;
        [SerializeField] private HUD hud;
        [SerializeField] private Recipe currentRecipe;
        
        private RecipeBuilder recipeBuilder;
        private int timer;
        private int score;

        private void Start()
        {
            score = 0;
            ResetRecipe();
            StartCoroutine(Countdown());
        }

        private IEnumerator Countdown()
        {
            timer = Constants.CountdownTime;
            hud.UpdateTimer(timer);
            
            while (timer > 0)
            {
                yield return new WaitForSeconds(1.0f);
                timer--;
                hud.UpdateTimer(timer);
            }
        }

        public void ResetRecipe()
        {
            recipeBuilder ??= new RecipeBuilder();
            currentRecipe = recipeBuilder.GenerateRecipe();
            fryingPan.ClearPan();
            //hud.ShowRecipe(currentRecipe, fryingPan);
        }

        public void CheckRecipe(List<int> cookingIngredients)
        {
            //bool matchingRecipe = cookingIngredients.Intersect(currentRecipe).Count() == currentRecipe.Count;
            //if(matchingRecipe)
            //{
            //    score++;
            //    hud.UpdateScore(score);
            //}
            //
            //ResetRecipe();
        }

        private void Update()
        {
            if(timer <= 0)
                GameOver();
        }

        private void GameOver()
        {
            StopCoroutine(nameof(Countdown));
            hud.ShowGameOver();
        }
    }
}