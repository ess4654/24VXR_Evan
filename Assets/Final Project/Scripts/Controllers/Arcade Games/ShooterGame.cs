using ArcadeGame.Data;
using Shared;
using Shared.Helpers;
using Shared.Helpers.Extensions;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ArcadeGame.Controllers.Games
{
    /// <summary>
    ///     Controls the game behaviour for the shooter arcade cabinet.
    /// </summary>
    public class ShooterGame : Singleton<ShooterGame>
    {
        #region VARAIABLE DECLARATIONS

        [SerializeField] private bool testing;
        [SerializeField] private BulletShot shotPrefab;
        [SerializeField] private Duck[] ducks;
        [SerializeField] private RectTransform screen;
        [SerializeField] private RectTransform[] screenBoundaries;

        [Header("Game Settings")]
        [SerializeField] private float spawnRate = 4;

        #endregion

        #region SETUP
        
        private void Start() => StartCoroutine(RunGame());

        /// <summary>
        ///     Run the game asynchronously.
        /// </summary>
        /// <returns>Game Routine</returns>
        private IEnumerator RunGame()
        {
            while (this)
            {
                yield return new WaitForSeconds(spawnRate);

                if (GameData.State == GameState.ShooterCabinet)
                    SpawnDuck();
            }
        }

        #endregion

        #region METHODS

        /// <summary>
        ///     Spawns an object into 2D space on the screen.
        /// </summary>
        /// <param name="prefab">Prefab to spawn.</param>
        /// <param name="screenPoint">Point on the screen to spawn the object.</param>
        /// <param name="screenChild">(Optional) Child of the screen to spawn the object parented to.</param>
        private void SpawnObject(Image prefab, Vector2 screenPoint, string screenChild = null)
        {
            if (prefab != null)
            {
                var shot = Instantiate(prefab, screenChild.IsNullOrWhiteSpace() ? screen : screen.Find(screenChild));
                shot.rectTransform.anchoredPosition = screenPoint;
            }
        }

        /// <summary>
        ///     Spawns a duck randomly at the boundary of the screen.
        /// </summary>
        private void SpawnDuck()
        {
            var randomDuck = ducks.SelectRandom();
            var randomBoundary = screenBoundaries.SelectRandom();
            SpawnObject(randomDuck.GetComponent<Image>(), randomBoundary.anchoredPosition, "Ducks");
        }

        /// <summary>
        ///     Fires a shot onto the screen point of the arcade cabinet.
        /// </summary>
        /// <param name="screenPoint">The screen point position that was shot at.</param>
        public void FireShot(Vector2 screenPoint) =>
            SpawnObject(shotPrefab.GetComponent<Image>(), screenPoint);

        #endregion

        #region UPDATES

        private void Update()
        {
            if (testing)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    FireShot(Input.mousePosition);
                }
            }

            if (GameData.State == GameState.ShooterCabinet)
            {

            }
        }

        #endregion
    }
}