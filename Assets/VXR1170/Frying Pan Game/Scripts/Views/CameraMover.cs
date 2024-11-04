using FryingPanGame.Core;
using FryingPanGame.Data;
using Shared.Helpers;
using Shared;
using System.Threading.Tasks;
using UnityEngine;

namespace FryingPanGame.Views
{
    /// <summary>
    ///     Moves the camera's used to generate the UI display for a recipe.
    /// </summary>
    public class CameraMover : Singleton<CameraMover>
    {
        [SerializeField] private Camera doughCam;
        [SerializeField] private Camera glazeCam;
        [SerializeField] private Camera sprinklesCam;

        /// <summary>
        ///     Moves the given ingredient camera to the zPosition of the ingredient specified.
        /// </summary>
        /// <param name="category">Category of the camera to move.</param>
        /// <param name="zPosition">ZPosition of the ingredient to position the camera above.</param>
        /// <returns>Completed move camera task.</returns>
        /// <exception cref="CameraExcepition">If the camera is not referenced in the inspector</exception>
        public async Task MoveCamera(IngredientType category, float zPosition)
        {
            //Get a reference to the camera to be moved
            Transform camTransform;
            switch (category)
            {
                case IngredientType.Dough:
                    if(doughCam == null)
                        throw new CameraExcepition("Dough");

                    camTransform = doughCam.transform;
                    break;
             
                case IngredientType.Glaze:
                    if(glazeCam == null)
                        throw new CameraExcepition("Glaze");

                    camTransform = glazeCam.transform;
                    break;

                case IngredientType.Sprinkles:
                    if(sprinklesCam == null)
                        throw new CameraExcepition("Sprinkles");

                    camTransform = sprinklesCam.transform;
                    break;

                default:
                    throw new CameraExcepition("Undefined");
            }

            //move the camera and take a screenshot
            var currentPosition = camTransform.position;
            currentPosition.z = zPosition;
            camTransform.gameObject.SetActive(false);
            camTransform.position = currentPosition;
            camTransform.gameObject.SetActive(true);
            await Timer.WaitForFrame();
            camTransform.gameObject.SetActive(false);
        }
    }
}