using HorrorHouse.Data;

namespace HorrorHouse.Controllers
{
    /// <summary>
    ///     Broadcasts game events that classes can subscribe to for the horror house game.
    /// </summary>
    public static class GameEventBroadcaster
    {
        #region DELEGATES

        /// <summary>
        ///     An event that passes a collected artifact type.
        /// </summary>
        /// <param name="artifactType">Type of artifact that was collected.</param>
        public delegate void CollectArtifactEvent(ArtifactType artifactType);
        
        /// <summary>
        ///     An event that triggers a void action.
        /// </summary>
        public delegate void VoidEvent();

        #endregion

        #region SUBSCRIBABLES

        /// <summary>
        ///     Called when the player collects an artifact.
        /// </summary>
        public static event CollectArtifactEvent OnArtifactCollected;
        
        /// <summary>
        ///     Called when the player lifts the curse.
        /// </summary>
        public static event VoidEvent OnGameOver;

        #endregion

        #region BROADCASTERS

        /// <summary>
        ///     Broadcasts that player has collected an artifact.
        /// </summary>
        /// <param name="artifactType">The type of artifact collected.</param>
        public static void BroadcastArtifactCollected(ArtifactType artifactType) =>
            OnArtifactCollected?.Invoke(artifactType);

        /// <summary>
        ///     Broadcasts that the curse has been lifted.
        /// </summary>
        public static void BroadcastGameOver() =>
            OnGameOver?.Invoke();

        #endregion
    }
}