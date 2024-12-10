using ArcadeGame.Data;
using Shared.Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace ArcadeGame.Controllers.Games
{
    /// <summary>
    ///     Controls the behaviour of the ducks for the shooter game.
    /// </summary>
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CircleCollider2D))]
    public class Duck : Shared.Behaviour
    {
        #region VARIABLE DECLARATIONS

        [SerializeField] private float flyForce = 10f;
        [SerializeField] private float flyAngularVeclocity = 1f;
        
        private RectTransform rectTransform;
        private Rigidbody2D body;
        private const float screenPadding = 10;
        private const float minimumForce = 3f;
        private const float maximumForce = 10f;

        #endregion

        #region SETUP

        protected async void Awake()
        {
            flyForce = Mathf.Max(flyForce * Random.value, minimumForce);
            flyForce = Mathf.Min(flyForce, maximumForce);
            rectTransform = GetComponent<RectTransform>();
            body = GetComponent<Rigidbody2D>();

            await Timer.WaitForSeconds(1); //wait for anchored position to be set before applying force of flight
            Fly();
        }

        #endregion

        #region METHODS

        /// <summary>
        ///     Applies motion to the duck based on the position it was spawned.
        /// </summary>
        private void Fly()
        {
            var force = Vector2.zero;
            bool positiveAngular = Random.value < 0.5f;
            var angularVelocity = flyAngularVeclocity * Random.value;

            if (rectTransform.anchoredPosition.y > screenPadding) //Top Boundary
                force = flyForce * -rectTransform.up + (angularVelocity * (positiveAngular ? Vector3.right : Vector3.left));
            else if (rectTransform.anchoredPosition.y < -screenPadding) //Bottom Boundary
                force = flyForce * rectTransform.up + (angularVelocity * (positiveAngular ? Vector3.right : Vector3.left));
            else if (rectTransform.anchoredPosition.x > screenPadding) //Right Boundary
                force = flyForce * -rectTransform.right + (angularVelocity * (positiveAngular ? Vector3.up : Vector3.down));
            else if (rectTransform.anchoredPosition.x < -screenPadding) //Left Boundary
                force = (flyForce * rectTransform.right) + (angularVelocity * (positiveAngular ? Vector3.up : Vector3.down));

            body.AddForceAtPosition(force, rectTransform.anchoredPosition, ForceMode2D.Impulse);
        }

        #region COLLISIONS

        /// <summary>
        ///     Checks for the collision of the duck with the bullet.
        /// </summary>
        /// <param name="collision">Data for the collider that was hit.</param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag(Constants.TAG_BULLET))
            {
                Destroy(collision.gameObject); //remove bullet from screen
                Die(true);
            }
        }

        /// <summary>
        ///     Check for leaving the screen.
        /// </summary>
        /// <param name="collision">Did we exit the trigger collider of the screen.</param>
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag(Constants.TAG_CABINET_SCREEN))
                Die(false); //no ticket awarded
        }

        #endregion

        /// <summary>
        ///     Kill the duck and award 1 point if the duck was killed from a bullet.
        /// </summary>
        private void Die(bool awardTicket)
        {
            if(awardTicket)
            {
                Debug.Log("Duck Killed!");
                //TODO
            }

            Destroy();
        }

        #endregion
    }
}