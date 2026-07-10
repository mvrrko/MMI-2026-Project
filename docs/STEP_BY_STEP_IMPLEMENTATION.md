# Step-by-step modular implementation

This repository now contains a modular Unity-ready script scaffold under:

`/home/runner/work/MMI-2026-Project/MMI-2026-Project/UnityProject/Assets/Scripts`

## Step 1: Create Unity scene shell
1. Create a new **3D (URP)** Unity project.
2. Copy the `UnityProject/Assets/Scripts` folder into your Unity project's `Assets`.
3. Create one scene named `LabEscape_Main`.
4. Add empty GameObjects:
   - `Managers`
   - `UI`
   - `Interactables`
   - `Player`

## Step 2: Set up managers
1. Add `InputRouter` to `Managers`.
2. Add `FusionManager` to `Managers`.
3. Add `ConflictResolutionManager` to `Managers`.
4. Add `ObjectiveManager` to `Managers`.
5. Add `InteractableExecutionManager` to `Managers`.
6. In `InputRouter`, assign:
   - `FusionManager`
   - `ConflictResolutionManager`
   - `ObjectiveManager`
7. In `InteractableExecutionManager`, assign `InputRouter` and keep `Auto Find Interactables` enabled (or manually bind all interactables).

## Step 3: Set up modality sources
1. Add `CenterScreenTargetResolver` to `Player` and set `Pointer Camera` to your FPS camera.
2. Add `KeyboardMouseInputSource` to `Player` (or `Managers`), assign the resolver in `Target Resolver`.
3. Add `FirstPersonMovementController` to `Player` (requires `CharacterController`).
4. Add `FirstPersonLookController` to the player camera and assign `Player Body` to the `Player` transform.
5. Keep Unity's `Horizontal`, `Vertical`, `Mouse X`, and `Mouse Y` axes in Input Manager defaults.
6. Add `VoiceInputSource` to `Managers`.
7. Register both in `InputRouter -> Command Sources`.
8. In `VoiceInputSource`, optional editor fallback lets you test parsing by pressing Enter on `Fallback Phrase`.

## Step 4: Set up gameplay objects
1. Create object `Keycard` with `KeycardInteractable`.
2. Create object `Generator` with `GeneratorInteractable`.
3. Create object `Door_Exit` with `DoorInteractable`.
4. Set object IDs to match command target IDs where needed.
5. Ensure pointer target names match command targets (`Keycard`, `Generator`, `Door_Exit`) so keyboard `E` interact can complete the full objective chain.

## Step 5: Set up UI
1. Add text UI for command feedback and bind to `CommandFeedbackUI`.
2. Enable `showRawCommandDetails` so voice failures and parser errors are visible during demos.
3. Add text UI for objective states and bind to `ObjectiveManager.statusText`.
4. Add text UI for completion message and bind to `ObjectiveManager.completionText`.
5. Add panel/text for conflict prompts and bind to `ConflictResolutionUI`.
6. Wire conflict buttons to:
   - `ChoosePointerTarget()`
   - `ChooseVoiceTarget()`
   - `CancelChoice()`
7. Add a win panel and bind it to `WinStateUI` (`ObjectiveManager` + `winPanel`) so final completion is clearly visible in recording.

## Step 6: Task 1 delivery
1. Use keyboard/mouse only.
2. Complete objective flow: keycard -> generator -> door open.
3. Record demonstration video for Task 1.

## Step 7: Task 2 delivery
1. Enable `VoiceInputSource`.
2. Use voice commands to perform the same objective flow.
3. Record demonstration video for Task 2.

## Step 8: Task 3 delivery (fusion + fission)
1. Fusion case:
   - point to a door
   - say `unlock this`
2. Fission case:
   - pointer target differs from spoken target
   - choose pointer/voice/cancel using `ConflictResolutionUI`
3. Record gameplay showing both outcomes.

## Step 9: Task 4 final recording
1. Make one short final video covering:
   - concept
   - modality 1 only
   - modality 2 only
   - fusion
   - fission
2. Compress output as needed for submission limits.

## Step 10: Final validation run (single final-product check)
1. Keyboard/mouse only run:
   - pick keycard with `E` or left click
   - activate generator
   - unlock/open exit door
2. Voice only run:
   - `pick up keycard`
   - `activate generator`
   - `unlock door`
   - `open door`
3. Fusion check:
   - point at exit door and say `unlock this`
4. Fission check:
   - set pointer on one target, speak command for another target, verify conflict popup choices and timeout cancel behavior.

## Step 11: Desktop build + submission prep
1. In Unity Build Settings, target `PC, Mac & Linux Standalone` and build desktop executable.
2. Record one uninterrupted walkthrough showing concept, both single modalities, fusion, fission, and win state.
3. Compress video to `<20MB` (example ffmpeg):
   - `ffmpeg -i input.mp4 -vcodec libx264 -crf 32 -preset veryslow -acodec aac -b:a 96k final_submission.mp4`
4. Re-check final file size and playback quality before upload.

## Extension points
- Add new modalities by implementing `ICommandSource`.
- Add new actions by extending `GameActionType` and `ObjectiveManager`.
- Replace simple keyword voice mapping with NLP later without changing routing.
- Change disambiguation policy in `ConflictResolutionManager` without touching input sources.
