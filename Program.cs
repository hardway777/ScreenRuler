namespace ScreenRuler
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args) // Добавляем аргументы
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            string? initialFilePath = args.Length > 0 ? args[0] : null;
            Application.Run(new RulerForm(initialFilePath));
        }
    }
}