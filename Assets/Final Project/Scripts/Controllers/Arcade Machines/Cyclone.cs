using ArcadeGame.Views;
using UnityEngine;

namespace ArcadeGame.Controllers.Machines
{
    /// <summary>
    ///     Controls the behaviour of the cyclone arcade machine.
    /// </summary>
    public class Cyclone : TokenMachineBase
    {
        [Header("Cyclone Settings")]
        [SerializeField] private LightCycler lightController;
    }
}