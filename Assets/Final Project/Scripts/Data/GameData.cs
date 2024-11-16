using System.Collections.Generic;

namespace ArcadeGame.Data
{
    /// <summary>
    ///     Global game data for the arcade game.
    /// </summary>
    public static class GameData
    {
        #region VARIABLE DECLARATIONS

        /// <summary>
        ///     Current state of the game.
        /// </summary>
        public static GameState State;

        /// <summary>
        ///     The number of tokens we currently have.
        /// </summary>
        public static int Tokens { get; private set; } = Constants.StartingTokens;

        /// <summary>
        ///     The number of tickets we currently have.
        /// </summary>
        public static int Tickets { get; private set; }

        /// <summary>
        ///     Prizes from the claw machine in our inventory.
        /// </summary>
        public static Dictionary<string, int> Prizes { get; private set; } = new Dictionary<string, int>();

        /// <summary>
        ///     Prize data in string format.
        /// </summary>
        public static string PrizeData
        {
            get
            {
                var prizes = "";

                foreach (var prize in Prizes)
                    prizes += $"{prize.Key}:{prize.Value},";

                return prizes;
            }
        }

        #endregion

        #region METHODS

        /// <summary>
        ///     Loads the save data into the global game data.
        /// </summary>
        /// <param name="saveData">Save data to load.</param>
        public static void LoadData(SaveData saveData)
        {
            if (saveData == null)
                throw new System.Exception("Unable to load null save data.");

            Tokens = saveData.Tokens;
            Tickets = saveData.Tickets;

            if (!string.IsNullOrWhiteSpace(saveData.Prizes))
            {
                Prizes.Clear(); //remove pre existing prize data

                foreach (var prize in saveData.Prizes.Split(","))
                {
                    //get the parsed prize data
                    var prizeData = prize.Split(":");
                    var key = prizeData[0];
                    var amount = int.TryParse(prizeData[1], out int count) ? count : 0;

                    Prizes.Add(key, amount); //add the prize to the data
                }
            }
        }

        /// <summary>
        ///     Resets the game data and exports a new save.
        /// </summary>
        public static void ResetGameData()
        {
            //reset values
            State = GameState.Arcade;
            Tokens = Constants.StartingTokens;
            Tickets = 0;
            Prizes.Clear();

            //export the save
            new SaveData().ExportSave(Constants.DefaultSaveFile); 
        }

        #endregion
    }
}