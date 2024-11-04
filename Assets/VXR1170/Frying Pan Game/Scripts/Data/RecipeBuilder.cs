using FryingPanGame.Controllers;
using Helpers;
using System.Collections.Generic;
using UnityEngine;

namespace FryingPanGame.Data
{
    /// <summary>
    ///     Generates recipe's for the chef.
    /// </summary>
    public class RecipeBuilder
    {
        #region VARIABLE DECLARATIONS

        /// <summary>
        ///     Randomly generated list of dough ID's for this game sesssion.
        /// </summary>
        private HashSet<int> DoughSelectionIDs = new HashSet<int>();

        /// <summary>
        ///     Randomly generated list of glaze ID's for this game sesssion.
        /// </summary>
        private HashSet<int> GlazeSelectionIDs = new HashSet<int>();

        /// <summary>
        ///     Randomly generated list of sprinkle ID's for this game sesssion.
        /// </summary>
        private HashSet<int> SprinkleSelectionIDs = new HashSet<int>();

        //number of available variations for each ingredient category.
        private const int doughVariations = 4;
        private const int glazeVariations = 6;
        private const int sprinkleVariations = 6;

        //number of available selections for each ingredient category on the table currently.
        private const int numberDoughSelections = 2;
        private const int numberGlazeSelections = 3;
        private const int numberSprinleSelections = 3;

        #endregion

        /// <summary>
        ///     Creates a new recipe builder.
        /// </summary>
        public RecipeBuilder()
        {
            ResetSelection();
        }

        /// <summary>
        ///     Resets the selection of available ingredients that are available on the table for use.
        /// </summary>
        public void ResetSelection()
        {
            //clear old selection ID's
            DoughSelectionIDs.Clear();
            GlazeSelectionIDs.Clear();
            SprinkleSelectionIDs.Clear();

            //generate available ingredients for dough
            while (this != null && DoughSelectionIDs.Count < numberDoughSelections)
                DoughSelectionIDs.Add(Random.Range(0, doughVariations));

            //generate available ingredients for glaze
            while (this != null && GlazeSelectionIDs.Count < numberGlazeSelections)
                GlazeSelectionIDs.Add(Random.Range(0, glazeVariations));

            //generate available ingredients for sprinkles
            while (this != null && SprinkleSelectionIDs.Count < numberSprinleSelections)
                SprinkleSelectionIDs.Add(Random.Range(0, sprinkleVariations));

            GameEventBroadcaster.BroadcastIngredientsSelected(DoughSelectionIDs, GlazeSelectionIDs, SprinkleSelectionIDs); //invoke event
        }

        /// <summary>
        ///     Generates a recipe from the list of available selection ingredients.
        /// </summary>
        /// <returns>Randomly generated recipe</returns>
        public Recipe GenerateRecipe() => new Recipe
        {
            doughID = (DoughSelectionIDs != null && DoughSelectionIDs.Count > 0) ? DoughSelectionIDs.SelectRandom() : -1,
            glazeID = (GlazeSelectionIDs != null && GlazeSelectionIDs.Count > 0) ? GlazeSelectionIDs.SelectRandom() : -1,
            sprinkleID = (SprinkleSelectionIDs != null & SprinkleSelectionIDs.Count > 0) ? SprinkleSelectionIDs.SelectRandom() : -1
        };
    }
}