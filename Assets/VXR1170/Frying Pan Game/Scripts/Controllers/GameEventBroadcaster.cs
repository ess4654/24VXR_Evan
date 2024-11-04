using FryingPanGame.Data;
using System.Collections.Generic;

namespace FryingPanGame.Controllers
{
    /// <summary>
    ///     Broadcasts game events that classes can subscribe to for the frying pan game.
    /// </summary>
    public static class GameEventBroadcaster
    {
        #region DELEGATES

        /// <summary>
        ///     An event that contains available IDs for each ingredient type.
        /// </summary>
        /// <param name="doughIDs">IDs of the dough being used.</param>
        /// <param name="glazeIDs">IDs of the glaze being used.</param>
        /// <param name="sprinkleIDs">IDs of sprinkles being used.</param>
        public delegate void IngredientsID_Event(HashSet<int> doughIDs, HashSet<int> glazeIDs, HashSet<int> sprinkleIDs);

        /// <summary>
        ///     An event that takes no parameters.
        /// </summary>
        public delegate void VoidEvent();

        /// <summary>
        ///     An event that contains an ingredient message.
        /// </summary>
        /// <param name="type">The type of ingredient being added.</param>
        /// <param name="ID">The ID number of that ingredient type.</param>
        public delegate void IngredientEvent(IngedientType type, int ID);

        #endregion

        #region SUBSCRIBABLES

        /// <summary>
        ///     Called when the recipe builder choses the ingredients for the current game session.
        /// </summary>
        public static event IngredientsID_Event OnAvailableIngredientsSelected;

        /// <summary>
        ///     Called when an ingredient is clicked on.
        /// </summary>
        public static event IngredientEvent OnIngredientAdded;

        /// <summary>
        ///     Called when the ingredients are reset.
        /// </summary>
        public static event VoidEvent OnResetIngredients;

        #endregion

        #region BROADCASTERS

        /// <summary>
        ///     Broadcast that an ingredient has been added to the frying pan.
        /// </summary>
        /// <param name="type">The category of the ingredient being added.</param>
        /// <param name="ID">ID of the ingredient.</param>
        public static void BroadcastIngredientAdded(IngedientType type, int ID) =>
            OnIngredientAdded?.Invoke(type, ID); //subscribed event

        /// <summary>
        ///     Broadcast that cooking time has finsihed, or the recipe was wrong.
        /// </summary>
        public static void BroacastResetIngredients() =>
            OnResetIngredients?.Invoke(); //subscribed event

        /// <summary>
        ///     Broadcast that the current selection for ingredients has been determined.
        /// </summary>
        /// <param name="doughIDs">IDs of the dough being used.</param>
        /// <param name="glazeIDs">IDs of the glaze being used.</param>
        /// <param name="sprinkleIDs">IDs of sprinkles being used.</param>
        public static void BroadcastIngredientsSelected(HashSet<int> doughIDs, HashSet<int> glazeIDs, HashSet<int> sprinkleIDs) =>
            OnAvailableIngredientsSelected?.Invoke(doughIDs, glazeIDs, sprinkleIDs);

        #endregion
    }
}