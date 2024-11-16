using Shared;
using UnityEngine;

namespace ArcadeGame.Data
{
    /// <summary>
    ///     Reads the static game data into the inspector during Update.
    /// </summary>
    public class GameDataReader : Singleton<GameDataReader>
    {
        protected override bool DontDestroy => true;

        [SerializeField] private GameState gameState;
        [SerializeField] private int tokens;
        [SerializeField] private int tickets;
        [SerializeField, TextArea(5, 10)] private string prizes;

        protected override void Awake()
        {
            //destroy this object if we are not debugging
            if(!Constants.Debugging)
            {
                Destroy();
                return;
            }

            base.Awake();
        }

        private void Update()
        {
            gameState = GameData.State;
            tokens = GameData.Tokens;
            tickets = GameData.Tickets;
            prizes = GameData.PrizeData;
        }

        /// <summary>
        ///     Resets the game data.
        /// </summary>
        [ContextMenu("Reset Game Data")]
        public void ResetGameData() =>
            GameData.ResetGameData();
    }
}