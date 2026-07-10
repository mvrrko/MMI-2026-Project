using System;
using System.Collections.Generic;
using MMI2026.LabEscape.Core;
using UnityEngine;
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
using UnityEngine.Windows.Speech;
#endif

namespace MMI2026.LabEscape.Input
{
    public class VoiceInputSource : MonoBehaviour, ICommandSource
    {
        public event Action<CommandData> OnCommand;

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        private KeywordRecognizer keywordRecognizer;
#endif
        private readonly Dictionary<string, (GameActionType action, string targetId)> commands =
            new Dictionary<string, (GameActionType action, string targetId)>(StringComparer.OrdinalIgnoreCase)
            {
                { "open inventory", (GameActionType.OpenInventory, string.Empty) },
                { "close inventory", (GameActionType.CloseInventory, string.Empty) },
                { "pick up keycard", (GameActionType.PickUpKeycard, "Keycard") },
                { "activate generator", (GameActionType.ActivateGenerator, "Generator") },
                { "unlock door", (GameActionType.UnlockDoor, "Door_Exit") },
                { "open door", (GameActionType.OpenDoor, "Door_Exit") },
                { "unlock this", (GameActionType.UnlockDoor, "this") },
                { "open this", (GameActionType.OpenDoor, "this") }
            };

        private void OnEnable() => StartListening();
        private void OnDisable() => StopListening();

        public void StartListening()
        {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            if (keywordRecognizer != null) return;
            keywordRecognizer = new KeywordRecognizer(new List<string>(commands.Keys).ToArray());
            keywordRecognizer.OnPhraseRecognized += HandlePhrase;
            keywordRecognizer.Start();
#endif
        }

        public void StopListening()
        {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            if (keywordRecognizer == null) return;
            keywordRecognizer.OnPhraseRecognized -= HandlePhrase;
            if (keywordRecognizer.IsRunning) keywordRecognizer.Stop();
            keywordRecognizer.Dispose();
            keywordRecognizer = null;
#endif
        }

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        private void HandlePhrase(PhraseRecognizedEventArgs args)
        {
            if (!commands.TryGetValue(args.text, out var mapped)) return;
            Emit(mapped.action, mapped.targetId, args.text);
        }
#endif

        private void Emit(GameActionType action, string targetId, string raw)
        {
            OnCommand?.Invoke(new CommandData
            {
                Action = action,
                TargetId = targetId,
                RawCommand = raw,
                SourceModality = ModalityType.Voice,
                TimestampUtc = DateTime.UtcNow
            });
        }
    }
}
