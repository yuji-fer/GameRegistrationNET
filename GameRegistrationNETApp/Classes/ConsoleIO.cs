using GameRegistrationNETApp.Interfaces;

namespace GameRegistrationNETApp
{
    public class ConsoleIO : IConsoleIO
    {
        public void Clear() => Console.Clear();

        public string ReadLine() => Console.ReadLine()!;

        public void Write(string s = "") => Console.Write(s);

        public void WriteLine(string s = "") => Console.WriteLine(s);
    }
}