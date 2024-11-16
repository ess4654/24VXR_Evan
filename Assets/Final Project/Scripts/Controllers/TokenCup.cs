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
        #region VARIABLE DECLARATIONS

        [SerializeField, Range(0f, 1f)] private float spawnChance = .5f;
        [SerializeField] private Vector2 spawnTime = Vector2.one;
        [SerializeField] private Material[] cupMaterials;

        /// <summary>
        ///     Has the cup been spawned into the world?
        /// </summary>
        private bool isSpawned => cup.enabled;
        private MeshRenderer cup;

        #endregion

        #region SETUP

        private void Awake()
        {
            GetComponent<BoxCollider>().isTrigger = true;
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
    }
}