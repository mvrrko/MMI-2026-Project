using System.Collections.Generic;
using MMI2026.LabEscape.Core;
using MMI2026.LabEscape.UI;
using UnityEngine;

namespace MMI2026.LabEscape.Managers
{
    public class InputRouter : MonoBehaviour
    {
        [SerializeField] private List<MonoBehaviour> commandSources = new List<MonoBehaviour>();
        [SerializeField] private FusionManager fusionManager;
        [SerializeField] private ConflictResolutionManager conflictResolutionManager;
        [SerializeField] private ObjectiveManager objectiveManager;
        [SerializeField] private CommandFeedbackUI feedbackUI;

        private readonly List<ICommandSource> resolvedSources = new List<ICommandSource>();

        private void Awake()
        {
            if (conflictResolutionManager != null)
            {
                conflictResolutionManager.OnCommandResolved += HandleResolvedCommand;
            }

            resolvedSources.Clear();
            foreach (var source in commandSources)
            {
                if (source is ICommandSource commandSource)
                {
                    resolvedSources.Add(commandSource);
                    commandSource.OnCommand += HandleIncomingCommand;
                }
            }
        }

        private void OnEnable()
        {
            foreach (var source in resolvedSources) source.StartListening();
        }

        private void OnDisable()
        {
            foreach (var source in resolvedSources) source.StopListening();
        }

        private void OnDestroy()
        {
            foreach (var source in resolvedSources) source.OnCommand -= HandleIncomingCommand;
            if (conflictResolutionManager != null)
            {
                conflictResolutionManager.OnCommandResolved -= HandleResolvedCommand;
            }
        }

        private void HandleIncomingCommand(CommandData command)
        {
            if (command == null) return;

            if (command.SourceModality == ModalityType.KeyboardMouse)
            {
                fusionManager?.TrackPointerContext(command);
                conflictResolutionManager?.TrackPointerContext(command);
            }

            var fused = fusionManager != null ? fusionManager.ApplyFusion(command) : command;
            if (fused == null) return;

            if (fused.SourceModality == ModalityType.Voice && conflictResolutionManager != null)
            {
                if (!conflictResolutionManager.TryResolveConflict(fused, out fused))
                {
                    return;
                }
            }

            HandleResolvedCommand(fused);
        }

        private void HandleResolvedCommand(CommandData command)
        {
            if (command == null) return;
            objectiveManager?.HandleCommand(command);
            feedbackUI?.Show(command);
        }
    }
}
