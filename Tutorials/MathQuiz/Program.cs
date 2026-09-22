namespace MathQuiz
{
    internal static class Program
    {
        // Harjutus: https://learn.microsoft.com/en-us/visualstudio/get-started/csharp/tutorial-windows-forms-math-quiz-create-project-add-controls
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}
