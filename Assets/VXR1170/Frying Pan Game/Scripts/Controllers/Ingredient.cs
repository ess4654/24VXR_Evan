using UnityEngine;

namespace Controllers
{
    /// <summary>
    ///     Handles the player input of clicking an ingredient.
    /// </summary>
    public class Ingredient : MonoBehaviour
    {
        /// <summary>
        ///     Unique ID of this ingredient.
        /// </summary>
        public int ID => _ID;
        [SerializeField] private int _ID;

        /// <summary>
        ///     Reference to the frying pan that this ingredient is added to.
        /// </summary>
        public FryingPan Pan { get; set;}

        private void OnMouseDown()
        {
            if(Pan)
                Pan.AddIngredient(ID);
        }
    }
}