using Shared.Helpers;
using System.Collections;
using UnityEngine;

namespace ArcadeGame.Controllers
{
    /// <summary>
    ///     Manages the randomly spawning arcade cups.
    /// </summary>
    [RequireComponent(typeof(BoxCollider), typeof(MeshRenderer))]
    public class TokenCup : Shared.Behaviour
    {
        public const string Filename = "Token Cup";

        #region VARIABLE DECLARATIONS

        [SerializeField, Range(0f, 1f)] private float spawnChance = .5f;
        [SerializeField] private Vector2 spawnTime = Vector2.one;
        [SerializeField] private Material[] cupMaterials;
        [SerializeField] private Vector2Int randomTokenRange;

        /// <summary>
        ///     Has the cup been spawned into the world?
        /// </summary>
        private bool isSpawned => cup.enabled;
        private MeshRenderer cup;
        private BoxCollider region;

        #endregion

        #region SETUP

        private void Awake()
        {
            region = GetComponent<BoxCollider>();
            region.enabled = false;

            cup = GetComponent<MeshRenderer>();
            cup.enabled = false;
            
            Respawn();
        }

        /// <summary>
        ///     Re-spawns the cup when it has been collected or on Awake.
        /// </summary>
        private void Respawn() =>
            StartCoroutine(Spawn(Random.Range(spawnTime.x, spawnTime.y)));
        
        /// <summary>
        ///     Wait for a given amount of time before spawning a cup.
        /// </summary>
        /// <param name="spawnTime">The amount of time to wait before spawning a cup.</param>
        /// <returns>Coroutine</returns>
        private IEnumerator Spawn(float spawnTime)
        {
            //wait for spawn time
            yield return new WaitForSeconds(spawnTime);
            
            //random chance the cup will not spawn
            if (Random.value <= spawnChance)
            {
                if (cupMaterials.Length > 0)
                    cup.material = cupMaterials.SelectRandom();
                cup.enabled = true;
            }
            else //re run routine if not spawned
                Respawn();
        }

        #endregion

        /// <summary>
        ///     Collect the token cup and award player with tokens.
        /// </summary>
        public void Collect()
        {
            cup.enabled = false;
            region.enabled = false;
            var tokensCollected = Random.Range(randomTokenRange.x, randomTokenRange.y + 1);
            GameEventBroadcaster.BroadcastTokensCollected(tokensCollected);
            Respawn(); //restart the spawn routine.
        }
    }
}