using UnityEngine;
using UnityEngine.UI;

namespace GuildsOfArcanaTerra.Combat.UI
{
    /// <summary>
    /// Debug script to help identify and fix UI connection issues
    /// </summary>
    public class UI_Connection_Debugger : MonoBehaviour
    {
        [Header("Components to Check")]
        [SerializeField] private CombatantTestUI uiManager;
        [SerializeField] private CombatantTest combatantTest;
        
        [Header("Debug Options")]
        [SerializeField] private bool debugOnStart = true;
        [SerializeField] private bool autoFixConnections = true;
        
        private void Start()
        {
            if (debugOnStart)
            {
                DebugUIConnections();
            }
        }
        
        /// <summary>
        /// Debug UI connections and identify issues
        /// </summary>
        [ContextMenu("Debug UI Connections")]
        public void DebugUIConnections()
        {
            Debug.Log("=== UI Connection Debugger ===");
            
            // Find components if not assigned
            if (uiManager == null)
                uiManager = FindObjectOfType<CombatantTestUI>();
            if (combatantTest == null)
                combatantTest = FindObjectOfType<CombatantTest>();
            
            // Check UI Manager
            Debug.Log("--- UI Manager Status ---");
            if (uiManager != null)
            {
                Debug.Log($"UI Manager found: {uiManager.name}");
                CheckUIElements(uiManager);
            }
            else
            {
                Debug.LogError("No CombatantTestUI found in scene!");
            }
            
            // Check CombatantTest
            Debug.Log("--- CombatantTest Status ---");
            if (combatantTest != null)
            {
                Debug.Log($"CombatantTest found: {combatantTest.name}");
                CheckCombatantTestConnections(combatantTest);
            }
            else
            {
                Debug.LogError("No CombatantTest found in scene!");
            }
            
            // Check Canvas
            Debug.Log("--- Canvas Status ---");
            var canvas = FindObjectOfType<Canvas>();
            if (canvas != null)
            {
                Debug.Log($"Canvas found: {canvas.name}");
                CheckCanvasElements(canvas);
            }
            else
            {
                Debug.LogError("No Canvas found in scene!");
            }
            
            // Auto-fix if enabled
            if (autoFixConnections && uiManager != null && combatantTest != null)
            {
                Debug.Log("--- Attempting Auto-Fix ---");
                AutoFixConnections();
            }
            
            Debug.Log("=== Debug Complete ===");
        }
        
        /// <summary>
        /// Check UI elements in the UI Manager
        /// </summary>
        private void CheckUIElements(CombatantTestUI ui)
        {
            Debug.Log($"StatusText: {(ui.StatusText != null ? "✓ Found" : "✗ Missing")}");
            Debug.Log($"SkillText: {(ui.SkillText != null ? "✓ Found" : "✗ Missing")}");
            Debug.Log($"LoadClassesButton: {(ui.LoadClassesButton != null ? "✓ Found" : "✗ Missing")}");
            Debug.Log($"StartCombatButton: {(ui.StartCombatButton != null ? "✓ Found" : "✗ Missing")}");
            Debug.Log($"EndTurnButton: {(ui.EndTurnButton != null ? "✓ Found" : "✗ Missing")}");
            Debug.Log($"UseSkillButton: {(ui.UseSkillButton != null ? "✓ Found" : "✗ Missing")}");
            Debug.Log($"ApplyBurnButton: {(ui.ApplyBurnButton != null ? "✓ Found" : "✗ Missing")}");
            Debug.Log($"ResetHealthButton: {(ui.ResetHealthButton != null ? "✓ Found" : "✗ Missing")}");
            Debug.Log($"SkillDropdown: {(ui.SkillDropdown != null ? "✓ Found" : "✗ Missing")}");
            Debug.Log($"TargetDropdown: {(ui.TargetDropdown != null ? "✓ Found" : "✗ Missing")}");
        }
        
        /// <summary>
        /// Check connections in CombatantTest
        /// </summary>
        private void CheckCombatantTestConnections(CombatantTest ct)
        {
            // Use reflection to check private fields
            var uiManagerField = typeof(CombatantTest).GetField("uiManager", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var statusTextField = typeof(CombatantTest).GetField("statusText", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var skillTextField = typeof(CombatantTest).GetField("skillText", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            var uiManagerValue = uiManagerField?.GetValue(ct);
            var statusTextValue = statusTextField?.GetValue(ct);
            var skillTextValue = skillTextField?.GetValue(ct);
            
            Debug.Log($"UI Manager Reference: {(uiManagerValue != null ? "✓ Assigned" : "✗ Not Assigned")}");
            Debug.Log($"StatusText Reference: {(statusTextValue != null ? "✓ Assigned" : "✗ Not Assigned")}");
            Debug.Log($"SkillText Reference: {(skillTextValue != null ? "✓ Assigned" : "✗ Not Assigned")}");
        }
        
        /// <summary>
        /// Check elements in Canvas
        /// </summary>
        private void CheckCanvasElements(Canvas canvas)
        {
            var texts = canvas.GetComponentsInChildren<Text>();
            var buttons = canvas.GetComponentsInChildren<Button>();
            var dropdowns = canvas.GetComponentsInChildren<Dropdown>();
            
            Debug.Log($"Text elements in Canvas: {texts.Length}");
            foreach (var text in texts)
            {
                Debug.Log($"  - {text.name}: '{text.text}'");
            }
            
            Debug.Log($"Button elements in Canvas: {buttons.Length}");
            foreach (var button in buttons)
            {
                Debug.Log($"  - {button.name}");
            }
            
            Debug.Log($"Dropdown elements in Canvas: {dropdowns.Length}");
            foreach (var dropdown in dropdowns)
            {
                Debug.Log($"  - {dropdown.name}: {dropdown.options.Count} options");
            }
        }
        
        /// <summary>
        /// Attempt to auto-fix connections
        /// </summary>
        private void AutoFixConnections()
        {
            Debug.Log("Attempting to auto-fix UI connections...");
            
            // Force UI manager to find/create elements
            uiManager.FindAndConnectUIElements();
            
            // Force CombatantTest to refresh UI connections
            var setupUIMethod = typeof(CombatantTest).GetMethod("SetupUI", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            setupUIMethod?.Invoke(combatantTest, null);
            
            Debug.Log("Auto-fix completed. Check debug output above for results.");
        }
        
        /// <summary>
        /// Create a complete working setup
        /// </summary>
        [ContextMenu("Create Complete Working Setup")]
        public void CreateCompleteWorkingSetup()
        {
            Debug.Log("Creating complete working setup...");
            
            // Create UI Manager if missing
            if (uiManager == null)
            {
                var uiManagerGO = new GameObject("UI Manager");
                uiManager = uiManagerGO.AddComponent<CombatantTestUI>();
                Debug.Log("Created UI Manager");
            }
            
            // Create CombatantTest if missing
            if (combatantTest == null)
            {
                var combatantTestGO = new GameObject("CombatantTest");
                combatantTest = combatantTestGO.AddComponent<CombatantTest>();
                Debug.Log("Created CombatantTest");
            }
            
            // Create complete UI
            uiManager.CreateCompleteUI();
            
            // Force connections
            AutoFixConnections();
            
            Debug.Log("Complete working setup created!");
        }
        
        /// <summary>
        /// Test UI functionality
        /// </summary>
        [ContextMenu("Test UI Functionality")]
        public void TestUIFunctionality()
        {
            Debug.Log("Testing UI functionality...");
            
            if (uiManager != null)
            {
                // Test text updates
                if (uiManager.StatusText != null)
                {
                    uiManager.StatusText.text = "Test Status - UI is working!";
                    Debug.Log("✓ StatusText updated successfully");
                }
                
                if (uiManager.SkillText != null)
                {
                    uiManager.SkillText.text = "Test Skills - UI is working!";
                    Debug.Log("✓ SkillText updated successfully");
                }
                
                // Test button interactions
                if (uiManager.LoadClassesButton != null)
                {
                    Debug.Log("✓ LoadClassesButton found and accessible");
                }
                
                if (uiManager.StartCombatButton != null)
                {
                    Debug.Log("✓ StartCombatButton found and accessible");
                }
            }
            else
            {
                Debug.LogError("✗ UI Manager not found for testing");
            }
            
            Debug.Log("UI functionality test completed!");
        }
        
        /// <summary>
        /// Clear all UI elements and recreate
        /// </summary>
        [ContextMenu("Clear and Recreate UI")]
        public void ClearAndRecreateUI()
        {
            Debug.Log("Clearing and recreating UI...");
            
            if (uiManager != null)
            {
                // Clear existing UI elements
                var canvas = FindObjectOfType<Canvas>();
                if (canvas != null)
                {
                    // Remove all children except the canvas itself
                    for (int i = canvas.transform.childCount - 1; i >= 0; i--)
                    {
                        DestroyImmediate(canvas.transform.GetChild(i).gameObject);
                    }
                }
                
                // Recreate UI
                uiManager.CreateCompleteUI();
                
                Debug.Log("UI cleared and recreated!");
            }
            else
            {
                Debug.LogError("No UI Manager found!");
            }
        }
    }
} 