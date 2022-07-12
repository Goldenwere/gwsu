using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Goldenwere.GWSU.CTRL.CHAR.Core;

namespace Goldenwere.GWSU.CTRL.CHAR.InputSystemModule
{
    /// <summary>
    /// Input Module that implements the UnityEngine's InputSystem.
    /// Note: The Input Module assumes the InputSystem sets its inputs to default 
    /// </summary>
    public class InputModule : BaseInputModule
    {
        /// <summary>
        /// Container for the actions associated with InputActions
        /// </summary>
        internal struct Callbacks
        {
            public Action<InputAction.CallbackContext> canceled;
            public Action<InputAction.CallbackContext> performed;
            public Action<InputAction.CallbackContext> started;
        }

        // Field backing the RegisteredCallbacks property.
        // This variable could possibly be null; reference the property instead.
        private Dictionary<Guid, Callbacks> registeredCallbacks;

        /// <summary>
        /// Callbacks that are currently registered with the Input Module
        /// </summary>
        private Dictionary<Guid, Callbacks> RegisteredCallbacks
        {
            get
            {
                if (registeredCallbacks == null)
                {
                    registeredCallbacks = new Dictionary<Guid, Callbacks>();
                }
                return registeredCallbacks;
            }
        }

        /// <summary>
        /// Whether HeldOrPressed inputs prefer Pressed (true) or Held (false)
        /// </summary>
        public bool preferToggledInputs;

        public override void RegisterModuleAction(BaseModuleAction _action)
        {
            if (TryGetReference(_action.Emitter, out InputActionReference _reference))
            {
                void canceled (InputAction.CallbackContext ctx)
                {
                    switch (_action.Type)
                    {
                        case ModuleActionType.HeldOrPressed:
                            if (!preferToggledInputs)
                            {
                                _action.Listener.Invoke();
                            }
                            break;
                        case ModuleActionType.Held:
                            _action.Listener.Invoke();
                            break;
                    }
                };

                void performed (InputAction.CallbackContext ctx)
                {
                    _action.Listener.Invoke();
                };

                // Subscribe to the corresponding events on the InputAction
                _reference.action.canceled += canceled;
                _reference.action.performed += performed;

                // Register the callbacks with the Input Module
                RegisteredCallbacks.Add
                (
                    _action.GUID,
                    new Callbacks
                    {
                        canceled = canceled,
                        performed = performed,
                    }
                );
            }
        }

        public override void RegisterModuleAction<T>(BaseModuleAction<T> _action)
        {
            if (TryGetReference(_action.Emitter, out InputActionReference _reference))
            {
                void canceled (InputAction.CallbackContext ctx)
                {
                    switch (_action.Type)
                    {
                        case ModuleActionType.Held:
                            _action.Listener.Invoke(ctx.ReadValue<T>());
                            break;
                        case ModuleActionType.HeldOrPressed:
                            if (!preferToggledInputs)
                            {
                                _action.Listener.Invoke(ctx.ReadValue<T>());
                            }
                            break;
                    }
                };

                void performed (InputAction.CallbackContext ctx)
                {
                    switch (_action.Type)
                    {
                        case ModuleActionType.Held:
                            _action.Listener.Invoke(ctx.ReadValue<T>());
                            break;
                    }
                };

                void started (InputAction.CallbackContext ctx)
                {
                    switch (_action.Type)
                    {
                        case ModuleActionType.HeldOrPressed:
                        case ModuleActionType.Pressed:
                            _action.Listener.Invoke(ctx.ReadValue<T>());
                            break;
                    }
                };

                // Subscribe to the corresponding events on the InputAction
                _reference.action.canceled += canceled;
                _reference.action.performed += performed;
                _reference.action.started += started;

                // Register the callbacks with the Input Module
                RegisteredCallbacks.Add
                (
                    _action.GUID,
                    new Callbacks
                    {
                        canceled = canceled,
                        performed = performed,
                        started = started,
                    }
                );
            }
        }

        public override void UnregisterModuleAction(BaseModuleAction _action)
        {
            if (TryGetReference(_action.Emitter, out InputActionReference _reference))
            {
                Unregister(_reference, _action.GUID);
            }
        }

        public override void UnregisterModuleAction<T>(BaseModuleAction<T> _action)
        {
            if (TryGetReference(_action.Emitter, out InputActionReference _reference))
            {
                Unregister(_reference, _action.GUID);
            }
        }

        /// <summary>
        /// Checks if the callback is registered
        /// </summary>
        /// <param name="_id">The id to check for</param>
        /// <returns>True if registered, false if not</returns>
        private bool CheckForCallback(Guid _id)
        {
            if (RegisteredCallbacks.ContainsKey(_id))
            {
                return true;
            }
            Debug.LogError("Attempted to unregister a module action that was not yet registered!");
            return false;
        }

        /// <summary>
        /// Checks if an emitter can be cast to InputActionReference and returns the result
        /// </summary>
        /// <param name="_emitter">The emitter to cast to an InputActionReference</param>
        /// <param name="_reference">The emitter cast to an InputActionReference</param>
        /// <returns>True if successfully cast, false if not</returns>
        private bool TryGetReference(ScriptableObject _emitter, out InputActionReference _reference)
        {
            if (_emitter is InputActionReference reference)
            {
                _reference = reference;
                return true;
            }
            else
            {
                Debug.LogError($"Emitter was not an InputActionReference!");
            }
            _reference = null;
            return false;
        }

        /// <summary>
        /// Generic method to unregister actions from the Input Module
        /// </summary>
        /// <param name="_reference">The emitter InputActionReference to unsubscribe from</param>
        /// <param name="_id">The id of the callbacks to unregister with</param>
        private void Unregister(InputActionReference _reference, Guid _id)
        {
            if (CheckForCallback(_id))
            {
                _reference.action.canceled -= RegisteredCallbacks[_id].canceled;
                _reference.action.performed -= RegisteredCallbacks[_id].performed;
                _reference.action.started -= RegisteredCallbacks[_id].started;
                RegisteredCallbacks.Remove(_id);
            }
        }
    }
}
