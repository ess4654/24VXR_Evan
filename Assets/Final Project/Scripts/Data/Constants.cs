namespace ArcadeGame.Data
{
    /// <summary>
    ///     Global game constants for the arcade game.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        ///     Are we testing the game?
        /// </summary>
        public const bool Debugging = true;

        /// <summary>
        ///     The maximum amount of tokens that any machine can require to play.
        /// </summary>
        public const int MaxMachineTokens = 10;

        /// <summary>
        ///     The number of tokens the player will start the game with.
        /// </summary>
        public const int StartingTokens = 10;

        /// <summary>
        ///     The default key to use for saving/loading player prefs.
        /// </summary>
        public const string DefaultSaveFile = "ArcadeGameData";

        /// <summary>
        ///     The odds of hitting a jackpot. Makes the game more difficult.
        /// </summary>
        public const float JackpotOdds = .1f;

        /// <summary>
        ///     The amount of time to keep a button pressed down.
        /// </summary>
        public const float ButtonDownTime = 3f;//1.0f;
    }
}