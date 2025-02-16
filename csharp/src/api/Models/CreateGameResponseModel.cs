namespace api.Models;

public class CreateGameResponseModel
{
    public string GameID { get; set; }
    public string MaskedWord { get; set; }
    public int AttemptsRemaining { get; set; }
}