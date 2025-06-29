using System.Runtime.InteropServices;

namespace KeepMeOn
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        /// 
        // Import keybd_event from user32.dll
        [DllImport("user32.dll", SetLastError = true)]
        static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        // Constants for key event flags
        const int KEYEVENTF_KEYDOWN = 0x0000; // Key down flag
        const int KEYEVENTF_KEYUP = 0x0002;   // Key up flag
        // Virtual key code for Control
        const byte VK_CONTROL = 0x11;

        static NotifyIcon trayIcon;
        static System.Threading.Timer keyTimer;

        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);

            // Setup tray icon
            trayIcon = new NotifyIcon
            {
                Icon = SystemIcons.Shield ,
                Visible = true,
                Text = "Keep me on"
            };

            // Context menu with "Stop" option
            var menu = new ContextMenuStrip();
            var exitItem = new ToolStripMenuItem("Stop and Exit");
            exitItem.Click += ExitApp;
            menu.Items.Add(exitItem);
            trayIcon.ContextMenuStrip = menu;

            keyTimer = new System.Threading.Timer(SendKey, null, 0, 295000); //Press virtual Control every 4 minutes and 55 seconds
            Application.Run();
        }

        static void SendKey(object state)
        {
            keybd_event(VK_CONTROL, 0, KEYEVENTF_KEYDOWN, 0);
            Thread.Sleep(1); 
            keybd_event(VK_CONTROL, 0, KEYEVENTF_KEYUP, 0);
        }

        static void ExitApp(object sender, EventArgs e)
        {
            keyTimer.Dispose();
            trayIcon.Visible = false;
            Application.Exit();
        }
    }
}