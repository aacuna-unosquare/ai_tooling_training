using System.Text.Json.Serialization;

namespace api.ViewModels;

public class GameViewModel
{
    [JsonPropertyName("AttemptsRemaining")]
    public int RemainingGuesses { get; set; }
    
    /// <summary>
    /// Masked word with underscores for each letter
    /// </summary>
    [JsonPropertyName("MaskedWord")]
    public string? Word { get; set; }

    /// <summary>
    /// Real word without masking
    /// </summary>
    [JsonIgnore] public string? UnmaskedWord { get; set; }
    
    public List<string> IncorrectGuesses { get; set; } = [];
    
    public List<string> Guesses { get; set; } = [];
    public string? Status { get; set; }
}