using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Accessit.Exchange.DroitDeconnexion.Presentation.Components
{
    /// <summary>
    /// Textbox wrapper in order to be multi thread safe.
    /// </summary>
    public class TextBoxOutputter : TextWriter
    {
        /// <summary>
        /// The internal textbox used to write the stream.
        /// </summary>
        TextBox textBox = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextBoxOutputter"/> class.
        /// </summary>
        /// <param name="output"></param>
        public TextBoxOutputter(TextBox output)
        {
            textBox = output;
        }

        /// <summary>
        /// Thread safe writer to the UI.
        /// </summary>
        /// <param name="value"></param>
        public override void Write(char value)
        {
            base.Write(value);
            textBox.Dispatcher.BeginInvoke(new Action(() =>
            {
                textBox.AppendText(value.ToString());
            }));
        }

        /// <summary>
        /// Gets the encoding used by the outputter.
        /// </summary>
        public override Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }
    }
}
