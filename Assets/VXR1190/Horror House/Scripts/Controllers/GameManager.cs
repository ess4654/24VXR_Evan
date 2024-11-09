using Shared;
using UnityEngine;

namespace HorrorHouse.Controllers
{
    /// <summary>
    ///     Manages the game.
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private GameObject[] destroyOnCurseLifted;

        #region METHODS

        #region ENGINE

        private void OnEnable()
        {
            GameEventBroadcaster.OnGameOver += CurseLifted;
        }

        private void OnDisable()
        {
            GameEventBroadcaster.OnGameOver -= CurseLifted;
        }

        #endregion

        //Destroys items on curse lifted
        private void CurseLifted()
        {
            foreach(var item in destroyOnCurseLifted)
                Destroy(item);
        }

        #endregion
    }
}