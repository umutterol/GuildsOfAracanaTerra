using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GuildsOfArcanaTerra.Combat.UI
{
    /// <summary>
    /// UI Manager for CombatantTest that automatically finds and connects UI elements
    /// </summary>
    public class CombatantTestUI : MonoBehaviour
    {
        [Header("Auto-Found UI Elements")]
        [SerializeField] private Text statusText;
        [SerializeField] private Text skillText;
        [SerializeField] private Button loadClassesButton;
        [SerializeField] private Button startCombatButton;
        [SerializeField] private Button endTurnButton;
        [SerializeField] private Button useSkillButton;
        [SerializeField] private Button applyBurnButton;
        [SerializeField] private Button resetHealthButton;
        [SerializeField] private Dropdown skillDropdown;
        [SerializeField] private Dropdown targetDropdown;
        
        [Header("UI Setup")]
        [SerializeField] private bool autoFindUIElements = true;
        [SerializeField] private Canvas targetCanvas;
        
        [Header("Debug")]
        [SerializeField] private bool debugMode = true;
        
        // Properties for external access
        public Text StatusText => statusText;
        public Text SkillText => skillText;
        public Button LoadClassesButton => loadClassesButton;
        public Button StartCombatButton => startCombatButton;
        public Button EndTurnButton => endTurnButton;
        public Button UseSkillButton => useSkillButton;
        public Button ApplyBurnButton => applyBurnButton;
        public Button ResetHealthButton => resetHealthButton;
        public Dropdown SkillDropdown => skillDropdown;
        public Dropdown TargetDropdown => targetDropdown;
        
        private void Start()
        {
            if (autoFindUIElements)
            {
                FindAndConnectUIElements();
            }
            
            if (debugMode)
            {
                LogUIStatus();
            }
        }
        
        /// <summary>
        /// Automatically find and connect to UI elements in the scene
        /// </summary>
        public void FindAndConnectUIElements()
        {
            // Find canvas if not assigned
            if (targetCanvas == null)
            {
                targetCanvas = FindObjectOfType<Canvas>();
                if (targetCanvas == null)
                {
                    Debug.LogError("CombatantTestUI: No Canvas found in scene!");
                    return;
                }
            }
            
            // Find UI elements by name
            FindUIElementsByName();
            
            // Create missing UI elements if needed
            CreateMissingUIElements();
            
            if (debugMode)
            {
                Debug.Log("CombatantTestUI: UI elements found and connected");
            }
        }
        
        /// <summary>
        /// Find UI elements by their expected names
        /// </summary>
        private void FindUIElementsByName()
        {
            // Find Text components
            statusText = FindUIElement<Text>("StatusText");
            skillText = FindUIElement<Text>("SkillText");
            
            // Find Button components
            loadClassesButton = FindUIElement<Button>("LoadClassesButton");
            startCombatButton = FindUIElement<Button>("StartCombatButton");
            endTurnButton = FindUIElement<Button>("EndTurnButton");
            useSkillButton = FindUIElement<Button>("UseSkillButton");
            applyBurnButton = FindUIElement<Button>("ApplyBurnButton");
            resetHealthButton = FindUIElement<Button>("ResetHealthButton");
            
            // Find Dropdown components
            skillDropdown = FindUIElement<Dropdown>("SkillDropdown");
            targetDropdown = FindUIElement<Dropdown>("TargetDropdown");
        }
        
        /// <summary>
        /// Find a UI element by name in the canvas hierarchy
        /// </summary>
        private T FindUIElement<T>(string elementName) where T : Component
        {
            if (targetCanvas == null) return null;
            
            // Search in canvas hierarchy
            var elements = targetCanvas.GetComponentsInChildren<T>(true);
            foreach (var element in elements)
            {
                if (element.name == elementName)
                {
                    return element;
                }
            }
            
            if (debugMode)
            {
                Debug.LogWarning($"CombatantTestUI: Could not find UI element '{elementName}' of type {typeof(T).Name}");
            }
            
            return null;
        }
        
        /// <summary>
        /// Create missing UI elements automatically
        /// </summary>
        private void CreateMissingUIElements()
        {
            if (targetCanvas == null) return;
            
            // Create status text if missing
            if (statusText == null)
            {
                statusText = CreateTextElement("StatusText", "Combatant Status:\n\nNo combatants loaded", new Vector2(300, 200));
            }
            
            // Create skill text if missing
            if (skillText == null)
            {
                skillText = CreateTextElement("SkillText", "Skills:\nNo skills available", new Vector2(300, 150));
            }
            
            // Create buttons if missing
            if (loadClassesButton == null)
                loadClassesButton = CreateButton("LoadClassesButton", "Load Classes", new Vector2(120, 30));
            
            if (startCombatButton == null)
                startCombatButton = CreateButton("StartCombatButton", "Start Combat", new Vector2(120, 30));
            
            if (endTurnButton == null)
                endTurnButton = CreateButton("EndTurnButton", "End Turn", new Vector2(120, 30));
            
            if (useSkillButton == null)
                useSkillButton = CreateButton("UseSkillButton", "Use Skill", new Vector2(120, 30));
            
            if (applyBurnButton == null)
                applyBurnButton = CreateButton("ApplyBurnButton", "Apply Burn", new Vector2(120, 30));
            
            if (resetHealthButton == null)
                resetHealthButton = CreateButton("ResetHealthButton", "Reset Health", new Vector2(120, 30));
            
            // Create dropdowns if missing
            if (skillDropdown == null)
                skillDropdown = CreateDropdown("SkillDropdown", new Vector2(150, 30));
            
            if (targetDropdown == null)
                targetDropdown = CreateDropdown("TargetDropdown", new Vector2(150, 30));
            
            // Position UI elements
            PositionUIElements();
        }
        
        /// <summary>
        /// Create a text element
        /// </summary>
        private Text CreateTextElement(string name, string defaultText, Vector2 size)
        {
            var go = new GameObject(name);
            go.transform.SetParent(targetCanvas.transform, false);
            
            var text = go.AddComponent<Text>();
            text.text = defaultText;
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = 14;
            text.color = Color.white;
            text.alignment = TextAnchor.UpperLeft;
            
            var rectTransform = text.GetComponent<RectTransform>();
            rectTransform.sizeDelta = size;
            
            return text;
        }
        
        /// <summary>
        /// Create a button element
        /// </summary>
        private Button CreateButton(string name, string buttonText, Vector2 size)
        {
            var go = new GameObject(name);
            go.transform.SetParent(targetCanvas.transform, false);
            
            var button = go.AddComponent<Button>();
            var image = go.AddComponent<Image>();
            image.color = new Color(0.2f, 0.2f, 0.2f, 1f);
            
            // Create text child
            var textGO = new GameObject("Text");
            textGO.transform.SetParent(go.transform, false);
            
            var text = textGO.AddComponent<Text>();
            text.text = buttonText;
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = 12;
            text.color = Color.white;
            text.alignment = TextAnchor.MiddleCenter;
            
            var textRect = text.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;
            
            var rectTransform = button.GetComponent<RectTransform>();
            rectTransform.sizeDelta = size;
            
            return button;
        }
        
        /// <summary>
        /// Create a dropdown element
        /// </summary>
        private Dropdown CreateDropdown(string name, Vector2 size)
        {
            var go = new GameObject(name);
            go.transform.SetParent(targetCanvas.transform, false);
            
            var dropdown = go.AddComponent<Dropdown>();
            var image = go.AddComponent<Image>();
            image.color = new Color(0.1f, 0.1f, 0.1f, 1f);
            
            // Create label child
            var labelGO = new GameObject("Label");
            labelGO.transform.SetParent(go.transform, false);
            
            var label = labelGO.AddComponent<Text>();
            label.text = "Select Option";
            label.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            label.fontSize = 12;
            label.color = Color.white;
            label.alignment = TextAnchor.MiddleLeft;
            
            var labelRect = label.GetComponent<RectTransform>();
            labelRect.anchorMin = Vector2.zero;
            labelRect.anchorMax = Vector2.one;
            labelRect.offsetMin = new Vector2(10, 0);
            labelRect.offsetMax = new Vector2(-10, 0);
            
            var rectTransform = dropdown.GetComponent<RectTransform>();
            rectTransform.sizeDelta = size;
            
            return dropdown;
        }
        
        /// <summary>
        /// Position UI elements in a logical layout
        /// </summary>
        private void PositionUIElements()
        {
            if (targetCanvas == null) return;
            
            // Status text - top left
            if (statusText != null)
            {
                var rect = statusText.GetComponent<RectTransform>();
                rect.anchorMin = new Vector2(0, 1);
                rect.anchorMax = new Vector2(0, 1);
                rect.anchoredPosition = new Vector2(10, -10);
            }
            
            // Skill text - top right
            if (skillText != null)
            {
                var rect = skillText.GetComponent<RectTransform>();
                rect.anchorMin = new Vector2(1, 1);
                rect.anchorMax = new Vector2(1, 1);
                rect.anchoredPosition = new Vector2(-10, -10);
            }
            
            // Buttons - bottom left
            float buttonY = -400;
            float buttonSpacing = 40;
            
            if (loadClassesButton != null)
            {
                var rect = loadClassesButton.GetComponent<RectTransform>();
                rect.anchorMin = new Vector2(0, 0);
                rect.anchorMax = new Vector2(0, 0);
                rect.anchoredPosition = new Vector2(10, buttonY);
            }
            
            if (startCombatButton != null)
            {
                var rect = startCombatButton.GetComponent<RectTransform>();
                rect.anchorMin = new Vector2(0, 0);
                rect.anchorMax = new Vector2(0, 0);
                rect.anchoredPosition = new Vector2(10, buttonY + buttonSpacing);
            }
            
            if (endTurnButton != null)
            {
                var rect = endTurnButton.GetComponent<RectTransform>();
                rect.anchorMin = new Vector2(0, 0);
                rect.anchorMax = new Vector2(0, 0);
                rect.anchoredPosition = new Vector2(10, buttonY + buttonSpacing * 2);
            }
            
            if (useSkillButton != null)
            {
                var rect = useSkillButton.GetComponent<RectTransform>();
                rect.anchorMin = new Vector2(0, 0);
                rect.anchorMax = new Vector2(0, 0);
                rect.anchoredPosition = new Vector2(10, buttonY + buttonSpacing * 3);
            }
            
            if (applyBurnButton != null)
            {
                var rect = applyBurnButton.GetComponent<RectTransform>();
                rect.anchorMin = new Vector2(0, 0);
                rect.anchorMax = new Vector2(0, 0);
                rect.anchoredPosition = new Vector2(10, buttonY + buttonSpacing * 4);
            }
            
            if (resetHealthButton != null)
            {
                var rect = resetHealthButton.GetComponent<RectTransform>();
                rect.anchorMin = new Vector2(0, 0);
                rect.anchorMax = new Vector2(0, 0);
                rect.anchoredPosition = new Vector2(10, buttonY + buttonSpacing * 5);
            }
            
            // Dropdowns - bottom right
            if (skillDropdown != null)
            {
                var rect = skillDropdown.GetComponent<RectTransform>();
                rect.anchorMin = new Vector2(1, 0);
                rect.anchorMax = new Vector2(1, 0);
                rect.anchoredPosition = new Vector2(-10, buttonY + buttonSpacing);
            }
            
            if (targetDropdown != null)
            {
                var rect = targetDropdown.GetComponent<RectTransform>();
                rect.anchorMin = new Vector2(1, 0);
                rect.anchorMax = new Vector2(1, 0);
                rect.anchoredPosition = new Vector2(-10, buttonY + buttonSpacing * 2);
            }
        }
        
        /// <summary>
        /// Log the status of UI elements
        /// </summary>
        private void LogUIStatus()
        {
            Debug.Log("CombatantTestUI Status:");
            Debug.Log($"StatusText: {(statusText != null ? "Found" : "Missing")}");
            Debug.Log($"SkillText: {(skillText != null ? "Found" : "Missing")}");
            Debug.Log($"LoadClassesButton: {(loadClassesButton != null ? "Found" : "Missing")}");
            Debug.Log($"StartCombatButton: {(startCombatButton != null ? "Found" : "Missing")}");
            Debug.Log($"EndTurnButton: {(endTurnButton != null ? "Found" : "Missing")}");
            Debug.Log($"UseSkillButton: {(useSkillButton != null ? "Found" : "Missing")}");
            Debug.Log($"ApplyBurnButton: {(applyBurnButton != null ? "Found" : "Missing")}");
            Debug.Log($"ResetHealthButton: {(resetHealthButton != null ? "Found" : "Missing")}");
            Debug.Log($"SkillDropdown: {(skillDropdown != null ? "Found" : "Missing")}");
            Debug.Log($"TargetDropdown: {(targetDropdown != null ? "Found" : "Missing")}");
        }
        
        /// <summary>
        /// Manually assign UI elements (for when auto-find doesn't work)
        /// </summary>
        [ContextMenu("Find UI Elements")]
        public void FindUIElements()
        {
            FindAndConnectUIElements();
        }
        
        /// <summary>
        /// Create a complete UI setup
        /// </summary>
        [ContextMenu("Create Complete UI")]
        public void CreateCompleteUI()
        {
            // Ensure we have a canvas
            if (targetCanvas == null)
            {
                targetCanvas = FindObjectOfType<Canvas>();
                if (targetCanvas == null)
                {
                    var canvasGO = new GameObject("Canvas");
                    targetCanvas = canvasGO.AddComponent<Canvas>();
                    targetCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
                    canvasGO.AddComponent<CanvasScaler>();
                    canvasGO.AddComponent<GraphicRaycaster>();
                }
            }
            
            // Clear existing elements and recreate
            statusText = null;
            skillText = null;
            loadClassesButton = null;
            startCombatButton = null;
            endTurnButton = null;
            useSkillButton = null;
            applyBurnButton = null;
            resetHealthButton = null;
            skillDropdown = null;
            targetDropdown = null;
            
            CreateMissingUIElements();
            
            if (debugMode)
            {
                Debug.Log("CombatantTestUI: Complete UI setup created");
            }
        }
    }
} 