using FryingPanGame.Controllers;
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
        [SerializeField] private Text finalScoreText;
        [SerializeField] private GameObject gameOverScreen;

        #region METHODS

        private void Start()
        {
            ShowGameOver(false);
        }

        /// <summary>
        ///     Updates the timer display in the HUD.
        /// </summary>
        /// <param name="time">Remaining time for the game.</param>
        public void UpdateTimer(int time)
        {
            if (timerText)
                timerText.text = time.ToString();
            Debug.Log("Remaining Time: " + time);
        }

        /// <summary>
        ///     Updates the score display in the HUD.
        /// </summary>
        /// <param name="score">Value of the score to display.</param>
        public void UpdateScore(int score)
        {
            if (scoreText)
                scoreText.text = $"Score: {score}";
            Debug.Log("Score: " + score);
        }

        /// <summary>
        ///     Hides/Shows the game over screen.
        /// </summary>
        /// <param name="gameOver">Whether the game is over or not.</param>
        public void ShowGameOver(bool gameOver)
        {
            if(gameOverScreen)
                gameOverScreen.SetActive(gameOver);

            if (gameOver)
            {
                var finalScore = GameManager.Instance.Score;
                Debug.Log("Final Score: " + finalScore);

                if(finalScoreText)
                    finalScoreText.text = $"Final Score: {finalScore}";
            }
        }

        #endregion
    }
}