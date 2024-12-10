using Shared.Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace ArcadeGame.Controllers.Games
{
    /// <summary>
    ///     Controls the behaviour of the bullet shots.
    /// </summary>
    [RequireComponent(typeof(CircleCollider2D), typeof(Image))]
    public class BulletShot : Shared.Behaviour
    {
        [SerializeField] private float decayTime = 1f;

        protected async void Awake()
        {
            //only keep the bullet on the screen for a short period of time.
            await Timer.WaitForSeconds(decayTime);
            if (this == null) return;

            Destroy(gameObject);
        }
    }
}