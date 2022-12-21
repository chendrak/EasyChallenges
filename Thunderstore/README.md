# EasyChallenges

#### Custom Challenges Made Easy
This mod allows you to easily add custom challenges into the game by specifying them in `JSON` files.

To add your own challenges, create a new folder inside `BepInEx\plugins` and create a new `.challenges.json` file (like `chendraks.challenges.json`) inside that folder.
An example structure could look like this:

```
BepInEx
  - plugins
    - ChendraksChallenges
      - chendraks.challenges.json
```

EasyChallenges will scan for `*.challenges.json` files in `BepInEx\plugins` subfolders.

The full file format is specified below.

## Troubleshooting
If your challenges don't show up, please check `BepInEx\LogOutput.log` and `BepInEx\ErrorLog.log` for potential ideas as to why.
If there is no information in the logs, feel free to swing by the [Rogue: Genesia Discord](https://discord.gg/WbrgtCaP4T) and ask there.

## Changelog

#### 1.0.4
* Fix more documentation issues
* Add the ability to banish cards by stat using `BanishedCardStats`

#### 1.0.3
* Fix some documentation issues

#### 1.0.2
* Updated for the current beta
* The mod now has version checks, so it will only load on supported game versions

#### 1.0.1
* Added `StagesHidden`, `StartingCards` and `BanishedCards` to `ChallengeModifier`
* `StartingCards` and `BanishedCards` have their own descriptions

#### 1.0.0

* Initial Release

## File Format
```json
{
  // ModSource will be displayed at the end of the challenge descriptions
  "ModSource": "Your Mod Name Here",
  "Challenges": [
    {
      // Internal name of your new challenge. This needs to be unique.
      // Consider using a prefix for all your challenges.
      // Required
      "Name": "CC_Challenge1", 
      
      // The difficulty this challenge will be shown under.
      // Required
      "Difficulty": "F",
      
      // The maximum allowed soul shop tier for your run.
      // Optional, Default is MAX
      "MaxAllowedSoulShopTier": "F",
      
      // Where should your challenge show in the list? Higher numbers will
      // result in your challenge being shown further down the list
      // Optional, defaults to 9
      "Order": 1,
      
      // Is this a hard mode challenge?
      // Optional, defaults to false
      "IsHardMode": true,
      
      // All soul coins you gain will be multiplied by this value
      // Optional, defaults to 1
      "SoulCoinModifier": 1.5,
      
      // Translations for your challenges name
      // Required
      "NameLocalization": {
          "en": "My First Challenge"
      },
        
      // Custom descriptions for your challenge
      // Optional
      "Descriptions": [
      {
          "Type": "Positive",
          "Localizations": { 
              "en": "A description for a positive modifier",
              "fr": "A french description"
          }
      },
      {
          "Type": "Neutral",
          "Localizations": {
              "en": "The Precursor for snuffheads favorite"
          }
      },
      {
          "Type": "Negative",
          "Localizations": {
              "en": "A description for a negative modifier"
          }
      }
      ],

      // Defines the modifiers for your challenge
      // Required
      "ChallengeModifier": {
          // The player is given these cards at the start of the run
          // Optional
          "StartingCards": [ "EvilRing" ],

          // These cards will be banished at the start of the run
          // Optional
          "BanishedCards": [ "Cardio" ],

          // Cards with these stats will be banished at the start of the run
          // Optional
          "BanishedCardStats": [ "CriticalChance", "CriticalMultiplier" ],
          
          // Which artifacts should not be available in your challenge?
          // Optional
          "BanishedArtifacts": [ "PhoenixTotem" ],
          
          // Which artifacts should be in your inventory at the start at your challenge?
          // Optional
          "StartingArtifacts": [ "TrainingWeight", "SageLeaf" ],
          
          // What should the maximum rarity of cards in your challenge be?
          // Optional
          "MaxAllowedCardRarity": "Ascended",
          
          // You can only have this many weapons
          // Optional
          "MaxNumberOfWeapons": 3,
          
          // Limits the level of your weapons
          // Optional
          "MaxAllowedWeaponLevel": 8,
          
          // Only have elite stages on the map
          // Optional
          "EliteOnly": true,
          
          // No shops or blacksmiths on the map
          // Optional
          "NoShop": true,
          
          // Removes soul card selections from the stage reward screen
          // Optional
          "NoSoulCardSelectionForStageReward": true,
          
          // Don't offer weapon evolutions when choosing cards
          // Optional
          "NoEvolutions": true,
          
          // Remove artifact selections from your runs (shops and chests on the map)
          // Optional
          "NoArtifacts": true,
          
          // All experience you pick up will be multiplied by this value
          // Optional
          "ExperienceMultiplier": 2,
          
          // Most of the players' stats will be multiplied by this
          // Optional
          "GlobalStatsMultiplier": 1.2,
          
          // Start the run with this corruption
          // Optional
          "StartingCorruption": 1,
          
          // Increase corruption by this value after each stage
          // Optional
          "CorruptionPerStage": 3,
          
          // Increase corruption by this value after killing the boss in each zone
          // Optional
          "CorruptionPerZone": 10,
          
          // When killing an enemy, gain this much health
          // Optional
          "HealthOnKill": 2,
          
          // Multiply elite monster health by this value
          // Optional
          "EliteHealthMultiplier": 0.1,

          // Stages are on the map are replaced by question marks 
          // Optional
          "StagesHidden": false,
          
          // More fine grained control over what stats will be changed at the start of this run
          // Optional
          "Modifiers": [
              {
                  "Stat": "MoveSpeed",
                  "ModifierType": "Additional",
                  "ModifierValue": 30
              }
          ]
      }
    }
  ]
}
```

## Challenge Properties

### Difficulty
The game difficulty this game should be displayed under.

Possible values: `F`, `E`, `D`, `C`, `B`, `A`, `S`

### MaxAllowedSoulShopTier
The game difficulty this game should be displayed under.

Possible values: `None`, `F`, `E`, `D`, `C`, `B`, `A`, `S`, `MAX`

### SoulCoinModifier
A basic multiplier for the soul coins earned.

Possible values: `0.1 or higher`

### Order
Defines where your challenge shows in the list.

Possible values: `1 or higher`

### IsHardMode
Is this a hard mode challenge?

Possible values: `true` or `false`

### NameLocalization
Localization values for your challenge.

### Descriptions
A list of custom additional descriptions for your challenge.

Example:
```json
{
    "Type": "Positive",
    "Localizations": { "en": "Positive :)" }
}
```

#### Type
The type of custom description. The value decides WHERE it shows up in the description.

Possible values: `Positive`, `Neutral`, `Negative`

#### Localizations
The translation text for your challenge.

### ChallengeModifier

#### BanishedArtifacts
A list of artifacts that will not be available during runs with your challenge.

Possible values: `SoulGauntlet`,`MagnifyingGlass`,`GilgameshRing`,`GilgameshCrown`,`GilgameshSeal`,`MidasSword`,`TrainingWeight`,`PersonalTrainer`,`CheatingHand`,`PhoenixTotem`,`PocketHealer`,`MartyrCloak`,`SageLeaf`,`WindLace`,`GlassSword`,`ChronosLace`,`HolyCross`,`OuroborosNecklace`,`MithrilCompass`,`AdamantiteRod`,`SacredSword`,`LightningBoots`,`CursedBelt`,`HermesHood`,`ChampionBracer`,`BhikkhuPearl`,`CowardSaphir`,`BattleRuby`,`ChickenStatue`,`PorkStatue`,`BeefStatue`,`BlackSphereStatue`,`ShopKeeperPlate`,`AdventurerLicence`,`CollectionBook`,`Dice`,`TrollBlood`,`SheepStatue`,`CorruptedCore`,`WanderHelmet`

#### StartingArtifacts
A list of artifacts that the player will start their run with.

Possible values: `SoulGauntlet`,`MagnifyingGlass`,`GilgameshRing`,`GilgameshCrown`,`GilgameshSeal`,`MidasSword`,`TrainingWeight`,`PersonalTrainer`,`CheatingHand`,`PhoenixTotem`,`PocketHealer`,`MartyrCloak`,`SageLeaf`,`WindLace`,`GlassSword`,`ChronosLace`,`HolyCross`,`OuroborosNecklace`,`MithrilCompass`,`AdamantiteRod`,`SacredSword`,`LightningBoots`,`CursedBelt`,`HermesHood`,`ChampionBracer`,`BhikkhuPearl`,`CowardSaphir`,`BattleRuby`,`ChickenStatue`,`PorkStatue`,`BeefStatue`,`BlackSphereStatue`,`ShopKeeperPlate`,`AdventurerLicence`,`CollectionBook`,`Dice`,`TrollBlood`,`SheepStatue`,`CorruptedCore`,`WanderHelmet`

##### BanishedCardStats
Cards with these stats will be banished at the start of the run.

Possible values: `MaxHealth`, `HealthRegen`, `Defence`, `DamageMitigation`, `XPMultiplier`, `PickUpDistance`, `AdditionalProjectile`, `ProjectilePiercing`, `ProjectileLifeTime`, `ProjectileSpeed`, `ProjectileSize`, `AreaSize`, `KnockBack`, `MoveSpeed`, `AttackCoolDown`, `AttackDelay`, `Damage`, `CriticalChance`, `CriticalMultiplier`, `DashSpeed`, `DashDuration`, `DashDelay`, `DashCoolDown`, `DashCharge`, `DashChargePerCoolDown`, `GoldMultiplier`, `SoulCoinMultiplier`, `DefencePiercing`, `Corruption`, `AnachronisticDurationMultiplier`, `CardDropChance_Tainted`, `CardDropChance_Normal`, `CardDropChance_Uncommon`, `CardDropChance_Rare`, `CardDropChance_Epic`, `CardDropChance_Heroic`, `CardDropChance_Ascended`, `CardDropChance_Synergy`, `CardDropChance_Evolution`, `CardDropChance_Moon`, `CardDropChance_Sun`, `CardDropChance_Fire`, `CardDropChance_Wind`, `CardDropChance_Hunt`, `CardDropChance_Wild`, `CardDropChance_Void`, `CardDropChance_Dark`, `CardDropChance_Metal`


#### MaxAllowedCardRarity
The player can not get cards higher than this rarity.

Possible values: `Tainted`, `Normal`, `Uncommon`, `Rare`, `Epic`, `Heroic`, `Ascended`, `Synergy`, `Evolution`

#### NoShop
Disable shops and blacksmiths on the map

Possible values: `true` or `false`

#### NoSoulCardSelectionForStageReward
Disable card selections on the end stage reward screen.

Possible values: `true` or `false`

#### NoEvolutions
Disable weapon evolutions

Possible values: `true` or `false`

#### NoArtifacts
Disable artifacts in shops and chests on the map

#### EliteOnly
All events on the map turn into elite stages

Possible values: `true` or `false`

#### MaxNumberOfWeapons
Only allow the player to have this many different weapons

Possible values: `1 or higher`

#### MaxAllowedWeaponLevel
Weapons can not be leveled up higher than this.

Possible values: `1 or higher`

#### ExperienceMultiplier
All experience will get multiplied by this value

Possible values: `0.1 or higher`

#### GlobalStatsMultiplier
Multiply most stats by this value

Possible values: `0.1 or higher`

#### StartingCorruption
Start the run with this amount of corruption

#### CorruptionPerStage
Increase corruption by this amount after every stage

#### CorruptionPerZone
Increase corruption by this amount after killing the boss in each zone

#### HealthOnKill
Gain this much health after killing a monster.

#### EliteHealthMultiplier
All elite monsters' health is multiplied by this value.

#### StagesHidden
Stages on the map are replaced by question marks.

#### Modifiers
Specifies modifiers that the challenge applies to the players' stats.

Example:
```json
{
  "ModifierValue": 15,
  
  // Specifies HOW the `ModifierValue` is applied
  "ModifierType": "Additional",
  
  // Specifies WHICH stat this modifier applies to
  "Stat": "CriticalChance"
}
```

##### Stat
A stat that your challenge modifies.

Possible values: `MaxHealth`, `HealthRegen`, `Defence`, `DamageMitigation`, `XPMultiplier`, `PickUpDistance`, `AdditionalProjectile`, `ProjectilePiercing`, `ProjectileLifeTime`, `ProjectileSpeed`, `ProjectileSize`, `AreaSize`, `KnockBack`, `MoveSpeed`, `AttackCoolDown`, `AttackDelay`, `Damage`, `CriticalChance`, `CriticalMultiplier`, `DashSpeed`, `DashDuration`, `DashDelay`, `DashCoolDown`, `DashCharge`, `DashChargePerCoolDown`, `GoldMultiplier`, `SoulCoinMultiplier`, `DefencePiercing`, `Corruption`, `AnachronisticDurationMultiplier`, `CardDropChance_Tainted`, `CardDropChance_Normal`, `CardDropChance_Uncommon`, `CardDropChance_Rare`, `CardDropChance_Epic`, `CardDropChance_Heroic`, `CardDropChance_Ascended`, `CardDropChance_Synergy`, `CardDropChance_Evolution`, `CardDropChance_Moon`, `CardDropChance_Sun`, `CardDropChance_Fire`, `CardDropChance_Wind`, `CardDropChance_Hunt`, `CardDropChance_Wild`, `CardDropChance_Void`, `CardDropChance_Dark`, `CardDropChance_Metal`

##### ModifierType
_How_ does your challenge modify a player stat?

Possible values: `Additional`, `Multiplier`, `Compound`

**NOTE:** For most multiplication cases, you will want to use `Compound`, especially for negative multipliers.
