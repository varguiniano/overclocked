using Varguiniano.Core.Runtime.Common;

namespace Varguiniano.Core.Runtime.Scenes
{
    /// <summary>
    /// Singleton with a reference to the scene library asset.
    /// </summary>
    public class SceneLibrary : Singleton<SceneLibrary>
    {
        /// <summary>
        /// Reference to the actual library.
        /// </summary>
        public SceneLibraryAsset Library;

        /// <summary>
        /// Quick accessor to the scenes.
        /// </summary>
        public static string[] Scenes => Instance.Library.Scenes;
    }
}