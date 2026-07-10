# Step-by-step modular implementation

This repository now contains a modular Unity-ready script scaffold under:

`/home/runner/work/MMI-2026-Project/MMI-2026-Project/UnityProject/Assets/Scripts`

## Step 1: Create Unity scene shell
1. Create a new 3D Unity project.
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
5. In `InputRouter`, assign:
   - `FusionManager`
   - `ConflictResolutionManager`
   - `ObjectiveManager`

## Step 3: Set up modality sources
1. Add `KeyboardMouseInputSource` to `Player` (or `Managers`).
2. Add `VoiceInputSource` to `Managers`.
3. Register both in `InputRouter -> Command Sources`.
4. Optionally assign a target transform to `KeyboardMouseInputSource` for pointing target ID.

## Step 4: Set up gameplay objects
1. Create object `Keycard` with `KeycardInteractable`.
2. Create object `Generator` with `GeneratorInteractable`.
3. Create object `Door_Exit` with `DoorInteractable`.
4. Set object IDs to match command target IDs where needed.
5. Ensure pointer target names match command targets (`Keycard`, `Generator`, `Door_Exit`) so keyboard `E` interact can complete the full objective chain.

## Step 5: Set up UI
1. Add text UI for command feedback and bind to `CommandFeedbackUI`.
2. Add text UI for objective states and bind to `ObjectiveManager.statusText`.
3. Add panel/text for conflict prompts and bind to `ConflictResolutionUI`.
4. Wire conflict buttons to:
   - `ChoosePointerTarget()`
   - `ChooseVoiceTarget()`
   - `CancelChoice()`

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

## Extension points
- Add new modalities by implementing `ICommandSource`.
- Add new actions by extending `GameActionType` and `ObjectiveManager`.
- Replace simple keyword voice mapping with NLP later without changing routing.
- Change disambiguation policy in `ConflictResolutionManager` without touching input sources.
