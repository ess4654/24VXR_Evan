using ArcadeGame.Controllers.Machines;
using ArcadeGame.Helpers.Audio;
using Assets.Final_Project.Scripts.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace ArcadeGame.Controllers
{
    /// <summary>
    ///     Manages button input events.
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class ButtonBehaviour : MonoBehaviour
    {
        [SerializeField] private TokenMachineBase machine;
        [SerializeField] private InteractionArea interactionRegion;

        /// <summary>
        ///     Called by the event system. Engages the machine to start the game.
        /// </summary>
        public void StartGame()
        {
            SoundManager.PlayAudioClip("button-press");
            if(interactionRegion != null)
                interactionRegion.MoveToInteraction();
        }

        /// <summary>
        ///     Called by the event system. Dis-Engages the machine to exit the game.
        /// </summary>
        public void LeaveGame()
        {
            SoundManager.PlayAudioClip("button-press");
            if (machine != null)
                machine.Leave();
        }
    }
}