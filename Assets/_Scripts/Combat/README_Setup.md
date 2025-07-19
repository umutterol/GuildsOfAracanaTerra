# Status Effect System Test Setup Guide

## Quick Setup (5 minutes)

### Step 1: Create Test Scene
1. Create a new scene called "StatusEffectTest"
2. Save it in `Assets/_Scenes/`

### Step 2: Add Required GameObjects

#### Create "Combat Systems" GameObject:
```
- Right-click in Hierarchy → Create Empty
- Rename to "Combat Systems"
- Add these components:
  ✓ TurnOrderSystem
  ✓ StatusEffectSystem  
  ✓ StatusEffectSystemTest
```

#### Create "UI Manager" GameObject:
```
- Right-click in Hierarchy → Create Empty
- Rename to "UI Manager"
- Add this component:
  ✓ StatusEffectTestUI
```

### Step 3: Configure Components

#### StatusEffectSystemTest Settings:
- ✅ **Debug Mode**: Enabled
- ✅ **Turn Order System**: Leave empty (auto-finds)
- ✅ **Status Effect System**: Leave empty (auto-finds)
- ✅ **UI Manager**: Leave empty (auto-finds)

#### StatusEffectTestUI Settings:
- ✅ **Auto Create UI**: Enabled
- ✅ **Status Text Position**: (10, -10)
- ✅ **Button Panel Position**: (10, -200)

### Step 4: Test the System

1. **Enter Play Mode**
2. **Check Console** for initialization messages:
   ```
   StatusEffectSystemTest: Systems found - TurnOrder: True, StatusEffect: True, UI: True
   StatusEffectSystemTest: Test system initialized!
   StatusEffectTestUI: Test UI created successfully!
   ```
3. **Use UI buttons** to test functionality

## Troubleshooting

### If you see "No StatusEffectSystem found":
- Make sure you added the `StatusEffectSystem` component to "Combat Systems"
- Check that the component is enabled

### If you see font errors:
- The system now uses `LegacyRuntime.ttf` (fixed)
- If you still see font errors, the UI will still work with default fonts

### If UI doesn't appear:
- Check that `StatusEffectTestUI` component is enabled
- Verify `Auto Create UI` is checked
- Look for "Test UI Canvas" in the hierarchy

### If buttons don't work:
- Check that `EventSystem` exists in the scene
- Verify the UI Manager is connected to the test script

## Expected Console Output

```
StatusEffectSystemTest: Systems found - TurnOrder: True, StatusEffect: True, UI: True
StatusEffectSystemTest: Created default test combatants
StatusEffectSystem: Registered Test Warrior for status effect management
StatusEffectSystem: Registered Test Mage for status effect management
StatusEffectSystem: Registered Test Rogue for status effect management
StatusEffectSystemTest: Test system initialized!
StatusEffectTestUI: Test UI created successfully!
StatusEffectTestUI: UI connected to test script!
```

## Testing Workflow

1. **Start Combat** → Initialize turn order
2. **Apply Burn** → Apply burn effect to test combatant
3. **End Turn** → Advance turns, see effects tick
4. **Watch Console** → See detailed debug output
5. **Clear Effects** → Remove all effects
6. **Reset Health** → Restore all health

## Manual Testing

If UI doesn't work, you can test manually:
- Right-click `StatusEffectSystemTest` component
- Select "Run Full Test" from context menu
- Watch console for results 