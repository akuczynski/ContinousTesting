namespace ContinueTesting
{
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    public class ContinueTestingContext : ApplicationContext
    {
        private NotifyIcon _trayIcon;

        private string _pathToBatFile;

        private string _testName;

        public ContinueTestingContext(string pathToBatFile, string testName)
        {
            // Initialize Tray Icon
            _trayIcon = new NotifyIcon
            {
                Icon = Resource.Icon,

                Visible = true
            };

            _pathToBatFile = pathToBatFile;
            _testName = testName;

            var thread = new Thread(RunUnitTests);
            thread.Start();
        }

        private void RunUnitTests()
        {
            var nUnitProc = new Process();
            nUnitProc.StartInfo.FileName = _pathToBatFile;
            nUnitProc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            nUnitProc.EnableRaisingEvents = true;

            try
            {
                nUnitProc.Start();
                nUnitProc.WaitForExit();

                var exitCode = nUnitProc.ExitCode;
                if (exitCode == 0)
                {
                    ShowSuccessTooltip(_testName);
                }
                else
                {
                    ShowFailTooltip(_testName);
                }
            }
            catch
            {
                // do nothing 
                ShowErrorTooltip();
            }

            _trayIcon.Visible = false;
            Program.Quit();
        }

        private void ShowSuccessTooltip(string title)
        {
            _trayIcon.ShowBalloonTip(20000, title, "All tests succeded", ToolTipIcon.Info);
        }

        private void ShowFailTooltip(string title)
        {
            _trayIcon.ShowBalloonTip(20000, title, "Some tests failed!", ToolTipIcon.Error);
        }

        private void ShowErrorTooltip()
        {
            _trayIcon.ShowBalloonTip(20000, "Error", "There was some exception!", ToolTipIcon.Error);
        }
    }
}