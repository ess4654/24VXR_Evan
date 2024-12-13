using ArcadeGame.Data;
using Shared;

namespace ArcadeGame.Controllers
{
    /// <summary>
    ///     Global game manager for the arcade game.
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {
        public const string Filename = "Game Manager";
        protected override bool DontDestroy => true;

        #region VARIABLE DECLARATIONS

        #endregion

        #region SETUP

        private void Start()
        {
            GameData.State = GameState.ClawMachine;
            GameData.LoadData();
        }

        private void OnEnable()
        {
            GameEventBroadcaster.OnTokensCollected += HandleTokensCollected;
            GameEventBroadcaster.OnTicketsWon += HandleTicketsWon;
        }

        private void OnDisable()
        {
            GameEventBroadcaster.OnTokensCollected -= HandleTokensCollected;
            GameEventBroadcaster.OnTicketsWon -= HandleTicketsWon;
        }

        #endregion

        #region METHODS

        /// <summary>
        ///     Handles the event broadcast by token cups when tokens are collected.
        /// </summary>
        /// <param name="amount">The amount of tokens collected.</param>
        private void HandleTokensCollected(int amount)
        {
            GameData.AddTokens(amount);
        }

        /// <summary>
        ///     Handles the event broadcast by ticket machines when tickets are won.
        /// </summary>
        /// <param name="amount">Amount of tickets won.</param>
        private void HandleTicketsWon(int amount)
        {
            GameData.AddTickets(amount);
        }

        #endregion
    }
}