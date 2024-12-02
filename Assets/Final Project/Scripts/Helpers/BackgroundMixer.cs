using ArcadeGame.Controllers;
using ArcadeGame.Data;
using Assets.Final_Project.Scripts.Controllers;
using Shared;
using Shared.Editor;
using UnityEngine;
using UnityEngine.Audio;

namespace ArcadeGame.Helpers
{
    /// <summary>
    ///     Controls the audio mixer for the background music of the arcade game.
    /// </summary>
    public class BackgroundMixer : Singleton<BackgroundMixer>
    {
        #region VARIABLE DECLARATIONS

        [SerializeField] private AudioMixerSnapshot[] snapshots;
        [SerializeField] private float transitionTime = 1f;

        private AudioMixerSnapshot currentSnapshot;
        private const string PitchParam = "Music Pitch";
        private const float PitchNormal = 1f;
        private const float PitchPaused = .97f;

        #endregion

        #region SETUP

        protected override void Awake()
        {
            base.Awake();
            currentSnapshot = snapshots[0];
        }

        private void OnEnable()
        {
            GameEventBroadcaster.OnGameStateChanged += SwitchTrack;
        }

        private void OnDisable()
        {
            GameEventBroadcaster.OnGameStateChanged -= SwitchTrack;
        }

        #endregion

        #region METHODS

        /// <summary>
        ///     Transitions to the new audio mixer snapshot.
        /// </summary>
        /// <param name="snapshot">Snapshot to transition to.</param>
        /// <param name="pitch">Pitch of the music tracks.</param>
        private void TransitionTo(AudioMixerSnapshot snapshot, float pitch)
        {
            if(snapshot != null)
            {
                snapshot.TransitionTo(transitionTime);
                snapshot.audioMixer.GetFloat(PitchParam, out float currentPitch);
                LeanTween
                    .value(currentPitch, pitch, transitionTime)
                    .setOnUpdate((float _pitch) => snapshot.audioMixer.SetFloat(PitchParam, _pitch))
                    .setEaseLinear();
            }
        }

        /// <summary>
        ///     Swaps the audio track when the game state has been changed.
        /// </summary>
        /// <param name="gameState">New Game State</param>
        private void SwitchTrack(GameState gameState)
        {
            // Arcade = 0
            // Cyclone = 1 & 6
            // BigWheel = 2
            // PirateSpin = 3
            // ShooterCabinet = 4
            // ClawMachine = 5

            int index = (int)gameState;
            float pitch = PitchNormal;
            if (index > -1)
                currentSnapshot = snapshots[gameState == GameState.Cyclone ? (InteractionArea.PlayerRegion == "Cyclone 1" ? 1 : 6) : index];
            else
                pitch = PitchPaused; //lower the pitch when paused
            
            TransitionTo(currentSnapshot, pitch);
        }

        #endregion

        #region DEBUGGING

        [Debugging]
        [SerializeField] GameState gameTrack;
        [SerializeField, InspectorButton("TestTransition")] bool m_TestTransition;
        void TestTransition() => SwitchTrack(gameTrack);

        #endregion
    }
}