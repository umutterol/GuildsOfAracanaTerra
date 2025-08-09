using UnityEditor;
using UnityEngine;
using System.IO;
using GuildsOfArcanaTerra.Combat.Skills.Effects;

namespace GuildsOfArcanaTerra.EditorTools
{
    public static class SkillEffectCreator
    {
        private const string RootResourcesPath = "Assets/Resources/Skills";

        [MenuItem("GOAT/Generate Sample Skill Effects")] 
        public static void GenerateSampleSkillEffects()
        {
            EnsureFolder(RootResourcesPath);

            // Arcane Bolt → Damage(INT)
            CreateDamageEffect("Arcane Bolt", baseDamage: 10f, scaling: 1.0f, DamageEffectSO.DamageScalingStat.Intelligence, critChance: 0.15f);

            // Basic Attack → Damage(STR)
            CreateDamageEffect("Basic Attack", baseDamage: 5f, scaling: 1.0f, DamageEffectSO.DamageScalingStat.Strength, critChance: 0.05f);

            // Heal → Heal(INT)
            CreateHealEffect("Heal", baseHealing: 12f, scaling: 1.0f);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("GOAT: Sample Skill Effects generated under Assets/Resources/Skills");
        }

        private static void CreateDamageEffect(string skillName, float baseDamage, float scaling, DamageEffectSO.DamageScalingStat stat, float critChance)
        {
            string folder = Path.Combine(RootResourcesPath, skillName);
            EnsureFolder(folder);
            string assetPath = Path.Combine(folder, "DamageEffect.asset");

            var asset = AssetDatabase.LoadAssetAtPath<DamageEffectSO>(assetPath);
            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<DamageEffectSO>();
                asset.effectName = $"{skillName} Damage";
                asset.baseDamage = baseDamage;
                asset.scaling = scaling;
                asset.scalingStat = stat;
                asset.critChance = critChance;
                AssetDatabase.CreateAsset(asset, assetPath);
            }
        }

        private static void CreateHealEffect(string skillName, float baseHealing, float scaling)
        {
            string folder = Path.Combine(RootResourcesPath, skillName);
            EnsureFolder(folder);
            string assetPath = Path.Combine(folder, "HealEffect.asset");

            var asset = AssetDatabase.LoadAssetAtPath<HealEffectSO>(assetPath);
            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<HealEffectSO>();
                asset.effectName = $"{skillName} Heal";
                asset.baseHealing = baseHealing;
                asset.scaling = scaling;
                AssetDatabase.CreateAsset(asset, assetPath);
            }
        }

        private static void EnsureFolder(string path)
        {
            var segments = path.Split('/');
            string current = segments[0];
            for (int i = 1; i < segments.Length; i++)
            {
                string next = current + "/" + segments[i];
                if (!AssetDatabase.IsValidFolder(next))
                {
                    AssetDatabase.CreateFolder(current, segments[i]);
                }
                current = next;
            }
        }
    }
}


