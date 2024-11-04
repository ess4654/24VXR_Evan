namespace FryingPanGame.Data
{
    /// <summary>
    ///     Global constants for the frying pan game.
    /// </summary>
    public static class Constants
    {
        public const int RequiredIngredients = 3;
        public const int CountdownTime = 60;
        
        public const int MinimumRecipeScore = 10;
        public const int MaximumRecipeScore = 31;

        public const int MinimumCookTime = 3;
        public const int MaximumCookTime = 8;

        /// <summary>
        ///     Score to penalize the player if the frying pan is clicked before the ingredients are added
        ///     or when the wrong ingredients for the recipe are added to the pan. 
        /// </summary>
        public const int PenaltyScore = 5;

        /// <summary>
        ///     Additional points modifier if the frying pan is clicked while the recipe is cooking.
        /// </summary>
        public const float BonusModifier = .2f;
    }
}