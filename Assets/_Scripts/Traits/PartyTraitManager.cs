using System.Collections.Generic;
using UnityEngine;
using GuildsOfArcanaTerra.Combat;

namespace GuildsOfArcanaTerra.Traits
{
    /// <summary>
    /// Manages all active IRL traits on the player's party and applies party-wide bonuses
    /// </summary>
    public class PartyTraitManager : MonoBehaviour
    {
        [Header("Party Members")]
        [SerializeField] private List<Combatant> partyMembers = new List<Combatant>();

        // Track active party-wide effects
        private float aoeDamageBonus = 0f;
        private float partyCritBonus = 0f;

        // Call this at the start of combat
        public void OnCombatStart()
        {
            aoeDamageBonus = 0f;
            partyCritBonus = 0f;

            foreach (var member in partyMembers)
            {
                if (member?.Trait == null) continue;

                // Glass Cannon Main: Party AoE bonus
                if (member.Trait is GlassCannonMainTrait)
                {
                    aoeDamageBonus += 0.10f; // +10% AoE damage per trait
                }
                // Drama Queen: Party crit bonus
                if (member.Trait is DramaQueenTrait)
                {
                    partyCritBonus += 0.05f; // +5% crit per trait
                }
            }

            // Apply party-wide bonuses
            ApplyPartyBonuses();
        }

        // Call this at the end of combat
        public void OnCombatEnd()
        {
            // Clear all party-wide bonuses
            foreach (var member in partyMembers)
            {
                member.SetCritModifier(1f);
                // If you have a global AoE damage modifier, reset it here
            }
            aoeDamageBonus = 0f;
            partyCritBonus = 0f;
        }

        // Apply party-wide bonuses to all members
        private void ApplyPartyBonuses()
        {
            foreach (var member in partyMembers)
            {
                // Apply crit bonus
                member.SetCritModifier(1f + partyCritBonus);
                // AoE damage bonus would be applied in skill logic (see below)
            }
        }

        // Get the current AoE damage bonus for use in skill calculations
        public float GetAOEDamageBonus()
        {
            return aoeDamageBonus;
        }

        // Utility: Set party members (e.g., from TurnOrderSystem or game setup)
        public void SetPartyMembers(List<Combatant> members)
        {
            partyMembers = members;
        }
    }
} 