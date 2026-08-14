namespace CognoDBGraph.Models;

public class TeammateRecommendation
{
    public string Name { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string SharedSkill { get; set; } = string.Empty;
}

public class DeveloperExplorerViewModel
{
    public string SelectedDevId { get; set; } = "dev1";
    public List<string> Skills { get; set; } = new();
    public List<TeammateRecommendation> Teammates { get; set; } = new();
    public string Message { get; set; } = string.Empty;
} 