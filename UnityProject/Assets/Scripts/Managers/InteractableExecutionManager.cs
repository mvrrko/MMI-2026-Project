using System;
using System.Collections.Generic;
using MMI2026.LabEscape.Core;
using MMI2026.LabEscape.Interactables;
using UnityEngine;

namespace MMI2026.LabEscape.Managers
{
    public class InteractableExecutionManager : MonoBehaviour
    {
        [SerializeField] private InputRouter inputRouter;
        [SerializeField] private List<InteractableBase> interactables = new List<InteractableBase>();
        [SerializeField] private bool autoFindInteractables = true;

        private readonly Dictionary<string, InteractableBase> interactableById =
            new Dictionary<string, InteractableBase>(StringComparer.OrdinalIgnoreCase);

        private void Awake()
        {
            RebuildRegistry();
        }

        private void OnEnable()
        {
            if (inputRouter != null)
            {
                inputRouter.OnCommandResolved += HandleResolvedCommand;
            }
        }

        private void OnDisable()
        {
            if (inputRouter != null)
            {
                inputRouter.OnCommandResolved -= HandleResolvedCommand;
            }
        }

        private void HandleResolvedCommand(CommandData command)
        {
            if (command == null) return;

            var targetId = ResolveTargetId(command);
            if (string.IsNullOrWhiteSpace(targetId)) return;
            if (!interactableById.TryGetValue(targetId, out var interactable) || interactable == null) return;

            interactable.Execute();
        }

        private string ResolveTargetId(CommandData command)
        {
            switch (command.Action)
            {
                case GameActionType.Interact:
                case GameActionType.UnlockDoor:
                case GameActionType.OpenDoor:
                case GameActionType.ActivateGenerator:
                case GameActionType.PickUpKeycard:
                    return command.TargetId;
                default:
                    return string.Empty;
            }
        }

        private void RebuildRegistry()
        {
            interactableById.Clear();
            if (autoFindInteractables)
            {
                interactables = new List<InteractableBase>(FindObjectsOfType<InteractableBase>());
            }

            foreach (var interactable in interactables)
            {
                if (interactable == null) continue;
                var objectId = interactable.ObjectId;
                if (string.IsNullOrWhiteSpace(objectId)) continue;
                interactableById[objectId] = interactable;
            }
        }
    }
}
