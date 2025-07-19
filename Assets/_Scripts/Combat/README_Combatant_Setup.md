# Combatant System Setup Guide

## Overview
The new Combatant system integrates with CharacterClassDefinition ScriptableObjects to provide a complete turn-based combat experience. Combatants automatically load their skills and stats from class definitions and integrate with the TurnOrderSystem, SkillSystem, and StatusEffectSystem.

## Components Created

### 1. Combatant.cs
- **Enhanced combatant** that loads CharacterClassDefinition ScriptableObjects
- **Automatic skill loading** from class definitions
- **Level scaling** for stats (10% increase per level)
- **Full integration** with existing combat systems
- **Rich event system** for UI updates

### 2. CombatantTest.cs
- **Complete test system** for the new Combatant
- **UI integration** with buttons, dropdowns, and status displays
- **Class loading** and combat management
- **Skill usage** and target selection

## Setup Instructions

### Step 1: Create Class Definitions
1. **In Unity Editor**: Go to `Guilds of Arcana Terra → Create Class Definitions`
2. **This creates**: 5 `.asset` files in `Assets/_ScriptableObjects/Classes/`
   - Warrior.asset
   - Mage.asset
   - Cleric.asset
   - Rogue.asset
   - Ranger.asset

### Step 2: Create Test Scene
1. **Create a new scene** or use existing test scene
2. **Add empty GameObjects** for each combatant:
   ```
   - Warrior (Empty GameObject)
   - Mage (Empty GameObject)
   - Cleric (Empty GameObject)
   ```

### Step 3: Add Combatant Components
1. **Add Combatant component** to each GameObject
2. **Assign class definitions** in the inspector:
   - Warrior → Warrior.asset
   - Mage → Mage.asset
   - Cleric → Cleric.asset

### Step 4: Add Required Systems
1. **Add TurnOrderSystem** to a GameObject in the scene
2. **Add StatusEffectSystem** to a GameObject in the scene
3. **Add SkillSystemManager** to a GameObject in the scene

### Step 5: Setup Test UI
1. **Create Canvas** with UI elements:
   ```
   - Status Text (Text component)
   - Skill Text (Text component)
   - Load Classes Button
   - Start Combat Button
   - End Turn Button
   - Use Skill Button
   - Apply Burn Button
   - Reset Health Button
   - Skill Dropdown
   - Target Dropdown
   ```

2. **Add CombatantTest component** to a GameObject
3. **Assign all references** in the inspector

## Usage

### Loading Classes
- **Click "Load Classes"** to load CharacterClassDefinition data
- **Combatants automatically** get their stats and skills
- **Skills are loaded** into SkillSet components

### Starting Combat
- **Click "Start Combat"** to begin turn-based combat
- **Turn order** is determined by AGI stat
- **Current combatant** is highlighted in UI

### Using Skills
- **Select skill** from dropdown (shows available skills only)
- **Click "Use Skill"** to use the selected skill
- **Cooldowns** are automatically managed

### Status Effects
- **Select target** from dropdown
- **Click "Apply Burn"** to test status effects
- **Effects tick** automatically on turn start

## Key Features

### Automatic Integration
- ✅ **TurnOrderSystem**: Combatants participate in turn order
- ✅ **SkillSystem**: Skills are managed with cooldowns
- ✅ **StatusEffectSystem**: Status effects can be applied
- ✅ **Event System**: Rich events for UI updates

### Class-Based Design
- ✅ **ScriptableObject-driven**: No coding required for new classes
- ✅ **Level scaling**: Stats increase with level
- ✅ **Skill validation**: Ensures classes are complete
- ✅ **Designer-friendly**: Easy to modify in Unity Inspector

### Combat Features
- ✅ **Health management**: Current/max health with percentages
- ✅ **Damage calculation**: Defense reduces incoming damage
- ✅ **Skill cooldowns**: Automatic cooldown management
- ✅ **Turn events**: Start/end turn callbacks
- ✅ **Death handling**: Automatic death detection

## Example Workflow

1. **Load Classes**: Combatants get their skills and stats
2. **Start Combat**: Turn order is established
3. **Select Skill**: Choose from available skills
4. **Use Skill**: Skill is used, cooldown is set
5. **End Turn**: Next combatant's turn begins
6. **Apply Effects**: Status effects tick automatically

## Debug Information

The system provides extensive debug logging:
- **Class loading**: Shows loaded skills and stats
- **Combat events**: Turn changes, skill usage
- **Damage/healing**: Detailed combat information
- **System integration**: Status effect and cooldown updates

## Troubleshooting

### Common Issues
1. **"No class definition assigned"**: Assign CharacterClassDefinition in inspector
2. **"No SkillSet component"**: Component is auto-added in Awake()
3. **"No StatusEffectManager"**: Component is auto-added in Awake()
4. **Skills not loading**: Check if class definition is valid

### Validation
- **Class definitions** are validated for completeness
- **Required components** are auto-created if missing
- **Event subscriptions** are handled automatically
- **Null checks** prevent runtime errors

## Next Steps

The Combatant system is ready for:
- **Actual skill effects**: Implement damage/healing logic
- **Target selection**: Add proper targeting system
- **Animation integration**: Connect to character animations
- **Sound effects**: Add audio feedback
- **Visual effects**: Add particle effects for skills 