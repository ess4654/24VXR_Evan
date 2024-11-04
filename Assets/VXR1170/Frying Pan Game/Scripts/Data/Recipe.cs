using UnityEngine;

namespace FryingPanGame.Data
{
    /// <summary>
    ///     Container for the ID numbers of a specific recipe.
    /// </summary>
    [System.Serializable]
    public struct Recipe
    {
        public int doughID;
        public int glazeID;
        public int sprinkleID;
        public int scoreForRecipe;
        public int cookTime;

        public Recipe GenerateScoreAndCookTime()
        {
            var rand = Random.Range(Constants.MinimumRecipeScore, Constants.MaximumRecipeScore);
            scoreForRecipe = rand - (rand % 5); //ensure scores are a multiple of 5
            cookTime = Random.Range(Constants.MinimumCookTime, Constants.MaximumCookTime);
            return this;
        }
    }
}