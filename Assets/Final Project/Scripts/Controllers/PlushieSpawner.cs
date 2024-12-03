using Shared.Editor;
using Shared.Helpers;
using Shared.Helpers.Extensions;
using System.Collections.Generic;
using UnityEngine;

namespace ArcadeGame.Controllers
{
    /// <summary>
    ///     Used by the claw machine to spawn all plushies into the interior.
    /// </summary>
    public class PlushieSpawner : Shared.Behaviour
    {
        [SerializeField] private Rigidbody[] plushies;

        private List<Transform> spawnPoints;
        private const float spawnChance = 0.75f;

        private void Awake()
        {
            //cache the spawn points for reference
            spawnPoints ??= new List<Transform>();
            foreach (Transform child in transform)
            {
                child.RemoveComponent<Collider>();
                child.RemoveComponent<MeshFilter>();
                child.RemoveComponent<MeshRenderer>();
                child.localScale = Vector3.one;
                spawnPoints.Add(child);
            }

            SpawnPlushies();
        }

        /// <summary>
        ///     Spawns plushies into the claw machine.
        /// </summary>
        public void SpawnPlushies()
        {
            if (plushies.Length == 0) return; //no plushies to spawn

            foreach(var spawnLocation in spawnPoints ?? new List<Transform>())
            {
                //clear previously spawned plushies
                for (int i = spawnLocation.childCount - 1; i >= 0; i--)
                {
                    var child = spawnLocation.GetChild(i);
                    Destroy(child.gameObject);
                }
                
                //randomly spawn plushies throughout the matrix
                if(Random.value < spawnChance)
                {
                    var randomRotation = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
                    Instantiate(plushies.SelectRandom(), spawnLocation.position, Quaternion.Euler(randomRotation), spawnLocation);
                }
            }
        }

        #region DEBUGGING
        [Debugging]
        [SerializeField, InspectorButton("SpawnPlushies")] bool m_SpawnPlushies;
        #endregion
    }
}