using ArcadeGame.Controllers;
using ArcadeGame.Data;

namespace ArcadeGame.Views
{
    /// <summary>
    ///     Flashes the text on the screen for the duck slayer game.
    /// </summary>
    public class DuckGameTitle : TextFlasher
    {
        private void OnEnable()
        {
            GameEventBroadcaster.OnGameStateChanged += HandleStateChange;
        }

        private void OnDisable()
        {
            GameEventBroadcaster.OnGameStateChanged -= HandleStateChange;
        }

        private void HandleStateChange(GameState state)
        {
            //hide the title text when the shooter game starts
            //label.enabled = state != GameState.ShooterCabinet;
        }
    }
}