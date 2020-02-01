using Sinbad.UnityRecyclingListView;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Varguiniano.Core.Runtime.Common;
using Varguiniano.Core.Runtime.Configuration;
using Varguiniano.Core.Runtime.Debug.CommandProcessors;

namespace Varguiniano.Core.Runtime.Debug
{
    /// <summary>
    /// Class that handles an in game console to view logs in game.
    /// It also allows for type custom console commands for debugging purposes.
    /// </summary>
    public class InGameConsole : Singleton<InGameConsole>
    {
        /// <summary>
        /// Command processors for this game.
        /// </summary>
        public InGameCommandProcessor[] CommandProcessors;

        /// <summary>
        /// Reference to the list view of the buttons.
        /// </summary>
        public RecyclingListView ListView;

        /// <summary>
        /// Reference to the scrollview.
        /// </summary>
        public ScrollRect ScrollView;

        /// <summary>
        /// Reference to the console input.
        /// </summary>
        public TMP_InputField ConsoleInput;

        /// <summary>
        /// Reference to the canvas.
        /// </summary>
        public GameObject Canvas;

        /// <summary>
        /// Used to show or hide the console.
        /// </summary>
        public bool Show
        {
            get => show;

            set
            {
                if (config == null)
                {
                    Logger.LogInfo("In game console waiting for configuration manager...", this);
                    return;
                }

                if (!config.InGameConsole)
                {
                    Logger.LogWarning("Tried to open in game console but it's disabled on the config file, not opening.",
                                      this);

                    return;
                }

                Canvas.SetActive(value);

                ActivateInputField();
                show = value;
            }
        }

        /// <summary>
        /// Backfield for Show.
        /// </summary>
        private bool show;

        /// <summary>
        /// Reference to the debug configuration.
        /// </summary>
        private DebugConfig config;

        /// <summary>
        /// Subscribe to input.
        /// </summary>
        private void OnEnable()
        {
            ListView.ItemCallback = PopulateItem;
            ConfigurationManager.OnServiceReady += service => config = service.GetConfig<DebugConfig>();
            ConsoleInput.onEndEdit.AddListener(InputReceived);
        }

        /// <summary>
        /// Unsubscribe from input.
        /// </summary>
        private void OnDisable() => ConsoleInput.onEndEdit.RemoveListener(InputReceived);

        /// <summary>
        /// Refresh log messages.
        /// </summary>
        private void Update()
        {
            if (config == null)
            {
                Logger.LogInfo("In game console waiting for configuration manager...", this);
                return;
            }

            if (!Show) return;

            if (ListView.RowCount == Logger.LogMessages.Count) return;
            ListView.RowCount = Logger.LogMessages.Count;
            ScrollView.verticalNormalizedPosition = 0;
        }

        /// <summary>
        /// Called when an input is entered on the console.
        /// </summary>
        /// <param name="input">The input entered.</param>
        private void InputReceived(string input)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input))
            {
                ActivateInputField();
                return;
            }

            bool commandProcessed = false;

            string module = input.Split('.')[0];

            for (int i = 0; i < CommandProcessors.Length; ++i)
                if (module == CommandProcessors[i].ModuleName)
                {
                    CommandProcessors[i].EnterCommand(input);
                    commandProcessed = true;
                }

            if (!commandProcessed) Logger.LogError("Module " + module + " not found!", this);

            ActivateInputField();
        }

        /// <summary>
        /// Cleans and activates the input field.
        /// </summary>
        private void ActivateInputField()
        {
            ConsoleInput.text = "";
            ConsoleInput.ActivateInputField();
            ConsoleInput.Select();
        }

        /// <summary>
        /// Populates the data of an item in the list.
        /// </summary>
        /// <param name="item">Item to populate.</param>
        /// <param name="rowIndex">Index of that item.</param>
        private static void PopulateItem(RecyclingListViewItem item, int rowIndex)
        {
            LogMessage message = Logger.LogMessages[rowIndex];
            InGameConsoleLine line = item as InGameConsoleLine;
            if (line != null) line.UpdateText(message.Time, message.LogType, message.Sender, message.Message);
        }
    }
}