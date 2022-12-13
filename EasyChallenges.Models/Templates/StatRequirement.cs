namespace EasyChallenges.Models.Templates;

public class StatRequirement
{
    public string Name { get; set; }
    public float Value { get; set; }

    public override string ToString()
    {
        return $"{nameof(this.Name)}: {this.Name}, {nameof(this.Value)}: {this.Value}";
    }
}
