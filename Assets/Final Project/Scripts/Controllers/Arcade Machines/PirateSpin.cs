using ArcadeGame.Views.Machines;
using Shared.Helpers;
using UnityEngine;

namespace ArcadeGame.Controllers.Machines
{
    /// <summary>
    ///     Controls the behaviour of the pirate spin arcade machine.
    /// </summary>
    public class PirateSpin : TicketMachineBase
    {
        public const string Filename = "Pirate Spin";

        #region VARIABLE DECLARATIONS

        private new PirateSpinAnimator animator => base.animator as PirateSpinAnimator;

        #endregion

        protected override void OnGameActive()
        {
            //UpdateSpinRate();
        }
    }
}