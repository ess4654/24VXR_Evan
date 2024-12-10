using UnityEngine;
using UnityEngine.UI;

namespace ArcadeGame.Views
{
    /// <summary>
    ///     Randomizes the look of the grass for the duck game.
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class GrassRandomizer : Shared.Behaviour
    {
        [SerializeField] bool alwaysModifyColor;

        private const float heightVariation = .5f;
        private const float widthVariation = .3f;
        private const float colorVariation = .4f;

        private void Awake()
        {
            transform.localScale = Vector3.one + new Vector3(Random.value * widthVariation * (Random.value < .5f ? 1 : -1), Random.value * heightVariation * (Random.value < .5f ? 1 : -1), 0);
            if(Random.value < .5f || alwaysModifyColor) //50% chance to not modify the color
                GetComponent<Image>().color = GetComponent<Image>().color - (Random.value * colorVariation * Color.white);
            Destroy(this);
        }
    }
}