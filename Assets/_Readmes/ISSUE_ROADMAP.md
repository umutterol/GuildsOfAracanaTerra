# 🎯 Guilds of Arcana Terra - Issue Roadmap

## 📋 **Issue Structure Based on GDD v1.1**

This document organizes all GitHub issues according to the Game Design Document v1.1 structure and priorities.

---

## 🏗️ **CORE SYSTEMS (High Priority)**

### **Combat System**
- **#21** - Implement Missing Status Effects (Bleed, Poison, Stun, Slow, Shield) **[combat, enhancement] - Phase 1**
- **#22** - Fix Class Definition Skill Loading (currently using default skills) **[combat, enhancement] - Phase 1**
- **#26** - Implement Skill Effect System (placeholder effects need real implementation) **[combat, enhancement] - Phase 1**
- **#42** - Implement Row-Based Combat Positioning (front/back row system) **[combat, enhancement] - Phase 1**

### **Character & Class System**
- **#37** - Implement All 12 Character Classes (complete class implementations) **[data, enhancement] - Phase 3**
- **#30** - Implement Experience and Leveling System (XP and progression) **[data, enhancement]**

### **Guild & Management Systems**
- **#39** - Implement Guild System (main progression layer) **[meta, enhancement] - Phase 4**
- **#44** - Implement Guild Hall Main Hub (central navigation) **[ui, enhancement] - Phase 2**
- **#28** - Implement Save/Load System (persistent storage) **[meta, enhancement]**

### **Tutorial & Onboarding**
- **#43** - Implement Complete Tutorial System (Guildmaster Certification) **[tutorial, enhancement] - Phase 2**
- **#7** - Develop Tutorial: Guildmaster Certification (existing tutorial work)

---

## 🎮 **GAMEPLAY SYSTEMS (Medium Priority)**

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
- **#40** - Implement Leaderboard System (competitive play) **[meta, enhancement] - Phase 4**
- **#41** - Implement 'Yes, Your GM' Mini-Events (post-dungeon events) **[meta, enhancement] - Phase 4**
- **#11** - Implement Solo Quest System (offscreen missions)
- **#12** - Add Post-Dungeon Analytics Summary

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

---

## 🧪 **QUALITY ASSURANCE (Medium Priority)**

### **Testing & Debugging**
- **#33** - Add Unit Tests (comprehensive testing) **[enhancement] - Phase 5**
- **#31** - Add Audio and Visual Effects (polish) **[ui, enhancement] - Phase 5**

---

## 🎭 **TUTORIAL & ONBOARDING (High Priority)**

### **Tutorial System**
- **#18** - Script onboarding narration (The Moderator)
- **#19** - Stage skill, turn, positioning test encounters
- **#20** - End tutorial with simulated boss + unlock main game

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

### **✨ Phase 4: Progression & Content (4 issues)**
- **#39** - Guild System **[meta, enhancement]**
- **#40** - Leaderboard System **[meta, enhancement]**
- **#41** - Mini-Events **[meta, enhancement]**
- **#38** - Complete Gear System **[data, enhancement]**

### **🎨 Phase 5: Polish & Quality (3 issues)**
- **#31** - Audio and Visual Effects **[ui, enhancement]**
- **#32** - Error Handling and Debugging **[enhancement]**
- **#33** - Unit Tests **[enhancement]**

---

## 🏷️ **LABEL BREAKDOWN**

### **Combat System** (6 issues)
- **#21, #22, #26, #42** - Core combat mechanics
- **#23** - Enemy AI system
- **#13, #14, #15, #16, #17** - Combat implementation

### **UI/UX** (5 issues)
- **#44** - Main hub navigation
- **#25** - Party selection polish
- **#31** - Audio/visual effects
- **#6, #9, #10** - UI components

### **Character Systems** (6 issues)
- **#34, #35, #36** - Character progression and personality
- **#37** - Complete class implementations
- **#30** - Experience and leveling
- **#29** - Equipment system

### **Meta Systems** (5 issues)
- **#39, #40, #41** - Guild and progression systems
- **#28** - Save/load system
- **#11, #12** - Solo quests and analytics

### **Content & Data** (6 issues)
- **#27, #38** - Content pipeline and gear
- **#37, #30, #29** - Character and equipment data
- **#24** - Dungeon implementation

### **Tutorial** (4 issues)
- **#43, #7** - Complete tutorial system
- **#18, #19, #20** - Tutorial components

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