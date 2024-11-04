namespace FryingPanGame.Core
{
    /// <summary>
    ///     Custom exception handles null cameras.
    /// </summary>
    public class CameraExcepition : System.Exception
    {
        public CameraExcepition(string cameraName) : base($"Unable to move '{cameraName}' camera as it's reference is null.")
        {

        }
    }
}