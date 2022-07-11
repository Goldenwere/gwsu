using UnityEngine;
using Goldenwere.GWSU.CTRL.CHAR.Core;

namespace Goldenwere.GWSU.CTRL.CHAR.InputSystemModule.SampleProject
{
    public class FakeControllerModule : BaseControllerModule
    {
        [SerializeField] private BaseInputModule module;
        [SerializeField] private ScriptableObject emitterOnCrouch;
        [SerializeField] private ScriptableObject emitterOnJump;
        [SerializeField] private ScriptableObject emitterOnMovement;

        private BaseModuleAction actionOnCrouch;
        private BaseModuleAction actionOnJump;
        private BaseModuleAction<Vector2> actionOnMovement;

        private void Awake()
        {
            actionOnCrouch = new BaseModuleAction(OnCrouch, emitterOnCrouch, ModuleActionType.HeldOrPressed);
            actionOnJump = new BaseModuleAction(OnJump, emitterOnJump, ModuleActionType.Pressed);
            actionOnMovement = new BaseModuleAction<Vector2>(OnMovement, emitterOnJump, ModuleActionType.Held);
        }

        private void OnEnable()
        {
            module.RegisterModuleAction(actionOnCrouch);
            module.RegisterModuleAction(actionOnJump);
            //module.RegisterModuleAction(actionOnMovement);
        }

        private void OnDisable()
        {
            module.UnregisterModuleAction(actionOnCrouch);
            module.UnregisterModuleAction(actionOnJump);
            //module.UnregisterModuleAction(actionOnMovement);
        }

        private void OnCrouch()
        {
            Debug.Log($"Crouched"); 
        }

        private void OnJump()
        {
            Debug.Log("Jumped");
        }

        private void OnMovement(Vector2 value)
        {
            Debug.Log($"Value was: {value}");
        }
    }
}
