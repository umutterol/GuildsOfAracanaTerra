using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.IO;
using GuildsOfArcanaTerra.Combat;
using GuildsOfArcanaTerra.Combat.UI;
using GuildsOfArcanaTerra.ScriptableObjects.Classes;

namespace GuildsOfArcanaTerra.EditorTools
{
    public static class TestArenaCreator
    {
        [MenuItem("Tools/GOAT/Create Test Arena Scene")]
        public static void CreateTestArenaScene()
        {
            // Ensure scenes folder exists
            const string scenesFolder = "Assets/_Scenes";
            if (!AssetDatabase.IsValidFolder(scenesFolder))
            {
                AssetDatabase.CreateFolder("Assets", "_Scenes");
            }

            // Create a new scene
            var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
            scene.name = "TestArena";

            // Remove default Main Camera and Directional Light if present (we will keep camera)
            // Keep existing camera and light for visibility

            // Systems root
            var systemsRoot = new GameObject("Systems");
            var statusEffectSystem = systemsRoot.AddComponent<StatusEffectSystem>();

            // Player combatant
            var playerGO = new GameObject("Player_Combatant");
            var player = playerGO.AddComponent<Combatant>();
            // Ensure required components that would normally be created in Awake()
            if (playerGO.GetComponent<GuildsOfArcanaTerra.Combat.Skills.SkillSet>() == null)
                playerGO.AddComponent<GuildsOfArcanaTerra.Combat.Skills.SkillSet>();
            if (playerGO.GetComponent<GuildsOfArcanaTerra.Combat.Effects.StatusEffectManager>() == null)
                playerGO.AddComponent<GuildsOfArcanaTerra.Combat.Effects.StatusEffectManager>();

            // Enemy combatant
            var enemyGO = new GameObject("Enemy_Combatant");
            var enemy = enemyGO.AddComponent<Combatant>();
            if (enemyGO.GetComponent<GuildsOfArcanaTerra.Combat.Skills.SkillSet>() == null)
                enemyGO.AddComponent<GuildsOfArcanaTerra.Combat.Skills.SkillSet>();
            if (enemyGO.GetComponent<GuildsOfArcanaTerra.Combat.Effects.StatusEffectManager>() == null)
                enemyGO.AddComponent<GuildsOfArcanaTerra.Combat.Effects.StatusEffectManager>();

            // Position them in scene view
            playerGO.transform.position = new Vector3(-3f, 0f, 0f);
            enemyGO.transform.position = new Vector3(3f, 0f, 0f);

            // Assign class definitions from Resources (fallback safe)
            var warrior = Resources.Load<CharacterClassDefinition>("Classes/Warrior");
            var mage = Resources.Load<CharacterClassDefinition>("Classes/Mage");

            if (warrior != null)
            {
                var classField = typeof(Combatant).GetField("classDefinition", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                classField?.SetValue(player, warrior);
                player.LoadClassDefinition();
            }
            if (mage != null)
            {
                var classField = typeof(Combatant).GetField("classDefinition", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                classField?.SetValue(enemy, mage);
                enemy.LoadClassDefinition();
            }

            // Canvas setup
            var canvasGO = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            var canvas = canvasGO.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            // EventSystem
            var eventSystem = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));

            // Create simple UI
            TextMeshProUGUI CreateTMP(string name, Transform parent, Vector2 anchoredPos, int fontSize = 28, TextAlignmentOptions align = TextAlignmentOptions.Left)
            {
                var go = new GameObject(name, typeof(RectTransform), typeof(TextMeshProUGUI));
                go.transform.SetParent(parent, false);
                var rect = go.GetComponent<RectTransform>();
                rect.anchorMin = new Vector2(0, 1);
                rect.anchorMax = new Vector2(0, 1);
                rect.pivot = new Vector2(0, 1);
                rect.anchoredPosition = anchoredPos;
                rect.sizeDelta = new Vector2(600, 40);
                var tmp = go.GetComponent<TextMeshProUGUI>();
                tmp.fontSize = fontSize;
                tmp.alignment = align;
                tmp.text = name;
                return tmp;
            }

            Button CreateButton(string name, Transform parent, Vector2 anchoredPos, string label)
            {
                var go = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
                go.transform.SetParent(parent, false);
                var rect = go.GetComponent<RectTransform>();
                rect.anchorMin = new Vector2(0, 1);
                rect.anchorMax = new Vector2(0, 1);
                rect.pivot = new Vector2(0, 1);
                rect.anchoredPosition = anchoredPos;
                rect.sizeDelta = new Vector2(180, 48);
                var img = go.GetComponent<Image>();
                img.color = new Color(0.2f, 0.2f, 0.2f, 0.85f);
                var btn = go.GetComponent<Button>();

                // Label
                var txt = new GameObject("Text", typeof(RectTransform), typeof(TextMeshProUGUI));
                txt.transform.SetParent(go.transform, false);
                var rt = txt.GetComponent<RectTransform>();
                rt.anchorMin = Vector2.zero;
                rt.anchorMax = Vector2.one;
                rt.offsetMin = Vector2.zero;
                rt.offsetMax = Vector2.zero;
                var tmp = txt.GetComponent<TextMeshProUGUI>();
                tmp.text = label;
                tmp.alignment = TextAlignmentOptions.Center;
                tmp.fontSize = 22;

                return btn;
            }

            // HUD texts
            var playerHealthTMP = CreateTMP("PlayerHealthText", canvasGO.transform, new Vector2(20, -20));
            var enemyHealthTMP = CreateTMP("EnemyHealthText", canvasGO.transform, new Vector2(20, -60));
            var logTMP = CreateTMP("CombatLogText", canvasGO.transform, new Vector2(20, -120), 20);
            logTMP.rectTransform.sizeDelta = new Vector2(900, 200);

            // Action buttons row
            var attackBtn = CreateButton("AttackButton", canvasGO.transform, new Vector2(20, -340), "Basic Attack");
            var healBtn = CreateButton("HealButton", canvasGO.transform, new Vector2(210, -340), "Heal");
            var resetBtn = CreateButton("ResetButton", canvasGO.transform, new Vector2(400, -340), "Reset");
            var endTurnBtn = CreateButton("EndTurnButton", canvasGO.transform, new Vector2(590, -340), "End Turn");

            // Skill buttons row
            var skillButtons = new Button[4];
            for (int i = 0; i < 4; i++)
            {
                skillButtons[i] = CreateButton($"SkillButton_{i+1}", canvasGO.transform, new Vector2(20 + i * 190, -400), $"Skill {i+1}");
            }

            // Add SimpleCombatUI and wire references via SerializedObject
            var uiRoot = new GameObject("SimpleCombatUI");
            var simpleUi = uiRoot.AddComponent<SimpleCombatUI>();

            var so = new SerializedObject(simpleUi);
            so.FindProperty("player").objectReferenceValue = player;
            so.FindProperty("enemy").objectReferenceValue = enemy;
            so.FindProperty("statusEffectSystem").objectReferenceValue = statusEffectSystem;
            so.FindProperty("playerHealthText").objectReferenceValue = playerHealthTMP;
            so.FindProperty("enemyHealthText").objectReferenceValue = enemyHealthTMP;
            so.FindProperty("combatLogText").objectReferenceValue = logTMP;
            so.FindProperty("attackButton").objectReferenceValue = attackBtn;
            so.FindProperty("healButton").objectReferenceValue = healBtn;
            so.FindProperty("resetButton").objectReferenceValue = resetBtn;
            so.FindProperty("endTurnButton").objectReferenceValue = endTurnBtn;

            var skillButtonsProp = so.FindProperty("skillButtons");
            if (skillButtonsProp != null && skillButtonsProp.isArray)
            {
                skillButtonsProp.arraySize = 4;
                for (int i = 0; i < 4; i++)
                {
                    skillButtonsProp.GetArrayElementAtIndex(i).objectReferenceValue = skillButtons[i];
                }
            }
            so.ApplyModifiedPropertiesWithoutUndo();

            // Save scene
            const string scenePath = scenesFolder + "/TestArena.unity";
            EditorSceneManager.SaveScene(scene, scenePath);
            AssetDatabase.SaveAssets();

            // Assign demo row positions: player front, enemy back
            if (player != null)
            {
                var soPlayer = new SerializedObject(player);
                var rowProp = soPlayer.FindProperty("row");
                if (rowProp != null) { rowProp.enumValueIndex = (int)GuildsOfArcanaTerra.Combat.Core.RowPosition.Front; soPlayer.ApplyModifiedPropertiesWithoutUndo(); }
            }
            if (enemy != null)
            {
                var soEnemy = new SerializedObject(enemy);
                var rowProp2 = soEnemy.FindProperty("row");
                if (rowProp2 != null) { rowProp2.enumValueIndex = (int)GuildsOfArcanaTerra.Combat.Core.RowPosition.Back; soEnemy.ApplyModifiedPropertiesWithoutUndo(); }
            }

            EditorUtility.DisplayDialog("GOAT", "Test Arena scene created at Assets/_Scenes/TestArena.unity", "OK");
        }
    }
}


