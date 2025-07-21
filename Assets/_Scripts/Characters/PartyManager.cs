using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using GuildsOfArcanaTerra.Combat;
using System;
using GuildsOfArcanaTerra.ScriptableObjects.Classes;
using GuildsOfArcanaTerra.Traits;

namespace GuildsOfArcanaTerra.Characters
{
    /// <summary>
    /// Manages a party of characters for combat and exploration, now with slot-based assignment
    /// </summary>
    public class PartyManager : MonoBehaviour
    {
        [Header("Party Configuration")]
        [SerializeField] private int maxPartySize = 5;
        [SerializeField] private CharacterDefinition[] partySlots = new CharacterDefinition[5]; // Each index is a slot (0-4)
        
        [Header("Combat Integration")]
        [SerializeField] private List<Combatant> activeCombatants = new List<Combatant>();
        
        [Header("References")]
        [SerializeField] private CharacterDataManager characterDataManager;
        
        [Header("Events")]
        public UnityEvent<PartyManager> OnPartyChanged;
        public UnityEvent<CharacterDefinition> OnCharacterAdded;
        public UnityEvent<CharacterDefinition> OnCharacterRemoved;
        public UnityEvent<PartyManager> OnPartyFormationChanged;
        
        [Header("Debug")]
        [SerializeField] private bool debugMode = false;
        
        // Properties
        public int MaxPartySize => maxPartySize;
        public int PartySize => partySlots.Count(c => c != null);
        public bool IsPartyFull => PartySize >= maxPartySize;
        public bool IsPartyEmpty => PartySize == 0;
        public CharacterDefinition[] PartySlots => (CharacterDefinition[])partySlots.Clone();
        public List<Combatant> ActiveCombatants => new List<Combatant>(activeCombatants);

        // Assign a character to a specific slot (0-4)
        public bool AssignCharacterToSlot(CharacterDefinition character, int slotIndex)
        {
            if (character == null || slotIndex < 0 || slotIndex >= maxPartySize)
                return false;
            if (partySlots.Contains(character))
            {
                Debug.LogWarning($"PartyManager: Character {character.characterName} is already assigned to a slot");
                return false;
            }
            partySlots[slotIndex] = character;
            if (debugMode)
                Debug.Log($"PartyManager: Assigned {character.characterName} to slot {slotIndex}");
            OnCharacterAdded?.Invoke(character);
            OnPartyChanged?.Invoke(this);
            OnPartyFormationChanged?.Invoke(this);
            return true;
        }

        // Remove a character from a specific slot
        public bool RemoveCharacterFromSlot(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= maxPartySize)
                return false;
            var character = partySlots[slotIndex];
            if (character == null)
                return false;
            partySlots[slotIndex] = null;
            if (debugMode)
                Debug.Log($"PartyManager: Removed {character.characterName} from slot {slotIndex}");
            OnCharacterRemoved?.Invoke(character);
            OnPartyChanged?.Invoke(this);
            OnPartyFormationChanged?.Invoke(this);
            return true;
        }

        // Get the character in a specific slot
        public CharacterDefinition GetCharacterInSlot(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= maxPartySize)
                return null;
            return partySlots[slotIndex];
        }

        // Get all assigned party members (non-null)
        public List<CharacterDefinition> GetPartyDataList()
        {
            return partySlots.Where(c => c != null).ToList();
        }

        // Clear the entire party
        public void ClearParty()
        {
            for (int i = 0; i < partySlots.Length; i++)
            {
                RemoveCharacterFromSlot(i);
            }
        }

        /// <summary>
        /// Add a character to the party
        /// </summary>
        public bool AddCharacter(CharacterDefinition character)
        {
            if (character == null)
            {
                Debug.LogWarning("PartyManager: Cannot add null character to party");
                return false;
            }
            
            if (IsPartyFull)
            {
                Debug.LogWarning($"PartyManager: Party is full (max {maxPartySize} members)");
                return false;
            }
            
            if (partySlots.Contains(character))
            {
                Debug.LogWarning($"PartyManager: Character {character.characterName} is already in the party");
                return false;
            }
            
            // Find the first available slot to add the character
            for (int i = 0; i < maxPartySize; i++)
            {
                if (partySlots[i] == null)
                {
                    partySlots[i] = character;
                    if (debugMode)
                    {
                        Debug.Log($"PartyManager: Added {character.characterName} to slot {i} (Size: {PartySize}/{maxPartySize})");
                    }
                    OnCharacterAdded?.Invoke(character);
                    OnPartyChanged?.Invoke(this);
                    OnPartyFormationChanged?.Invoke(this);
                    return true;
                }
            }
            
            return false; // Should not happen if IsPartyFull is true
        }
        
        /// <summary>
        /// Remove a character from the party
        /// </summary>
        public bool RemoveCharacter(CharacterDefinition character)
        {
            if (character == null) return false;
            
            // Find the character in any slot
            for (int i = 0; i < maxPartySize; i++)
            {
                if (partySlots[i] == character)
                {
                    RemoveCharacterFromSlot(i);
                    return true;
                }
            }
            
            Debug.LogWarning($"PartyManager: Character {character.characterName} not found in any slot.");
            return false;
        }
        
        /// <summary>
        /// Remove a character by name
        /// </summary>
        public bool RemoveCharacter(string characterName)
        {
            for (int i = 0; i < maxPartySize; i++)
            {
                var character = partySlots[i];
                if (character != null && character.characterName == characterName)
                {
                    RemoveCharacterFromSlot(i);
                    return true;
                }
            }
            return false;
        }
        
        /// <summary>
        /// Get a character by name
        /// </summary>
        public CharacterDefinition GetCharacter(string characterName)
        {
            for (int i = 0; i < maxPartySize; i++)
            {
                var character = partySlots[i];
                if (character != null && character.characterName == characterName)
                {
                    return character;
                }
            }
            return null;
        }
        
        /// <summary>
        /// Get a character by index
        /// </summary>
        public CharacterDefinition GetCharacter(int index)
        {
            if (index >= 0 && index < maxPartySize)
            {
                return partySlots[index];
            }
            return null;
        }
        
        /// <summary>
        /// Check if a character is in the party
        /// </summary>
        public bool HasCharacter(CharacterDefinition character)
        {
            for (int i = 0; i < maxPartySize; i++)
            {
                if (partySlots[i] == character)
                {
                    return true;
                }
            }
            return false;
        }
        
        /// <summary>
        /// Check if a character is in the party by name
        /// </summary>
        public bool HasCharacter(string characterName)
        {
            for (int i = 0; i < maxPartySize; i++)
            {
                var character = partySlots[i];
                if (character != null && character.characterName == characterName)
                {
                    return true;
                }
            }
            return false;
        }
        
        /// <summary>
        /// Get all alive party members
        /// </summary>
        public List<CharacterDefinition> GetAliveMembers()
        {
            return partySlots.Where(c => c != null && c.level > 0).ToList(); // Simple alive check
        }
        
        /// <summary>
        /// Get all dead party members
        /// </summary>
        public List<CharacterDefinition> GetDeadMembers()
        {
            return partySlots.Where(c => c != null && c.level <= 0).ToList(); // Simple dead check
        }
        
        /// <summary>
        /// Get the party's total level
        /// </summary>
        public int GetTotalPartyLevel()
        {
            return partySlots.Where(c => c != null).Sum(c => c.level);
        }
        
        /// <summary>
        /// Get the party's average level
        /// </summary>
        public float GetAveragePartyLevel()
        {
            if (PartySize == 0) return 0f;
            return (float)GetTotalPartyLevel() / PartySize;
        }
        
        /// <summary>
        /// Get party composition summary
        /// </summary>
        public string GetPartySummary()
        {
            if (PartySize == 0)
            {
                return "Empty Party";
            }
            
            var summary = $"Party ({PartySize}/{maxPartySize}):\n";
            
            for (int i = 0; i < maxPartySize; i++)
            {
                var character = partySlots[i];
                if (character != null)
                {
                    var status = character.level > 0 ? "Alive" : "Dead";
                    // For class display:
                    string className = character.characterClass != null ? character.characterClass.ClassName : "Unknown";
                    // For trait display:
                    string traitName = (character.traits != null && character.traits.Count > 0) ? character.traits[0].TraitName : "None";
                    summary += $"  - Slot {i}: {character.characterName} (Lv.{character.level}) - {status}\n";
                }
                else
                {
                    summary += $"  - Slot {i}: Empty\n";
                }
            }
            
            summary += $"Average Level: {GetAveragePartyLevel():F1}";
            
            return summary;
        }
        
        /// <summary>
        /// Get party by class type
        /// </summary>
        public List<CharacterDefinition> GetMembersByClass(CharacterClassDefinition classSO)
        {
            return partySlots.Where(c => c != null && c.characterClass == classSO).ToList();
        }
        
        /// <summary>
        /// Get party by trait type
        /// </summary>
        public List<CharacterDefinition> GetMembersByTrait(IRLTraitSO traitSO)
        {
            return partySlots.Where(c => c != null && c.traits.Any(t => t == traitSO)).ToList();
        }
        
        /// <summary>
        /// Check if party has a specific class
        /// </summary>
        public bool HasClass(CharacterClassDefinition classSO)
        {
            return partySlots.Any(c => c != null && c.characterClass == classSO);
        }
        
        /// <summary>
        /// Check if party has a specific trait
        /// </summary>
        public bool HasTrait(IRLTraitSO traitSO)
        {
            return partySlots.Any(c => c != null && c.traits.Any(t => t == traitSO));
        }
        
        /// <summary>
        /// Get the lowest level character in the party
        /// </summary>
        public CharacterDefinition GetLowestLevelMember()
        {
            return partySlots.Where(c => c != null).OrderBy(c => c.level).FirstOrDefault();
        }
        
        /// <summary>
        /// Get the highest level character in the party
        /// </summary>
        public CharacterDefinition GetHighestLevelMember()
        {
            return partySlots.Where(c => c != null).OrderByDescending(c => c.level).FirstOrDefault();
        }
        
        /// <summary>
        /// Get the lowest HP character in the party (for healing priority)
        /// </summary>
        public CharacterDefinition GetLowestHPMember()
        {
            return partySlots.Where(c => c != null && c.level > 0).OrderBy(c => c.level).FirstOrDefault(); // Using level as proxy for HP
        }
        
        /// <summary>
        /// Set the maximum party size
        /// </summary>
        public void SetMaxPartySize(int newMaxSize)
        {
            if (newMaxSize < 1)
            {
                Debug.LogWarning("PartyManager: Max party size must be at least 1");
                return;
            }
            
            maxPartySize = newMaxSize;
            
            // Remove excess members if new size is smaller
            while (PartySize > maxPartySize)
            {
                var removedCharacter = partySlots[PartySize - 1];
                RemoveCharacterFromSlot(PartySize - 1);
            }
            
            if (debugMode)
            {
                Debug.Log($"PartyManager: Max party size set to {maxPartySize}");
            }
            
            OnPartyChanged?.Invoke(this);
        }
        
        /// <summary>
        /// Load party from saved data
        /// </summary>
        public void LoadParty(List<CharacterDefinition> savedParty)
        {
            if (savedParty == null)
            {
                Debug.LogWarning("PartyManager: Cannot load null party data");
                return;
            }
            
            ClearParty();
            
            foreach (var character in savedParty)
            {
                if (PartySize < maxPartySize)
                {
                    AddCharacter(character);
                }
                else
                {
                    Debug.LogWarning($"PartyManager: Cannot add {character.characterName} - party is full");
                    break;
                }
            }
            
            if (debugMode)
            {
                Debug.Log($"PartyManager: Loaded party with {PartySize} members");
            }
        }
        
        /// <summary>
        /// Save current party data
        /// </summary>
        public List<CharacterDefinition> SaveParty()
        {
            return new List<CharacterDefinition>(partySlots.Where(c => c != null).ToList());
        }
        
        #region Combat Integration
        
        /// <summary>
        /// Create combatants for all party members
        /// </summary>
        public List<Combatant> CreateCombatants()
        {
            activeCombatants.Clear();
            
            foreach (var characterDefinition in partySlots.Where(c => c != null))
            {
                var combatant = CreateCombatantFromCharacter(characterDefinition);
                if (combatant != null)
                {
                    activeCombatants.Add(combatant);
                }
            }
            
            if (debugMode)
            {
                Debug.Log($"PartyManager: Created {activeCombatants.Count} combatants for party");
            }
            
            return new List<Combatant>(activeCombatants);
        }
        
        /// <summary>
        /// Create a single combatant from character data
        /// </summary>
        private Combatant CreateCombatantFromCharacter(CharacterDefinition characterDefinition)
        {
            // Create a new GameObject for the combatant
            var combatantGO = new GameObject($"Combatant_{characterDefinition.characterName}");
            var combatant = combatantGO.AddComponent<Combatant>();
            
            // Initialize the combatant with character data using reflection
            InitializeCombatantFromCharacterData(combatant, characterDefinition);
            
            if (debugMode)
            {
                Debug.Log($"PartyManager: Created combatant for {characterDefinition.characterName}");
            }
            
            return combatant;
        }
        
        /// <summary>
        /// Initialize combatant with character data using reflection
        /// </summary>
        private void InitializeCombatantFromCharacterData(Combatant combatant, CharacterDefinition characterDefinition)
        {
            try
            {
                // Set character name using reflection
                var nameField = typeof(Combatant).GetField("characterName", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (nameField != null)
                {
                    nameField.SetValue(combatant, characterDefinition.characterName);
                }
                
                // Set level using reflection
                var levelField = typeof(Combatant).GetField("level", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (levelField != null)
                {
                    levelField.SetValue(combatant, characterDefinition.level);
                }
                
                // Set class definition if available (use SO reference directly)
                if (characterDefinition.characterClass != null)
                {
                    var classField = typeof(Combatant).GetField("classDefinition", 
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    if (classField != null)
                    {
                        classField.SetValue(combatant, characterDefinition.characterClass);
                    }
                }
                
                // Load the class definition to set up skills and stats
                // Only call LoadClassDefinition if we have a class definition
                var classDefField = typeof(Combatant).GetField("classDefinition", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (classDefField != null && classDefField.GetValue(combatant) != null)
                {
                    combatant.LoadClassDefinition();
                }
                else
                {
                    // Set up basic stats without class definition
                    SetupBasicCombatantStats(combatant, characterDefinition);
                }
                
                if (debugMode)
                {
                    Debug.Log($"PartyManager: Initialized combatant {characterDefinition.characterName} (Lv.{characterDefinition.level})");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"PartyManager: Could not fully initialize combatant {characterDefinition.characterName}: {e.Message}");
            }
        }
        
        /// <summary>
        /// Set up basic combatant stats without class definition
        /// </summary>
        private void SetupBasicCombatantStats(Combatant combatant, CharacterDefinition characterDefinition)
        {
            try
            {
                // Set basic stats using reflection
                var stats = new Dictionary<string, int>
                {
                    { "strength", 10 + characterDefinition.level * 2 },
                    { "agility", 10 + characterDefinition.level * 2 },
                    { "intelligence", 10 + characterDefinition.level * 2 },
                    { "defense", 5 + characterDefinition.level },
                    { "vitality", 10 + characterDefinition.level * 2 }
                };
                
                foreach (var stat in stats)
                {
                    var field = typeof(Combatant).GetField(stat.Key, 
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    if (field != null)
                    {
                        field.SetValue(combatant, stat.Value);
                    }
                }
                
                // Set health based on vitality
                var vitalityField = typeof(Combatant).GetField("vitality", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var maxHealthField = typeof(Combatant).GetField("maxHealth", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var currentHealthField = typeof(Combatant).GetField("currentHealth", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                
                if (vitalityField != null && maxHealthField != null && currentHealthField != null)
                {
                    int vitality = (int)vitalityField.GetValue(combatant);
                    int maxHealth = 50 + (vitality * 10); // Base 50 + 10 per vitality point
                    maxHealthField.SetValue(combatant, maxHealth);
                    currentHealthField.SetValue(combatant, maxHealth);
                }
                
                if (debugMode)
                {
                    Debug.Log($"PartyManager: Set up basic stats for {characterDefinition.characterName} (Lv.{characterDefinition.level})");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"PartyManager: Could not set up basic stats for {characterDefinition.characterName}: {e.Message}");
            }
        }
        
        /// <summary>
        /// Get all alive combatants
        /// </summary>
        public List<Combatant> GetAliveCombatants()
        {
            return activeCombatants.Where(c => c.IsAlive).ToList();
        }
        
        /// <summary>
        /// Get all dead combatants
        /// </summary>
        public List<Combatant> GetDeadCombatants()
        {
            return activeCombatants.Where(c => !c.IsAlive).ToList();
        }
        
        /// <summary>
        /// Clear all combatants
        /// </summary>
        public void ClearCombatants()
        {
            foreach (var combatant in activeCombatants)
            {
                if (combatant != null)
                {
                    DestroyImmediate(combatant.gameObject);
                }
            }
            
            activeCombatants.Clear();
        }
        
        #endregion
        
        #region Unity Lifecycle
        
        private void Start()
        {
            // Initialize party size
            if (maxPartySize <= 0)
            {
                maxPartySize = 5; // Default party size
            }
            
            // Find CharacterDataManager if not assigned
            if (characterDataManager == null)
            {
                characterDataManager = FindObjectOfType<CharacterDataManager>();
            }
        }
        
        private void OnDestroy()
        {
            // Clean up events
            OnPartyChanged?.RemoveAllListeners();
            OnCharacterAdded?.RemoveAllListeners();
            OnCharacterRemoved?.RemoveAllListeners();
            OnPartyFormationChanged?.RemoveAllListeners();
            
            // Clean up combatants
            ClearCombatants();
        }
        
        #endregion
    }
} 