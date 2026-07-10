# MMI-2026-Project

## Project concept: **Lab Escape** (desktop)

You are trapped in a small laboratory and must restore power and unlock the final exit door.

### Core objective (3-5 minute loop)
Complete these 3 objectives in one compact scene:
1. Find and pick up a keycard
2. Activate the generator
3. Unlock and open the exit door

The game is designed so all assignment requirements are built on the same actions:
- pick/use/open/activate
- target object selection
- command feedback and disambiguation

---

## Modalities

### First modality (Task 1): Keyboard + mouse
- Move with `WASD`
- Look/aim with mouse
- Interact with `E` (or left click)
- Open inventory with `I`

The entire level must be completable using only this modality.

### Second modality (Task 2): Voice commands
Suggested command set (small and reliable):
- `open inventory`
- `close inventory`
- `pick up keycard`
- `activate generator`
- `unlock door`
- `open door`

Voice commands should trigger the same underlying game actions as keyboard/mouse.

---

## Fusion and fission design (Task 3)

### Modality fusion
Trigger one action from two channels:
- User points at a door (pointer target)
- User says: `unlock this`
- System combines both inputs and unlocks the pointed door

### Modality fission (conflict case)
Two channels disagree:
- Pointer target: Door A
- Voice target: `unlock Door B`

Disambiguation strategy:
1. Show conflict UI: `Targets differ: pointer=Door A, voice=Door B`
2. Offer quick choice:
   - `Use pointer target`
   - `Use voice target`
   - `Cancel`
3. If no selection in 3 seconds, cancel and ask user to repeat

---

## Technical implementation plan

### Intro task
- Finalize interaction map (objects, actions, win condition)
- Build one playable lab scene using free assets
- Set desktop as build target

### Task 1
- Implement player movement and interaction raycast
- Implement interactables:
  - Keycard pickup
  - Generator activation
  - Exit door lock/open logic
- Add objective UI and completion screen
- Record gameplay using only keyboard/mouse

### Task 2
- Add voice recognizer and command router
- Map voice commands to existing actions from Task 1
- Add on-screen feedback:
  - recognized phrase
  - success/failure reason
- Record gameplay using only voice

### Task 3
- Implement shared command model:
  - action
  - target
  - source modality
  - timestamp
- Add fusion rule for deictic voice commands (`this`, `there`)
- Add fission conflict detector and resolution popup
- Record gameplay showing both fusion and fission

### Task 4
- Record final walkthrough:
  - game concept
  - Task 1 modality
  - Task 2 modality
  - fusion demonstration
  - fission demonstration
- Compress output video to under 20MB

---

## Suggested Unity scene structure

- `Player`
  - movement controller
  - camera + interaction ray
- `Interactables`
  - `Keycard`
  - `Generator`
  - `Door_Exit`
- `Managers`
  - `GameStateManager`
  - `InputManager` (keyboard/mouse + voice)
  - `ObjectiveManager`
  - `ConflictResolutionManager`
- `UI`
  - objective list
  - command feedback text
  - fission popup
  - end screen

---

## Acceptance checklist

- [ ] Game runs as desktop build
- [ ] Task is completable with only modality 1
- [ ] Task is completable with only modality 2
- [ ] Fusion action works (`unlock this` + pointing)
- [ ] Fission conflict is detected and resolved via UI
- [ ] Final video includes all required demonstrations
- [ ] Final video size is under 20MB

---

## Implemented repository scaffold (modular)

This repository now includes modular Unity-ready C# scaffolding:

- `/home/runner/work/MMI-2026-Project/MMI-2026-Project/UnityProject/Assets/Scripts/Core`
- `/home/runner/work/MMI-2026-Project/MMI-2026-Project/UnityProject/Assets/Scripts/Input`
- `/home/runner/work/MMI-2026-Project/MMI-2026-Project/UnityProject/Assets/Scripts/Managers`
- `/home/runner/work/MMI-2026-Project/MMI-2026-Project/UnityProject/Assets/Scripts/Interactables`
- `/home/runner/work/MMI-2026-Project/MMI-2026-Project/UnityProject/Assets/Scripts/UI`

And a practical setup guide:

- `/home/runner/work/MMI-2026-Project/MMI-2026-Project/docs/STEP_BY_STEP_IMPLEMENTATION.md`

## Final-product readiness updates

The scaffold now also includes:
- runtime center-screen raycast targeting for keyboard/mouse interaction
- `InteractableExecutionManager` to execute resolved commands on scene interactables
- win-state signaling (`ObjectiveManager.OnEscapeCompleted`) and `WinStateUI` support
- updated implementation guide steps for final validation, desktop build, and video compression
