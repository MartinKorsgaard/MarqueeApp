namespace MarqueeApp
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string[] args = Environment.GetCommandLineArgs();

            string message = "";

            for (int i = 0;i < args.Length; i++)
            {
                string arg = args[i].ToLower();

                if (arg.StartsWith("/"))
                {
                    // i've put /message "Hello World!" into the debug command line arguments
                    if (arg.Equals("/message"))
                    {
                        message = args[i+1];
                    }
                }
            }

            if (!string.IsNullOrEmpty(message))
            {
                // To customize application configuration such as set high DPI settings or default font,
                // see https://aka.ms/applicationconfiguration.
                ApplicationConfiguration.Initialize();
                Application.Run(new FormMarquee(message));
            }
        }
    }
}