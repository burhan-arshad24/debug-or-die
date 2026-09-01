# DEBUG OR DIE

A first-person horror game developed as an OOP project.

## About the Game

Debug or Die is a first-person horror game where the player
must complete a series of tasks inside a dark and mysterious house.

The power is completely off, making the environment dangerous
and difficult to navigate.

The player must explore the house, find important objects,
solve simple problems, and restore the electricity.

Every task brings the player closer to completing the assignment
and escaping the terrifying situation.

The game combines exploration, interaction, task management,
horror elements, and basic puzzle mechanics.

---

## Project Information

Project Name: Debug or Die

Project Type: First-Person Horror Game

Academic Project: 3rd Semester OOP Project

Engine: Unity

Programming Language: C#

Developer: Burhan Arshad

---

## Main Objective

The main objective is to complete all assigned tasks while
surviving inside the horror house.

The player starts by completing an assignment-related task.

As the game progresses, the player must explore different
areas of the house and interact with various objects.

The final objective is to restore the power and complete
the remaining computer-based tasks.

---

## Gameplay

The game starts with an opening cutscene.

The player is automatically moved through different locations
while the story is displayed on screen.

After the cutscene, the player receives the first task.

The player can move around the environment and interact with
objects using the interaction system.

Each task provides a specific objective.

Completing one task unlocks the next part of the game.

A timer system adds additional pressure to the gameplay.

If the timer reaches zero, the player receives a Game Over.

The player can retry the current task after losing.

---

## Task System

The game is divided into multiple tasks.

### Task 0 - Torch

The player starts in a dark environment.

A torch is automatically added to the player's inventory.

The player must turn on the torch.

Once the torch is turned on, Task 0 is completed.

The next objective is displayed on the HUD.

---

### Task 1 - Assignment

The player must find a note inside the house.

The note provides information about the next objective.

After reading the note, the player must find a semicolon object.

Once the semicolon is collected, the player can continue
towards the PC for submission.

This task introduces the basic exploration and interaction
mechanics.

---

### Task 2 - Variable Puzzle

The player must find the correct variable name.

Several variable objects are placed inside the environment.

Only one variable is correct.

After finding the correct variable, an office door becomes locked.

The player must then find a key.

Finding the correct key completes the task objective.

---

### Task 3 - Hidden Information

The player must find a secret chest.

A crowbar is required to open the chest.

After opening the chest, a hidden book becomes available.

The player must find a UV lamp.

The UV lamp requires a battery to function.

After finding the battery, the player can reveal hidden
information from the book.

The discovered information gives the player the next objective.

---

### Task 4 - Restore Electricity

The final major task focuses on restoring electricity.

The player must first find an access card.

The access card allows access to the library.

Inside the library, the player must find a book.

After finding the book, the player must find a screwdriver.

The screwdriver is required to open the vent.

The player then enters the vent and searches for a fuse.

The fuse is required to restore power.

After restoring electricity, the player can interact with
the PC and complete the final objective.

---

## Horror Elements

The game takes place in a dark environment.

Lighting is used to create a disturbing atmosphere.

The horror lighting mode is activated at important points
during the game.

The player must explore dark areas using the torch.

The environment contains locked doors, hidden objects,
dark rooms, and unexpected events.

The game also includes scripted horror moments and jumpscare
events.

---

## Core Systems

The project contains several interconnected systems.

- Player movement
- First-person camera
- Mouse look system
- Interaction system
- Inventory system
- Task management
- Task timers
- Game Over system
- Retry system
- Main menu
- Pause menu
- Cutscene system
- Lighting system
- Door interaction
- Pickup system
- PC interaction
- Chest interaction
- Vent interaction
- Power restoration system

---

## Object-Oriented Programming

The project was developed as an OOP-based Unity project.

Different gameplay features are separated into individual
C# classes.

Examples include:

- TaskManager
- CutsceneManager
- MouseLook
- PlayerControlManager
- Inventory
- TorchManager
- NoteInteractable
- KeyPickup
- ChestInteraction
- BookInteraction
- BatteryPickup
- CrowbarPickup
- VentInteraction
- PowerPanel
- PCInteractable

This structure makes the project easier to manage and extend.

Each class is responsible for a specific gameplay feature.

---

## TaskManager

The TaskManager controls the overall task progression.

It stores the state of every task and controls task completion.

It also manages:

- Current task
- Task timers
- Objectives
- Task completion
- Game Over
- Retry system
- Task objects
- Inventory-related progression

This keeps the main gameplay progression organized.

---

## Inventory System

The inventory system allows the player to collect important
objects during gameplay.

Items such as the torch, access card, keys, battery, and other
required objects can be managed through the inventory.

Inventory requirements are also used to control progression.

---

## Cutscene System

The CutsceneManager controls the opening sequence.

It can:

- Move the player through waypoints
- Display story text
- Open doors automatically
- Control camera behavior
- Change lighting
- Lock player controls
- Skip the cutscene
- Start the first task

The cutscene introduces the story before gameplay begins.

---

## UI System

The game includes several UI elements.

The HUD displays the current objective.

A timer displays the remaining time for the active task.

The project also contains:

- Main Menu
- Pause Menu
- Game Over Screen
- Cutscene Text
- Objective Display

These systems provide the player with important gameplay
information.

---

## Controls

W / A / S / D
    Move the player.

Mouse
    Look around.

E
    Interact with objects.

P
    Pause or resume the game.

Space
    Skip the opening cutscene.

---

## Technologies Used

Unity
    Game development engine.

C#
    Main programming language.

Unity Input System / Input
    Player and interaction controls.

TextMeshPro
    UI and dialogue text.

Unity Coroutines
    Cutscenes, movement, timers, and sequences.

Unity Lighting
    Horror atmosphere and power-related effects.

---

## Project Structure

Assets/
├── Scenes/
├── Scripts/
├── Prefabs/
├── Materials/
├── Models/
├── Textures/
├── Audio/
└── UI/

Scripts are separated according to their gameplay
responsibilities.

This makes the project easier to understand and maintain.

---

## Learning Outcomes

This project was created to apply Object-Oriented Programming
concepts in a practical Unity environment.

The project provided experience with:

- Classes and objects
- Encapsulation
- Methods
- References between objects
- State management
- Event-based interactions
- Coroutines
- Unity components
- Game architecture
- Debugging
- Gameplay logic

---

## Future Improvements

Possible future improvements include:

- More horror events
- More puzzles
- Additional levels
- Better enemy AI
- Improved animations
- More detailed environments
- Advanced inventory UI
- Save and load system
- More sound effects
- Multiple endings

---

## Developer

Built by 'Burhan Arshad'

3rd Semester
Object-Oriented Programming Project

---

## Conclusion

Debug or Die is a first-person horror game focused on
exploration, task completion, and problem solving.

The player must complete multiple objectives inside a dark
house while dealing with limited time and horror elements.

The project demonstrates how Object-Oriented Programming
concepts can be applied to a complete Unity game.

The main goal was not only to create a playable horror game,
but also to apply programming concepts in a practical project.
