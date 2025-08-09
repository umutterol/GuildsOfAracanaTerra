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

            // Fireball → Damage(INT) + Burn
            CreateDamageEffect("Fireball", baseDamage: 14f, scaling: 1.0f, DamageEffectSO.DamageScalingStat.Intelligence, critChance: 0.10f);
            CreateApplyStatusEffect("Fireball", ApplyStatusEffectSO.StatusKind.Burn, duration: 2);

            // Shield Bash → Stun
            CreateApplyStatusEffect("Shield Bash", ApplyStatusEffectSO.StatusKind.Stun, duration: 1);

            // Cleave → Damage(STR)
            CreateDamageEffect("Cleave", baseDamage: 12f, scaling: 1.0f, DamageEffectSO.DamageScalingStat.Strength, critChance: 0.05f);

            // Quick Stab → Damage(AGI) + Bleed
            CreateDamageEffect("Quick Stab", baseDamage: 8f, scaling: 1.0f, DamageEffectSO.DamageScalingStat.Agility, critChance: 0.10f);
            CreateApplyStatusEffect("Quick Stab", ApplyStatusEffectSO.StatusKind.Bleed, duration: 3);

            // Mana Shield → Shield (INT-scaling)
            CreateApplyStatusEffect("Mana Shield", ApplyStatusEffectSO.StatusKind.ShieldIntScaling, duration: 2);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("GOAT: Sample Skill Effects generated under Assets/Resources/Skills");
        }

        private static void CreateDamageEffect(string skillName, float baseDamage, float scaling, DamageEffectSO.DamageScalingStat stat, float critChance)
        {
            string folder = CombineAssetPath(RootResourcesPath, skillName);
            EnsureFolder(folder);
            string assetPath = CombineAssetPath(folder, "DamageEffect.asset");

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
            string folder = CombineAssetPath(RootResourcesPath, skillName);
            EnsureFolder(folder);
            string assetPath = CombineAssetPath(folder, "HealEffect.asset");

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

        private static void CreateApplyStatusEffect(string skillName, ApplyStatusEffectSO.StatusKind kind, int duration)
        {
            string folder = CombineAssetPath(RootResourcesPath, skillName);
            EnsureFolder(folder);
            string assetPath = CombineAssetPath(folder, $"{kind}Effect.asset");

            var asset = AssetDatabase.LoadAssetAtPath<ApplyStatusEffectSO>(assetPath);
            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<ApplyStatusEffectSO>();
                asset.effectName = $"{skillName} {kind}";
                asset.statusKind = kind;
                asset.duration = duration;
                AssetDatabase.CreateAsset(asset, assetPath);
            }
        }

        private static void EnsureFolder(string path)
        {
            string normalized = path.Replace('\\', '/');
            var segments = normalized.Split('/');
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

        private static string CombineAssetPath(params string[] parts)
        {
            return string.Join("/", parts).Replace('\\', '/');
        }
    }
}


