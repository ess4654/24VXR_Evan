using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    /// <summary>
    ///     Generates recipe's for the chef.
    /// </summary>
    public class RecipeBuilder
    {
        /// <summary>
        ///     List of unique ingredients that the recipe builder can select from.
        /// </summary>
        public List<int> Ingredients { get; private set; }

        /// <summary>
        ///     Creates a new recipe builder.
        /// </summary>
        /// <param name="ingredients">Possible ingredients that a recipe can be generated from.</param>
        public RecipeBuilder(List<int> ingredients)
        {
            Ingredients = ingredients;
        }

        /// <summary>
        ///     Generates a recipe with the number of requested ingredients.
        /// </summary>
        /// <param name="numberIngredients">Number of ingredients to include.</param>
        /// <returns>Randomly generated recipe</returns>
        public List<int> GenerateRecipe(int numberIngredients)
        {
            var recipeIngredients = new List<int>();
            for(int i = 0; i < numberIngredients; i++)
                recipeIngredients.Add(Ingredients[Random.Range(0, Ingredients.Count)]);

            return recipeIngredients;
        }
    }
}