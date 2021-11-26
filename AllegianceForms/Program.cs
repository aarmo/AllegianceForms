using AllegianceForms.Engine;
using System;
using System.Threading;
using System.Windows.Forms;

namespace AllegianceForms
{
    static class Program
    {
        public static log4net.ILog Log;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Log = log4net.LogManager.GetLogger(typeof(Program));

                Log.Info("Starting Allegiance Forms...");

                Application.ThreadException += ApplicationOnThreadException;
                AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;

                //Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Forms.Menu());
            }
            catch (Exception e)
            {
                Log.Fatal("Entry Error - Entry", e);
            }
            finally
            {
                SoundEffect.Dispose();
            }
        }

        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Fatal("Unhandled Domain Error", e.ExceptionObject as Exception);
        }

        private static void ApplicationOnThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Log.Fatal("Application Thread Error", e.Exception);
        }
    }
}
