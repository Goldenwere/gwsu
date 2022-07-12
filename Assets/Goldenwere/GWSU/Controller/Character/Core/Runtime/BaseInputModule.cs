using UnityEngine;

namespace Goldenwere.GWSU.CTRL.CHAR.Core
{
    /// <summary>
    /// Controller module that performs the bridging between a module with actions and a lower-level input system.
    /// An Input Module should determine how and when to invoke module actions based on the specified action types.
    /// </summary>
    public abstract class BaseInputModule : MonoBehaviour
    {
        /// <summary>
        /// Registers a module's action with the input module and the input system it implements.
        /// </summary>
        /// <param name="_action">The action to register</param>
        public abstract void RegisterModuleAction(BaseModuleAction _action);

        /// <summary>
        /// Registers a module's action with the input module and the input system it implements.
        /// </summary>
        /// <typeparam name="T">The value-type associated with the action</typeparam>
        /// <param name="_action">The action to register</param>
        public abstract void RegisterModuleAction<T>(BaseModuleAction<T> _action) where T : struct;

        /// <summary>
        /// Unregisters a module's action from the input module and the input system it implements.
        /// </summary>
        /// <param name="_action">The action to register</param>
        public abstract void UnregisterModuleAction(BaseModuleAction _action);

        /// <summary>
        /// Unregisters a module's action from the input module and the input system it implements.
        /// </summary>
        /// <typeparam name="T">The value-type associated with the action</typeparam>
        /// <param name="_action">The action to register</param>
        public abstract void UnregisterModuleAction<T>(BaseModuleAction<T> _action) where T : struct;
    }
}
