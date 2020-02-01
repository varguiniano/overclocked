using System;
using Varguiniano.Core.Runtime.Debug;

namespace Varguiniano.Core.Runtime.Common
{
    /// <summary>
    ///     Base class for the implementation of the inversion of control principle.
    ///     This class is used to represent a service, and includes the code required to
    ///     inject itself into its clients.
    /// </summary>
    /// <typeparam name="T">The class that will be used as a service. It provides the name and the signatures</typeparam>
    public abstract class Service<T> : Singleton<T> where T : Service<T>
    {
        /// <summary>
        ///     A reference to the current implementation of this service.
        /// </summary>
        private static T typedReference;

        /// <summary>
        ///     Flag used to indicate whether the service is ready or not.
        /// </summary>
        // ReSharper disable once StaticMemberInGenericType
        protected static bool IsReady;

        /// <summary>
        ///     Internal event handler, used to notify clients about this service's availability.
        /// </summary>
        private static Action<T> serve;

        /// <summary>
        ///     Public access to the event that notifies about this service's availability.
        /// </summary>
        public static event Action<T> OnServiceReady
        {
            add
            {
                // Double instance check makes sure that if it wasn't instanced the first time that call instances it the second one.
                // TODO: This shouldn't happen though, we should recheck in which cases Instance returns null.
                // Only if something goes wrong on the instantiation it will return.
                if (Instance == null)
                    if (Instance == null)
                        return;

                if (IsReady) value.Invoke(typedReference);

                serve -= value; // Prevent double calls
                serve += value;
            }
            remove => serve -= value;
        }

        /// <summary>
        ///     Notifies all the clients about the availability of the service and injects a reference.
        /// </summary>
        protected void Ready()
        {
            if (IsReady) return;
            typedReference = (T) this;
            IsReady = true;
            
            Logger.LogInfo(PrefabName + " initialized.", this);

            serve?.Invoke(typedReference);
        }
    }
}