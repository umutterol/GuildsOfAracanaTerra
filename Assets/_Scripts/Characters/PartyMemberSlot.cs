using UnityEngine;
using TMPro;
using UnityEngine.UI;
using GuildsOfArcanaTerra.ScriptableObjects.Classes;
using GuildsOfArcanaTerra.Traits;

namespace GuildsOfArcanaTerra.Characters
{
    /// <summary>
    /// UI component for displaying a party member slot
    /// </summary>
    public class PartyMemberSlot : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI classText;
        [SerializeField] private Image characterIcon;
        [SerializeField] private Button removeButton;
        [SerializeField] private GameObject emptySlotIndicator;
        [SerializeField] private GameObject occupiedSlotIndicator;
        
        private int slotIndex;
        private PartyManager partyManager;
        private CharacterDefinition currentCharacter;
        
        public void Initialize(int index, PartyManager manager)
        {
            slotIndex = index;
            partyManager = manager;
            
            if (removeButton != null)
                removeButton.onClick.AddListener(RemoveCharacter);
            
            SetEmpty();
        }
        
        public void SetCharacter(CharacterDefinition character)
        {
            currentCharacter = character;
            
            if (character != null)
            {
                SetOccupied(character);
            }
            else
            {
                SetEmpty();
            }
        }
        
        private void SetOccupied(CharacterDefinition character)
        {
            if (nameText != null)
                nameText.text = character.characterName;
            
            if (levelText != null)
                levelText.text = $"Lv.{character.level}";
            
            if (classText != null)
            {
                string className = character.characterClass != null ? character.characterClass.ClassName : "Unknown";
                classText.text = className;
            }
            
            if (removeButton != null)
                removeButton.gameObject.SetActive(true);
            
            if (emptySlotIndicator != null)
                emptySlotIndicator.SetActive(false);
            
            if (occupiedSlotIndicator != null)
                occupiedSlotIndicator.SetActive(true);
        }
        
        private void SetEmpty()
        {
            if (nameText != null)
                nameText.text = "Empty";
            
            if (levelText != null)
                levelText.text = "";
            
            if (classText != null)
                classText.text = "";
            
            if (removeButton != null)
                removeButton.gameObject.SetActive(false);
            
            if (emptySlotIndicator != null)
                emptySlotIndicator.SetActive(true);
            
            if (occupiedSlotIndicator != null)
                occupiedSlotIndicator.SetActive(false);
        }
        
        private void RemoveCharacter()
        {
            if (currentCharacter != null && partyManager != null)
            {
                partyManager.RemoveCharacter(currentCharacter);
            }
        }
    }
} 