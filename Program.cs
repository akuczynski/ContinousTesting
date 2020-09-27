namespace ContinueTesting
{
    using System;
    using System.Threading;
    using System.Windows.Forms;

    static class Program
    {
        private static ContinueTestingContext appContext;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            bool createdNew = true;

            using (Mutex mutex = new Mutex(true, "ContinueTesting", out createdNew))
            {
                if (createdNew)
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                    appContext = new ContinueTestingContext(args[0], args[1]);
                    Application.Run(appContext);
                }
            }
        }

        public static void Quit()
        {
            appContext.ExitThread();
        }
    }
}