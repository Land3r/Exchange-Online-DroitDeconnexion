using Accessit.Exchange.DroitDeconnexion.Business.Emails;
using Accessit.Exchange.DroitDeconnexion.Logic.Powershell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace Accessit.Exchange.DroitDeconnexion.Logic.Helpers
{
    /// <summary>
    /// Helper to interact with the Quarantined emails.
    /// </summary>
    public class QuarantinedMailHelper
    {
        /// <summary>
        /// Gets all quarantined emails.
        /// </summary>
        /// <returns></returns>
        public ICollection<QuarantineEmail> GetAllQuarantinedMails()
        {
            ICollection<QuarantineEmail> functionResults = new List<QuarantineEmail>();

            using (PowerShellExecutor pse = new PowerShellExecutor())
            {
                PSDataCollection<PSObject> results = pse.ExecuteAsynchronously(Scripts.GetQuarantinedMails);
                foreach (PSObject item in results)
                {
                    if (item.BaseObject.ToString().StartsWith("Result:"))
                    {
                        string value = item.BaseObject.ToString();
                        value = value.Replace("Result:", "");
                        string[] values = value.Split(';');
                        QuarantineEmail email = new QuarantineEmail
                        {
                            Id = values.FirstOrDefault(),
                            Sender = values.LastOrDefault()
                        };
                        functionResults.Add(email);
                    }
                }
            }

            return functionResults;
        }

        public bool ReleaseAllMails()
        {
            using (PowerShellExecutor pse = new PowerShellExecutor())
            {
                pse.ExecuteAsynchronously(Scripts.ReleaseQuarantinedMails, new KeyValuePair<string, string>("QuarantineEmails", "ALL"));
            }

            return true;
        }

        public bool ReleaseMail(string[] mails)
        {
            using (PowerShellExecutor pse = new PowerShellExecutor())
            {
                pse.ExecuteAsynchronously(Scripts.ReleaseQuarantinedMails, new KeyValuePair<string, string>("QuarantineEmails", "NONE"));
            }

            return true;
        }
    }
}
