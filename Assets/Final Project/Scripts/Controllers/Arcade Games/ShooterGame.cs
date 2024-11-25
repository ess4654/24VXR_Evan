using Shared;
using UnityEngine;
using UnityEngine.UI;

namespace ArcadeGame.Controllers.Games
{
    /// <summary>
    ///     Controls the game behaviour for the shooter arcade cabinet.
    /// </summary>
    public class ShooterGame : Singleton<ShooterGame>
    {
        [SerializeField] private Image shotPrefab;
        [SerializeField] private RectTransform screen;

        public void FireShot(Vector2 screenPoint)
        {
            var shot = Instantiate(shotPrefab, screen);
            shot.rectTransform.anchoredPosition = screenPoint;
        }
    }
}