using UnityEngine;
using UnityEngine.UI;

namespace GuildsOfArcanaTerra.Combat.UI
{
    /// <summary>
    /// UI Manager for the Status Effect System Test
    /// Automatically creates and manages the test UI
    /// </summary>
    public class StatusEffectTestUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Canvas canvas;
        [SerializeField] private Text statusText;
        [SerializeField] private Button applyBurnButton;
        [SerializeField] private Button applyBleedButton;
        [SerializeField] private Button startCombatButton;
        [SerializeField] private Button endTurnButton;
        [SerializeField] private Button clearEffectsButton;
        [SerializeField] private Button resetHealthButton;
        
        [Header("UI Settings")]
        [SerializeField] private bool autoCreateUI = true;
        [SerializeField] private Vector2 statusTextPosition = new Vector2(10, -10);
        [SerializeField] private Vector2 buttonPanelPosition = new Vector2(10, -200);
        
        private StatusEffectSystemTest testScript;
        
        private void Start()
        {
            if (autoCreateUI)
            {
                CreateTestUI();
            }
            
            // Find and connect to the test script
            testScript = FindObjectOfType<StatusEffectSystemTest>();
            if (testScript != null)
            {
                ConnectUIToTestScript();
            }
            else
            {
                Debug.LogWarning("StatusEffectTestUI: No StatusEffectSystemTest found in scene!");
            }
        }
        
        /// <summary>
        /// Create the complete test UI
        /// </summary>
        public void CreateTestUI()
        {
            // Create Canvas if it doesn't exist
            if (canvas == null)
            {
                CreateCanvas();
            }
            
            // Create UI elements
            CreateStatusText();
            CreateButtonPanel();
            
            Debug.Log("StatusEffectTestUI: Test UI created successfully!");
        }
        
        /// <summary>
        /// Create the main canvas
        /// </summary>
        private void CreateCanvas()
        {
            var canvasGO = new GameObject("Test UI Canvas");
            canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 100;
            
            // Add CanvasScaler for proper scaling
            var scaler = canvasGO.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            
            // Add GraphicRaycaster for button interactions
            canvasGO.AddComponent<GraphicRaycaster>();
            
            // Add EventSystem if it doesn't exist
            if (FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
            {
                var eventSystemGO = new GameObject("EventSystem");
                eventSystemGO.AddComponent<UnityEngine.EventSystems.EventSystem>();
                eventSystemGO.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
            }
        }
        
        /// <summary>
        /// Create the status text display
        /// </summary>
        private void CreateStatusText()
        {
            var statusPanelGO = new GameObject("Status Panel");
            statusPanelGO.transform.SetParent(canvas.transform, false);
            
            var statusPanel = statusPanelGO.AddComponent<RectTransform>();
            statusPanel.anchorMin = new Vector2(0, 1);
            statusPanel.anchorMax = new Vector2(0, 1);
            statusPanel.pivot = new Vector2(0, 1);
            statusPanel.anchoredPosition = statusTextPosition;
            statusPanel.sizeDelta = new Vector2(400, 300);
            
            // Add background image
            var background = statusPanelGO.AddComponent<Image>();
            background.color = new Color(0, 0, 0, 0.8f);
            
            // Create status text
            var textGO = new GameObject("Status Text");
            textGO.transform.SetParent(statusPanelGO.transform, false);
            
            var textRect = textGO.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = new Vector2(10, 10);
            textRect.offsetMax = new Vector2(-10, -10);
            
            statusText = textGO.AddComponent<Text>();
            statusText.text = "Status Effect System Test\n\nClick 'Start Combat' to begin...";
            statusText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            statusText.fontSize = 14;
            statusText.color = Color.white;
            statusText.alignment = TextAnchor.UpperLeft;
            statusText.horizontalOverflow = HorizontalWrapMode.Wrap;
            statusText.verticalOverflow = VerticalWrapMode.Overflow;
        }
        
        /// <summary>
        /// Create the button panel
        /// </summary>
        private void CreateButtonPanel()
        {
            var buttonPanelGO = new GameObject("Button Panel");
            buttonPanelGO.transform.SetParent(canvas.transform, false);
            
            var buttonPanel = buttonPanelGO.AddComponent<RectTransform>();
            buttonPanel.anchorMin = new Vector2(0, 1);
            buttonPanel.anchorMax = new Vector2(0, 1);
            buttonPanel.pivot = new Vector2(0, 1);
            buttonPanel.anchoredPosition = buttonPanelPosition;
            buttonPanel.sizeDelta = new Vector2(400, 200);
            
            // Add background image
            var background = buttonPanelGO.AddComponent<Image>();
            background.color = new Color(0.2f, 0.2f, 0.2f, 0.9f);
            
            // Create buttons
            startCombatButton = CreateButton(buttonPanelGO, "Start Combat", new Vector2(0, -20));
            applyBurnButton = CreateButton(buttonPanelGO, "Apply Burn", new Vector2(0, -60));
            applyBleedButton = CreateButton(buttonPanelGO, "Apply Bleed", new Vector2(0, -100));
            endTurnButton = CreateButton(buttonPanelGO, "End Turn", new Vector2(0, -140));
            clearEffectsButton = CreateButton(buttonPanelGO, "Clear Effects", new Vector2(200, -20));
            resetHealthButton = CreateButton(buttonPanelGO, "Reset Health", new Vector2(200, -60));
        }
        
        /// <summary>
        /// Create a single button
        /// </summary>
        private Button CreateButton(GameObject parent, string text, Vector2 position)
        {
            var buttonGO = new GameObject(text + " Button");
            buttonGO.transform.SetParent(parent.transform, false);
            
            var buttonRect = buttonGO.AddComponent<RectTransform>();
            buttonRect.anchorMin = new Vector2(0, 1);
            buttonRect.anchorMax = new Vector2(0, 1);
            buttonRect.pivot = new Vector2(0, 1);
            buttonRect.anchoredPosition = position;
            buttonRect.sizeDelta = new Vector2(180, 30);
            
            // Add button component
            var button = buttonGO.AddComponent<Button>();
            
            // Add background image
            var background = buttonGO.AddComponent<Image>();
            background.color = new Color(0.3f, 0.3f, 0.3f, 1f);
            
            // Create text
            var textGO = new GameObject("Text");
            textGO.transform.SetParent(buttonGO.transform, false);
            
            var textRect = textGO.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;
            
            var buttonText = textGO.AddComponent<Text>();
            buttonText.text = text;
            buttonText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            buttonText.fontSize = 12;
            buttonText.color = Color.white;
            buttonText.alignment = TextAnchor.MiddleCenter;
            
            // Set button colors
            var colors = button.colors;
            colors.normalColor = new Color(0.3f, 0.3f, 0.3f, 1f);
            colors.highlightedColor = new Color(0.4f, 0.4f, 0.4f, 1f);
            colors.pressedColor = new Color(0.2f, 0.2f, 0.2f, 1f);
            button.colors = colors;
            
            return button;
        }
        
        /// <summary>
        /// Connect the UI elements to the test script
        /// </summary>
        private void ConnectUIToTestScript()
        {
            if (testScript == null) return;
            
            // Connect buttons to test methods
            if (startCombatButton != null)
                startCombatButton.onClick.AddListener(testScript.TestStartCombat);
            
            if (applyBurnButton != null)
                applyBurnButton.onClick.AddListener(testScript.TestApplyBurn);
            
            if (applyBleedButton != null)
                applyBleedButton.onClick.AddListener(testScript.TestApplyBleed);
            
            if (endTurnButton != null)
                endTurnButton.onClick.AddListener(testScript.TestEndTurn);
            
            if (clearEffectsButton != null)
                clearEffectsButton.onClick.AddListener(testScript.TestClearAllEffects);
            
            if (resetHealthButton != null)
                resetHealthButton.onClick.AddListener(testScript.TestResetHealth);
            
            Debug.Log("StatusEffectTestUI: UI connected to test script!");
        }
        
        /// <summary>
        /// Update the status text (called by test script)
        /// </summary>
        public void UpdateStatusText(string text)
        {
            if (statusText != null)
            {
                statusText.text = text;
            }
        }
        
        /// <summary>
        /// Enable/disable buttons based on combat state
        /// </summary>
        public void SetCombatState(bool isCombatActive)
        {
            if (startCombatButton != null)
                startCombatButton.interactable = !isCombatActive;
            
            if (endTurnButton != null)
                endTurnButton.interactable = isCombatActive;
        }
        
        /// <summary>
        /// Manual UI creation method (can be called from Inspector)
        /// </summary>
        [ContextMenu("Create Test UI")]
        public void CreateTestUIManual()
        {
            CreateTestUI();
        }
        
        /// <summary>
        /// Manual UI connection method (can be called from Inspector)
        /// </summary>
        [ContextMenu("Connect UI")]
        public void ConnectUIManual()
        {
            ConnectUIToTestScript();
        }
    }
} 