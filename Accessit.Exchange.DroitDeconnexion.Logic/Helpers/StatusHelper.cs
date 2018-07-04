using Accessit.Exchange.DroitDeconnexion.Logic.Powershell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Accessit.Exchange.DroitDeconnexion.Logic.Helpers
{
    public class StatusHelper
    {
        /// <summary>
        /// Gets the control associated with the helper.
        /// </summary>
        public TextBlock Control { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusHelper"/> class.
        /// </summary>
        /// <param name="control">The control tu update</param>
        public StatusHelper(TextBlock control)
        {
            this.Control = control;
        }

        /// <summary>
        /// Gets the status of the programm.
        /// </summary>
        /// <returns></returns>
        public bool GetStatus()
        {
            using (PowerShellExecutor pse = new PowerShellExecutor())
            {
                PSDataCollection<PSObject> results = pse.ExecuteAsynchronously(Scripts.GetSolutionStatus);
                foreach (PSObject item in results)
                {
                    if (item.BaseObject.ToString().StartsWith("Result:"))
                    {
                        string value = item.BaseObject.ToString();
                        value = value.Replace("Result:", "");
                        Control.Text = value;
                    }
                }
            }

            return true;
        }
    }
}
