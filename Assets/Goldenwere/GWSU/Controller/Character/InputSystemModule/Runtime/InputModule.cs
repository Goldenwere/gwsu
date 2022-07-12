using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Goldenwere.GWSU.CTRL.CHAR.Core;

namespace Goldenwere.GWSU.CTRL.CHAR.InputSystemModule
{
    public class InputModule : BaseInputModule
    {
        internal struct Callbacks
        {
            public Action<InputAction.CallbackContext> canceled;
            public Action<InputAction.CallbackContext> performed;
            public Action<InputAction.CallbackContext> started;
        }

        public bool preferToggledInputs;

        private Dictionary<Guid, Callbacks> registeredCallbacks;

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

        public override void RegisterModuleAction(BaseModuleAction _action)
        {
            if (TryGetReference(_action.Emitter, out InputActionReference _reference))
            {
                Action<InputAction.CallbackContext> canceled = (InputAction.CallbackContext ctx) =>
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

                Action<InputAction.CallbackContext> performed = (InputAction.CallbackContext ctx) =>
                {
                    _action.Listener.Invoke();
                };

                _reference.action.canceled += canceled;
                _reference.action.performed += performed;
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
                Action<InputAction.CallbackContext> canceled = (InputAction.CallbackContext ctx) =>
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
                Action<InputAction.CallbackContext> performed = (InputAction.CallbackContext ctx) =>
                {
                    switch (_action.Type)
                    {
                        case ModuleActionType.Held:
                            _action.Listener.Invoke(ctx.ReadValue<T>());
                            break;
                    }
                };
                Action<InputAction.CallbackContext> started = (InputAction.CallbackContext ctx) =>
                {
                    switch (_action.Type)
                    {
                        case ModuleActionType.HeldOrPressed:
                        case ModuleActionType.Pressed:
                            _action.Listener.Invoke(ctx.ReadValue<T>());
                            break;
                    }
                };

                _reference.action.canceled += canceled;
                _reference.action.performed += performed;
                _reference.action.started += started;
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

        private bool CheckForCallback(Guid id)
        {
            if (RegisteredCallbacks.ContainsKey(id))
            {
                return true;
            }
            Debug.LogError("Attempted to unregister a module action that was not yet registered!");
            return false;
        }

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
