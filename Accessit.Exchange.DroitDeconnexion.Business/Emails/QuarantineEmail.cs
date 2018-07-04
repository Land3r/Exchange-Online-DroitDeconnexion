using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accessit.Exchange.DroitDeconnexion.Business.Emails
{
    /// <summary>
    /// Represents a Quarantine email.
    /// </summary>
    public class QuarantineEmail
    {
        /// <summary>
        /// Gets or sets the Id of the <see cref="QuarantineEmail"/>.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the sender of the <see cref="QuarantineEmail"/>.
        /// </summary>
        public string Sender { get; set; }
    }
}
