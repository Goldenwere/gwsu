using System;
using UnityEngine;

namespace Goldenwere.GWSU.CTRL.CHAR.Core
{
    public enum ModuleActionType : uint
    {
        Held = 0,
        Pressed = 1,
        HeldOrPressed = 2,
    }

    public class BaseModuleAction
    {
        [SerializeField] private ScriptableObject emitter;
        [SerializeField] private Action listener;
        [SerializeField] private ModuleActionType type;

        private Guid guid;

        public Guid GUID => guid;
        public Action Listener => listener;
        public ScriptableObject Emitter => emitter;
        public ModuleActionType Type => type;

        public BaseModuleAction(
            Action _listener,
            ScriptableObject _emitter,
            ModuleActionType _type
        )
        {
            guid = Guid.NewGuid();
            listener = _listener;
            emitter = _emitter;
            type = _type;
        }
    }

    public class BaseModuleAction<T>
    {
        [SerializeField] private ScriptableObject emitter;
        [SerializeField] private Action<T> listener;
        [SerializeField] private ModuleActionType type;

        private Guid guid;

        public Guid GUID => guid;
        public Action<T> Listener => listener;
        public ScriptableObject Emitter => emitter;
        public ModuleActionType Type => type;

        public BaseModuleAction(
            Action<T> _listener,
            ScriptableObject _emitter,
            ModuleActionType _type
        )
        {
            guid = Guid.NewGuid();
            listener = _listener;
            emitter = _emitter;
            type = _type;
        }
    }
}
