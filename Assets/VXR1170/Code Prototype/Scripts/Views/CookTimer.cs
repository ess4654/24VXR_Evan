using FryingPanGame.Controllers;
using FryingPanGame.Data;
using FryingPanGame.Helpers;
using Shared;
using UnityEngine;
using UnityEngine.UI;

namespace FryingPanGame.Views
{
    /// <summary>
    ///     Manages the cooking timer display.
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class CookTimer : Singleton<CookTimer>
    {
        #region VARIABLE DECLARATIONS

        /// <summary>
        ///     Is the timer currently cooking the recipe?
        /// </summary>
        public static bool IsCooking => Instance.cooking;
        [SerializeField] private bool cooking;

        [Header("Audio")]
        [SerializeField] private AudioClip cookingFinished;

        private Image timer;
        private float fill;
        private float time;

        #endregion

        #region METHODS

        #region ENGINE

        protected override void Awake()
        {
            base.Awake();
            timer = GetComponent<Image>();
        }

        private void OnEnable()
        {
            GameEventBroadcaster.OnNewRecipe += ResetTimer;
        }
        

        private void OnDisable()
        {
            GameEventBroadcaster.OnNewRecipe -= ResetTimer;
        }

        private void Update()
        {
            if (cooking && timer != null)
            {
                fill += Time.deltaTime;
                timer.fillAmount = Mathf.Clamp01(fill / time);
                
                if(fill >= time)
                {
                    SoundManager.Instance.StopSound();
                    timer.fillAmount = 0;
                    cooking = false;
                    if(GameManager.Instance.GameOn)
                        SoundManager.Instance.PlayClip(cookingFinished);
                }
            }
        }

        #endregion

        /// <summary>
        ///     Resets the cooking timer.
        /// </summary>
        private void ResetTimer(Recipe _)
        {
            time = 0;
            fill = 0;
            cooking = false;
            if(timer != null)
                timer.fillAmount = 0;
        }

        /// <summary>
        ///     Start the visual cook timer.
        /// </summary>
        /// <param name="cookTime">The amount of time to cook this recipe.</param>
        public void RunTimer(int cookTime)
        {
            time = cookTime;
            fill = 0;
            cooking = true;
        }

        #endregion
    }
}