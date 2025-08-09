# 🎯 Guilds of Arcana Terra - Issue Roadmap

## 📋 Current roadmap synced with GitHub issues

This roadmap reflects the latest open/closed issues and organizes them into a prioritized development plan aligned with GDD v1.1.

---

## 🏗️ **CORE SYSTEMS (High Priority)**

### **Combat System**
- **#21** - Status Effects: Core + GDD Keywords **[combat, traits] - Phase 1**
- **#22** - Combat: Class Definition Skill Loading (Enable SO-driven Skills) **[combat, data] - Phase 1**
- **#45** - Combat: Skill Effect System (Epic) **[combat] - Phase 1**
- **#42** - Combat: Row-Based Positioning **[combat] - Phase 1**

### **Character & Class System**
- **#37** - Implement All 12 Character Classes (complete class implementations) **[data, enhancement] - Phase 3**
- **#30** - Implement Experience and Leveling System (XP and progression) **[data, enhancement]**

### **Guild & Management Systems**
- **#39** - Meta: Guild System (Expanded) **[meta] - Phase 4**
  - Subtasks: **#80** LFG Board, **#81** Scout Other Guilds, **#82** Incoming Applications, **#83** Roster Limits, **#84** Retirement, **#85** Recruit Generation & Quality, **#86** Fame & Level Integration
- **#44** - UI: Guild Hall Main Hub **[ui] - Phase 2**
- **#28** - Implement Save/Load System (persistent storage) **[meta] - Phase 4**

### **Tutorial & Onboarding**
- **#43** - Implement Complete Tutorial System (Guildmaster Certification) **[tutorial, enhancement] - Phase 2**
- **#7** - Develop Tutorial: Guildmaster Certification (existing tutorial work)

---

## 🎮 GAMEPLAY SYSTEMS (Medium Priority)

### **Combat & Tactics**
- **#23** - Implement Enemy Combat System (replace placeholder enemies) **[combat, enhancement]**
- **#24** - Complete Dungeon Scene Implementation (scene is empty) **[dungeon, enhancement]**
- **#13** - Create Prototype Combat Scene (TestArena.unity)
- **#14** - Implement Row-Based Positioning Logic
- **#15** - Create Skill Execution System
- **#16** - Implement Basic Enemy AI Controller
- **#17** - Add Victory & Defeat State to Combat

### **Character Progression**
- **#34** - Implement Morale System (stress-lite system) **[traits, enhancement] - Phase 3**
- **#35** - Implement Combat Affinity System (character relationships) **[traits, enhancement] - Phase 3**
- **#36** - Implement Trait Barks System (character dialogue) **[traits, enhancement] - Phase 3**

### **Content & Equipment**
- **#27** - Add Content Pipeline Expansion (more characters, classes, dungeons) **[data, enhancement]**
- **#29** - Add Equipment System (weapons, armor, accessories) **[data, enhancement]**
- **#38** - Implement Complete Gear System (upgrades, salvage, crafting) **[data, enhancement] - Phase 4**

### **Meta Systems**
- **#40** - Leaderboard: System Implementation **[meta] - Phase 4**
  - Subtasks: **#63** Data Schema, **#64** Capture Results, **#65** Per-Dungeon World First (First 100), **#66** Per-Dungeon Speedrun (Best-per-Guild), **#67** Global World First, **#68** Global Speedrun, **#69** Dungeon Screen UI, **#70** Global Leaderboard UI, **#71** Season Reset & Archival
- **#41** - Implement 'Yes, Your GM' Mini-Events **[meta] - Phase 4**
- **#11** - Meta: Solo Quest System **[meta] - Phase 4**

---

## 🎨 **UI & USER EXPERIENCE (Medium Priority)**

### **Party Management**
- **#6** - Design Party Builder UI (existing work)
- **#25** - Polish Party Selection UI (visual improvements) **[ui, enhancement]**

### **Communication & Social**
- **#9** - Create Guild Chat System (Tabs + Flavor Messages)
- **#10** - Add Whisper Mini-Events (character interactions)

### **Dungeon & Combat UI**
- **#8** - Implement Craghold Dungeon Encounter Flow
- **#32** - Improve Error Handling and Debugging **[enhancement] - Phase 5**

### **Results & Analytics**
- **#72** - Results: Post-Dungeon Screen (Epic) **[ui, meta] - Phase 4**
  - Subtasks: **#73** UI Layout & Sections, **#74** Stats & Party Summary, **#75** Loot & Drop Integration, **#76** Leaderboard Submission Confirmation, **#77** Revive Token Flow UI, **#78** Morale & Affinity Deltas, **#79** Mini-Event & Solo Quest Summary
- **#12** - Results: Analytics Summary **[ui, meta] - Phase 4**

### **Settings & Accessibility**
- **#87** - Settings: Centralized Menu (Audio/UI/Accessibility) **[ui] - Phase 5**
- **#59** - UI: Bark Frequency & Delivery **[ui, traits] - Phase 5**

---

## 🏷️ Label Breakdown (current)

- Combat: #45, #42, #23, #22, #21, #17, #14, #13
- UI: #6, #9, #10
- Dungeon: #8
- Meta: #12, #11
- Tutorial: #7, #18, #19, #20

---

## 🧪 Legacy / Superseded

- #26 - Implement Skill Effect System — Closed; replaced by #45
- #16 - Implement Basic Enemy AI Controller — Closed as duplicate of #23
- #15 - Combat: Skill Execution System — Closed; superseded by #45
- #14 - Row-Based Positioning (Legacy) — Keep for context; do active work in #42
- #13 - Prototype TestArena (Legacy) — Keep for historical/testing context

---

## 📊 **DEVELOPMENT PHASES**

### **🔥 Phase 1: Core Combat (4 issues)**
- **#21** - Missing Status Effects **[combat, enhancement]**
- **#22** - Class Definition Skill Loading **[combat, enhancement]**
- **#26** - Skill Effect System **[combat, enhancement]**
- **#42** - Row-Based Combat Positioning **[combat, enhancement]**

### **⚡ Phase 2: Tutorial & Navigation (2 issues)**
- **#43** - Complete Tutorial System **[tutorial, enhancement]**
- **#44** - Guild Hall Main Hub **[ui, enhancement]**

### **🎯 Phase 3: Character Systems (4 issues)**
- **#34** - Morale System **[traits, enhancement]**
- **#35** - Combat Affinity System **[traits, enhancement]**
- **#36** - Trait Barks System **[traits, enhancement]**
- **#37** - All 12 Character Classes **[data, enhancement]**

### **✨ Phase 4: Progression & Content**
- **#39** - Meta: Guild System (Expanded)
- **#40** - Leaderboard: System Implementation
- **#41** - Implement 'Yes, Your GM' Mini-Events
- **#38** - Meta: Gear System (Upgrades/Salvage/Crafting)
- **#61** - Dungeon: Rewards & Drop Tables
- **#62** - Dungeon: Sample Boss - Foreman Vex (Craghold)
- **#72** - Results: Post-Dungeon Screen (Epic)

### **🎨 Phase 5: Polish & Quality**
- **#31** - UI: Audio and Visual Effects
- **#32** - Stability & Error Handling (Centralized Logger, Validation)
- **#33** - Unit Tests (Edit/Playmode + Coverage)
- **#87** - Settings: Centralized Menu (Audio/UI/Accessibility)
- **#88** - Meta: Data Validators (ScriptableObjects & Lints)
- **#89** - Tech Debt: Remove Legacy JSON Loaders (Migrate to SO Pipeline)

---

## 🏷️ **LABEL BREAKDOWN**

### **Combat System** (key issues)
- **#21, #22, #42, #45** - Core combat mechanics
- **#23** - Enemy System (AI + Spawning)
- **#53** - UI: Turn Queue & Position Indicators
- **#54** - Combat: Missing GDD Keywords
- **#55** - Combat: Stealth/Camouflage for Shadowstep
- **#17** - Victory & Defeat State

### **UI/UX** (selected)
- **#44** - Guild Hall Main Hub
- **#25** - Party Selection - Polish
- **#31** - Audio/Visual Effects
- **#6, #9, #10** - Party Builder / Chat UI
- **#58** - Affinity Visualization (Party Builder)
- **#57** - Party Builder - Validation Enhancements
- **#59** - Bark Frequency & Delivery

### **Character Systems** (6 issues)
- **#34, #35, #36** - Character progression and personality
- **#37** - Complete class implementations
- **#30** - Experience and leveling
- **#29** - Equipment system

### **Meta Systems**
- **#39, #40, #41** - Guild, Leaderboards, Mini-Events
- **#28** - Save/Load System
- **#11, #12** - Solo Quests and Results Analytics
- **#52** - Meta: Revive Token System
- **#56** - Dungeon: Encounter Generator & Pre-Combat Preview

### **Content & Data**
- **#27, #38** - Content pipeline and gear
- **#37, #30, #29** - Character kits and progression
- **#24, #61, #62** - Dungeon implementation & rewards

### **Tutorial**
- **#43** - Tutorial Epic (consolidates #7, #18, #19, #20)

### **Quality Assurance** (2 issues)
- **#32, #33** - Error handling and testing

---

## 🎮 **GDD v1.1 COMPLIANCE CHECK**

### **✅ Implemented Systems**
- Basic combat framework
- Party management UI
- Character data structure
- ScriptableObject architecture

### **🔄 In Progress Systems**
- Status effects (partial)
- Skill system (partial)
- Tutorial system (partial)
- Enemy AI (placeholder)

### **❌ Missing Systems (From GDD)**
- **Morale System** (Issue #34) **[traits, enhancement] - Phase 3**
- **Combat Affinity System** (Issue #35) **[traits, enhancement] - Phase 3**
- **Trait Barks System** (Issue #36) **[traits, enhancement] - Phase 3**
- **Complete 12 Classes** (Issue #37) **[data, enhancement] - Phase 3**
- **Gear System** (Issue #38) **[data, enhancement] - Phase 4**
- **Guild System** (Issue #39) **[meta, enhancement] - Phase 4**
- **Leaderboard System** (Issue #40) **[meta, enhancement] - Phase 4**
- **Mini-Events** (Issue #41) **[meta, enhancement] - Phase 4**
- **Row-Based Positioning** (Issue #42) **[combat, enhancement] - Phase 1**
- **Complete Tutorial** (Issue #43) **[tutorial, enhancement] - Phase 2**
- **Guild Hall Hub** (Issue #44) **[ui, enhancement] - Phase 2**

---

## 📈 **DEVELOPMENT ROADMAP**

### **Phase 1: Core Combat (Issues #21, #22, #26, #42)**
- Complete the combat system foundation
- Implement all status effects
- Fix skill loading from class definitions
- Add row-based positioning

### **Phase 2: Tutorial & Navigation (Issues #43, #44)**
- Complete tutorial system
- Implement Guild Hall main hub
- Establish proper game flow

### **Phase 3: Character Systems (Issues #34, #35, #36, #37)**
- Implement morale system
- Add affinity relationships
- Create trait barks
- Complete all 12 character classes

### **Phase 4: Progression & Content (Issues #39, #40, #41, #38)**
- Implement guild system
- Add leaderboards
- Create mini-events
- Complete gear system

### **Phase 5: Polish & Quality (Issues #31, #32, #33)**
- Add audio and visual effects
- Improve error handling
- Implement unit tests

---

## 🧭 Prioritization (Numeric Order)

Numbers indicate implementation order considering complexity (1–10), dependencies, and impact.

1. #45 Combat: Skill Effect System (Epic) — complexity 9
2. #23 Combat: Enemy System (AI + Spawning) — complexity 9
3. #21 Status Effects: Core + GDD Keywords — complexity 8
4. #42 Combat: Row-Based Positioning — complexity 8
5. #22 Combat: Class Definition Skill Loading (Enable SO-driven Skills) — complexity 7
6. #37 Combat: Class Kits - Implement 12 Classes — complexity 8
7. #24 Dungeon: Encounter Flow & Integration — complexity 6
8. #43 Tutorial: Complete Tutorial System — complexity 8
9. #44 UI: Guild Hall Main Hub — complexity 6
10. #38 Meta: Gear System (Upgrades/Salvage/Crafting) — complexity 8
11. #39 Meta: Guild System (Expanded) — complexity 8
12. #34 Meta: Morale System — complexity 7
13. #29 Meta: Equipment System — complexity 7
14. #28 Meta: Save/Load System — complexity 7
15. #35 Meta: Combat Affinity System — complexity 6
16. #36 Meta: Trait Barks System — complexity 6
17. #40 Leaderboard: System Implementation — complexity 6
18. #41 Implement 'Yes, Your GM' Mini-Events — complexity 6
19. #30 Meta: Experience & Leveling System — complexity 6
20. #27 Meta: Content Pipeline Expansion — complexity 5
21. #72 Results: Post-Dungeon Screen (Epic) — complexity 6
22. #53 UI: Turn Queue & Position Indicators — complexity 5
23. #52 Meta: Revive Token System — complexity 5
24. #54 Combat: Missing GDD Keywords (Silence, Mark, Camouflage, Overdrive) — complexity 6
25. #55 Combat: Stealth/Camouflage for Shadowstep — complexity 5
26. #56 Dungeon: Encounter Generator & Pre-Combat Preview — complexity 6
27. #61 Dungeon: Rewards & Drop Tables — complexity 6
28. #62 Dungeon: Sample Boss - Foreman Vex (Craghold) — complexity 6
29. #58 UI: Affinity Visualization (Party Builder) — complexity 5
30. #57 UI: Party Builder - Validation Enhancements — complexity 5
31. #59 UI: Bark Frequency & Delivery — complexity 5
32. #31 UI: Audio and Visual Effects — complexity 5
33. #32 Stability & Error Handling (Centralized Logger, Validation) — complexity 5
34. #33 Unit Tests (Edit/Playmode + Coverage) — complexity 5
35. #25 UI: Party Selection - Polish — complexity 3

Notes:
- Subtasks under #39, #40, and #72 follow their parent’s slot and should be scheduled within that window.
- Dependencies: #22 precedes #37; #21/#22/#42 precede #23; #34/#35/#36 feed into #72; #40 consumes data from #64 (Capture Results).

---

## 🎯 **NEXT STEPS**

1. **Focus on Critical Path issues first** (#21, #22, #42) - Phase 1
2. **Complete tutorial system** (#43) - Phase 2
3. **Implement Guild Hall** (#44) - Phase 2
4. **Build out character systems** (#34, #35, #36, #37) - Phase 3
5. **Add progression systems** (#39, #40, #41, #38) - Phase 4

This roadmap ensures development follows the GDD v1.1 structure while maintaining logical progression from core systems to enhanced features.

---

## 📋 **MILESTONE SUMMARY**

- **Phase 1: Core Combat** - 4 issues (combat foundation)
- **Phase 2: Tutorial & Navigation** - 2 issues (onboarding)
- **Phase 3: Character Systems** - 4 issues (character depth)
- **Phase 4: Progression & Content** - 4 issues (game systems)
- **Phase 5: Polish & Quality** - 3 issues (final polish)

**Total Milestone Issues: 17** (out of 44 total issues) 