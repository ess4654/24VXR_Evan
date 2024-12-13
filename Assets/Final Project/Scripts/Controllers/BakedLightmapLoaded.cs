using UnityEngine;

namespace ArcadeGame.Views
{
    /// <summary>
    ///     Loading the lightmaps dynamically at runtime because
    ///     
    ///     *in the voice of Miranda*: Unity
    /// </summary>
    public class BakedLightmapLoaded : MonoBehaviour
    {
        [SerializeField] private GameObject lightMaps;

        private void Start()
        {
            Instantiate(lightMaps);
        }
    }
}