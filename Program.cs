using System;
using System.Windows.Forms;

namespace Fishing {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args) 
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form form = null;
            if (args.Length == 0)
            {
                args = new string[] { "No_args" };
            }
            else if (args.Length == 1 && args[0] == "--force-database-sync")
            {
                if (DialogResult.Yes == MessageBox.Show("You are about to force sync your FishDB with the remote database.\r\nThis will take a long time.\r\nThe program will exit when the sync is complete.\r\n\r\nIt is suggested that you do not run another FishingForm while the sync is happening.\r\n\r\nAre you sure you want to do this?", "Force Sync?", MessageBoxButtons.YesNo))
                {
                    form = new FishingDBSyncForm();
                }
                else
                {
                    args = new string[] { "No_args" };
                }
            }
            try
            {
                if (null != form)
                {
                    Application.Run(form);
                }
                else
                {
                    Application.Run(new FishingForm(args));
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message + "\r\r" + e.StackTrace, "FishingForm Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
