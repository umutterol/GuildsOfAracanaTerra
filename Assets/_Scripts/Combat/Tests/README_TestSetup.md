# Combat System Test Setup Guide

## Current Issues and Solutions

### 1. Missing Script References
**Issue**: "The referenced script (Unknown) on this Behaviour is missing!"

**Solution**: 
- Check that all script files are properly saved and compiled
- Ensure no script files have been moved or renamed
- Reimport the Assets folder if needed

### 2. Missing StatusEffectSystem
**Issue**: "❌ StatusEffectSystem not found"

**Solution**: 
- The test will automatically create a StatusEffectSystem if one doesn't exist
- For manual setup, add a GameObject with StatusEffectSystem component to the scene

### 3. Class and Trait Lookup Failures
**Issue**: "Class with ID X not found! Available range: 0--1"

**Solution**: 
- In the CharacterDataManager component, assign the available class and trait ScriptableObjects to the respective lists
- Ensure the ScriptableObject assets exist in the project
- The lists should contain:
  - `availableClasses`: CharacterClassDefinition assets (Warrior, Mage, etc.)
  - `availableTraits`: IRLTraitSO assets (AFKFarmerTrait, DramaQueenTrait, etc.)

### 4. Null Skill Definitions
**Issue**: "NullReferenceException in CreateSkillFromDefinition"

**Solution**:
- Ensure CharacterClassDefinition assets have their skill arrays properly populated
- Check that BasicAttack and ActiveSkills arrays contain valid SkillDefinition assets
- The system now has null checks to prevent crashes

## Test Scene Setup Checklist

### Required Components:
- [ ] Combatant GameObjects with Combatant scripts
- [ ] SkillSet components on combatants
- [ ] StatusEffectManager components on combatants (auto-created)
- [ ] StatusEffectSystem in scene (auto-created if missing)
- [ ] CharacterDataManager with populated class/trait lists
- [ ] TurnOrderSystem (optional, for full combat simulation)

### Required Assets:
- [ ] CharacterClassDefinition ScriptableObjects (Warrior, Mage, etc.)
- [ ] IRLTraitSO ScriptableObjects (AFKFarmerTrait, etc.)
- [ ] SkillDefinition ScriptableObjects for each skill
- [ ] characters.json file in Resources folder

## Running Tests

1. Open the test scene (TestArena.unity)
2. Ensure all required components are assigned in the inspector
3. Press Play to run the automated tests
4. Use the UI buttons to test individual skills
5. Check the console for detailed test results

## Expected Test Results

When properly set up, you should see:
- ✅ All combatants loaded successfully
- ✅ Skills executing without errors
- ✅ Damage/healing calculations working
- ✅ Status effects applying (if StatusEffectSystem is present)
- ✅ Character data loading from JSON
- ✅ Class and trait lookups working

## Troubleshooting

If tests are still failing:
1. Check the Unity Console for specific error messages
2. Verify all ScriptableObject assets are properly configured
3. Ensure all required components are present in the scene
4. Check that the characters.json file exists and is valid JSON
5. Rebuild the project if script compilation issues persist 