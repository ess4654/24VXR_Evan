namespace FryingPanGame.Core
{
    /// <summary>
    ///     Custom exception handles null cameras.
    /// </summary>
    public class CameraException : System.Exception
    {
        public CameraException(string cameraName) : base($"Unable to move '{cameraName}' camera as it's reference is null.")
        {

        }
    }
}