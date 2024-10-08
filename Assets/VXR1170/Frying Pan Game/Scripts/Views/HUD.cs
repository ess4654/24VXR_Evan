using Controllers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    /// <summary>
    ///     Controls the visuals of the game canvas.
    /// </summary>
    public class HUD : MonoBehaviour
    {
        [SerializeField] List<Ingredient> ingredientPrefabs;
        [SerializeField] List<Transform> ingredientSpawnPoints;
        [SerializeField] private Text scoreText;
        [SerializeField] private Text timerText;
        [SerializeField] private GameObject gameOverScreen;

        private void Start()
        {
            gameOverScreen.SetActive(false);
        }

        public void UpdateTimer(int time)
        {
            timerText.text = time.ToString();
        }

        public void UpdateScore(int score)
        {
            scoreText.text = $"$ {score}";
        }

        public void ShowRecipe(List<int> recipe, FryingPan fryingPan)
        {
            int i = 0;
            foreach(var ingredient in recipe)
            {
                var prefab = ingredientPrefabs.First(x => x.ID == ingredient);
                var ingredientInstance = Instantiate(prefab, ingredientSpawnPoints[i]);
                ingredientInstance.Pan = fryingPan;
                i++;
            }
        }

        public void ShowGameOver()
        {
            gameOverScreen.SetActive(true);
        }
    }
}