using Accessit.Exchange.DroitDeconnexion.Logic.Powershell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accessit.Exchange.DroitDeconnexion.Logic.Helpers
{
    public class DeployerHelper
    {
        public bool Deploy()
        {
            using (PowerShellExecutor pse = new PowerShellExecutor())
            {
                pse.ExecuteAsynchronously(Scripts.DeploySolution);
            }

            return true;
        }

        public bool Retract()
        {
            using (PowerShellExecutor pse = new PowerShellExecutor())
            {
                pse.ExecuteAsynchronously(Scripts.RetractSolution);
            }

            return true;
        }
    }
}
