using UnityEngine;
using UnityEngine.UI;

namespace HorrorHouse.Views
{
    /// <summary>
    ///     Handles the display of each inventory item on the UI canvas.
    /// </summary>
    public class InventoryButton : MonoBehaviour
    {
        [SerializeField] private Image button;
        [SerializeField] private Image icon;
        [SerializeField] private Color activeButton = Color.white;
        [SerializeField] private Sprite activeIcon;

        /// <summary>
        ///     Activates this artifact in the UI.
        /// </summary>
        [ContextMenu("Activate")]
        public void Activate()
        {
            if(button)
                button.color = activeButton;
            if(icon)
                icon.sprite = activeIcon;
        }
    }
}