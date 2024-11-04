using System;

namespace FryingPanGame.Data
{
    /// <summary>
    ///     Container for the ID numbers of a specific recipe.
    /// </summary>
    [Serializable]
    public struct Recipe
    {
        public int doughID;
        public int glazeID;
        public int sprinkleID;
    }
}