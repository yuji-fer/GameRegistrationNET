using GameRegistrationNETApp.Interfaces;

namespace GameRegistrationNETApp
{
    class Program
    {
        static void Main(string[] args)
        {
            IBaseRepository<Game> gameRepository = new GameRepository();
            IConsoleIO consoleIO = new ConsoleIO();
            GameMenu gameMenu = new GameMenu(gameRepository, consoleIO);
            gameMenu.Show();
        }
    }
}