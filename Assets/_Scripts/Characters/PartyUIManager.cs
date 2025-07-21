using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GuildsOfArcanaTerra.Combat;
using UnityEngine.SceneManagement;

namespace GuildsOfArcanaTerra.Characters
{
    public class PartyUIManager : MonoBehaviour
    {
        public PartyManager partyManager;
        public GuildManager guildManager;
        public List<CharacterDefinition> availableCharacters; // This will be set from guildManager

        // UI elements for each slot (e.g., buttons or panels)
        public Button[] slotButtons = new Button[5];
        public TextMeshProUGUI[] slotLabels = new TextMeshProUGUI[5];

        // UI for available characters (e.g., a scroll view or grid)
        public Transform availableCharactersPanel;
        public GameObject characterButtonPrefab;
        public Button startDungeonButton;
        public TextMeshProUGUI feedbackText; // Optional feedback for validation
        // public DungeonDefinition selectedDungeonDefinition; // (Removed for runtime assignment)

        private void Start()
        {
            if (guildManager != null)
                availableCharacters = guildManager.guildMembers;
            if (startDungeonButton != null)
                startDungeonButton.onClick.AddListener(OnStartDungeonClicked);
            RefreshUI();
        }

        // Call this when a character is selected for a slot
        public void AssignCharacterToSlot(CharacterDefinition character, int slotIndex)
        {
            if (partyManager.AssignCharacterToSlot(character, slotIndex))
            {
                RefreshUI();
            }
        }

        // Call this to remove a character from a slot
        public void RemoveCharacterFromSlot(int slotIndex)
        {
            if (partyManager.RemoveCharacterFromSlot(slotIndex))
            {
                RefreshUI();
            }
        }

        // Call this to confirm party selection and proceed to dungeon
        public void ConfirmPartySelection()
        {
            // TODO: Add validation (e.g., at least 1 character assigned)
            Debug.Log("Party selection confirmed!");
            // Proceed to dungeon run scene or enable DungeonRunManager
        }

        public void OnStartDungeonClicked()
        {
            if (!IsPartyValid())
            {
                if (feedbackText != null)
                    feedbackText.text = "Please fill all party slots with unique characters!";
                Debug.LogWarning("Party validation failed: not all slots filled or duplicates present.");
                return;
            }
            if (feedbackText != null)
                feedbackText.text = "";
            Debug.Log("Party selection confirmed! Starting dungeon...");
            // Use SceneFlowManager's currentDungeon, which should be set at runtime
            if (SceneFlowManager.Instance != null && SceneFlowManager.Instance.currentDungeon != null)
            {
                var party = new List<CharacterDefinition>();
                for (int i = 0; i < partyManager.PartySlots.Length; i++)
                {
                    var character = partyManager.GetCharacterInSlot(i);
                    if (character != null)
                        party.Add(character);
                }
                SceneFlowManager.Instance.StartDungeon(SceneFlowManager.Instance.currentDungeon, party);
            }
            else
            {
                Debug.LogError("No DungeonDefinition assigned in SceneFlowManager!");
            }
        }

        private bool IsPartyValid()
        {
            // Check all slots filled and no duplicates
            var assigned = new List<CharacterDefinition>();
            for (int i = 0; i < partyManager.PartySlots.Length; i++)
            {
                var character = partyManager.GetCharacterInSlot(i);
                if (character == null)
                    return false;
                assigned.Add(character);
            }
            // Check for duplicates
            var set = new HashSet<CharacterDefinition>(assigned);
            return set.Count == assigned.Count;
        }

        // Refreshes the UI to show current slot assignments and available characters
        public void RefreshUI()
        {
            Debug.Log($"partyManager: {partyManager}");
            Debug.Log($"partyManager.PartySlots: {partyManager?.PartySlots}");
            Debug.Log($"availableCharactersPanel: {availableCharactersPanel}");
            Debug.Log($"characterButtonPrefab: {characterButtonPrefab}");
            Debug.Log($"availableCharacters: {availableCharacters}");
            // Update slot labels
            for (int i = 0; i < slotLabels.Length; i++)
            {
                if (slotLabels[i] == null)
                {
                    Debug.LogWarning($"PartyUIManager: slotLabels[{i}] is not assigned!");
                    continue;
                }
                var character = partyManager.GetCharacterInSlot(i);
                slotLabels[i].text = character != null ? character.characterName : "Empty";
            }

            // Clear and repopulate available characters UI
            foreach (Transform child in availableCharactersPanel)
            {
                Destroy(child.gameObject);
            }
            foreach (var character in availableCharacters)
            {
                if (character == null) continue;
                if (System.Array.IndexOf(partyManager.PartySlots, character) == -1)
                {
                    var btnObj = Instantiate(characterButtonPrefab, availableCharactersPanel);
                    var btn = btnObj.GetComponentInChildren<Button>();
                    if (btn == null)
                    {
                        Debug.LogError("characterButtonPrefab is missing a Button component in its children!");
                        continue;
                    }
                    var txt = btnObj.GetComponentInChildren<TextMeshProUGUI>();
                    if (txt != null)
                        txt.text = character.characterName;
                    btn.onClick.AddListener(() => {
                        int emptySlot = System.Array.FindIndex(partyManager.PartySlots, c => c == null);
                        if (emptySlot != -1)
                        {
                            AssignCharacterToSlot(character, emptySlot);
                        }
                    });
                }
            }
        }
    }
} 