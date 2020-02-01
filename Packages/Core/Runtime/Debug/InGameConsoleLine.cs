using Sinbad.UnityRecyclingListView;
using TMPro;
using UnityEngine;

namespace Varguiniano.Core.Runtime.Debug
{
    /// <summary>
    /// Class that handles an single ingame console line.
    /// </summary>
    public class InGameConsoleLine : RecyclingListViewItem
    {
        /// <summary>
        /// Reference to the time text.
        /// </summary>
        public TMP_Text Time;

        /// <summary>
        /// Reference to the type text.
        /// </summary>
        public TMP_Text Type;

        /// <summary>
        /// Reference to the sender text.
        /// </summary>
        public TMP_Text Sender;

        /// <summary>
        /// Reference to the message text.
        /// </summary>
        public TMP_Text Message;

        /// <summary>
        /// Update the text on the line.
        /// </summary>
        /// <param name="time">Log time.</param>
        /// <param name="logType">Log type.</param>
        /// <param name="sender">Log sender.</param>
        /// <param name="message">Log message.</param>
        public void UpdateText(string time, LogType logType, string sender, string message)
        {
            Time.SetText(time);

            Type.SetText(logType.ToString());

            switch (logType)
            {
                case LogType.Exception:
                case LogType.Error:
                    Type.color = Color.red;
                    break;
                case LogType.Warning:
                    Type.color = Color.yellow;
                    break;
                default:
                    Type.color = Color.white;
                    break;
            }

            Sender.SetText(string.IsNullOrEmpty(sender) ? "Unknown sender" : sender);

            Message.SetText(message);
        }
    }
}