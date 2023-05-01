namespace EasyChallenges.Models.Templates;

using Extensions;
using Generated;
using RogueGenesia.Data;

public class ChallengeModifierTemplate
{
    public SoulShopUpgradeSO.ETier MaxAllowedSoulShopTier = SoulShopUpgradeSO.ETier.MAX;
    public CardRarity MaxAllowedCardRarity = CardRarity.NONE;
    public List<ModifierTemplate> Modifiers { get; set; } = new();
    public List<ArtifactTemplate> BanishedArtifacts = new();
    public List<ArtifactTemplate> StartingArtifacts = new();
    public List<string> StartingCards = new();
    public List<string> BanishedCards = new();
    public List<TemplateStatsType> BanishedCardStats = new();

    public int MaxNumberOfWeapons = -1;
    public int MaxAllowedWeaponLevel = -1;

    public bool EliteOnly = false;
    public bool NoShop = false;
    public bool NoSoulCardSelectionForStageReward = false;
    public bool NoEvolutions = false;
    public bool NoArtifacts = false;
    public bool StagesHidden = false;

    public float ExperienceMultiplier = 1f;
    public float GlobalStatsMultiplier = 1f;
    public float EliteHealthMultiplier = 1f;
    public float WeaponDropChance = 1f;

    public float StartingCorruption = 0f;
    public float CorruptionPerStage = 0f;
    public float CorruptionPerZone = 0f;
    public float HealthOnKill = 0f;


    public ChallengeModifier ToChallengeModifier()
    {
        var challengeModifier = new ChallengeModifier
        {
            ChallengeMaxAllowedSoulShopTier = this.MaxAllowedSoulShopTier,
            ChallengeMaxAllowedRarity = this.MaxAllowedCardRarity,
            StatsModifier = this.ConvertModifiersToStatsModifier(),
            BannishedArtifacts = this.BanishedArtifacts.ConvertAll(template => template.ToString()),
            BonusArtifacts = this.StartingArtifacts.ConvertAll(template => template.ToString()),
            ChallengeMaxAllowedWeapon = this.MaxNumberOfWeapons,
            ChallengeMaxAllowedWeaponLevel = this.MaxAllowedWeaponLevel,
            EliteOnly = this.EliteOnly,
            NoShop = this.NoShop,
            NoCardPickup = this.NoSoulCardSelectionForStageReward,
            NoEvolution = this.NoEvolutions,
            NoArtifact = this.NoArtifacts,
            ExperienceMultiplier = this.ExperienceMultiplier,
            GlobalStatsMultiplier = this.GlobalStatsMultiplier,
            StartingCorruption = this.StartingCorruption,
            CorruptionGrowth = this.CorruptionPerStage,
            CorruptionGrowthBoss = this.CorruptionPerZone,
            WeaponDropChance = this.WeaponDropChance,
            HealthOnKill = this.HealthOnKill,
            EliteHealthMultiplier = this.EliteHealthMultiplier,
        };

        challengeModifier.SetBoolModifier("HiddenStage", this.StagesHidden);

        return challengeModifier;
    }
}
