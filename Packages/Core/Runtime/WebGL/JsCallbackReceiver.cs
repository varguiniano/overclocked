using System;
using System.Collections;
using UnityEngine.Networking;
using Varguiniano.Core.Runtime.Common;
using Varguiniano.Core.Runtime.Debug;
using Varguiniano.Core.Runtime.Localization;

namespace Varguiniano.Core.Runtime.WebGL
{
    /// <summary>
    /// Class that receives callbacks from JS.
    /// </summary>
    public class JsCallbackReceiver : Singleton<JsCallbackReceiver>
    {
        /// <summary>
        /// Event called when a file is received.
        /// </summary>
        public Action<byte[]> FileReceived;

        /// <summary>
        /// This function is called by JS when the file has been uploaded.
        /// </summary>
        /// <param name="url">Url where the file is.</param>
        // ReSharper disable once UnusedMember.Global
        public void ReceiveFile(string url) => StartCoroutine(ReceiveFileRoutine(url));

        /// <summary>
        /// Routine that makes a web request to where the file is to retrieve it.
        /// </summary>
        /// <param name="url">Url of the file.</param>
        private IEnumerator ReceiveFileRoutine(string url)
        {
            if (url == new LocalizableString {Key = "webgl_cancelled"}.LocalizedValue
             || url == new LocalizableString {Key = "webgl_no_file_selected"}.LocalizedValue)
            {
                Logger.LogInfo("Operation cancelled.", this);
                FileReceived = null;
                yield break;
            }

            using (UnityWebRequest www = UnityWebRequest.Get(url))
            {
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Logger.LogError(www.error, this);
                    FileReceived = null;
                }
                else
                {
                    Logger.LogInfo("File received from js.", this);
                    FileReceived?.Invoke(www.downloadHandler.data);
                }
            }
        }
    }
}