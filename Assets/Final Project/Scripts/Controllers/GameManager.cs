using ArcadeGame.Data;
using Shared;

namespace ArcadeGame.Controllers
{
    /// <summary>
    ///     Global game manager for the arcade game.
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {
        protected override bool DontDestroy => true;

        private void Start()
        {
            GameData.State = GameState.Arcade;
        }
    }
}