# **🎮 Guilds of Arcana Terra – Game Design Document (v1.1)**

## **🧭 Game Overview**

**Guilds of Arcana Terra** is a single-player, turn-based tactical RPG that blends the tension of *Darkest Dungeon*, the progression of *AFK Journey*, and the satirical charm of managing an MMO guild. You are not just commanding adventurers — you are the **Guild Master** of a fictional MMO world, curating a roster of eccentric, trait-driven characters who behave like real-life players.

Your job is to assemble balanced teams, conquer tactical dungeons, grow your reputation through seasonal leaderboards, and navigate the chaotic drama of your guild’s internal ecosystem.

---

## **🧱 Core Gameplay Pillars**

1. **Tactical Turn-Based Combat**

   * Positioning, initiative, cooldowns, and status effects form a rich strategic layer.

2. **IRL Trait System**

   * Characters embody MMO player archetypes (e.g., Min-Maxer, Drama Queen), affecting skills, events, and team synergy.

3. **Guild Management**

   * Recruit, level, rotate, and retire characters. Balance classes, traits, and affinities.

4. **Progression Systems**

   * Gear upgrades, crafting, affinity bonds, morale stability, and fame-based cosmetics.

5. **Meta-Narrative Systems**

   * Chat-driven flavor, trait barks, mini-events, solo quests, and decision dilemmas.

---

## **💡 Design Philosophy**

* **Accessible Depth**: Easy to pick up, with layered mastery.

* **Systemic Humor**: IRL traits drive satire and emergent comedy.

* **No Bloat**: Streamlined mechanics; no skill trees, no rerolls.

* **Player Expression**: Wide room for creative comp building and story ownership.

* **Tactical Identity**: Each class feels distinct and strategically relevant.

---

## **📖 Table of Contents (v1.1)**

1. Stat System

2. Combat System

3. IRL Trait System

4. Morale System *(new)*

5. Combat Affinity System *(new)*

6. Trait Barks *(new)*

7. Character Class Definitions *(expanded to 12 classes)*

8. Gear, Upgrades, Salvage & Crafting

9. Dungeon Flow & Enemy AI

10. Party Management

11. Guild System

12. Meta-Systems: Leaderboards, Solo Quests, Events

13. UI & Game Flow

14. Tutorial Design

---

## ***📊 1\. Stat System***

*Guilds of Arcana Terra uses a focused, impactful stat system designed to power both combat decisions and strategic team-building.*

### ***🧮 Primary Stats***

*Each character has 3 primary stats:*

* ***STR (Strength)** – Increases melee damage and tanking capability. Affects many Warrior, Paladin, and Berserker skills.*  
* ***AGI (Agility)** – Determines initiative (turn order), dodge chance, and crit rate. Core for Rogue, Monk, Ranger.*  
* ***INT (Intelligence)** – Powers magic damage, healing, and cooldown reduction. Essential for Mage, Cleric, Arcanist, Druid.*

### ***🛡 Secondary Stats***

*Derived or conditional effects:*

* ***DEF (Defense)** – Reduces incoming damage flatly.*  
* ***VIT (Vitality)** – Governs total HP.*  
* ***CRIT** – Chance to deal 150% damage. Derived from AGI.*  
* ***HIT** – Chance to avoid missing. Most attacks are guaranteed unless stated.*  
* ***DODGE** – Chance to evade an attack entirely. Derived from AGI and situational buffs.*  
* ***RESISTANCES** (future scope) – For resisting DOTs or CC.*

### ***🔁 Cooldown System***

*There is **no mana/energy system**. Instead:*

* *All active skills use cooldowns (CD) to pace usage.*  
* *Cooldowns begin ticking down the moment combat starts.*  
* *Every character has 1 Basic Attack (no CD).*  
* *Some effects and passives modify cooldown behavior (e.g. Mana Surge, Theorycrafter Trait).*

### ***🌀 Stat Progression***

*Characters level up to 20\. Each level grants:*

* *\+5 to a class-relevant primary stat*  
* *\+2 to a secondary stat (rotates between DEF/VIT/CRIT)*  
* *\+20 HP*  
* *Minor skill upgrades at level 5/10/15/20*

*Leveling is meant to reinforce class identity, not transform it.*

---

## **⚔️ 2\. Combat System**

Guilds of Arcana Terra features a tactical, turn-based battle system focused on skill timing, positioning, and trait synergy.

### **🌀 Turn Order**

* Initiative is determined at the start of each round based on AGI.  
* Higher AGI acts first; ties are broken randomly.  
* Turn order is updated at the start of each round — not live/dynamic.

### **🧭 Battlefield Layout**

* Two rows per side: **Frontline** and **Backline**.  
* Frontline: 3 max characters. Takes brunt of melee and AoE skills.  
* Backline: 2 max characters. Often safer but vulnerable to ranged or flanking attacks.

### **🎯 Targeting & Zones**

* Skills are zone-tagged: **Single-Target**, **Front Row AoE**, **Back Row AoE**, **Random 3**, **Full Field**, etc.  
* Positioning matters — AoE cleaves won't hit backline unless stated.

### **🔁 Cooldown Flow**

* Skills start **off cooldown** at the beginning of combat.  
* CDs reduce by 1 at the end of the user's turn.  
* Stunning, silencing, or delaying a unit does **not** pause cooldown timers.

### **🧠 Tactical Depth**

* Skill combos and timing are core: chaining a debuff \+ a finisher.  
* Control skills like **Stun**, **Slow**, and **Silence** shift initiative.  
* Traits and positioning determine risk/reward tradeoffs.  
* Revive Tokens add a layer of tension and second-chance mechanics.

### **🛡 Status Effects & Keywords**

| Keyword | Description |
| ----- | ----- |
| **Stun** | Target skips their next turn. Immune for 1 round after. |
| **Slow** | \-10% AGI for 2 turns. Affects dodge & turn order. |
| **Burn** | Deals 25% INT at start of turn (2 turns). |
| **Poison** | Deals 15% AGI and \-25% healing (3 turns). |
| **Bleed** | 20% AGI per turn. Stacks up to 3x. |
| **Silence** | Can't use skills for 1 turn. Can still basic attack. |
| **Shield** | Absorbs damage. Based on caster INT or flat value. |
| **Overdrive** | \+50% skill damage for 1 turn. Usually triggered by traits. |
| **Mark** | \+20% incoming damage from all sources. |
| **Camouflage** | Untargetable for 1 turn. Avoids all targeted abilities. |

---

## **🎭 3\. IRL Trait System**

Every character in Guilds of Arcana Terra is not just an adventurer — they're a representation of a fictional MMO player with a unique behavioral identity. These personalities, known as **IRL Traits**, are the heart of the game’s satire and systemic complexity.

### **🧠 What Traits Represent**

IRL Traits simulate real-world MMO player archetypes — like "Min-Maxers", "Drama Queens", or "AFK Farmers". They introduce:

* Unique personal gameplay modifiers  
* Team-wide passive effects  
* Negative quirks or limitations  
* Event triggers and bark generators

Each character has **one permanent IRL Trait**, locked at recruitment.

### **⚙️ Trait Structure**

An IRL Trait has:

* **Name** (e.g. "Hardcore Permadeather")  
* **Tagline** (flavor quote)  
* **Personal Modifier** – changes their own behavior, stats, or abilities  
* **Party Effect** – applies a passive or triggered bonus to the entire party  
* **Downside** – limits usage, behavior, or introduces risk

### **🔄 Combat Integration**

Traits affect how characters behave in battle:

* Some reward specific behaviors (e.g. not repeating skills)  
* Others penalize “off-brand” decisions (e.g. using basic attacks)  
* Traits can interact with cooldowns, morale, or affinity

### **📣 Narrative Integration**

* **Trait Barks** trigger during combat or events, reinforcing flavor  
* **Mini-Events** between dungeons may be based on trait interactions  
* **Whispers** simulate chat messages flavored by IRL Traits

### **📋 Trait List (v3.0 Sample Selection)**

| Trait | Flavor Quote | Personal Effect | Party Effect | Downside |
| ----- | ----- | ----- | ----- | ----- |
| **Theorycrafter** | "Knows every frame." | \+10% skill damage if not repeated within 3 turns | \-1 CD on all party skills in round 1 | Can only basic attack once per combat |
| **AFK Farmer** | "I'll be back in 3 hours..." | \+30% damage after skipping a turn | Allies gain \+10% DEF if this unit skips a turn | Always acts last |
| **Drama Queen** | "I soloed it while on fire." | Overdrive on being hit by 3+ enemies | \+5% crit if an ally is under 50% HP | Causes \-10 morale to team on crit |
| **Hardcore Permadeather** | "If I die, delete me." | \+50% stats until death | Revive Tokens are \+20% stronger | Cannot be revived in dungeon |
| **Meta Slave** | "Saw it on a tier list." | \+15% damage if targeting same enemy as previous attacker | \+5% hit chance to party | Cannot equip un-upgraded items |
| **Guild Leader** | "Follow my lead." | Slot 1 gives next 2 allies \+10% damage | If all traits unique: \-1 CD to all | Must be in slot 1 |

Note: Final launch roster includes 12–15 traits, each with systemic hooks.

---

## **🧠 4\. Morale System (Stress-lite)**

The **Morale System** simulates a character’s mental state — a blend of motivation, focus, and psychological fatigue. Unlike health, morale doesn't determine life or death, but it **heavily influences performance and behavior**.

### **🎯 System Goals**

* Add long-term consequences to repeated use or poor synergy  
* Encourage roster rotation and downtime  
* Deepen the expression of IRL Traits and narrative tone

### **📉 Morale Scale**

Each character has a **Morale score (0–100)**:

* Starts at **100** for each dungeon run  
* Can persist between dungeons depending on rest, events, or outcomes

### **🛑 Negative Morale Triggers**

| Event | Morale Loss |
| ----- | ----- |
| Party member dies | \-25 |
| Dungeon failure | \-30 |
| Repeated skill use violating trait (e.g. for Theorycrafter) | \-10 |
| Benched too long (3+ dungeons) | \-15 |
| Trait-specific penalty (e.g. Drama Queen on crit) | \-10 to others |

### **✅ Positive Morale Triggers**

| Event | Morale Gain |
| :---- | :---- |
| Dungeon win | \+20 |
| Leaderboard Top 3 finish | \+25 |
| Trait synergy event | \+10 |
| Rest period / Guild Festival | \+20 |
| Solo quest success | \+10–15 |

### **⚠️ Morale Threshold Effects**

| Morale Range | Effect |
| :---- | :---- |
| 70–100 | No penalty |
| 40–69 | \-5% DEF and INT (fog of war, emotional distraction) |
| 20–39 | May randomly skip turn or reject buffs |
| 0–19 | Random skill use, disables trait bonuses |

Note: Morale is **visible in the character tooltip and party builder**, but not presented as a primary stat.

### **🛏 Rest & Recovery**

* **Rest tokens** are granted every few dungeon runs  
* Assigning a character to Rest removes them from the party for 1 run and restores **\+50 Morale**  
* Events, mini-games, and solo quests can also passively boost morale over time

### **🔄 Trait & Event Interactions**

* Traits can amplify or mitigate morale effects  
* Decisions in “Yes, Your GM” events often affect morale (e.g. siding with one guildmate)  
* Barks are triggered at morale breakpoints to add flavor and emotional feedback

---

## **🤝 5\. Combat Affinity System**

The **Combat Affinity System** tracks the relational bond between any two characters based on how often they fight together. This system supports strategic synergy, party experimentation, and narrative expression.

### **🎯 System Goals**

* Encourage party rotation and duo experimentation  
* Reward consistency and long-term character pairing  
* Add flavor, barks, and bonus effects without over-powering metas

### **🔗 Affinity Score**

Each unique pair of characters shares an **Affinity Score (0–100)**:

* Starts at 0, increases only when both characters are in the same dungeon run  
* Stored permanently in the Guild save file

### **📈 Gaining Affinity**

| Condition | Affinity Gain |
| ----- | ----- |
| Dungeon run together | \+10 |
| Synergy moment (buff → heal → kill) | \+5 |
| Shared victory with no deaths | \+5 |
| Mini-event featuring both characters | \+10–15 |

### **🧨 Optional Conflict (Future Scope)**

* Negative Affinity could be introduced for trait clashes or decision fallout (e.g. Bard vs. Drama Queen)

### **🎁 Affinity Threshold Rewards**

| Score | Reward |
| :---- | :---- |
| 25 | Unique barks between pair, new chatlines in \[Guild\] tab |
| 50 | Passive bonus (e.g. \+5% healing or crit chance when adjacent) |
| 75 | Combat combo bonus if acting back-to-back (e.g. \+10% crit or defense shred) |
| 100 | Unlock "Linked Trait" — minor shared passive or visual effect (cosmetic or narrative only) |

### **📊 UI & Feedback**

* Affinity is shown in the Party Builder as a **small icon or colored line** between slotted characters  
* Hovering shows numerical score \+ recent synergy history  
* Post-dungeon screen can display "Affinity Gain" pop-ups like EXP

### **🔄 Narrative Integration**

* Affinity affects mini-events (e.g. party arguments, joint missions)  
* Can be referenced in Whispers and Trait Barks  
* Affects likelihood of combo interactions (e.g. one character finishing the other's target)

Design Note: Affinity bonuses are **not power spikes** — they offer consistent flavor and mild tactical advantage, but never replace class synergy or trait design.

## **🗯 6\. Trait Barks System**

The **Trait Barks System** gives voice to each character's IRL personality, turning gameplay moments into flavorful, reactive text. Barks are brief lines of dialogue that trigger contextually and reflect the character’s IRL Trait — contributing to immersion, humor, and emotional feedback.

### **🎯 System Goals**

* Add charm and reactivity to combat and events  
* Reinforce IRL Traits through behavior and dialogue  
* Provide emergent storytelling and meta-parody moments

### **🔄 When Barks Trigger**

| Category | Examples |
| ----- | ----- |
| **Combat** | On kill, on crit, on being downed, on dungeon start |
| **Morale Events** | When morale hits a threshold (e.g. drops below 40 or hits 100\) |
| **Trait Interactions** | A trait condition is met (e.g. Meta Slave follows someone’s attack) |
| **Event System** | During Yes, Your GM decisions or solo quest outcomes |
| **Affinity Milestones** | Unique duo barks at affinity 25+ |

### **🎭 Bark Types**

* **Standard Barks**: Solo character reactions based on their IRL Trait  
* **Paired Barks**: Affinity-based dialog lines (e.g. "You again? Try not to die this time.")  
* **Meta Barks**: Refer to MMO culture and systems (“This fight feels like a DPS check.”)

### **🗃 Trait Bark Structure**

Each IRL Trait has a **bark pool** of 5–10 lines per trigger type:

* **Drama Queen**  
  * \*"I soloed a world boss with one HP and a dream\!"  
  * \*"Guess who’s carrying again? You’re welcome."  
* **AFK Farmer**  
  * \*"Wait… what phase are we in?"  
  * \*"Let me auto this real quick."  
* **Theorycrafter**  
  * \*"Not optimal… but statistically viable."  
  * \*"That was a 14.3% chance to proc. Calculated."

### **💬 Delivery Mechanism**

* Barks are shown in the **chat window** (\[Guild\], \[Local\], \[Whispers\])  
* Optional **combat popups** for critical events (downed, boss kill)  
* Audio delivery can be explored in future scope for VO expansion

### **📊 UI & Modularity**

* Players can toggle bark frequency in settings (e.g. Full / Important Only / None)  
* Barks are linked directly to Trait ScriptableObjects for extensibility  
* Event designers can plug bark triggers into event data structures

Design Note: Trait Barks do not affect gameplay outcomes but **may influence Morale or Affinity passively** when narrative decisions reference them.

---

## **🧙‍♂️ 7\. Character Class Definitions**

Guilds of Arcana Terra features 12 playable classes, grouped across four archetypes: Tank/Bruiser, Support/Hybrid, Melee DPS, and Ranged/Control DPS. Each class has a strong identity with a consistent skill kit:

* **1 Basic Attack** (no cooldown)

* **4 Active Skills** (with cooldowns)

* **1 Passive Skill** (always-on bonus)

Skills scale off primary stats (STR, AGI, INT), and all skills are designed to interlock with targeting rules, trait behaviors, and dungeon demands.

---

### **🛡 Tank / Bruiser Classes**

#### **Warrior (STR-Based)**

* **Identity**: Classic taunt tank with cleave damage.

* **Highlights**: AoE cleave, taunt control, DEF scaling.

* **Passive**: \+DEF scaling with missing HP.

#### **Paladin (STR/INT Hybrid)**

* **Identity**: Self-sustaining tank with shields and small heals.

* **Highlights**: AoE shielding, team buffs, Light damage.

* **Passive**: Applies healing to lowest HP ally after using any buff skill.

#### **Warden (STR-Based)**

* **Identity**: Attrition tank with DOTs and damage reflection.

* **Highlights**: Thorns-style passive, slow but unkillable.

* **Passive**: Returns % of melee damage taken as DOT to attacker.

---

### **✨ Support / Hybrid Classes**

#### **Cleric (INT-Based)**

* **Identity**: Pure healer with cleanse, shielding, and hybrid AoEs.

* **Highlights**: Reactive healing, AoE heal/damage.

* **Passive**: Healing after damage dealt.

#### **Druid (INT-Based)**

* **Identity**: DOT-centric healer with nature-based regen and poisons.

* **Highlights**: Dual scaling heals and poisons, terrain-style effects.

* **Passive**: Heals party over time whenever a DOT is active on enemies.

#### **Bard (AGI-Based)**

* **Identity**: Tempo manipulator with buffs, debuffs, and minor healing.

* **Highlights**: Turn order control, Overdrive enabler.

* **Passive**: Extends buff durations by 1 turn when a combo is completed.

---

### **🗡 Melee DPS Classes**

#### **Rogue (AGI-Based)**

* **Identity**: Assassin-style backline killer with poison and evasion.  
* **Highlights**: Teleportation, DOTs, crit scaling.  
* **Passive**: Bonus crit chance if not targeted last round.

#### **Monk (AGI-Based)**

* **Identity**: Counter-punch brawler with stuns and healing.  
* **Highlights**: Stun \+ heal combos, dodge scaling.  
* **Passive**: Heal self for a % of damage dealt if stunned an enemy.

#### **Berserker (STR-Based)**

* **Identity**: Glass cannon melee with fury-style stacking damage.  
* **Highlights**: Escalating damage with each turn.  
* **Passive**: Gains \+5% damage each turn unspent on defensive actions.

---

### **🎯 Ranged / Control DPS Classes**

#### **Mage (INT-Based)**

* **Identity**: AoE nuker with status-heavy spell kit.  
* **Highlights**: Burn, slow, huge AoE burst.  
* **Passive**: Party INT buff \+ cooldown reduction every 5 turns.

#### **Ranger (AGI-Based)**

* **Identity**: Precision ranged striker with trap effects.  
* **Highlights**: Stealth, mark synergy, critical combos.  
* **Passive**: Crits extend active buffs on self.

  #### **Ballist (STR-Based)**

* **Identity**: Ranged physical bruiser who hurls heavy projectiles (javelins, ballista bolts) from the backline.  
* **Highlights**: Piercing single-target attacks, armor shred, long cooldown burst.  
* **Passive**: Deals \+10% damage to enemies with shields or DEF buffs.


All class kits follow the cooldown-based system and support class/trait synergy. Each has their own dedicated item sets and affinity pairings.

---

*...\[snip for brevity\]...*

---

## ***🧙‍♂️ 7\. Character Class Definitions***

*...\[snip for brevity\]...*

---

## ***🛠 8\. Gear, Upgrade, Salvage & Crafting System***

*Equipment progression in GOAT is designed to be linear, impactful, and synergistic with classes, traits, and dungeons — without overwhelming complexity.*

### ***🎒 Equipment Slots***

*Each character equips 4 core pieces:*

* ***Weapon***

* ***Helmet***

* ***Chest***

* ***Legs***

*Future scope: Cosmetic/offhand/vanity slots.*

*Each item provides:*

* ***\+DEF** (always)*

* ***\+VIT** (always)*

* ***\+Primary Stat** (STR/AGI/INT — item-dependent)*

### ***🌈 Item Rarity Tiers***

| *Rarity* | *Bonus Effects* |
| ----- | ----- |
| *Common* | *Basic stats only* |
| *Rare* | *Higher stat rolls* |
| *Epic* | *One secondary bonus (crit, hit, etc.)* |
| *Legendary* | *Set bonuses, unique effects* |

### ***🧩 Set Items***

*Each class has at least 2 dedicated item sets. Sets provide:*

* ***2-piece bonus** (stat buff or trigger)*

* ***4-piece bonus** (mechanic-altering effect)*

#### ***Example: Berserker Set***

* *2-piece: \+10 STR*

* *4-piece: If HP \< 50%, gain \+25% damage*

#### ***Example: Druid Set***

* *2-piece: \+10 INT*

* *4-piece: Poisons last 1 turn longer*

### ***🧪 Upgrade System***

*Items can be upgraded from **\+0 to \+5** using dungeon materials. Effects stack:*

| *Upgrade* | *Effect* |
| ----- | ----- |
| *\+1* | *Add \+1 to a stat* |
| *\+2* | *\+1 VIT or DEF* |
| *\+3* | *Add missing stat if not present* |
| *\+4* | *Double one stat bonus* |
| *\+5* | *Add minor passive or trigger if Epic+* |

*   
  ***Materials**:*

  * ***Basic Alloy**: Common/Rare*

  * ***Enchanted Filament**: Epic*

  * ***Legendary Core**: Legendary*

*Each upgrade consumes more material than the last.*

### ***♻️ Salvage System***

* *Salvaging items returns a % of their upgrade material:*

|  *Upgrade Level* |  *Material Returned* |
| ----- | ----- |
| *\+0* | *0%* |
| *\+1* | *25%* |
| *\+2* | *40%* |
| *\+3* | *60%* |
| *\+4* | *60%* |
| *\+5* | *60%* |

* *Only upgrade materials are returned — not base stats.*

* *Salvaging Legendary items gives only Legendary Cores.*

### ***🔧 Crafting Flow***

1. ***Find item in dungeon drop***

2. ***Use or salvage it***

3. ***Use materials to upgrade key gear***

### ***📦 Future Enhancements***

* *Dungeon-specific visual variants*

* *Named legendaries with story lore*

* *Class-specific relic slots or socketing*

---

## ***🗺 9\. Dungeon Flow & Enemy AI***

*Dungeons in GOAT are repeatable, instanced combat sequences featuring escalating enemy encounters, boss mechanics, and randomized events. Each dungeon rewards gear, materials, and leaderboard scores.*

### ***🧱 Dungeon Structure***

* *Each dungeon contains **3–5 encounters**, escalating in difficulty.*

* *Final room is a **Boss or Elite Encounter**.*

* *Enemy compositions vary per run using class-based templates.*

### ***🧭 Encounter Flow***

1. ***Pre-Combat***

   * *Visual setup, enemy silhouettes, ability preview*

2. ***Combat Phase***

   * *Standard turn-based tactical battle*

3. ***Post-Combat***

   * *Loot, EXP, Morale/Affinity changes, random event triggers*

### ***🔁 Dungeon Loop***

* *Party persists across encounters*

* *No HP/morale recovery between fights unless via trait/event/ability*

* *Revive Tokens usable **between** fights only*

#### Revive Tokens
- Consumable currency available in limited quantity per run/season
- Usable only during intermission between fights and on the Post‑Dungeon Screen
- Effects may be modified by traits (e.g., Hardcore Permadeather: tokens are stronger but cannot revive self in‑dungeon)
- Usage is recorded for Results Screen summaries and leaderboard audit

### ***🧠 Enemy Design Philosophy***

*Enemies are not generic mobs. They mimic the class-based structure of player characters, making skill reads and counterplay possible.*

#### 

#### 

#### ***AI Behavior Profiles***

| *AI Type* | *Description* |
| ----- | ----- |
| *Aggressor* | *Focuses lowest HP, burst damage priority* |
| *Disruptor* | *Applies status effects (DOTs, slows, debuffs)* |
| *Protector* | *Shields/tanks for allies, uses taunts* |
| *Healer* | *Prioritizes heals and cleanse under 50% HP* |
| *Wild Card* | *Unpredictable skill use, rotates patterns* |

#### ***Example Enemies (Craghold Excavation)***

* ***Tunnel Skirmisher** (Rogue-type, Aggressor)*

* ***Cult Pyromancer** (Mage-type, Disruptor)*

* ***Rebel Brawler** (Warrior-type, Protector)*

* ***Blood Acolyte** (Cleric-type, Healer)*

### ***💀 Boss Design Notes***

* *Bosses have 3 Active Skills, no passive or traits*

* *Always use Wild Card AI*

* *Trigger special mechanics based on turn or deaths*

#### ***Sample Boss: Foreman Vex***

* *Class Base: Mage/Warrior Hybrid*

* *Skills: Fireball, Arcane Bolt, Warcry*

* *Trait: Starts with shield, gains \+DMG when ally dies*

### ***⚔ Rewards Structure***

* *Per‑dungeon drop tables with themed loot (e.g., STR/AGI themed dungeons)*

* *Guaranteed base drops \+ materials per clear*

* *Bonus rewards for no deaths, fast clears, or trait synergy*

* *Seasonal cosmetics/frames may drop from high placements or special challenges*

Design Note: Rewards feed Gear and Guild systems, emphasizing replayability and optimization.

---

## ***🔧 10\. Party Management System***

*Party composition is a critical layer in GOAT’s tactical and narrative gameplay. Players select and manage five active members from a larger roster of guild recruits, with decisions affecting performance, morale, synergy, and leaderboard viability.*

### ***🧩 Party Formation Rules***

* *Exactly **5 characters per dungeon run***

* ***No class or trait duplication restrictions***

* ***Frontline/Backline position** must be assigned manually*

* *Traits, Affinity, Morale, and gear should all be considered when selecting a party*

### ***↔️ Positioning Mechanics***

* *Frontline: absorbs melee and most AoE skills; ideal for tanks/melee*

* *Backline: safer for ranged/support; vulnerable to specific skills (e.g. multi-hit, teleport)*

### ***🔁 Rotation & Bench Behavior***

* ***Benched characters** do not participate in dungeons*

* *They do **not earn EXP**, unless under effects from solo quests or guild upgrades*

* *Characters left benched for **3+ runs** may suffer **\-15 morale***

* *Swapping characters is only allowed **between dungeons**, not mid-run*

### ***🧠 Party Synergy Factors***

1. ***Class Roles** – Ensure balanced distribution (Tank/Heal/DPS)*

2. ***IRL Traits** – Some traits punish or boost specific combinations*

3. ***Affinity** – High-affinity pairs gain bonus effects when placed together*

4. ***Morale** – Low morale characters are prone to failure or randomness*

5. ***Leader Traits** – Traits like “Guild Leader” require specific positioning*

### ***🧙‍♂️ Party UI Design***

* *Party Builder supports **drag & drop**, **filter by trait/class/stat***

* *UI displays:*

  * *Morale score*

  * *Trait summary*

  * *Gear score*

  * *Affinity lines between units*

### ***📋 Pre-Run Validation***

* *Validation ensures all slots filled*

* *Warns if:*

  * *Duplicate character (same class/trait combo)*

  * *Characters have low morale (\<20)*

  * *Illegal position traits (e.g. Guild Leader not in slot 1\)*

### 

### ***📈 EXP Distribution***

* ***Full EXP** awarded only to characters who completed the dungeon*

* *Special events or upgrades may offer **partial EXP to bench***

* *EXP sharing is **equal across party members***

---

## ***🧱 11\. Guild System***

*In GOAT, the player is the Guild Master — a fictional MMO account managing a roster of adventurers (characters). All progression, customization, and strategic expansion is driven by the Guild layer.*

### ***🏰 Guild Identity***

* *Players choose a **guild name**, **banner**, and **motto** at start*

* *Cosmetic upgrades unlock via Fame and Guild Level*

* *Guild UI serves as the main hub (roster, crafting, leaderboard, settings)*

### ***📈 Guild Level & Fame***

| *System* | *Function* |
| ----- | ----- |
| ***Guild EXP*** | *Earned from dungeon clears, boss kills, event outcomes* |
| ***Fame*** | *Gained from leaderboard placement, trait events, and long-term play* |
| ***Guild Level*** | *Unlocks more roster slots, better recruit odds, and cosmetic frames* |

#### ***Leveling Rewards***

* *\+1 Roster Slot every level (max 30 planned)*

* *\+5% chance to encounter Rare/Epic recruits*

* *Unlocks Guild Hall cosmetics, title cards*

### ***🧲 Recruitment System***

*There are three core recruitment paths:*

1. ***Looking for Guild Board***

   * *Rotating list of potential recruits*

   * *Shows class, trait, level, gear preview*

   * *Players may reroll options daily (uses Fame)*

2. ***Scout Other Guilds***

   * *Browse AI-controlled guild rosters*

   * *Attempt to recruit using persuasion (dialogue check \+ Fame cost)*

   * *Results influenced by shared class/trait preferences*

3. ***Incoming Applications***

   * *Recruits apply based on dungeon success or unique events*

   * *Higher Guild Level improves quality*

   * *May include pre-leveled or pre-affinitized characters*

### ***🚫 Roster Limits & Retirement***

* *Total characters capped by Guild Level*

* *Retiring a character is permanent but grants:*

  * *Partial Guild EXP refund*

  * *Transfer token (move trait or gear to a new character — future scope)*

### ***🧠 Trait Variety Rules***

* *Duplicate traits are allowed in roster, but discouraged*

* *Only **one exact duplicate** of a class \+ trait combo allowed at a time*

### ***🛠 Guild Upgrades (Planned)***

* *Systems to passively:*

  * *Train benched characters*

  * *Boost gear drop rates*

  * *Improve crafting yields*

* *Unlocked via Guild Level or mini-events*

---

## ***🌐 12\. Meta-Systems: Leaderboards, Solo Quests, Yes, Your GM***

*GOAT's metagame systems reward creativity, long-term investment, and humor. These systems give your guild identity beyond combat and contribute to persistent progression.*

### ***🏆 Leaderboards***

Each dungeon exposes two leaderboards, and there are two global leaderboards:

#### Per-Dungeon Boards
- **World First**: Tracks the first 100 unique guild clears ordered by UTC datetime. Once filled, the board is locked for that dungeon/season.
- **Speedrun**: Tracks fastest clear times per dungeon. Multiple entries per guild may be stored, but the board view shows the guild's best run. Also captures total turns and party composition (class + trait per character).

Per-run data captured:
- Guild name/id
- Dungeon id
- Clear datetime (UTC)
- Clear time (ms)
- Total turns
- Party composition (class + trait per character)
- Build/season version

#### Global Boards
- **Global World First**: Aggregates world-first placements across all dungeons into a global standing (policy: rank points per placement).
- **Global Speedrun**: Aggregates the best-per-guild-per-dungeon speedrun results across all dungeons to rank the fastest guilds overall.

#### Seasons
- Boards reset seasonally with archival; past seasons remain browsable.
- Season boundary and version are stored per entry for auditability.

Design Note: Leaderboards reinforce strategic optimization and competitive expression through both discovery (world first) and mastery (speedrun). Traits and composition variety are encouraged.

### ***🎒 Solo Quests (Offscreen Missions)***

*Benched characters can be assigned **solo quests** between dungeons.*

* *Runs parallel to main dungeon flow*  
* *One quest per benched character (limit: 2 active)*

#### ***Quest Parameters***

* ***Theme**: e.g. "Gather Herbs", "Train Recruits", "PvP Duel"*  
* ***Duration**: 1 dungeon run*  
* ***Success Chance**: Based on Trait synergy or match*  
* ***Outcome Range**: Failure / Partial / Success*

#### ***Rewards***

* *EXP for benched character*  
* *Crafting materials or gold*  
* *Temporary morale or affinity boosts*  
* *Rare chance of triggering a Mini-Event or Whisper*

*Purpose: Add passive reward flow \+ make bench management meaningful.*

### ***🎭 “Yes, Your GM” — Mini-Events***

*After each dungeon run, one **random event** may trigger based on:*

* *Traits in the active party*  
* *Combat outcomes*  
* *Affinity scores*

#### ***Event Format***

* *Text-based popup with brief setup and 2–3 choice options*  
* *Includes direct character quotes (uses Trait Barks)*

#### ***Examples***

* ***Pie Duel**: Bard and Cleric argue over buffs*  
* ***DPS Greed**: Rogue refuses to pass gear*  
* ***Injury Report**: Monk requests rest for morale*

#### ***Effects***

* *Morale changes*  
* *Affinity gain/loss*  
* *Temporary buffs/debuffs for next dungeon*  
* *Fame gains/losses if decision goes public (Whispers)*

*“Yes, Your GM” events turn downtime into storytelling. Over time, these build your guild's reputation and history.*

---

## ***🧭 13\. UI & Game Flow***

*GOAT presents a modular, clear UI system that reinforces its layered gameplay. Every interaction — from party management to dungeon results — happens within stylized but information-rich scenes.*

### ***🏠 Guild Hall (Main Hub)***

* *Central access point to:*  
  * ***Roster View***  
  * ***Recruitment Board***  
  * ***Party Builder***  
  * ***Crafting Station***  
  * ***Leaderboards***  
  * ***Settings & Saves***  
* *Shows guild level, fame, and visual customizations*

### ***🧙 Roster & Party Management***

* *Grid/list toggle of all characters*  
* *Shows: level, trait, class, morale, gear score, affinity lines*  
* *Drag and drop into party slots*  
* *Search / Sort / Filter by:*  
  * *Trait category*  
  * *Class role*  
  * *Primary/secondary stat focus*  
  * *Morale threshold*  
  * *Affinity presence (highlight pair synergies)*

### ***🗺 Dungeon Selection Screen***

* *World map or dungeon list with:*  
  * *Leaderboard badges (World First filled? Your best Speedrun time)*  
  * *Enemy archetypes preview (e.g., Aggressor/Disruptor mix)*  
  * *Recommended level & trait tips*  
  * *Optional challenges/lockouts (seasonal/weekly)*  
  * *Preview of themed rewards (drop table summary)*

### ***⚔ Combat Interface***

* *Left panel: party (HP, CDs, buffs)*  
* *Right panel: enemies (same info)*  
* *Center: turn queue tracker*  
* *Bottom: skill bar (active, greyed out if on cooldown)*  
* *Status badges with durations for keywords (Stun, Slow, Burn, Poison, Bleed, Silence, Shield, Overdrive, Mark, Camouflage)*  
* *Immunity/resistance indicators when applicable*  
* *Row/slot position indicators (front/back) influencing targeting previews*  
* *Optional: trait tooltips on hover, bark overlays, log recap*

### ***📦 Post-Dungeon Screen***

* *Outcome: win/loss, clear time, total turns*  
* *Party summary (class + trait per character) and per-character outcomes (HP/morale)*  
* *EXP and loot breakdown (base + bonus rewards)*  
* *Leaderboard submission confirmation (world-first and speedrun), with rank delta if applicable*  
* *Revive Token flow (between fights only) and usage tracker*  
* *Morale and Affinity deltas (per character, with reasons)*  
* *Event or Solo Quest results summary*

### ***🏆 Leaderboard UI***

* *Per-dungeon and global*  
* *Columns:*  
  * *Guild name*  
  * *Party composition (class icons \+ trait tags)*  
  * *Time*  
* *Clicking opens gear summary and positioning preview*

### ***💬 Chat System UI***

* *\[Guild\], \[Local\], \[Whispers\] tabs*  
* *Barks appear inline with combat or event logs*  
* *Whispers used for Solo Quest returns or event consequences*

### ***⚙️ Settings***
* *Audio: master/music/SFX sliders; mute toggles*
* *UI: bark frequency (Full / Important Only / None), combat popups for critical barks*
* *Accessibility: colorblind‑safe status badges, font scaling, reduced VFX mode*
* *Gameplay: tutorial replay, confirmation prompts for Revive Tokens*

*Design Goal: Deliver fast, clear player feedback without sacrificing charm or narrative flavor.*

## ***🎓 14\. Tutorial Design – Guildmaster Certification Simulation***

*GOAT opens with a short, flavorful tutorial embedded in the world’s fiction: a **Guildmaster Certification Trial** — a simulated onboarding exam for new MMO guild leaders.*

### ***🧠 Tutorial Philosophy***

* *Teach combat flow, trait interaction, positioning, and morale*  
* *Keep tone humorous and meta-aware*  
* *Lasts under 10 minutes*  
* *Prevent failure, but allow experimentation*

### ***🧪 Narrative Framing***

* *Set inside a holographic arena called the **Guildmaster Certification Room***  
* *Overseen by an AI NPC known as **"The Moderator"** — a sarcastic, all-knowing tutorial narrator*  
* *Tutorial characters are simulations with clearly labeled tags (e.g. "Simulated Warrior")*

### ***🧩 Tutorial Sequence (Scripted)***

1. ***Welcome & Trait Intro***  
   * *The Moderator introduces the concept of IRL Traits*  
   * *Tooltips show Trait effects and bark lines*  
2. ***Turn Order & Basic Attack***  
   * *1v1 with Warrior vs Dummy*  
   * *Learns turn queue and cooldown-less Basic Attacks*  
3. ***Skills & Cooldowns***  
   * *Adds Cleric and Mage to party*  
   * *Uses one skill per class to observe cooldown flow*  
4. ***Positioning & Zones***  
   * *Introduces front/back row logic*  
   * *Mage is targeted unless positioned in backline*  
5. ***Morale & Trait Conflict***  
   * *Trigger a scripted moment where the Cleric misuses a skill and the Moderator comments on morale loss*  
6. ***Final Wave – Mini Encounter***  
   * *Fight 3 Dummies with cooldowns refreshed*  
   * *Trainee must apply trait, targeting, and healing knowledge*

### ***🏆 Tutorial End***

* *Player receives:*  
  * ***\+1 Guild Fame***  
  * ***\+1 Revive Token***  
  * *Unlocks full access to:*  
    * *Guild Hall*  
    * *Party Management*  
    * *First Dungeon: Craghold Excavation*

*Design Note: The tutorial is repeatable from settings and intended to reflect the game’s personality — snarky, strategic, and satirically self-aware.*

---

