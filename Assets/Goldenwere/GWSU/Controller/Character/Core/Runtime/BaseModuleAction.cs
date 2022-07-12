using System;
using UnityEngine;

namespace Goldenwere.GWSU.CTRL.CHAR.Core
{
    /// <summary>
    /// Defines how a module action should be implemented.
    /// Notes regarding implementation are ideal and may not represent
    /// the final method an input module implements them.
    /// </summary>
    public enum ModuleActionType : uint
    {
        /// <summary>
        /// <para>
        /// Specifies that an input:<br/>
        /// - Starts when it is first pressed<br/>
        /// - Ends when it is first released<br/>
        /// </para><br/>
        /// <para>
        /// For BaseModuleAction with void type, this means that
        /// the listener should be invoked on press and on release.
        /// </para>
        /// <para>
        /// For BaseModuleAction with value type, this means that
        /// the listener should be invoked every time the value is updated,
        /// including on release.
        /// </para>
        /// </summary>
        Held = 0,
        /// <summary>
        /// <para>
        /// Specifies that an input:<br/>
        /// - Is called when it is first pressed<br/>
        /// </para><br/>
        /// <para>
        /// For BaseModuleAction with void type, this means that
        /// the listener should be invoked on press.
        /// </para>
        /// <para>
        /// For BaseModuleAction with value type, this means that
        /// the listener should be invoked with the value
        /// that it was first invoked with. Value types typically don't
        /// make sense with pressed inputs.
        /// </para>
        /// </summary>
        Pressed = 1,
        /// <summary>
        /// <para>
        /// Specifies that an input:<br/>
        /// - Is called when it is first pressed<br/>
        /// - Is called when it is released
        ///   only if preference given to held inputs<br/>
        /// This is useful for actions that can function either toggled or held,
        /// and ideally should be used over type Held.
        /// For example, actions like crouch and sprint can be HeldOrPressed,
        /// whereas traditional first-person movement in most cases only works as Held.
        /// </para><br/>
        /// <para>
        /// For BaseModuleAction with void type, this means that
        /// the listener should be invoked on press
        /// and on release if allowed by some sort of bool setting.
        /// </para>
        /// <para>
        /// For BaseModuleAction with value type, this means that
        /// the listener should be invoked with the value
        /// that it was first invoked with,
        /// then be invoked with the default/zero value on release.
        /// Value types typically don't make sense with HeldOrPressed inputs.
        /// </para>
        /// </summary>
        HeldOrPressed = 2,
    }

    /// <summary>
    /// <para>
    /// Class representing a base module action with no association to any value type.
    /// </para>
    /// <para>
    /// At base, a module is a bridge between an emitter and a listener.<br/>
    /// - The emitter is some sort of ScriptableObject which emits input
    /// from a general input system. E.g. InputActionReference<br/>
    /// - The listener is the method on a module that subscribes to the emitter.<br/>
    /// </para>
    /// <para>
    /// Additionally, it defines the action's type,
    /// which describes how the controller should implement its emitted input.
    /// </para>
    /// </summary>
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

    /// <summary>
    /// <para>
    /// Class representing a base module action associated with a value type.
    /// </para>
    /// <para>
    /// At base, a module is a bridge between an emitter and a listener.<br/>
    /// - The emitter is some sort of ScriptableObject which emits input
    /// from a general input system. E.g. InputActionReference<br/>
    /// - The listener is the method on a module that subscribes to the emitter.<br/>
    /// </para>
    /// <para>
    /// Additionally, it defines the action's type,
    /// which describes how the controller should implement its emitted input.
    /// </para>
    /// </summary>
    public class BaseModuleAction<T>
        where T : struct
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
