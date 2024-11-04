using FryingPanGame.Data;
using Shared;
using UnityEngine;
using UnityEngine.UI;

namespace FryingPanGame.Views
{
    /// <summary>
    ///     Controls the visuals of the game canvas.
    /// </summary>
    public class HUD : Singleton<HUD>
    {
        [SerializeField] private Text scoreText;
        [SerializeField] private Text timerText;
        [SerializeField] private GameObject gameOverScreen;
        [SerializeField] private Recipe currentRecipe;

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

        public void ShowGameOver()
        {
            gameOverScreen.SetActive(true);
        }
    }
}