using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Shared.Helpers
{
    /// <summary>
    ///     Timer handles asynchronous threading.
    /// </summary>
    public static class Timer
    {
        /// <summary>
        ///     Wait for the next frame.
        /// </summary>
        /// <returns>Completed await task</returns>
        public static async Task WaitForFrame() =>
            await Task.Delay((int)(1000 * Time.deltaTime));

        /// <summary>
        ///     Wait until the required condition has been met.
        /// </summary>
        /// <param name="predicate">The condition to exit the wait loop.</param>
        /// <returns>Completed waiting task</returns>
        public static async Task WaitUntil(Func<bool> predicate)
        {
            while(!predicate())
                await WaitForFrame();
        }
    }
}