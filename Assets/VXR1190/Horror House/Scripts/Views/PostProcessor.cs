using HorrorHouse.Controllers;
using UnityEngine;
using UnityEngine.Rendering;

namespace HorrorHouse.Views
{
    /// <summary>
    ///     Manages post processing for the horror house.
    /// </summary>
    [RequireComponent(typeof(Volume))]
    public class PostProcessor : MonoBehaviour
    {
        private Volume volume;

        #region METHODS

        private void Awake()
        {
            volume = GetComponent<Volume>();
        }

        private void OnEnable()
        {
            GameEventBroadcaster.OnGameOver += GameOver;
        }

        private void OnDisable()
        {
            GameEventBroadcaster.OnGameOver -= GameOver;
        }

        private void GameOver()
        {
            volume.weight = 0f;
        }

        #endregion
    }
}