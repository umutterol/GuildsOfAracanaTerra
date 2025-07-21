using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GuildsOfArcanaTerra.Characters;
using GuildsOfArcanaTerra.Combat;
using System.Reflection;

namespace GuildsOfArcanaTerra.UI
{
    /// <summary>
    /// Comprehensive UI setup for the party management system
    /// Creates all necessary UI elements and connects them to the party system
    /// </summary>
    public class PartyManagementUISetup : MonoBehaviour
    {
        [Header("Auto Setup")]
        [SerializeField] private bool autoSetupOnStart = true;
        
        [Header("UI References")]
        [SerializeField] private PartyUIManager partyUIManager;
        [SerializeField] private Canvas mainCanvas;
        
        [Header("Prefab References")]
        [SerializeField] private GameObject partyMemberSlotPrefab;
        // [SerializeField] private GameObject characterCardPrefab; // Removed as per edit hint
        
        [Header("Auto Create Prefabs")]
        [SerializeField] private bool createPrefabsIfMissing = true;
        
        [Header("UI Layout Settings")]
        [SerializeField] private Vector2 partyPanelSize = new Vector2(600, 400);
        [SerializeField] private Vector2 characterListSize = new Vector2(400, 600);
        [SerializeField] private Vector2 slotSize = new Vector2(120, 150);
        [SerializeField] private Vector2 cardSize = new Vector2(140, 180);
        
        private void Start()
        {
            if (autoSetupOnStart)
            {
                SetupCompletePartyUI();
            }
        }
        
        /// <summary>
        /// Set up the complete party management UI system
        /// </summary>
        [ContextMenu("Setup Complete Party UI")]
        public void SetupCompletePartyUI()
        {
            Debug.Log("=== SETTING UP COMPLETE PARTY MANAGEMENT UI ===");
            
            // 1. Ensure core systems exist
            EnsureCoreSystems();
            
            // 2. Create main canvas if needed
            CreateMainCanvas();
            
            // 3. Create party management UI
            CreatePartyManagementUI();
            
            // 4. Create character selection UI
            CreateCharacterSelectionUI();
            
            // 5. Create action buttons
            CreateActionButtons();
            
            // 6. Connect everything
            ConnectUIElements();
            
            Debug.Log("=== PARTY MANAGEMENT UI SETUP COMPLETE ===");
            Debug.Log("You can now use the party management system!");
        }
        
        private void EnsureCoreSystems()
        {
            // Ensure PartyManager exists
            var partyManager = FindObjectOfType<PartyManager>();
            if (partyManager == null)
            {
                Debug.Log("Creating PartyManager...");
                var partyManagerGO = new GameObject("PartyManager");
                partyManager = partyManagerGO.AddComponent<PartyManager>();
            }
            
            // Ensure CharacterDataManager exists
            var characterDataManager = FindObjectOfType<CharacterDataManager>();
            if (characterDataManager == null)
            {
                Debug.Log("Creating CharacterDataManager...");
                var characterDataManagerGO = new GameObject("CharacterDataManager");
                characterDataManager = characterDataManagerGO.AddComponent<CharacterDataManager>();
            }
            
            // Ensure PartyUIManager exists
            if (partyUIManager == null)
            {
                partyUIManager = FindObjectOfType<PartyUIManager>();
                if (partyUIManager == null)
                {
                    Debug.Log("Creating PartyUIManager...");
                    var partyUIManagerGO = new GameObject("PartyUIManager");
                    partyUIManager = partyUIManagerGO.AddComponent<PartyUIManager>();
                }
            }
        }
        
        private void CreateMainCanvas()
        {
            if (mainCanvas == null)
            {
                var existingCanvas = FindObjectOfType<Canvas>();
                if (existingCanvas != null)
                {
                    mainCanvas = existingCanvas;
                }
                else
                {
                    Debug.Log("Creating main Canvas...");
                    var canvasGO = new GameObject("MainCanvas");
                    mainCanvas = canvasGO.AddComponent<Canvas>();
                    mainCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
                    mainCanvas.sortingOrder = 0;
                    
                    canvasGO.AddComponent<CanvasScaler>();
                    canvasGO.AddComponent<GraphicRaycaster>();
                }
            }
        }
        
        private void CreatePartyManagementUI()
        {
            // Create main party management panel
            var partyPanelGO = new GameObject("PartyManagementPanel");
            partyPanelGO.transform.SetParent(mainCanvas.transform, false);
            
            var partyPanel = partyPanelGO.AddComponent<Image>();
            partyPanel.color = new Color(0.1f, 0.1f, 0.1f, 0.95f);
            
            var partyPanelRect = partyPanel.rectTransform;
            partyPanelRect.anchorMin = new Vector2(0, 0);
            partyPanelRect.anchorMax = new Vector2(0, 0);
            partyPanelRect.sizeDelta = partyPanelSize;
            partyPanelRect.anchoredPosition = new Vector2(20, 20);
            
            // Create party title
            CreateTextElement(partyPanelGO, "PartyTitle", "PARTY MANAGEMENT", 
                new Vector2(0.5f, 0.95f), new Vector2(0.5f, 0.95f), 
                new Vector2(0, 0), new Vector2(400, 40), 24, Color.white);
            
            // Create party summary
            CreateTextElement(partyPanelGO, "PartySummary", "Party Summary", 
                new Vector2(0.5f, 0.85f), new Vector2(0.5f, 0.85f), 
                new Vector2(0, 0), new Vector2(400, 60), 14, Color.white);
            
            // Create party stats
            CreateTextElement(partyPanelGO, "PartyStats", "Party Stats", 
                new Vector2(0.5f, 0.75f), new Vector2(0.5f, 0.75f), 
                new Vector2(0, 0), new Vector2(400, 80), 12, Color.white);
            
            // Create party member container
            var partyMemberContainerGO = new GameObject("PartyMemberContainer");
            partyMemberContainerGO.transform.SetParent(partyPanelGO.transform, false);
            
            var partyMemberContainer = partyMemberContainerGO.AddComponent<HorizontalLayoutGroup>();
            partyMemberContainer.spacing = 10;
            partyMemberContainer.padding = new RectOffset(10, 10, 10, 10);
            partyMemberContainer.childControlWidth = true;
            partyMemberContainer.childControlHeight = true;
            partyMemberContainer.childForceExpandWidth = false;
            partyMemberContainer.childForceExpandHeight = false;
            
            var partyMemberContainerRect = partyMemberContainerGO.GetComponent<RectTransform>();
            partyMemberContainerRect.anchorMin = new Vector2(0, 0.1f);
            partyMemberContainerRect.anchorMax = new Vector2(1, 0.6f);
            partyMemberContainerRect.offsetMin = Vector2.zero;
            partyMemberContainerRect.offsetMax = Vector2.zero;
            
            // Store reference for PartyUIManager using reflection
            var partyMemberContainerField = partyUIManager.GetType().GetField("partyMemberContainer", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            if (partyMemberContainerField != null)
            {
                partyMemberContainerField.SetValue(partyUIManager, partyMemberContainer.transform);
            }
        }
        
        private void CreateCharacterSelectionUI()
        {
            // Create character selection panel
            var characterPanelGO = new GameObject("CharacterSelectionPanel");
            characterPanelGO.transform.SetParent(mainCanvas.transform, false);
            
            var characterPanel = characterPanelGO.AddComponent<Image>();
            characterPanel.color = new Color(0.15f, 0.15f, 0.15f, 0.95f);
            
            var characterPanelRect = characterPanel.rectTransform;
            characterPanelRect.anchorMin = new Vector2(1, 0);
            characterPanelRect.anchorMax = new Vector2(1, 0);
            characterPanelRect.sizeDelta = characterListSize;
            characterPanelRect.anchoredPosition = new Vector2(-20, 20);
            
            // Create character selection title
            CreateTextElement(characterPanelGO, "CharacterSelectionTitle", "AVAILABLE CHARACTERS", 
                new Vector2(0.5f, 0.95f), new Vector2(0.5f, 0.95f), 
                new Vector2(0, 0), new Vector2(350, 40), 18, Color.white);
            
            // Create scroll view for character cards
            var scrollViewGO = new GameObject("CharacterScrollView");
            scrollViewGO.transform.SetParent(characterPanelGO.transform, false);
            
            var scrollView = scrollViewGO.AddComponent<ScrollRect>();
            var scrollViewRect = scrollViewGO.GetComponent<RectTransform>();
            scrollViewRect.anchorMin = new Vector2(0, 0.1f);
            scrollViewRect.anchorMax = new Vector2(1, 0.9f);
            scrollViewRect.offsetMin = Vector2.zero;
            scrollViewRect.offsetMax = Vector2.zero;
            
            // Create viewport
            var viewportGO = new GameObject("Viewport");
            viewportGO.transform.SetParent(scrollViewGO.transform, false);
            
            var viewport = viewportGO.AddComponent<Image>();
            viewport.color = new Color(0.1f, 0.1f, 0.1f, 0.5f);
            viewport.type = Image.Type.Sliced;
            
            var viewportRect = viewport.rectTransform;
            viewportRect.anchorMin = Vector2.zero;
            viewportRect.anchorMax = Vector2.one;
            viewportRect.offsetMin = Vector2.zero;
            viewportRect.offsetMax = Vector2.zero;
            
            var viewportMask = viewportGO.AddComponent<Mask>();
            viewportMask.showMaskGraphic = false;
            
            // Create content container
            var contentGO = new GameObject("Content");
            contentGO.transform.SetParent(viewportGO.transform, false);
            
            var content = contentGO.AddComponent<GridLayoutGroup>();
            content.cellSize = cardSize;
            content.spacing = new Vector2(10, 10);
            content.padding = new RectOffset(10, 10, 10, 10);
            content.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            content.constraintCount = 2;
            
            var contentRect = contentGO.GetComponent<RectTransform>();
            contentRect.anchorMin = new Vector2(0, 1);
            contentRect.anchorMax = new Vector2(1, 1);
            contentRect.pivot = new Vector2(0.5f, 1);
            contentRect.sizeDelta = new Vector2(0, 0);
            
            // Setup scroll view
            scrollView.viewport = viewport.GetComponent<RectTransform>();
            scrollView.content = content.GetComponent<RectTransform>();
            scrollView.horizontal = false;
            scrollView.vertical = true;
            
            // Store reference for PartyUIManager using reflection
            var availableCharactersContainerField = partyUIManager.GetType().GetField("availableCharactersContainer", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            if (availableCharactersContainerField != null)
            {
                availableCharactersContainerField.SetValue(partyUIManager, content.transform);
            }
        }
        
        private void CreateActionButtons()
        {
            // Create action buttons panel
            var actionPanelGO = new GameObject("ActionButtonsPanel");
            actionPanelGO.transform.SetParent(mainCanvas.transform, false);
            
            var actionPanel = actionPanelGO.AddComponent<Image>();
            actionPanel.color = new Color(0.2f, 0.2f, 0.2f, 0.9f);
            
            var actionPanelRect = actionPanelGO.GetComponent<RectTransform>();
            actionPanelRect.anchorMin = new Vector2(0, 1);
            actionPanelRect.anchorMax = new Vector2(0, 1);
            actionPanelRect.sizeDelta = new Vector2(partyPanelSize.x, 80);
            actionPanelRect.anchoredPosition = new Vector2(20, -20);
            
            // Create horizontal layout for buttons
            var buttonLayout = actionPanelGO.AddComponent<HorizontalLayoutGroup>();
            buttonLayout.spacing = 10;
            buttonLayout.padding = new RectOffset(10, 10, 10, 10);
            buttonLayout.childControlWidth = true;
            buttonLayout.childControlHeight = true;
            buttonLayout.childForceExpandWidth = false;
            buttonLayout.childForceExpandHeight = false;
            
            // Create action buttons
            CreateButton(actionPanelGO, "ClearPartyButton", "Clear Party", Color.red);
            CreateButton(actionPanelGO, "SavePartyButton", "Save Party", Color.blue);
            CreateButton(actionPanelGO, "LoadPartyButton", "Load Party", Color.green);
            CreateButton(actionPanelGO, "StartCombatButton", "Start Combat", Color.yellow);
        }
        
        private void CreateTextElement(GameObject parent, string name, string text, 
            Vector2 anchorMin, Vector2 anchorMax, Vector2 position, Vector2 size, 
            int fontSize, Color color)
        {
            var textGO = new GameObject(name);
            textGO.transform.SetParent(parent.transform, false);
            
            var textComponent = textGO.AddComponent<TextMeshProUGUI>();
            textComponent.text = text;
            textComponent.fontSize = fontSize;
            textComponent.color = color;
            textComponent.alignment = TextAlignmentOptions.Center;
            
            var textRect = textGO.GetComponent<RectTransform>();
            textRect.anchorMin = anchorMin;
            textRect.anchorMax = anchorMax;
            textRect.anchoredPosition = position;
            textRect.sizeDelta = size;
        }
        
        private void CreateButton(GameObject parent, string name, string text, Color color)
        {
            var buttonGO = new GameObject(name);
            buttonGO.transform.SetParent(parent.transform, false);
            
            var button = buttonGO.AddComponent<Button>();
            var buttonImage = buttonGO.AddComponent<Image>();
            buttonImage.color = color;
            
            var buttonRect = buttonGO.GetComponent<RectTransform>();
            buttonRect.sizeDelta = new Vector2(120, 40);
            
            // Create button text
            var textGO = new GameObject("Text");
            textGO.transform.SetParent(buttonGO.transform, false);
            
            var textComponent = textGO.AddComponent<TextMeshProUGUI>();
            textComponent.text = text;
            textComponent.fontSize = 14;
            textComponent.color = Color.white;
            textComponent.alignment = TextAlignmentOptions.Center;
            
            var textRect = textGO.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;
        }
        
        private void ConnectUIElements()
        {
            if (partyUIManager == null) return;
            
            // Find UI elements by name
            var partySummaryText = GameObject.Find("PartySummary")?.GetComponent<TextMeshProUGUI>();
            var partyStatsText = GameObject.Find("PartyStats")?.GetComponent<TextMeshProUGUI>();
            var clearPartyButton = GameObject.Find("ClearPartyButton")?.GetComponent<Button>();
            var savePartyButton = GameObject.Find("SavePartyButton")?.GetComponent<Button>();
            var loadPartyButton = GameObject.Find("LoadPartyButton")?.GetComponent<Button>();
            var startCombatButton = GameObject.Find("StartCombatButton")?.GetComponent<Button>();
            
            // Connect references using reflection
            if (partySummaryText != null)
            {
                var partySummaryTextField = partyUIManager.GetType().GetField("partySummaryText", 
                    BindingFlags.NonPublic | BindingFlags.Instance);
                if (partySummaryTextField != null)
                    partySummaryTextField.SetValue(partyUIManager, partySummaryText);
            }
            
            if (partyStatsText != null)
            {
                var partyStatsTextField = partyUIManager.GetType().GetField("partyStatsText", 
                    BindingFlags.NonPublic | BindingFlags.Instance);
                if (partyStatsTextField != null)
                    partyStatsTextField.SetValue(partyUIManager, partyStatsText);
            }
            
            if (clearPartyButton != null)
            {
                var clearPartyButtonField = partyUIManager.GetType().GetField("clearPartyButton", 
                    BindingFlags.NonPublic | BindingFlags.Instance);
                if (clearPartyButtonField != null)
                    clearPartyButtonField.SetValue(partyUIManager, clearPartyButton);
            }
            
            if (savePartyButton != null)
            {
                var savePartyButtonField = partyUIManager.GetType().GetField("savePartyButton", 
                    BindingFlags.NonPublic | BindingFlags.Instance);
                if (savePartyButtonField != null)
                    savePartyButtonField.SetValue(partyUIManager, savePartyButton);
            }
            
            if (loadPartyButton != null)
            {
                var loadPartyButtonField = partyUIManager.GetType().GetField("loadPartyButton", 
                    BindingFlags.NonPublic | BindingFlags.Instance);
                if (loadPartyButtonField != null)
                    loadPartyButtonField.SetValue(partyUIManager, loadPartyButton);
            }
            
            if (startCombatButton != null)
            {
                var startCombatButtonField = partyUIManager.GetType().GetField("startCombatButton", 
                    BindingFlags.NonPublic | BindingFlags.Instance);
                if (startCombatButtonField != null)
                    startCombatButtonField.SetValue(partyUIManager, startCombatButton);
            }
            
            // Create or load prefabs
            if (partyMemberSlotPrefab == null)
            {
                if (createPrefabsIfMissing)
                {
                    partyMemberSlotPrefab = CreatePartyMemberSlotPrefab();
                }
                else
                {
                    partyMemberSlotPrefab = Resources.Load<GameObject>("PartyMemberSlotPrefab");
                    if (partyMemberSlotPrefab == null)
                    {
                        Debug.LogWarning("PartyMemberSlotPrefab not found in Resources folder");
                    }
                }
            }
            
            // Remove all references to CharacterCard and characterCardPrefab
            // For character display, use CharacterDefinition and its traits list
            
            if (partyMemberSlotPrefab != null)
            {
                var partyMemberSlotPrefabField = partyUIManager.GetType().GetField("partyMemberSlotPrefab", 
                    BindingFlags.NonPublic | BindingFlags.Instance);
                if (partyMemberSlotPrefabField != null)
                    partyMemberSlotPrefabField.SetValue(partyUIManager, partyMemberSlotPrefab);
            }
            
            // Initialize the UI using reflection
            var initializeUIMethod = partyUIManager.GetType().GetMethod("InitializeUI", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            if (initializeUIMethod != null)
            {
                initializeUIMethod.Invoke(partyUIManager, null);
            }
        }
        
        /// <summary>
        /// Quick setup for testing - creates minimal UI
        /// </summary>
        [ContextMenu("Quick Setup")]
        public void QuickSetup()
        {
            Debug.Log("=== QUICK SETUP - MINIMAL PARTY UI ===");
            
            EnsureCoreSystems();
            CreateMainCanvas();
            
            // Create a simple test panel
            var testPanelGO = new GameObject("TestPanel");
            testPanelGO.transform.SetParent(mainCanvas.transform, false);
            
            var testPanel = testPanelGO.AddComponent<Image>();
            testPanel.color = new Color(0.2f, 0.2f, 0.2f, 0.9f);
            
            var testPanelRect = testPanel.rectTransform;
            testPanelRect.anchorMin = new Vector2(0.5f, 0.5f);
            testPanelRect.anchorMax = new Vector2(0.5f, 0.5f);
            testPanelRect.sizeDelta = new Vector2(400, 300);
            testPanelRect.anchoredPosition = Vector2.zero;
            
            CreateTextElement(testPanelGO, "TestTitle", "PARTY SYSTEM READY", 
                new Vector2(0.5f, 0.8f), new Vector2(0.5f, 0.8f), 
                Vector2.zero, new Vector2(350, 50), 20, Color.white);
            
            CreateTextElement(testPanelGO, "TestInfo", "Check console for test commands", 
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), 
                Vector2.zero, new Vector2(350, 100), 14, Color.white);
            
            Debug.Log("=== QUICK SETUP COMPLETE ===");
            Debug.Log("Use SimplePartyTest.cs for console testing");
        }
        
        /// <summary>
        /// Create a party member slot prefab programmatically
        /// </summary>
        private GameObject CreatePartyMemberSlotPrefab()
        {
            var prefabGO = new GameObject("PartyMemberSlot");
            
            // Add required components
            var rectTransform = prefabGO.AddComponent<RectTransform>();
            var canvasRenderer = prefabGO.AddComponent<CanvasRenderer>();
            var image = prefabGO.AddComponent<Image>();
            var layoutElement = prefabGO.AddComponent<LayoutElement>();
            var partyMemberSlot = prefabGO.AddComponent<PartyMemberSlot>();
            
            // Set up image
            image.color = new Color(0.2f, 0.2f, 0.2f, 0.8f);
            
            // Set up layout element
            layoutElement.preferredWidth = slotSize.x;
            layoutElement.preferredHeight = slotSize.y;
            
            // Create child elements
            CreateSlotChild(prefabGO, "CharacterIcon", new Vector2(0.5f, 0.7f), new Vector2(0.5f, 0.9f), new Vector2(60, 60));
            CreateSlotChild(prefabGO, "LevelText", new Vector2(0.1f, 0.5f), new Vector2(0.9f, 0.6f), Vector2.zero);
            CreateSlotChild(prefabGO, "ClassText", new Vector2(0.1f, 0.35f), new Vector2(0.9f, 0.45f), Vector2.zero);
            CreateSlotChild(prefabGO, "RemoveButton", new Vector2(0.8f, 0.8f), new Vector2(0.95f, 0.95f), Vector2.zero);
            CreateSlotChild(prefabGO, "EmptySlotIndicator", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(100, 100));
            CreateSlotChild(prefabGO, "OccupiedSlotIndicator", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(100, 100));
            
            return prefabGO;
        }
        
        /// <summary>
        /// Create a child element for party member slot
        /// </summary>
        private void CreateSlotChild(GameObject parent, string name, Vector2 anchorMin, Vector2 anchorMax, Vector2 sizeDelta)
        {
            var childGO = new GameObject(name);
            childGO.transform.SetParent(parent.transform, false);
            
            var rectTransform = childGO.AddComponent<RectTransform>();
            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;
            rectTransform.sizeDelta = sizeDelta;
            rectTransform.anchoredPosition = Vector2.zero;
            
            var canvasRenderer = childGO.AddComponent<CanvasRenderer>();
            var image = childGO.AddComponent<Image>();
            
            // Set up based on type
            switch (name)
            {
                case "CharacterIcon":
                    image.color = Color.white;
                    break;
                case "RemoveButton":
                    image.color = new Color(0.8f, 0.2f, 0.2f, 1f);
                    childGO.AddComponent<Button>();
                    break;
                case "EmptySlotIndicator":
                    image.color = new Color(0.3f, 0.3f, 0.3f, 0.5f);
                    break;
                case "OccupiedSlotIndicator":
                    image.color = new Color(0.2f, 0.8f, 0.2f, 0.3f);
                    childGO.SetActive(false);
                    break;
                default:
                    // Text elements
                    DestroyImmediate(image);
                    var text = childGO.AddComponent<TextMeshProUGUI>();
                    text.text = name.Replace("Text", "");
                    text.color = Color.white;
                    text.fontSize = name.Contains("Level") ? 12 : 10;
                    text.alignment = TextAlignmentOptions.Center;
                    break;
            }
        }
    }
} 