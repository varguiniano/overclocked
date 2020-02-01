using UnityEngine;
using Logger = Varguiniano.Core.Runtime.Debug.Logger;

namespace Varguiniano.Core.Runtime.Common
{
    /// <summary>
    ///     Be aware this will not prevent a non singleton constructor
    ///     such as `T myT = new T();`
    ///     To prevent that, add `protected T () {}` to your singleton class.
    ///     As a note, this is made as MonoBehaviour because we need Coroutines.
    /// </summary>
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        /// <summary>
        ///     Path where the prefab would sit, you can override this in your implementation.
        /// </summary>
        private const string PrefabPath = "Prefabs/Singleton/";

        /// <summary>
        ///     Prefab name, you can override this in your implementation.
        /// </summary>
        protected static readonly string PrefabName = typeof(T).Name;

        /// <summary>
        ///     Singleton instance.
        /// </summary>
        private static T instance;

        /// <summary>
        ///     Lock to prevent multiple instantiations.
        /// </summary>
        // ReSharper disable once StaticMemberInGenericType
        private static readonly object Lock = new object();

        /// <summary>
        ///     Path to the prefab of this singleton.
        ///     You can create the prefab by yourself or it will autocreate if you don't.
        /// </summary>
        private static T Prefab => Resources.Load<T>(PrefabPath + PrefabName);

        /// <summary>
        ///     Public singleton access, this is where magic happens.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (ApplicationIsQuitting)
                {
                    Logger.LogWarning("Instance '"
                                    + PrefabName
                                    + "' already destroyed on application quit."
                                    + " Won't create again - returning null.",
                                      PrefabName);

                    return null;
                }

                lock (Lock)
                {
                    if (instance != null)
                    {
                        CheckDontDestroyOnLoad();
                        return instance;
                    }

                    instance = (T) FindObjectOfType(typeof(T));
                    CheckDontDestroyOnLoad();

                    if (FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        Logger.LogError("Something went really wrong "
                                      + " - there should never be more than 1 singleton!"
                                      + " Reopening the scene might fix it.",
                                        PrefabName);

                        return instance;
                    }

                    if (instance != null) return instance;
                    GameObject singleton;

                    if (Prefab == null)
                    {
                        Logger.LogInfo("There is no prefab for " + PrefabName + ". Creating a new GameObject.",
                                       PrefabName);

                        singleton = new GameObject();
                        instance = singleton.AddComponent<T>();
                    }
                    else
                    {
                        Logger.LogInfo("Found a prefab for " + Prefab.name + ". Instantiating it.", PrefabName);
                        singleton = Instantiate(Prefab).gameObject;
                        instance = singleton.GetComponent<T>();
                    }

                    singleton.name = "[Singleton] " + PrefabName;

                    DontDestroyOnLoad(singleton);

                    return instance;
                }
            }
        }

        /// <summary>
        /// Checks if the singleton is a DontDestroyOnLoad and sets it up if it isn't.
        /// </summary>
        private static void CheckDontDestroyOnLoad()
        {
            if (instance == null) return;
            if (instance.transform.parent != null) return;
            if (instance.gameObject.IsDontDestroyOnLoad()) return;

            Logger.LogInfo(instance.name + " is not DontDestroyOnLoad so setting it up.", PrefabName);

            DontDestroyOnLoad(instance);
        }

        /// <summary>
        ///     Flag to know when the application is quitting.
        /// </summary>
        // ReSharper disable once StaticMemberInGenericType
        private static bool ApplicationIsQuitting { get; set; }

        /// <summary>
        ///     When Unity quits, it destroys objects in a random order.
        ///     In principle, a Singleton is only destroyed when application quits.
        ///     If any script calls Instance after it have been destroyed,
        ///     it will create a buggy ghost object that will stay on the Editor scene
        ///     even after stopping playing the Application. Really bad!
        ///     So, this was made to be sure we're not creating that buggy ghost object.
        /// </summary>
        public virtual void OnDestroy() => ApplicationIsQuitting = true;
    }
}