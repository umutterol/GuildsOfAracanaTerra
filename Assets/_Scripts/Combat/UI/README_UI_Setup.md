# Combat UI Setup Guide

## 🎮 **How to Connect UI Elements to Combat System**

### **Step 1: Create the UI Manager**
1. Create a new GameObject in your scene named "CombatUIManager"
2. Add the `CombatUIManager` component to it

### **Step 2: Set Up Basic UI Layout**

#### **Required UI Elements:**

**📊 Combatant Info Panels:**
- **Player Info Panel** (GameObject with UI elements)
  - Player Name Text (TextMeshProUGUI)
  - Player Health Text (TextMeshProUGUI) 
  - Player Health Bar (Slider)
  - Player Stats Text (TextMeshProUGUI)
  - Player Class Text (TextMeshProUGUI)

- **Enemy Info Panel** (GameObject with UI elements)
  - Enemy Name Text (TextMeshProUGUI)
  - Enemy Health Text (TextMeshProUGUI)
  - Enemy Health Bar (Slider)
  - Enemy Stats Text (TextMeshProUGUI)

**⚔️ Skill Buttons:**
- 4 Skill Button GameObjects (Button components)
- Each button should have:
  - Button component
  - TextMeshProUGUI for skill name
  - TextMeshProUGUI for cooldown (optional)

**🎯 Action Buttons:**
- End Turn Button (Button)
- Auto Attack Button (Button)
- Heal Button (Button)
- Reset Button (Button)

**📝 Combat Log:**
- Combat Log Text (TextMeshProUGUI)
- Scroll Rect (ScrollRect component)

**🔄 Turn Info:**
- Turn Info Text (TextMeshProUGUI)
- Turn Order Text (TextMeshProUGUI)

**✨ Status Effects:**
- Player Status Effects Container (Transform)
- Enemy Status Effects Container (Transform)
- Status Effect Prefab (GameObject with TextMeshProUGUI)

### **Step 3: Connect Everything in Inspector**

#### **Combat System Section:**
- **Turn Order System**: Drag your TurnOrderSystem GameObject
- **Status Effect System**: Drag your StatusEffectSystem GameObject

#### **Combatants Section:**
- **Player Combatant**: Drag your player Combatant GameObject
- **Enemy Combatant**: Drag your enemy Combatant GameObject

#### **Combatant Info Panels:**
- **Player Info Panel**: Drag the GameObject containing player UI elements
- **Enemy Info Panel**: Drag the GameObject containing enemy UI elements

#### **Player Info UI:**
- **Player Name Text**: Drag the TextMeshProUGUI component
- **Player Health Text**: Drag the TextMeshProUGUI component
- **Player Health Bar**: Drag the Slider component
- **Player Stats Text**: Drag the TextMeshProUGUI component
- **Player Class Text**: Drag the TextMeshProUGUI component

#### **Enemy Info UI:**
- **Enemy Name Text**: Drag the TextMeshProUGUI component
- **Enemy Health Text**: Drag the TextMeshProUGUI component
- **Enemy Health Bar**: Drag the Slider component
- **Enemy Stats Text**: Drag the TextMeshProUGUI component

#### **Skill Buttons:**
- **Skill Buttons**: Drag your 4 skill button GameObjects
- **Skill Button Texts**: Drag the TextMeshProUGUI components from each button
- **Skill Cooldown Texts**: Drag the TextMeshProUGUI components for cooldowns

#### **Action Buttons:**
- **End Turn Button**: Drag your end turn button
- **Auto Attack Button**: Drag your auto attack button
- **Heal Button**: Drag your heal button
- **Reset Button**: Drag your reset button

#### **Combat Log:**
- **Combat Log Text**: Drag the TextMeshProUGUI component
- **Combat Log Scroll Rect**: Drag the ScrollRect component

#### **Status Effects:**
- **Player Status Effects Container**: Drag the Transform for player effects
- **Enemy Status Effects Container**: Drag the Transform for enemy effects
- **Status Effect Prefab**: Drag a prefab with TextMeshProUGUI for effect display

#### **Turn Info:**
- **Turn Info Text**: Drag the TextMeshProUGUI component
- **Turn Order Text**: Drag the TextMeshProUGUI component

### **Step 4: Quick Setup Example**

#### **Minimal UI Setup:**
If you want a quick test, you only need:

1. **CombatUIManager** GameObject with the component
2. **Player Combatant** and **Enemy Combatant** assigned
3. **One TextMeshProUGUI** for combat log
4. **One Button** for testing skills

The system will auto-create missing systems and work with minimal UI.

### **Step 5: Test the Connection**

#### **What Should Happen:**
1. **Health bars** update automatically when combatants take damage
2. **Skill buttons** show available skills and cooldowns
3. **Combat log** displays all actions and events
4. **Buttons** execute skills when clicked
5. **Turn system** manages player/enemy turns

#### **Testing Checklist:**
- [ ] Health bars update when damage is taken
- [ ] Skill buttons show correct skill names
- [ ] Buttons are disabled when skills are on cooldown
- [ ] Combat log shows action messages
- [ ] Turn information updates correctly
- [ ] Status effects display (if any are applied)

### **Step 6: Customization**

#### **UI Styling:**
- Modify the UI elements' appearance in the Inspector
- Change colors, fonts, sizes as needed
- The system will work with any UI styling

#### **Additional Features:**
- Add animations for damage/healing
- Create custom status effect displays
- Add sound effects for button clicks
- Implement tooltips for skills

### **Troubleshooting:**

#### **Common Issues:**
1. **Buttons not working**: Check that Combatants are assigned
2. **UI not updating**: Ensure autoUpdateUI is enabled
3. **Missing text**: Verify TextMeshProUGUI components are assigned
4. **Skills not showing**: Check that Combatants have skills loaded

#### **Debug Tips:**
- Enable debug mode in the CombatUIManager
- Check the console for error messages
- Use the ForceUpdateUI() method to manually refresh
- Verify all required components are present

### **Advanced Features:**

#### **Dynamic Skill Loading:**
The system automatically loads skills from your Combatant's SkillSet and displays them on buttons.

#### **Real-time Updates:**
UI updates automatically every 0.1 seconds (configurable) to show current health, cooldowns, and status.

#### **Event-driven Updates:**
UI also updates immediately when combat events occur (damage, healing, turn changes).

#### **Turn-based Combat:**
The system manages turn order and prevents actions during enemy turns.

This UI system provides a complete interface for your combat mechanics and can be easily customized for your specific needs! 