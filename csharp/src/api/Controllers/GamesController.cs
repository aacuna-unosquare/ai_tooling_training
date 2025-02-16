using System.Text.RegularExpressions;
using api.Models;
using api.ViewModels;
using api.Utils;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]
public partial class GamesController(IIdentifierGenerator identifierGenerator) : ControllerBase
{
    public static readonly Dictionary<Guid, GameViewModel> Games = new();
    private readonly string[] _words = ["banana", "canine", "unosquare", "airport"];

    [GeneratedRegex(@"[a-zA-Z0-9_]")]
    private static partial Regex GuessRegex();

    /// <summary>
    /// Creates a new game and returns the game ID, masked word, and remaining attempts.
    /// </summary>
    /// <returns>ActionResult containing the game ID, masked word, and remaining attempts.</returns>
    [HttpPost]
    public ActionResult<Guid> CreateGame()
    {
        var newGameWord = RetrieveWord();
        var newGameId = identifierGenerator.RetrieveIdentifier();

        const int maxGames = 5;

        var vm = new GameViewModel
        {
            RemainingGuesses = maxGames,
            UnmaskedWord = newGameWord,
            Word = GuessRegex().Replace(newGameWord, "_"),
            Status = "In Progress",
            IncorrectGuesses = []
        };

        Games.Add(newGameId, vm);

        return Ok(new CreateGameResponseModel
        {
            GameID = newGameId.ToString(),
            MaskedWord = vm.Word,
            AttemptsRemaining = maxGames
        });
    }

    /// <summary>
    /// Retrieves the game state for a given game ID.
    /// </summary>
    /// <param name="gameId">The ID of the game to retrieve.</param>
    /// <returns>ActionResult containing the game state.</returns>
    [HttpGet("{gameId:guid}")]
    public ActionResult<GameViewModel> GetGame([FromRoute] Guid gameId)
    {
        var game = RetrieveGame(gameId);

        if (game == null)
        {
            return NotFound();
        }

        return Ok(game);
    }

    /// <summary>
    /// Makes a guess for a given game ID and updates the game state.
    /// </summary>
    /// <param name="gameId">The ID of the game to update.</param>
    /// <param name="request">The guess request containing the letter guessed.</param>
    /// <returns>ActionResult containing the updated game state.</returns>
    [HttpPut("{gameId:guid}")]
    public ActionResult<GameViewModel> MakeGuess([FromRoute] Guid gameId, [FromBody] MakeGuessRequestModel request)
    {
        var game = RetrieveGame(gameId);

        if (game == null)
        {
            return NotFound();
        }

        if (game.RemainingGuesses-- < 1)
        {
            return BadRequest(new ResponseErrorViewModel
            {
                Message = "No more guesses allowed"
            });
        }

        if (string.IsNullOrWhiteSpace(request.Letter) || request.Letter?.Length != 1)
        {
            return BadRequest(new ResponseErrorViewModel
            {
                Message = "Letter cannot accept more than 1 character"
            });
        }

        if (game.UnmaskedWord.Contains(request.Letter, StringComparison.InvariantCultureIgnoreCase))
        {
            game.Guesses.Add(request.Letter);
            game.Word = new string(game.UnmaskedWord.Select(c => game.Guesses.Contains(c.ToString(), StringComparer.InvariantCultureIgnoreCase) ? c : '_').ToArray());

            if (game.Word == game.UnmaskedWord)
            {
                game.Status = "Won";
            }
            else if (game.RemainingGuesses > 0)
            {
                game.Status = "In progress";
            }
            else
            {
                game.Status = "Lost";
            }
        }
        else
        {
            game.IncorrectGuesses.Add(request.Letter);
        }

        return Ok(game);
    }

    /// <summary>
    /// Clears the game state for a given game ID.
    /// </summary>
    /// <param name="gameId">The ID of the game to clear.</param>
    /// <returns>ActionResult indicating the result of the operation.</returns>
    [HttpDelete("{gameId:guid}")]
    public ActionResult ClearGame([FromRoute] Guid gameId)
    {
        if (!Games.Remove(gameId))
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Retrieves the game state for a given game ID from the dictionary.
    /// </summary>
    /// <param name="gameId">The ID of the game to retrieve.</param>
    /// <returns>The game state if found, otherwise null.</returns>
    private static GameViewModel? RetrieveGame(Guid gameId)
    {
        return Games.GetValueOrDefault(gameId);
    }

    /// <summary>
    /// Retrieves a random word from the predefined list of words.
    /// </summary>
    /// <returns>A random word from the list.</returns>
    private string RetrieveWord()
    {
        return _words[new Random().Next(1, _words.Length - 1)];
    }
}