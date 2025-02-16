using Xunit;
using Moq;
using api.Controllers;
using api.Models;
using api.ViewModels;
using api.Utils;
using Microsoft.AspNetCore.Mvc;
using System;

public class GamesControllerTests
{
    private readonly Mock<IIdentifierGenerator> _mockIdentifierGenerator;
    private readonly GamesController _controller;

    public GamesControllerTests()
    {
        _mockIdentifierGenerator = new Mock<IIdentifierGenerator>();
        _controller = new GamesController(_mockIdentifierGenerator.Object);
    }

    [Fact]
    public void CreateGame_ReturnsNewGame()
    {
        _mockIdentifierGenerator.Setup(x => x.RetrieveIdentifier()).Returns(Guid.NewGuid());

        var result = _controller.CreateGame();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<CreateGameResponseModel>(okResult.Value);
        Assert.NotNull(response.GameID);
        Assert.NotNull(response.MaskedWord);
        Assert.Equal(5, response.AttemptsRemaining);
    }

    [Fact]
    public void GetGame_ReturnsGame()
    {
        var gameId = Guid.NewGuid();
        var game = new GameViewModel { UnmaskedWord = "banana", Word = "______", RemainingGuesses = 5 };
        GamesController.Games.Add(gameId, game);

        var result = _controller.GetGame(gameId);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<GameViewModel>(okResult.Value);
        Assert.Equal(game, response);
    }

    [Fact]
    public void GetGame_ReturnsNotFound()
    {
        var result = _controller.GetGame(Guid.NewGuid());

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public void MakeGuess_CorrectGuess_UpdatesGame()
    {
        var gameId = Guid.NewGuid();
        var game = new GameViewModel { UnmaskedWord = "banana", Word = "______", RemainingGuesses = 5, Guesses = new List<string>() };
        GamesController.Games.Add(gameId, game);

        var request = new MakeGuessRequestModel { Letter = "a" };
        var result = _controller.MakeGuess(gameId, request);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<GameViewModel>(okResult.Value);
        Assert.Contains("a", response.Guesses);
        Assert.Equal("_a_a_a", response.Word);
    }

    [Fact]
    public void MakeGuess_IncorrectGuess_UpdatesGame()
    {
        var gameId = Guid.NewGuid();
        var game = new GameViewModel { UnmaskedWord = "banana", Word = "______", RemainingGuesses = 5, IncorrectGuesses = new List<string>() };
        GamesController.Games.Add(gameId, game);

        var request = new MakeGuessRequestModel { Letter = "z" };
        var result = _controller.MakeGuess(gameId, request);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<GameViewModel>(okResult.Value);
        Assert.Contains("z", response.IncorrectGuesses);
        Assert.Equal(4, response.RemainingGuesses);
    }

    [Fact]
    public void MakeGuess_NoMoreGuesses_ReturnsBadRequest()
    {
        var gameId = Guid.NewGuid();
        var game = new GameViewModel { UnmaskedWord = "banana", Word = "______", RemainingGuesses = 0 };
        GamesController.Games.Add(gameId, game);

        var request = new MakeGuessRequestModel { Letter = "a" };
        var result = _controller.MakeGuess(gameId, request);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        var response = Assert.IsType<ResponseErrorViewModel>(badRequestResult.Value);
        Assert.Equal("No more guesses allowed", response.Message);
    }

    [Fact]
    public void ClearGame_RemovesGame()
    {
        var gameId = Guid.NewGuid();
        var game = new GameViewModel { UnmaskedWord = "banana", Word = "______", RemainingGuesses = 5 };
        GamesController.Games.Add(gameId, game);

        var result = _controller.ClearGame(gameId);

        Assert.IsType<NoContentResult>(result);
        Assert.False(GamesController.Games.ContainsKey(gameId));
    }

    [Fact]
    public void ClearGame_GameNotFound_ReturnsNotFound()
    {
        var result = _controller.ClearGame(Guid.NewGuid());

        Assert.IsType<NotFoundResult>(result);
    }
}