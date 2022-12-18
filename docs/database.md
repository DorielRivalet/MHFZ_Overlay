# Database structure and Entity-Relationship Model

- Players
  - Primary key: PlayerID (auto-increment)
  - Columns: PlayerName, Gender, Headgear, Chestplate, Armgear, Waistgear, Leggear, Cuffs, Weapon, WeaponAttack, TotalDefense, ZenithSkills, AutomaticSkills, ActiveSkills, CaravanSkills, DivaSkill, GuildFood, StyleRank, Items, Ammo, PoogieItem, RoadSkills
  - Relationships:
    - One-to-many relationship with PlayerGear (one player can have multiple sets of gear)
    - One-to-many relationship with Skills (one player can have multiple skills)

- PlayerGear
  - Primary key: RunID (auto-increment)
  - Foreign key: PlayerID (references Players.PlayerID)
  - Columns: HelmetID, ChestplateID, WeaponID
  - Relationships:
    - One-to-one relationship with Gear (each set of gear corresponds to a single row in the Gear table)
    - Many-to-one relationship with Players (multiple sets of gear can belong to the same player)
    - Many-to-one relationship with Quests (multiple sets of gear can be used in the same quest)

- Gear
  - Primary key: GearID (auto-increment)
  - Columns: GearName, GearType
  - Relationships:
    - One-to-one relationship with PlayerGear (each row in the PlayerGear table corresponds to a single row in the Gear table)

- PlayerInventory
  - Primary key: RunID (auto-increment)
  - Foreign key: PlayerID (references Players.PlayerID)
  - Columns: ItemID, ItemQuantity
  - Relationships:
    - Many-to-one relationship with Players (a player can have multiple items in their inventory)
    - Many-to-one relationship with Quests (items can be used in multiple quests)

- Skills
  - Primary key: SkillID (auto-increment)
  - Foreign key: PlayerID (references Players.PlayerID)
  - Columns: SkillType (ZenithSkills, AutomaticSkills, ActiveSkills, CaravanSkills, DivaSkill, GuildFood, RoadSkills), - SkillName
  - Relationships:
    - Many-to-one relationship with Players (a player can have multiple skills)

- Quests
  - Primary key: RunID (auto-increment)
  - Columns: QuestID, QuestName, EndTime, ObjectiveType, ObjectiveQuantity, StarGrade, RankName, ObjectiveName, Date
  - Relationships:
    - Many-to-one relationship with PlayerGear (multiple sets of gear can be used in the same quest)
    - Many-to-one relationship with PlayerInventory (multiple items can be used in the same quest)

Players
PlayerID (PK, auto-increment)
PlayerName
Gender
Weapon

PlayerGear
RunID (PK, FK, references Quests.RunID)
HeadgearID (FK, references Gear.GearID)
ChestplateID (FK, references Gear.GearID)
ArmgearID (FK, references Gear.GearID)
WaistgearID (FK, references Gear.GearID)
LeggearID (FK, references Gear.GearID)

Gear
GearID (PK, auto-increment)
GearName
GearType

PlayerInventory
RunID (PK, FK, references Quests.RunID)
ItemID
ItemQuantity

Skills
SkillID (PK, auto-increment)
PlayerID (FK, references Players.PlayerID)
SkillName

Quests
RunID (PK, auto-increment)
QuestID
QuestName
EndTime
ObjectiveType
ObjectiveQuantity
StarGrade
RankName
ObjectiveName
Date

In this diagram:
PK denotes a primary key
FK denotes a foreign key
The lines connecting the tables denote relationships between the tables. The arrowhead points towards the table with the foreign key.

```text
                                      +------------+
                                      |  Players   |
                                      +------------+
                                      |PlayerID(PK)|
                                      |PlayerName  |
                                      |Gender      |
                                      |Weapon      |
                                      +------------+
                                               |
                                      +------------+
                                      | PlayerGear |
                                      +------------+
                                      |RunID(PK,FK)|
                                      |HeadgearID  |
                                      |ChestplateID|
                                      |ArmgearID   |
                                      |WaistgearID |
                                      |LeggearID   |
                                      +------------+
                                      |         |
                                      |         |
                                      |         |
                                      |         v
                                      |  +------------+
                                      |  |    Gear    |
                                      |  +------------+
                                      |  |GearID(PK)  |
                                      |  |GearName    |
                                      |  |GearType    |
                                      |  +------------+
                                      |
                                      |  +-----------------+
                                      |  | PlayerInventory |
                                      |  +-----------------+
                                      |  |RunID(PK,FK)     |
                                      |  |ItemID          |
                                      |  |ItemQuantity    |
                                      |  +-----------------+
                                      |
                                      |  +------------+
                                      |  |   Skills   |
                                      |  +------------+
                                      |  |SkillID(PK) |
                                      |  |PlayerID    |
                                      |  |SkillName   |
                                      |  +------------+
                                      v
                              +----------------+
                              |     Quests     |
                              +----------------+
                              |RunID(PK,autoinc)|
                              |QuestID          |
                              |QuestName        |
                              |EndTime          |
                              |ObjectiveType    |
                              |ObjectiveQuantity|
                              |StarGrade        |
                              |RankName         |
                              |ObjectiveName    |
                              |Date             |
                              +----------------+

```

```text
                            __________
                           |          |
                           |  Quests  |
                           |__________|
                            /         \
                           /           \
  __________              /             \   __________
 |          |  1      *  |               |  |          |
 |  Players |------------|  PlayerGear   |  |   Gear   |
 |__________|            |_______________|  |__________|
                            /         \
                           /           \
                          /             \
 __________              /               \   __________
|          |  1      *  |                 |  |          |
| Skills   |------------|   PlayerInventory |  |          |
|__________|            |___________________|  |          |

In this model:

Quests has a one-to-many relationship with PlayerGear, with Quests being the "one" side and PlayerGear being the "many" side.
Players has a one-to-many relationship with PlayerGear, with Players being the "one" side and PlayerGear being the "many" side.
PlayerGear has a one-to-one relationship with Gear.
Players has a one-to-many relationship with Skills, with Players being the "one" side and Skills being the "many" side.
PlayerInventory has a one-to-many relationship with PlayerGear, with PlayerInventory being the "one" side and PlayerGear being the "many" side.

In this model, the primary keys are denoted by the bolded attributes (PlayerID, QuestID, etc.), and the foreign keys are denoted by the attributes with a "fk_" prefix (fk_PlayerID, fk_QuestID, etc.). The asterisks indicate the multiplicity of the relationships, with "1" indicating a one-to-many relationship and "*" indicating a many-to-many relationship.
```

This database is currently in 3rd Normal Form (3NF). 3NF is a normal form that is achieved when a database is in 2NF and all of its attributes are non-transitively dependent on the primary key. This means that all attributes in each table depend directly on the primary key and not on other attributes in the same table.

To achieve 3NF, a database must first be in 1st Normal Form (1NF), which requires that all values in a column are atomic (i.e., indivisible). The database must also be in 2NF, which requires that it is in 1NF and that all non-key attributes are fully dependent on the primary key.

In this database, the primary keys are PlayerID in the Players table, RunID in the Quests and PlayerInventory tables, SkillID in the Skills table, and GearID in the Gear table. All of the attributes in each table depend directly on the primary key and not on any other attributes in the same table, so the database is in 3NF.

It is generally considered good practice to design a database to be in 3NF, as it can help to reduce redundancy and ensure the integrity of the data. However, depending on the specific requirements of your application, it may be necessary to further normalize the database to a higher normal form, such as Boyce-Codd Normal Form (BCNF) or Fourth Normal Form (4NF).
