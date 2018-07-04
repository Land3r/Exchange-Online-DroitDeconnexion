using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accessit.Exchange.DroitDeconnexion.Logic
{
    /// <summary>
    /// Scripts static class used to act as an ENUM with a string value for referencing the scripts.
    /// </summary>
    public static class Scripts
    {
        /// <summary>
        /// The name of the script used to deploy the solution on exchange online.
        /// </summary>
        public static readonly string DeploySolution = "DeploySolution.ps1";

        /// <summary>
        /// The name of the script used to retract the solution from exchange online.
        /// </summary>
        public static readonly string RetractSolution = "RetractSolution.ps1";

        /// <summary>
        /// The name of the script used to enable the solution from exchange online.
        /// </summary>
        public static readonly string EnableSolution = "EnableSolution.ps1";

        /// <summary>
        /// The name of the script used to disable the solution from exchange online.
        /// </summary>
        public static readonly string DisableSolution = "DisableSolution.ps1";

        /// <summary>
        /// The name of the script used to retrieve the status of the solution from exchange online.
        /// </summary>
        public static readonly string GetSolutionStatus = "GetSolutionStatus.ps1";

        /// <summary>
        /// The name of the script used to retrieve all the emails in quarantine for 'le droit à la deconnexion' from exchange online.
        /// </summary>
        public static readonly string GetQuarantinedMails = "GetQuarantinedMails.ps1";

        /// <summary>
        /// The name of the script used to retrieve all the emails in quarantine for 'le droit à la deconnexion' from exchange online.
        /// </summary>
        public static readonly string ReleaseQuarantinedMails = "ReleaseQuarantinedMails.ps1";
    }
}
