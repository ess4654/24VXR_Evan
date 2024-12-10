using Shared.Helpers;
using System.Collections;
using TMPro;
using UnityEngine;

namespace ArcadeGame.Views
{
    /// <summary>
    ///     Ping-pongs the alpha value of a text label.
    /// </summary>
    public class TextFlasher : Shared.Behaviour
    {
        #region VARIABLE DECLARATIONS

        [SerializeField] private float flashTime = 2f;

        protected TMP_Text label;
        
        private WaitForSeconds animationTime;
        private float alpha;

        #endregion

        #region SETUP

        private void Awake()
        {
            label = GetComponent<TMP_Text>();
            animationTime = new WaitForSeconds(flashTime);
            StartCoroutine(Flash());
        }

        #endregion

        #region METHODS

        /// <summary>
        ///     Animates the alpha of the text label.
        /// </summary>
        /// <returns>Flashing text routine</returns>
        IEnumerator Flash()
        {
            while (this)
            {
                //fade text
                LeanTween
                    .value(label.color.a, alpha, flashTime)
                    .setOnUpdate((float a) => label.color = label.color.ChangeAlpha(a))
                    .setEaseLinear();

                yield return animationTime;
                alpha = alpha == 1 ? 0 : 1;
            }
        }

        #endregion
    }
}