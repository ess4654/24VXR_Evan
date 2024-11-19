using System;
using UnityEngine;

namespace ArcadeGame.Data
{
    /// <summary>
    ///     Container of save data used by the game data manager.
    /// </summary>
    [Serializable]
    public class SaveData
    {
        #region VARIABLE DECLARATIONS

        /// <summary>
        ///     The number of tokens held by the player.
        /// </summary>
        public int Tokens => tokens;
        [SerializeField] private int tokens;
        
        /// <summary>
        ///     The number of tickets held by the player.
        /// </summary>
        public int Tickets => tickets;
        [SerializeField] private int tickets;

        /// <summary>
        ///     The prizes in our inventory.
        /// </summary>
        public string Prizes => prizes;
        [SerializeField] private string prizes;

        #endregion

        #region CONSTRUCTORS

        /// <summary>
        ///     Construct a container of save data taking current values from the global Game Data.
        /// </summary>
        public SaveData()
        {
            tokens = GameData.Tokens;
            tickets = GameData.Tickets;
            prizes = GameData.PrizeData;
        }

        /// <summary>
        ///     Constructs a container of save data loading values from the system.
        /// </summary>
        /// <param name="saveKey">Key to load the save data from.</param>
        public SaveData(string saveKey)
        {
            //load the data from player prefs
            var data = PlayerPrefs.GetString(saveKey, string.Empty);
            if (string.IsNullOrWhiteSpace(data))
                throw new Exception("Data not loaded successfully.");

            //set the values locally
            try
            {
                var loadedData = JsonUtility.FromJson<SaveData>(data);
                tokens = loadedData.tokens;
                tickets = loadedData.tickets;
                prizes = loadedData.prizes;
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        #endregion

        #region METHODS

        /// <summary>
        ///     Updates the game data using the local values from this save data container.
        /// </summary>
        /// <returns>Reference to the save data</returns>
        public SaveData UpdateGameData()
        {
            //set the values of the global game data
            GameData.LoadData(this);

            return this;
        }

        /// <summary>
        ///     Exports this save data to the player prefs.
        /// </summary>
        public void ExportSave(string saveKey)
        {
            PlayerPrefs.GetString(saveKey, ToString());
            PlayerPrefs.Save(); //commit changes
        }

        public override string ToString() => JsonUtility.ToJson(this, Constants.Debugging);

        #endregion
    }
}