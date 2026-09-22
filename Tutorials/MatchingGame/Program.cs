namespace MatchingGame
{
    internal static class Program
    {
        // Harjutus: https://learn.microsoft.com/en-us/visualstudio/get-started/csharp/tutorial-windows-forms-create-match-game
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}
