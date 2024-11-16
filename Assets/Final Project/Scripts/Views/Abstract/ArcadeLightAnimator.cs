using UnityEngine;

namespace ArcadeGame.Views
{
    /// <summary>
    ///     Base class use by all light animators for arcade machines.
    /// </summary>
    public abstract class ArcadeLightAnimator : Shared.Behaviour
    {
        [SerializeField] protected Material lightOffMaterial;
        [SerializeField] protected Material lightOnMaterial;
        [SerializeField] protected MeshRenderer[] lights;

        /// <summary>
        ///     Turn the light at the given index on/off.
        /// </summary>
        /// <param name="index">Index of the light to modify.</param>
        /// <param name="on">Whether the light is on or off.</param>
        protected void UpdateLight(int index, bool on)
        {
            var light = lights[index];
            light.material = on ? lightOnMaterial : lightOffMaterial;

            if (light.GetComponentInChildren<Light>())
                light.GetComponentInChildren<Light>().enabled = on;
        }
    }
}