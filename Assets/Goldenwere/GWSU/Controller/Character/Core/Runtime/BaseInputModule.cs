using System;
using UnityEngine;

namespace Goldenwere.GWSU.CTRL.CHAR.Core
{
    public abstract class BaseInputModule : MonoBehaviour
    {
        public abstract void RegisterModuleAction(BaseModuleAction _action);
        public abstract void RegisterModuleAction<T>(BaseModuleAction<T> _action) where T : struct;
        public abstract void UnregisterModuleAction(BaseModuleAction _action);
        public abstract void UnregisterModuleAction<T>(BaseModuleAction<T> _action) where T : struct;
    }
}
