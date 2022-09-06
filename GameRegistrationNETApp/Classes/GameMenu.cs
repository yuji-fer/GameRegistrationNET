using GameRegistrationNETApp.Enums;
using GameRegistrationNETApp.Interfaces;

namespace GameRegistrationNETApp
{
    public class GameMenu
    {
        private readonly IBaseRepository<Game> _gameRepository;
        private readonly IConsoleIO _consoleIO;
        private bool _gameListIsEmpty;

        public GameMenu(IBaseRepository<Game> gameRepository, IConsoleIO consoleIO)
        {
            _gameRepository = gameRepository;
            _consoleIO = consoleIO;
        }

        private string GetUserOption()
        {
            _consoleIO.WriteLine();
            _consoleIO.WriteLine("DIO Games a seu dispor!!!");
            _consoleIO.WriteLine("Informe a opção desejada:");

            _gameListIsEmpty = _gameRepository.GetAll().Count.Equals(0);

            _consoleIO.WriteLine("1- Inserir novo game");
            if (!_gameListIsEmpty)
            {
                _consoleIO.WriteLine("2- Atualizar game");
                _consoleIO.WriteLine("3- Excluir game");
                _consoleIO.WriteLine("4- Visualizar game");
                _consoleIO.WriteLine("5- Listar games");
            }

            _consoleIO.WriteLine("C- Limpar Tela");
            _consoleIO.WriteLine("X- Sair");
            _consoleIO.WriteLine();

            string userOption = _consoleIO.ReadLine().ToUpper();
            _consoleIO.WriteLine();

            return userOption;
        }

        public void Show()
        {
            var userOption = GetUserOption();
            while (userOption.ToUpper() != "X")
			{
				switch (userOption)
				{
					case "1":
						InsertGame();
						break;
					case var x when (x.Equals("2") && !_gameListIsEmpty):
						UpdateGame();
						break;
					case var x when (x.Equals("3") && !_gameListIsEmpty):
						DeleteGame();
						break;
					case var x when (x.Equals("4") && !_gameListIsEmpty):
						ViewGame();
						break;
					case var x when (x.Equals("5") && !_gameListIsEmpty):
						ListGames();
						break;
					case "C":
						_consoleIO.Clear();
						break;

					default:
						_consoleIO.WriteLine("Opção inválida! Tente novamente!");
                        _consoleIO.ReadLine();
                        break;
				}

				userOption = GetUserOption();
			}

			_consoleIO.WriteLine("Obrigado por utilizar nossos serviços.");
			_consoleIO.ReadLine();
        }

        private void DeleteGame()
		{
			_consoleIO.Write("Digite o id do game: ");
			int gameIndex;
            if (!int.TryParse(_consoleIO.ReadLine(), out gameIndex))
                throw new ArgumentException("O id do game deve ser um número!");

			_gameRepository.Delete(gameIndex);

            _consoleIO.WriteLine("Game excluído com sucesso!");
		}

        private void ViewGame()
		{
			_consoleIO.Write("Digite o id do game: ");
            int gameIndex;
			if (!int.TryParse(_consoleIO.ReadLine(), out gameIndex))
                throw new ArgumentException("O id do game deve ser um número!");

			var game = _gameRepository.GetById(gameIndex);

			_consoleIO.WriteLine(game.ToString());
		}

        private void UpdateGame()
        {
            _consoleIO.Write("Digite o id do game: ");
            int gameIndex;
            if (!int.TryParse(_consoleIO.ReadLine(), out gameIndex))
                throw new ArgumentException("O id do game deve ser um número!");

            Game updatedGame = RequestGameData();

            _gameRepository.Update(gameIndex, updatedGame);

            _consoleIO.WriteLine("Game atualizado com sucesso!");
        }

        private Game RequestGameData()
        {
            // https://docs.microsoft.com/pt-br/dotnet/api/system.enum.getvalues?view=netcore-3.1
            // https://docs.microsoft.com/pt-br/dotnet/api/system.enum.getname?view=netcore-3.1
            foreach (int i in Enum.GetValues(typeof(Genre)))
            {
                _consoleIO.WriteLine($"{i}-{Enum.GetName(typeof(Genre), i)}");
            }
            _consoleIO.Write("Digite o gênero entre as opções acima: ");
            int inputGenre;
            if (!int.TryParse(_consoleIO.ReadLine(), out inputGenre))
                throw new ArgumentException("O gênero do game deve ser um número!");
            else if (Enum.GetValues<Genre>().All(x => x != (Genre)inputGenre))
                throw new ArgumentException("O número informado não é um gênero válido!");

            _consoleIO.Write("Digite o título do game: ");
            string inputTitle = _consoleIO.ReadLine();

            _consoleIO.Write("Digite o ano de início do game: ");
            int inputYear;
            if (!int.TryParse(_consoleIO.ReadLine(), out inputYear))
                throw new ArgumentException("O ano de início do game deve ser um número!");

            _consoleIO.Write("Digite a descrição do game: ");
            string inputDescription = _consoleIO.ReadLine();

            Game game = new Game
            {
                Genre = (Genre)inputGenre,
                Title = inputTitle,
                Year = inputYear,
                Description = inputDescription
            };

            return game;
        }

        private void ListGames()
		{
			_consoleIO.WriteLine("Listar games");

			var listGames = _gameRepository.GetAll();

			foreach (var game in listGames)
			{
				_consoleIO.WriteLine($"#ID {game.Id}: - {game.Title} {(game.Deleted ? "*Excluído*" : "")}");
			}
		}

        private void InsertGame()
		{
			_consoleIO.WriteLine("Inserir novo game");

			Game gameToInsert = RequestGameData();

			_gameRepository.Insert(gameToInsert);

            _consoleIO.WriteLine("Game cadastrado com sucesso!");
		}
    }
}