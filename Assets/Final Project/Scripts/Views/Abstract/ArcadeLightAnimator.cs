using Shared.Editor;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ArcadeGame.Views
{
    /// <summary>
    ///     Base class use by all light animators for arcade machines.
    /// </summary>
    public abstract class ArcadeLightAnimator : Shared.Behaviour
    {
        #region VARIABLE DECLARATIONS

        [Header("Materials")]
        [SerializeField, DependsUpon("!useShaderVariableInstead")] protected Material lightOffMaterial;
        [SerializeField, DependsUpon("!useShaderVariableInstead")] protected Material lightOnMaterial;
        
        [Header("Shader")]
        [SerializeField] protected bool useShaderVariableInstead;
        [SerializeField, DependsUpon("useShaderVariableInstead")] protected string variableName;
        [SerializeField, DependsUpon("useShaderVariableInstead")] protected float variableOffValue = 0f;
        [SerializeField, DependsUpon("useShaderVariableInstead")] protected float variableOnValue = 1f;
        
        [SerializeField] protected MeshRenderer[] lights;

        private Action<Renderer, bool> swapLightFunction;
        private Dictionary<Renderer, MaterialPropertyBlock> lightPropertyBlocks;

        #endregion

        #region SETUP

        protected virtual void Awake()
        {
            //setup for swapping shader variable
            if(useShaderVariableInstead)
            {
                lightPropertyBlocks = new Dictionary<Renderer, MaterialPropertyBlock>();
                foreach (var light in lights)
                {
                    if(light != null)
                    {
                        var shader = new MaterialPropertyBlock();
                        light.GetPropertyBlock(shader);
                        lightPropertyBlocks.Add(light, shader);
                        SwapLightShader(light, false); //disable light at start
                    }
                }

                swapLightFunction = SwapLightShader;
            }
            //setup for swapping mesh material
            else
            {
                swapLightFunction = SwapLightMaterial;
            }
        }

        #endregion

        #region METHODS

        /// <summary>
        ///     Swaps between on/off materials.
        /// </summary>
        /// <param name="light">Reference to the light renderer.</param>
        /// <param name="on">Is the light being turned on/off?</param>
        private void SwapLightMaterial(Renderer light, bool on) =>
            light.material = on ? lightOnMaterial : lightOffMaterial;

        /// <summary>
        ///     Swaps between on/off values of a shader property.
        /// </summary>
        /// <param name="light">Reference to the light renderer.</param>
        /// <param name="on">Is the light being turned on/off?</param>
        private void SwapLightShader(Renderer light, bool on)
        {
            if (light != null && lightPropertyBlocks != null && lightPropertyBlocks.ContainsKey(light))
            {
                var shader = lightPropertyBlocks[light];
                shader.SetFloat(variableName, on ? variableOnValue : variableOffValue);
                light.SetPropertyBlock(shader);
            }
        }

        /// <summary>
        ///     Turn the light at the given index on/off.
        /// </summary>
        /// <param name="index">Index of the light to modify.</param>
        /// <param name="on">Whether the light is on or off.</param>
        protected void UpdateLight(int index, bool on)
        {
            var light = lights[index];
            swapLightFunction?.Invoke(light, on);

            if (light.GetComponentInChildren<Light>())
                light.GetComponentInChildren<Light>().enabled = on;
        }

        /// <summary>
        ///     Turn all lights on/off.
        /// </summary>
        /// <param name="on">Whether the lights are on or off.</param>
        protected void UpdateAllLights(bool on)
        {
            for(var i = 0; i < lights.Length; i++)
                UpdateLight(i, on);
        }

        #endregion
    }
}