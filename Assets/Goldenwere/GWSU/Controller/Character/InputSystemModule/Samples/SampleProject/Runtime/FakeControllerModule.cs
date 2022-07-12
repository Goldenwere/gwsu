using UnityEngine;
using Goldenwere.GWSU.CTRL.CHAR.Core;

namespace Goldenwere.GWSU.CTRL.CHAR.InputSystemModule.SampleProject
{
    /// <summary>
    /// Fake module to demonstrate the input module with
    /// </summary>
    public class FakeControllerModule : BaseControllerModule
    {
        [SerializeField] private BaseInputModule module;

        [Header("Input Emitters")]
        [SerializeField] private ScriptableObject emitterOnCrouch;
        [SerializeField] private ScriptableObject emitterOnJump;
        [SerializeField] private ScriptableObject emitterOnMovement;

        /* actions */
        private BaseModuleAction actionOnCrouch;
        private BaseModuleAction actionOnJump;
        private BaseModuleAction<Vector2> actionOnMovement;

        /* state fields */
        private Vector2 valueMovement;
        private bool valueCrouched;

        /// <summary>
        /// Initializes the module on Awake
        /// </summary>
        private void Awake()
        {
            // Set up module actions by assigning emitters to the corresponding actions in the module
            actionOnCrouch = new BaseModuleAction(OnCrouch, emitterOnCrouch, ModuleActionType.HeldOrPressed);
            actionOnJump = new BaseModuleAction(OnJump, emitterOnJump, ModuleActionType.Pressed);
            actionOnMovement = new BaseModuleAction<Vector2>(OnMovement, emitterOnMovement, ModuleActionType.Held);
        }

        /// <summary>
        /// Registers actions on Enable
        /// </summary>
        private void OnEnable()
        {
            module.RegisterModuleAction(actionOnCrouch);
            module.RegisterModuleAction(actionOnJump);
            module.RegisterModuleAction(actionOnMovement);
        }

        /// <summary>
        /// Unregisters actions on Disable
        /// </summary>
        private void OnDisable()
        {
            module.UnregisterModuleAction(actionOnCrouch);
            module.UnregisterModuleAction(actionOnJump);
            module.UnregisterModuleAction(actionOnMovement);
        }

        /// <summary>
        /// Handler for the crouch action
        /// </summary>
        private void OnCrouch()
        {
            valueCrouched = !valueCrouched;
        }

        /// <summary>
        /// Handler for the jump action
        /// </summary>
        private void OnJump()
        {
            Debug.Log("Jumped");
        }

        /// <summary>
        /// Handler for the movement action
        /// </summary>
        /// <param name="value">The value of the input</param>
        private void OnMovement(Vector2 value)
        {
            valueMovement = value;
        }

        /// <summary>
        /// Displays testing information to the screen
        /// </summary>
        private void OnGUI()
        {
            int size = Screen.height / 32;
            GUI.Label(
                new Rect(10, 10, Screen.width - 10, size),
                $"Movement: {valueMovement}",
                new GUIStyle { fontSize = size }
            );
            GUI.Label(
                new Rect(10, 10 + size, Screen.width - 10, size),
                $"Crouched: {valueCrouched}",
                new GUIStyle { fontSize = size }
            );
        }
    }
}
