using System.Text.RegularExpressions;
using api.Models;
using api.Utils;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]
public partial class GamesController(IIdentifierGenerator identifierGenerator) : ControllerBase
{
    private readonly string[] _words = ["Banana", "Canine", "Unosquare", "Airport"];

    private static readonly Dictionary<Guid, Game> Games = new ();
    
    [GeneratedRegex(@"[a-zA-Z0-9_]")]
    private static partial Regex GuessRegex();

    [HttpPost]
    public ActionResult<Guid> CreateGame()
    {
        var newGameWord = RetrieveWord();
        var newGameId = identifierGenerator.RetrieveIdentifier();
        
        Games.Add(newGameId, new Game
        {
            RemainingGuesses = 3,
            UnmaskedWord = newGameWord,
            Word = GuessRegex().Replace(newGameWord, "_"),
            Status = "In Progress",
            IncorrectGuesses = []
        });

        return Ok(newGameId);
    }

    [HttpGet("{gameId:guid}")]
    public ActionResult<Game> GetGame([FromRoute] Guid gameId)
    {
        var game = RetrieveGame(gameId);
        
        return Ok(game);
    }
    
    [HttpPut("{gameId:guid}")]
    public ActionResult<Game> MakeGuess([FromRoute] Guid gameId, [FromBody] Guess guess)
    {
        var game =  RetrieveGame(gameId);
        if (game == null) return NotFound();

        if (string.IsNullOrWhiteSpace(guess.Letter) || guess.Letter.Length != 1)
        {
            return BadRequest(new ResponseError
            {
                Message = "Guess must be supplied with 1 letter"
            });
        }
        
        return Ok(game);
    }

    private static Game? RetrieveGame(Guid gameId) => Games.GetValueOrDefault(gameId);
    
    private string RetrieveWord() {
        var rand = new Random();
        return _words[rand.Next(1, _words.Length - 1)];
    }
}
