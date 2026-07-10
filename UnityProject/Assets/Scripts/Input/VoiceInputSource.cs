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
        [Header("Optional editor fallback")]
        [SerializeField] private bool enableEditorFallbackInput;
        [SerializeField] private string fallbackPhrase = "unlock this";
        [SerializeField] private KeyCode fallbackSubmitKey = KeyCode.Return;

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

        private void Update()
        {
            if (!enableEditorFallbackInput) return;
            if (!UnityEngine.Input.GetKeyDown(fallbackSubmitKey)) return;

            ParseAndEmit(fallbackPhrase);
        }

        public void StartListening()
        {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            if (keywordRecognizer != null) return;
            keywordRecognizer = new KeywordRecognizer(new List<string>(commands.Keys).ToArray());
            keywordRecognizer.OnPhraseRecognized += HandlePhrase;
            keywordRecognizer.Start();
#else
            EmitFailure("voice:error: keyword recognizer unavailable on this platform");
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
            ParseAndEmit(args.text);
        }
#endif

        public void ParseAndEmit(string spokenText)
        {
            if (string.IsNullOrWhiteSpace(spokenText))
            {
                EmitFailure("voice:error: empty command");
                return;
            }

            if (!commands.TryGetValue(spokenText.Trim(), out var mapped))
            {
                EmitFailure($"voice:error: unrecognized command '{spokenText}'");
                return;
            }

            Emit(mapped.action, mapped.targetId, spokenText);
        }

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

        private void EmitFailure(string message)
        {
            Emit(GameActionType.None, string.Empty, message);
        }
    }
}
