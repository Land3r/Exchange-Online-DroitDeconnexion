using Accessit.Exchange.DroitDeconnexion.Logic.Powershell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accessit.Exchange.DroitDeconnexion.Logic.Helpers
{
    public class ActivatorHelper
    {
        public bool Enable()
        {
            using (PowerShellExecutor pse = new PowerShellExecutor())
            {
                pse.ExecuteAsynchronously(Scripts.EnableSolution);
            }

            return true;
        }

        public bool Disable()
        {
            using (PowerShellExecutor pse = new PowerShellExecutor())
            {
                pse.ExecuteAsynchronously(Scripts.DisableSolution);
            }

            return true;
        }
    }
}
