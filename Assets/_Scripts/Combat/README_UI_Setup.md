# UI Setup Guide - CombatantTest

## Problem
The CombatantTest script cannot see UI elements in the inspector for assignment.

## Solution
Use the new **CombatantTestUI** component that automatically finds and creates UI elements.

## Quick Setup

### Step 1: Add UI Manager
1. **Create an empty GameObject** called "UI Manager"
2. **Add CombatantTestUI component** to it
3. **The UI manager will automatically** find or create all needed UI elements

### Step 2: Add CombatantTest
1. **Create an empty GameObject** called "CombatantTest"
2. **Add CombatantTest component** to it
3. **Assign the UI Manager** in the inspector (drag the UI Manager GameObject)

### Step 3: Auto-Create UI (Optional)
If you want to create the UI automatically:
1. **Select the UI Manager GameObject**
2. **Right-click in inspector** → "Create Complete UI"
3. **This creates** all buttons, text, and dropdowns automatically

## Alternative: Manual UI Creation

If you prefer to create UI manually:

### Step 1: Create Canvas
1. **Right-click in Hierarchy** → UI → Canvas
2. **This creates** Canvas with EventSystem

### Step 2: Create UI Elements
Create these GameObjects as children of Canvas:

```
Canvas/
├── StatusText (Text component)
├── SkillText (Text component)
├── LoadClassesButton (Button component)
├── StartCombatButton (Button component)
├── EndTurnButton (Button component)
├── UseSkillButton (Button component)
├── ApplyBurnButton (Button component)
├── ResetHealthButton (Button component)
├── SkillDropdown (Dropdown component)
└── TargetDropdown (Dropdown component)
```

### Step 3: Assign in Inspector
1. **Select CombatantTest GameObject**
2. **Drag each UI element** to the corresponding field in inspector

## Context Menu Options

The CombatantTestUI component provides context menu options:

- **"Find UI Elements"** - Searches for existing UI elements by name
- **"Create Complete UI"** - Creates all UI elements automatically

## Debug Information

The UI manager will log:
- Which UI elements were found
- Which UI elements were created
- Any missing elements

## Common Issues

### "No Canvas found"
- **Solution**: Create a Canvas (UI → Canvas)
- **Or**: Let the UI manager create one automatically

### "UI elements not visible in inspector"
- **Solution**: Use the UI manager approach
- **Or**: Ensure UI elements are children of Canvas

### "Buttons not working"
- **Solution**: Check that EventSystem exists in scene
- **Or**: Let UI manager create complete setup

## Recommended Approach

**Use the UI Manager approach** - it's the easiest and most reliable:

1. Add CombatantTestUI component
2. Add CombatantTest component
3. Assign UI Manager reference
4. Everything else is automatic!

The UI manager handles all the complexity of finding, creating, and positioning UI elements. 