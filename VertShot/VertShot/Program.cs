using System;

namespace VertShot
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        static void Main(string[] args)
        {
            // Doesn't work -.-
//#if WINDOWS
//            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\XNA\Framework\v4.0");
//            if (key == null || !Array.Exists(key.GetValueNames(), s => s == "Refresh1Installed") || key.GetValue("Refresh1Installed").ToString() != "1")
//            {
//                if (System.Windows.Forms.MessageBox.Show("Microsoft XNA Framework Redistributable 4.0 Refresh ist nicht installiert!\n\nDas aktuelle Paket von microsoft.com herunterladen?", "Fehler", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
//                    System.Diagnostics.Process.Start("http://www.microsoft.com/en-us/download/details.aspx?id=27598");

//            }
//            else
//#endif
                using (Game1 game = new Game1())
                {
                    game.Run();
                }
        }
    }
#endif
}

