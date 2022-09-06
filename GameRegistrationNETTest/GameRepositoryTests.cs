using GameRegistrationNETApp;
using GameRegistrationNETApp.Enums;

namespace GameRegistrationNETTest;

public class GameRepositoryTests
{
    private GameRepository CreateDefaultGameRepository(List<Game> games)
    {
        return new GameRepository(games);
    }

    private Game CreateOtherGame()
    {
        return new Game
        {
            Id = 0,
            Title = "Hitman 47",
            Description = "Hitman 47",
            Year = 2006,
            Genre = Genre.Action
        };
    }

    [
        Theory,
        InlineData("NFSMW", "Need For Speed Most Wanted 2012", 2012, Genre.Racing),
        InlineData("NFSHP", "Need For Speed Hot Porsuit 2010", 2010, Genre.Racing)
    ]
    public void Insert_ListEmptyAndInsertingValidGame_ListWithOneGame(
        string title,
        string description,
        int year,
        Genre genre)
    {
        var stubListGames = new List<Game>();
        var game = new Game
        {
            Title = title,
            Description = description,
            Year = year,
            Genre = genre
        };
        var gameRepository = CreateDefaultGameRepository(stubListGames);

        gameRepository.Insert(game);

        Assert.True(stubListGames.Count.Equals(1), "The Game list must have one item");
        Assert.Equal(game, stubListGames[0]);
    }

    [
        Theory,
        InlineData("NFSMW", "Need For Speed Most Wanted 2012", -1, Genre.Racing),
        InlineData("NFSMW", "", 2012, Genre.Racing),
        InlineData("", "Need For Speed Most Wanted 2012", 2012, Genre.Racing),
    ]
    public void Insert_ListEmptyAndInsertingInvalidGame_ListEmptyAndError(
        string title,
        string description,
        int year,
        Genre genre)
    {
        var stubListGames = new List<Game>();
        var game = new Game
        {
            Title = title,
            Description = description,
            Year = year,
            Genre = genre
        };
        var gameRepository = CreateDefaultGameRepository(stubListGames);

        var action = () => gameRepository.Insert(game);

        var ex = Assert.Throws<ArgumentException>(action);
        Assert.Equal("The Game data is invalid.", ex.Message);
        Assert.Empty(stubListGames);
    }

    [Fact]
    public void Insert_ListEmptyAndInsertingNullGame_ListEmptyAndError()
    {
        var stubListGames = new List<Game>();
        var gameRepository = CreateDefaultGameRepository(stubListGames);

        var action = () => gameRepository.Insert(null);

        var ex = Assert.Throws<ArgumentNullException>(action);
        Assert.Equal("The Game cannot be null.", ex.Message);
        Assert.Empty(stubListGames);
    }

    [
        Theory,
        InlineData("NFSMW", "Need For Speed Most Wanted 2012", 2012, Genre.Racing),
        InlineData("NFSHP", "Need For Speed Hot Porsuit 2010", 2010, Genre.Racing)
    ]
    public void Insert_ListWithOneGameAndInsertingValidGame_ListWithTwoGamesAndIdEqualsIndex(
        string title,
        string description,
        int year,
        Genre genre)
    {
        var game = new Game
        {
            Title = title,
            Description = description,
            Year = year,
            Genre = genre
        };
        var stubListGames = new List<Game> {
            CreateOtherGame()
        };
        var gameRepository = CreateDefaultGameRepository(stubListGames);

        gameRepository.Insert(game);

        Assert.True(stubListGames.Count.Equals(2), "The Game list must have two items");
        for (int i = 0; i < stubListGames.Count; i++)
        {
            Assert.Equal(i, stubListGames[i].Id);
        }
    }

    [Fact]
    public void GetAll_ListEmptyCtorWithArgs_ReturnListEmpty()
    {
        var stubListGames = new List<Game>();
        var gameRepository = CreateDefaultGameRepository(stubListGames);

        var result = gameRepository.GetAll();

        Assert.Empty(result);
        Assert.Equal(stubListGames, result);
    }

    [Fact]
    public void GetAll_ListEmptyCtorNoArgs_ReturnListEmpty()
    {
        var gameRepository = new GameRepository();

        var result = gameRepository.GetAll();

        Assert.Empty(result);
    }

    [Fact]
    public void GetAll_ListNotEmpty_ReturnListNotEmpty()
    {
        var stubListGames = new List<Game> {
            CreateOtherGame()
        };
        var gameRepository = CreateDefaultGameRepository(stubListGames);

        var result = gameRepository.GetAll();

        Assert.NotEmpty(result);
        Assert.Equal(stubListGames, result);
    }

    [Fact]
    public void GetById_ListEmpty_ThrowsArgumentOutOfRangeException()
    {
        var stubListGames = new List<Game>();
        var gameRepository = CreateDefaultGameRepository(stubListGames);

        var action = () => gameRepository.GetById(0);

        var ex = Assert.Throws<ArgumentOutOfRangeException>(action);
        Assert.Equal("Game not found.", ex.Message);
        Assert.Empty(stubListGames);
    }

    [Fact]
    public void GetById_ListNotEmptyAndIdNotExist_ThrowsArgumentOutOfRangeException()
    {
        var stubListGames = new List<Game> {
            CreateOtherGame()
        };
        var gameRepository = CreateDefaultGameRepository(stubListGames);

        var action = () => gameRepository.GetById(1);

        var ex = Assert.Throws<ArgumentOutOfRangeException>(action);
        Assert.Equal("Game not found.", ex.Message);
        Assert.True(stubListGames.Count.Equals(1));
    }

    [Fact]
    public void GetById_ListNotEmptyAndIdExist_ReturnGame()
    {
        var game = CreateOtherGame();
        var stubListGames = new List<Game> { game };
        var gameRepository = CreateDefaultGameRepository(stubListGames);

        var gameResult = gameRepository.GetById(0);

        Assert.True(gameResult.Id.Equals(0));
        Assert.Equal(game, gameResult);
    }

    [Fact]
    public void Update_ListEmpty_ThrowsArgumentOutOfRangeException()
    {
        var gameToUpdate = CreateOtherGame();
        var stubListGames = new List<Game>();
        var gameRepository = CreateDefaultGameRepository(stubListGames);

        var action = () => gameRepository.Update(0, gameToUpdate);

        var ex = Assert.Throws<ArgumentOutOfRangeException>(action);
        Assert.Equal("Game not found.", ex.Message);
        Assert.Empty(stubListGames);
    }

    [Fact]
    public void Update_ListWithOneGameAndGameIdNotExist_ThrowsArgumentOutOfRangeException()
    {
        var gameToUpdate = CreateOtherGame();
        var stubListGames = new List<Game> { CreateOtherGame() };
        var gameRepository = CreateDefaultGameRepository(stubListGames);

        var action = () => gameRepository.Update(1, gameToUpdate);

        var ex = Assert.Throws<ArgumentOutOfRangeException>(action);
        Assert.Equal("Game not found.", ex.Message);
        Assert.True(stubListGames.Count.Equals(1));
    }

    [Fact]
    public void Update_ListWithOneGameAndGameIdExistAndGameEntityNull_ThrowsArgumentNullException()
    {
        var stubListGames = new List<Game> { CreateOtherGame() };
        var gameRepository = CreateDefaultGameRepository(stubListGames);

        var action = () => gameRepository.Update(0, null);

        var ex = Assert.Throws<ArgumentNullException>(action);
        Assert.Equal("The Game cannot be null.", ex.Message);
        Assert.True(stubListGames.Count.Equals(1));
    }

    [
        Theory,
        InlineData("NFSMW", "Need For Speed Most Wanted 2012", -1, Genre.Racing),
        InlineData("NFSMW", "", 2012, Genre.Racing),
        InlineData("", "Need For Speed Most Wanted 2012", 2012, Genre.Racing),
    ]
    public void Update_ListWithOneGameAndGameIdExistAndGameEntityInvalid_ThrowsArgumentException(
        string title,
        string description,
        int year,
        Genre genre)
    {
        var gameToUpdate = new Game
        {
            Title = title,
            Description = description,
            Year = year,
            Genre = genre
        };
        var stubListGames = new List<Game> { CreateOtherGame() };
        var gameRepository = CreateDefaultGameRepository(stubListGames);

        var action = () => gameRepository.Update(0, gameToUpdate);

        var ex = Assert.Throws<ArgumentException>(action);
        Assert.Equal("The Game data is invalid.", ex.Message);
        Assert.True(stubListGames.Count.Equals(1));
    }

    [
        Theory,
        InlineData("NFSMW", "Need For Speed Most Wanted 2012", 2012, Genre.Racing),
        InlineData("NFSHP", "Need For Speed Hot Porsuit 2010", 2010, Genre.Racing)
    ]
    public void Update_ListWithOneGameAndGameIdExistAndGameEntityValid_ListWithUpdatedGame(
        string title,
        string description,
        int year,
        Genre genre)
    {
        const int ID = 0;
        var gameToUpdate = new Game
        {
            Title = title,
            Description = description,
            Year = year,
            Genre = genre
        };
        var stubListGames = new List<Game> { CreateOtherGame() };
        var gameRepository = CreateDefaultGameRepository(stubListGames);

        gameRepository.Update(ID, gameToUpdate);

        Assert.True(stubListGames.Count.Equals(1));
        Assert.Equal(gameToUpdate, stubListGames[ID]);
    }

    [Fact]
    public void Delete_ListEmpty_ThrowsArgumentOutOfRangeException()
    {
        var stubListGames = new List<Game>();
        var gameRepository = CreateDefaultGameRepository(stubListGames);

        var action = () => gameRepository.Delete(0);

        var ex = Assert.Throws<ArgumentOutOfRangeException>(action);
        Assert.Equal("Game not found.", ex.Message);
        Assert.Empty(stubListGames);
    }

    [Fact]
    public void Delete_ListWithOneGameAndIdNotExist_ThrowsArgumentOutOfRangeException()
    {
        var stubListGames = new List<Game> { CreateOtherGame() };
        var gameRepository = CreateDefaultGameRepository(stubListGames);

        var action = () => gameRepository.Delete(1);

        var ex = Assert.Throws<ArgumentOutOfRangeException>(action);
        Assert.Equal("Game not found.", ex.Message);
        Assert.True(stubListGames.Count.Equals(1));
    }

    [Fact]
    public void Delete_ListWithOneGameAndIdExist_ChangeDeleteProperty()
    {
        var stubListGames = new List<Game> { CreateOtherGame() };
        var gameRepository = CreateDefaultGameRepository(stubListGames);

        gameRepository.Delete(0);

        Assert.True(stubListGames.Count.Equals(1));
        Assert.True(stubListGames[0].Deleted);
    }
}