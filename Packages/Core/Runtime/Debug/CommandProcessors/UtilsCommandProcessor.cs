using Varguiniano.Core.Runtime.Build;
using Varguiniano.Core.Runtime.Common;
using Varguiniano.Core.Runtime.Localization;
using Varguiniano.Core.Runtime.Savegames;
using Varguiniano.Core.Runtime.WebGL;

namespace Varguiniano.Core.Runtime.Debug.CommandProcessors
{
    /// <summary>
    /// Command processor for common utility commands.
    /// </summary>
    public class UtilsCommandProcessor : InGameCommandProcessor
    {
        /// <summary>
        /// Module name.
        /// </summary>
        public override string ModuleName => "Game";

        /// <summary>
        /// Reference to the game version.
        /// </summary>
        public Version Version;

        /// <summary>
        /// Process the commands.
        /// </summary>
        /// <param name="command">Command given.</param>
        /// <param name="parameters">Parameters.</param>
        // ReSharper disable once CyclomaticComplexity
        protected override bool ProcessCommand(string command, params string[] parameters)
        {
            switch (command)
            {
                case "Exit":

                    if (parameters.Length != 0)
                    {
                        WrongParamNumber(command, parameters.Length, 0);
                        return false;
                    }

                    Utils.CloseGame();
                    break;

                case "Log":

                    if (parameters.Length != 1)
                    {
                        WrongParamNumber(command, parameters.Length, 1);
                        return false;
                    }

                    Logger.LogInfo(parameters[0], ModuleName);
                    break;

                case "Save":

                {
                    if (parameters.Length != 0)
                    {
                        WrongParamNumber(command, parameters.Length, 0);
                        return false;
                    }

                    SavegameManager.Instance.SaveGame();
                    break;
                }

                case "Load":

                {
                    if (parameters.Length != 0)
                    {
                        WrongParamNumber(command, parameters.Length, 0);
                        return false;
                    }

                    SavegameManager.Instance.LoadGame();
                    break;
                }

                case "GetVersion":

                {
                    if (parameters.Length != 0)
                    {
                        WrongParamNumber(command, parameters.Length, 0);
                        return false;
                    }

                    Logger.LogInfo("Game version: " + Version.FullVersion, this);
                    break;
                }

                case "GetLanguage":
                {
                    if (parameters.Length != 0)
                    {
                        WrongParamNumber(command, parameters.Length, 0);
                        return false;
                    }

                    Logger.LogInfo("Current language: " + LocalizationManager.Instance.CurrentLanguageName, this);
                    break;
                }

                case "SetLanguage":
                {
                    if (parameters.Length != 1)
                    {
                        WrongParamNumber(command, parameters.Length, 1);
                        return false;
                    }

                    if (uint.TryParse(parameters[0], out uint newLanguage))
                        LocalizationManager.Instance.CurrentLanguage = newLanguage;
                    else
                        WrongParams(command);

                    break;
                }

                #region WebGlCommands

                #pragma warning disable 162
                // ReSharper disable HeuristicUnreachableCode

                case "DownloadLogFile":

                {
                    #if !UNITY_WEBGL || UNITY_EDITOR
                    Logger.LogError("Run this command only on web.", ModuleName);
                    return false;
                    #endif
                    if (parameters.Length != 0)
                    {
                        WrongParamNumber(command, parameters.Length, 0);
                        return false;
                    }

                    #if UNITY_WEBGL
                    WebGlUtils.DownloadFile(Logger.FilePath);
                    #endif
                    break;
                }

                case "SaveAndDownload":

                {
                    #if !UNITY_WEBGL || UNITY_EDITOR
                    Logger.LogError("Run this command only on web.", ModuleName);
                    return false;
                    #endif

                    if (parameters.Length != 0)
                    {
                        WrongParamNumber(command, parameters.Length, 0);
                        return false;
                    }

                    #if UNITY_WEBGL
                    WebGlUtils.SaveAndDownloadSaveGame();
                    #endif
                    break;
                }

                case "UploadSave":

                {
                    #if !UNITY_WEBGL || UNITY_EDITOR
                    Logger.LogError("Run this command only on web.", ModuleName);
                    return false;
                    #endif
                    if (parameters.Length != 0)
                    {
                        WrongParamNumber(command, parameters.Length, 0);
                        return false;
                    }

                    #if UNITY_WEBGL
                    WebGlUtils.UploadAndLoadSavegame();
                    #endif
                    break;
                }

                // ReSharper enable HeuristicUnreachableCode
                #pragma warning restore 162

                #endregion

                default: return false;
            }

            return true;
        }
    }
}