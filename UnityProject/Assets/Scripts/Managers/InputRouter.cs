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
        }

        private void HandleIncomingCommand(CommandData command)
        {
            if (command == null) return;

            if (command.SourceModality == ModalityType.KeyboardMouse)
            {
                conflictResolutionManager?.TrackPointerContext(command);
            }

            var fused = fusionManager != null ? fusionManager.ApplyFusion(command) : command;
            if (fused == null) return;

            if (fused.SourceModality == ModalityType.Voice && conflictResolutionManager != null)
            {
                conflictResolutionManager.TryResolveConflict(fused, out fused);
            }

            objectiveManager?.HandleCommand(fused);
            feedbackUI?.Show(fused);
        }
    }
}
