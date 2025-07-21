# Guilds of Arcana Terra

## Overview
Guilds of Arcana Terra is a party-based RPG project built in Unity. The project is now fully data-driven using ScriptableObjects for all character, class, and trait data. The architecture is designed for modularity, scalability, and ease of use for both developers and designers.

---

## 🏗️ Core Architecture
- **ScriptableObject-Driven:**
  - All characters, classes, and traits are defined as ScriptableObjects.
  - `CharacterDefinition` assets hold all data for each character, including prefab, class, and a list of trait ScriptableObjects.
- **Party Management:**
  - `PartyManager` manages the current party using `CharacterDefinition` assets.
  - `GuildManager` holds the full roster of available characters (as `CharacterDefinition` assets).
  - `PartyUIManager` provides a modern, slot-based party selection UI, pulling from the GuildManager.
- **Dungeon Flow:**
  - `DungeonRunManager` handles the dungeon gameplay loop: party selection → sequential encounters → results screen.
  - Enemy parties and encounters are defined using ScriptableObjects.

---

## 🎮 Gameplay Loop
1. **Party Selection:**
   - Player selects party members from the guild roster (using the Party Selection UI).
   - Each party member is assigned to a slot (up to 5).
2. **Dungeon Run:**
   - The selected party is passed to the DungeonRunManager.
   - The party progresses through a series of encounters, each defined by ScriptableObjects.
3. **Results Screen:**
   - After all encounters, a results screen displays victory/defeat and stats.

---

## 🛠️ Setup Instructions
- **Create CharacterDefinition assets** for each character, assigning prefab, class, and traits.
- **Add all CharacterDefinition assets to GuildManager** to make them available for party selection.
- **Assign GuildManager to PartyUIManager** in the Inspector.
- **Use PartyManagementUISetup** to auto-create and connect all UI elements for party selection and management.
- **Define dungeons and encounters** using ScriptableObjects for modular, designer-friendly content creation.

---

## 🧹 Clean Codebase
- All legacy test/debug scripts and old data models (CharacterData, CharacterCard, etc.) have been removed.
- All systems now use CharacterDefinition and ScriptableObject references exclusively.
- The codebase is ready for further gameplay, UI, and content expansion.

---

## 📋 For ChatGPT and Future Developers
- This README is up to date as of the latest refactor.
- See PROJECT_STATUS.md for a detailed progress log and next steps.
- All future development should continue using the ScriptableObject-driven architecture. 

## 🟢 Party Selection UI (Update)
- The Party Selection scene is now fully functional.
- Players can select and slot guild members into party slots.
- Validation ensures all slots are filled and no duplicate characters are assigned.
- The "Start Dungeon" button triggers party validation and attempts to load the dungeon scene.
- If the dungeon scene is not present, a warning is shown (expected until dungeon is implemented).
- Next step: Create and integrate the DungeonRunManager and dungeon gameplay loop. 