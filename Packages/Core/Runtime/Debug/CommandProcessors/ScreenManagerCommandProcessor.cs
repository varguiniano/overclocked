using UnityEngine;
using Varguiniano.Core.Runtime.Screen;

namespace Varguiniano.Core.Runtime.Debug.CommandProcessors
{
    /// <summary>
    /// Command processor for ScreenManager related methods.
    /// </summary>
    public class ScreenManagerCommandProcessor : InGameCommandProcessor
    {
        /// <summary>
        /// Module name for this processor.
        /// </summary>
        public override string ModuleName => "ScreenManager";

        /// <summary>
        /// Process the command.
        /// </summary>
        /// <param name="command">Command to process.</param>
        /// <param name="parameters">Command parameters.</param>
        protected override bool ProcessCommand(string command, params string[] parameters)
        {
            switch (command)
            {
                case "SetRes":

                    if (parameters.Length != 3)
                    {
                        WrongParamNumber(command, parameters.Length, 3);
                        return false;
                    }

                    if (!int.TryParse(parameters[0], out int width))
                    {
                        WrongParams(command);
                        return false;
                    }

                    if (!int.TryParse(parameters[1], out int height))
                    {
                        WrongParams(command);
                        return false;
                    }

                    FullScreenMode fullScreenMode;

                    switch (parameters[2])
                    {
                        case "W":
                            fullScreenMode = FullScreenMode.Windowed;
                            break;
                        case "MW":
                            fullScreenMode = FullScreenMode.MaximizedWindow;
                            break;
                        case "F":
                            fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                            break;
                        case "BW":
                            fullScreenMode = FullScreenMode.FullScreenWindow;
                            break;
                        default:
                            WrongParams(command);
                            return false;
                    }

                    SetRes(width, height, fullScreenMode);
                    break;
                case "ResetRes":

                    if (parameters.Length != 0)
                    {
                        Logger.LogInfo(parameters[0], this);
                        WrongParamNumber(command, parameters.Length, 0);
                        return false;
                    }

                    ResetRes();
                    break;
                default: return false;
            }

            return true;
        }

        /// <summary>
        /// Sets the screen resolution based on the given parameters.
        /// </summary>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        /// <param name="fullScreenMode">Mode.</param>
        private static void SetRes(int width, int height, FullScreenMode fullScreenMode) =>
            ScreenManager.Instance.UpdateScreenResolution(width, height, fullScreenMode);

        /// <summary>
        /// Resets the screen resolution to the file values.
        /// </summary>
        private static void ResetRes() => ScreenManager.Instance.UpdateScreenResolution();
    }
}