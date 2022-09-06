using GameRegistrationNETApp;
using GameRegistrationNETApp.Enums;
using GameRegistrationNETApp.Interfaces;
using Moq;

namespace GameRegistrationNETTest
{
    public class GameMenuTests
    {
        [Fact]
        public void Show_GetUserOptionXAndGameListIsEmpty_CallGetAllOnceAndCallReadLineTwiceAndNoCallSpecificWriteLineAndNoErrors()
        {
            var mockGameRepository = new Mock<IBaseRepository<Game>>();
            mockGameRepository.Setup(x => x.GetAll()).Returns(new List<Game>());
            var mockConsoleIO = new Mock<IConsoleIO>();
            mockConsoleIO.Setup(x => x.ReadLine()).Returns("x");
            var gameMenu = new GameMenu(mockGameRepository.Object, mockConsoleIO.Object);

            gameMenu.Show();

            mockGameRepository.Verify(x => x.GetAll(), Times.Once);
            mockConsoleIO.Verify(x => x.ReadLine(), Times.Exactly(2));
            mockConsoleIO.Verify(x => x.WriteLine("2- Atualizar game"), Times.Never);
        }

        [Fact]
        public void Show_GetUserOptionCAndAfterXAndGameListIsEmpty_CallGetAllTwiceAndCallReadLineThreeTimesAndNoErrors()
        {
            var mockGameRepository = new Mock<IBaseRepository<Game>>();
            mockGameRepository.Setup(x => x.GetAll()).Returns(new List<Game>());
            var mockConsoleIO = new Mock<IConsoleIO>();
            mockConsoleIO.SetupSequence(x => x.ReadLine()).Returns("c").Returns("x");
            var gameMenu = new GameMenu(mockGameRepository.Object, mockConsoleIO.Object);

            gameMenu.Show();

            mockGameRepository.Verify(x => x.GetAll(), Times.Exactly(2));
            mockConsoleIO.Verify(x => x.ReadLine(), Times.Exactly(3));
        }

        [
            Theory,
            InlineData("2"),
            InlineData("3"),
            InlineData("4"),
            InlineData("5"),
        ]
        public void Show_GetUserOptionInvalidAndAfterXAndGameListIsEmpty_CallGetAllTwiceAndCallReadLineFourTimesAndCallSpecificWriteLineAndNoErrors(string option)
        {
            var mockGameRepository = new Mock<IBaseRepository<Game>>();
            mockGameRepository.Setup(x => x.GetAll()).Returns(new List<Game>());
            var mockConsoleIO = new Mock<IConsoleIO>();
            mockConsoleIO.SetupSequence(x => x.ReadLine()).Returns(option).Returns("").Returns("x");
            var gameMenu = new GameMenu(mockGameRepository.Object, mockConsoleIO.Object);

            gameMenu.Show();

            mockGameRepository.Verify(x => x.GetAll(), Times.Exactly(2));
            mockConsoleIO.Verify(x => x.ReadLine(), Times.Exactly(4));
            mockConsoleIO.Verify(x => x.WriteLine("Opção inválida! Tente novamente!"), Times.Once);
        }

        [Fact]
        public void Show_GameListIsEmptyAndInsertValidGame_CallGetAllTwiceAndCallReadLineSevenTimesAndCallSpecificWriteLineAndNoErrors()
        {
            var mockGameRepository = new Mock<IBaseRepository<Game>>();
            mockGameRepository.Setup(x => x.GetAll()).Returns(new List<Game>());
            var mockConsoleIO = new Mock<IConsoleIO>();
            mockConsoleIO.SetupSequence(x => x.ReadLine())
                .Returns("1")
                .Returns("1")
                .Returns("Hitman")
                .Returns("2006")
                .Returns("Hitman")
                .Returns("x");
            var gameMenu = new GameMenu(mockGameRepository.Object, mockConsoleIO.Object);

            gameMenu.Show();

            mockGameRepository.Verify(x => x.GetAll(), Times.Exactly(2));
            mockConsoleIO.Verify(x => x.ReadLine(), Times.Exactly(7));
            mockConsoleIO.Verify(x => x.WriteLine("Game cadastrado com sucesso!"), Times.Once);
        }

        [
            Theory,
            InlineData("0", "Hitman", "2006", "Hitman", 2),
            InlineData("x", "Hitman", "2006", "Hitman", 2),
            InlineData("1", "Hitman", "x", "Hitman", 4),
        ]
        public void Show_GameListIsEmptyAndInsertInvalidGame_CallGetAllOnceAndCallReadLineXTimesAndNoCallSpecificWriteLineAndHasError(string genre, string title, string year, string description, int countReadLine)
        {
            var mockGameRepository = new Mock<IBaseRepository<Game>>();
            mockGameRepository.Setup(x => x.GetAll()).Returns(new List<Game>());
            var mockConsoleIO = new Mock<IConsoleIO>();
            mockConsoleIO.SetupSequence(x => x.ReadLine())
                .Returns("1")
                .Returns(genre)
                .Returns(title)
                .Returns(year)
                .Returns(description)
                .Returns("x");
            var gameMenu = new GameMenu(mockGameRepository.Object, mockConsoleIO.Object);

            var action = () => gameMenu.Show();

            Assert.Throws<ArgumentException>(action);
            mockGameRepository.Verify(x => x.GetAll(), Times.Exactly(1));
            mockConsoleIO.Verify(x => x.ReadLine(), Times.Exactly(countReadLine));
            mockConsoleIO.Verify(x => x.WriteLine("Game cadastrado com sucesso!"), Times.Never);
        }

        [Fact]
        public void Show_GameListNotEmptyAndUpdateValidGame_CallGetAllTwiceAndCallReadLineEightTimesAndCallSpecificWriteLineAndNoErrors()
        {
            var mockGameRepository = new Mock<IBaseRepository<Game>>();
            mockGameRepository.Setup(x => x.GetAll()).Returns(new List<Game> {
                new Game
                {
                    Id = 0,
                    Title = "Hitman 47",
                    Description = "Hitman 47",
                    Year = 2006,
                    Genre = Genre.Action
                }
            });
            var mockConsoleIO = new Mock<IConsoleIO>();
            mockConsoleIO.SetupSequence(x => x.ReadLine())
                .Returns("2")
                .Returns("0")
                .Returns("5")
                .Returns("NFSMW")
                .Returns("2012")
                .Returns("NFSMW")
                .Returns("x");
            var gameMenu = new GameMenu(mockGameRepository.Object, mockConsoleIO.Object);

            gameMenu.Show();

            mockGameRepository.Verify(x => x.GetAll(), Times.Exactly(2));
            mockConsoleIO.Verify(x => x.ReadLine(), Times.Exactly(8));
            mockConsoleIO.Verify(x => x.WriteLine("Game atualizado com sucesso!"), Times.Once);
        }

        [
            Theory,
            InlineData("x", "1", "Hitman", "2006", "Hitman", 2),
            InlineData("0", "0", "Hitman", "2006", "Hitman", 3),
            InlineData("0", "x", "Hitman", "2006", "Hitman", 3),
            InlineData("0", "1", "Hitman", "x", "Hitman", 5),
        ]
        public void Show_GameListNotEmptyAndUpdateInvalidGame_CallGetAllOnceAndCallReadLineXTimesAndNoCallSpecificWriteLineAndHasError(string id, string genre, string title, string year, string description, int countReadLine)
        {
            var mockGameRepository = new Mock<IBaseRepository<Game>>();
            mockGameRepository.Setup(x => x.GetAll()).Returns(new List<Game> {
                new Game()
            });
            var mockConsoleIO = new Mock<IConsoleIO>();
            mockConsoleIO.SetupSequence(x => x.ReadLine())
                .Returns("2")
                .Returns(id)
                .Returns(genre)
                .Returns(title)
                .Returns(year)
                .Returns(description)
                .Returns("x");
            var gameMenu = new GameMenu(mockGameRepository.Object, mockConsoleIO.Object);

            var action = () => gameMenu.Show();

            Assert.Throws<ArgumentException>(action);
            mockGameRepository.Verify(x => x.GetAll(), Times.Exactly(1));
            mockConsoleIO.Verify(x => x.ReadLine(), Times.Exactly(countReadLine));
            mockConsoleIO.Verify(x => x.WriteLine("Game atualizado com sucesso!"), Times.Never);
        }

        [Fact]
        public void Show_GameListNotEmptyAndDeleteValidGame_CallGetAllTwiceAndCallReadLineFourTimesAndCallSpecificWriteLineAndNoErrors()
        {
            var mockGameRepository = new Mock<IBaseRepository<Game>>();
            mockGameRepository.Setup(x => x.GetAll()).Returns(new List<Game> {
                new Game()
            });
            var mockConsoleIO = new Mock<IConsoleIO>();
            mockConsoleIO.SetupSequence(x => x.ReadLine())
                .Returns("3")
                .Returns("0")
                .Returns("x");
            var gameMenu = new GameMenu(mockGameRepository.Object, mockConsoleIO.Object);

            gameMenu.Show();

            mockGameRepository.Verify(x => x.GetAll(), Times.Exactly(2));
            mockConsoleIO.Verify(x => x.ReadLine(), Times.Exactly(4));
            mockConsoleIO.Verify(x => x.WriteLine("Game excluído com sucesso!"), Times.Once);
        }

        [Fact]
        public void Show_GameListNotEmptyAndDeleteInvalidGame_CallGetAllOnceAndCallReadLineTwiceAndNoCallSpecificWriteLineAndError()
        {
            var mockGameRepository = new Mock<IBaseRepository<Game>>();
            mockGameRepository.Setup(x => x.GetAll()).Returns(new List<Game> {
                new Game()
            });
            var mockConsoleIO = new Mock<IConsoleIO>();
            mockConsoleIO.SetupSequence(x => x.ReadLine())
                .Returns("3")
                .Returns("x")
                .Returns("x");
            var gameMenu = new GameMenu(mockGameRepository.Object, mockConsoleIO.Object);

            var action = () => gameMenu.Show();

            Assert.Throws<ArgumentException>(action);
            mockGameRepository.Verify(x => x.GetAll(), Times.Exactly(1));
            mockConsoleIO.Verify(x => x.ReadLine(), Times.Exactly(2));
            mockConsoleIO.Verify(x => x.WriteLine("Game excluído com sucesso!"), Times.Never);
        }

        [Fact]
        public void Show_GameListNotEmptyAndViewValidGame_CallGetAllTwiceAndCallReadLineFourTimesAndCallSpecificWriteLineAndNoErrors()
        {
            var game = new Game
            {
                Id = 0,
                Title = "Hitman 47",
                Description = "Hitman 47",
                Year = 2006,
                Genre = Genre.Action,
                Deleted = true
            };
            var mockGameRepository = new Mock<IBaseRepository<Game>>();
            mockGameRepository.Setup(x => x.GetAll()).Returns(new List<Game> { game });
            mockGameRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(game);
            var mockConsoleIO = new Mock<IConsoleIO>();
            mockConsoleIO.SetupSequence(x => x.ReadLine())
                .Returns("4")
                .Returns("0")
                .Returns("x");
            var gameMenu = new GameMenu(mockGameRepository.Object, mockConsoleIO.Object);

            gameMenu.Show();

            mockGameRepository.Verify(x => x.GetAll(), Times.Exactly(2));
            mockConsoleIO.Verify(x => x.ReadLine(), Times.Exactly(4));
            mockConsoleIO.Verify(x => x.WriteLine(game.ToString()), Times.Once);
        }

        [Fact]
        public void Show_GameListNotEmptyAndViewInvalidGame_CallGetAllOnceAndCallReadLineTwiceAndNoCallSpecificWriteLineAndError()
        {
            var game = new Game
            {
                Id = 0,
                Title = "Hitman 47",
                Description = "Hitman 47",
                Year = 2006,
                Genre = Genre.Action
            };
            var mockGameRepository = new Mock<IBaseRepository<Game>>();
            mockGameRepository.Setup(x => x.GetAll()).Returns(new List<Game> { game });
            var mockConsoleIO = new Mock<IConsoleIO>();
            mockConsoleIO.SetupSequence(x => x.ReadLine())
                .Returns("4")
                .Returns("x")
                .Returns("x");
            var gameMenu = new GameMenu(mockGameRepository.Object, mockConsoleIO.Object);

            var action = () => gameMenu.Show();

            Assert.Throws<ArgumentException>(action);
            mockGameRepository.Verify(x => x.GetAll(), Times.Exactly(1));
            mockConsoleIO.Verify(x => x.ReadLine(), Times.Exactly(2));
            mockConsoleIO.Verify(x => x.WriteLine(game.ToString()), Times.Never);
        }

        [
            Theory,
            InlineData(true),
            InlineData(false),
        ]
        public void Show_GameListNotEmptyAndListGames_CallGetAllThreeTimesAndCallReadLineThreeTimesAndCallSpecificWriteLineAndNoErrors(bool deleted)
        {
            var game = new Game
            {
                Id = 0,
                Title = "Hitman 47",
                Description = "Hitman 47",
                Year = 2006,
                Genre = Genre.Action,
                Deleted = deleted
            };
            var mockGameRepository = new Mock<IBaseRepository<Game>>();
            mockGameRepository.Setup(x => x.GetAll()).Returns(new List<Game> { game });
            var mockConsoleIO = new Mock<IConsoleIO>();
            mockConsoleIO.SetupSequence(x => x.ReadLine())
                .Returns("5")
                .Returns("x");
            var gameMenu = new GameMenu(mockGameRepository.Object, mockConsoleIO.Object);

            gameMenu.Show();

            mockGameRepository.Verify(x => x.GetAll(), Times.Exactly(3));
            mockConsoleIO.Verify(x => x.ReadLine(), Times.Exactly(3));
            mockConsoleIO.Verify(x => x.WriteLine(It.Is<string>(s => s.StartsWith("#ID"))), Times.Once);
        }
    }
}