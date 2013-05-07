using Fishing.Properties;
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
                args = new string[] { Resources.ArgumentNoArgs };
            }
            else if (args.Length == 1 && args[0] == Resources.ArgumentForceSync)
            {
                string[] message =
                    {
                        "You are about to force sync your FishDB with the remote database.",
                        "This will take a long time.",
                        "The program will exit when the sync is complete.",
                        "",
                        "It is suggested that you do not run another FishingForm while the sync is happening.",
                        "",
                        "Are you sure you want to do this?"
                    };
                if (DialogResult.Yes == MessageBox.Show(string.Join(Environment.NewLine, message), Resources.MessageTitleForceSync, MessageBoxButtons.YesNo))
                {
                    form = new FishingDBSyncForm();
                }
                else
                {
                    args = new string[] { Resources.ArgumentNoArgs };
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
                MessageBox.Show(e.Message + Environment.NewLine + Environment.NewLine + e.StackTrace, Resources.MessageTitleError, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
