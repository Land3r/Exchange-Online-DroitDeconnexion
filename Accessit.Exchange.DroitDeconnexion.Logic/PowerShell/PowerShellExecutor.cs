using Accessit.Exchange.DroitDeconnexion.Business.Emails;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Configuration;

namespace Accessit.Exchange.DroitDeconnexion.Logic.Powershell
{
    /// <summary>
    /// Provides PowerShell script execution examples
    /// </summary>
    class PowerShellExecutor : IDisposable
    {
        public string GetExecutionPath()
        {
            string powerShellResult = string.Empty;

            using (PowerShell PowerShellInstance = PowerShell.Create())
            {
                PowerShellInstance.AddScript("write-output $PSScriptRoot");

                PSDataCollection<PSObject> outputCollection = new PSDataCollection<PSObject>();
                outputCollection.DataAdded += outputCollection_DataAdded;

                // begin invoke execution on the pipeline
                // use this overload to specify an output stream buffer
                IAsyncResult result = PowerShellInstance.BeginInvoke<PSObject, PSObject>(null, outputCollection);

                // do something else until execution has completed.
                // this could be sleep/wait, or perhaps some other work
                while (result.IsCompleted == false)
                {
                    Thread.Sleep(1000);
                }

                foreach (PSObject outputItem in outputCollection)
                {
                    powerShellResult += outputItem.BaseObject.ToString();
                }
                return powerShellResult;
            }
        }

        /// <summary>
        /// Sample execution scenario 1: Synchronous
        /// </summary>
        /// <remarks>
        /// Executes a PowerShell script synchronously with input parameters and script output handling.
        /// </remarks>
        public void ExecuteSynchronously()
        {
            using (PowerShell PowerShellInstance = PowerShell.Create())
            {
                // use "AddScript" to add the contents of a script file to the end of the execution pipeline.
                // use "AddCommand" to add individual commands/cmdlets to the end of the execution pipeline.
                PowerShellInstance.AddScript("param($param1) $d = get-date; $s = 'test string value'; " +
                "$d; $s; $param1; get-service");

                // use "AddParameter" to add a single parameter to the last command/script on the pipeline.
                PowerShellInstance.AddParameter("param1", "parameter 1 value!");

                // invoke execution on the pipeline (collecting output)
                Collection<PSObject> PSOutput = PowerShellInstance.Invoke();

                // check the other output streams (for example, the error stream)
                if (PowerShellInstance.Streams.Error.Count > 0)
                {
                    // error records were written to the error stream.
                    // do something with the items found.
                }

                // loop through each output object item
                foreach (PSObject outputItem in PSOutput)
                {
                    // if null object was dumped to the pipeline during the script then a null
                    // object may be present here. check for null to prevent potential NRE.
                    if (outputItem != null)
                    {
                        //TODO: do something with the output item 
                        Console.WriteLine(outputItem.BaseObject.GetType().FullName);
                        Console.WriteLine(outputItem.BaseObject.ToString() + "\n");
                    }
                }
            }
        }

        /// <summary>
        /// Sample execution scenario 2: Asynchronous
        /// </summary>
        /// <remarks>
        /// Executes a PowerShell script asynchronously with script output and event handling.
        /// </remarks>
        public PSDataCollection<PSObject> ExecuteAsynchronously(string scriptName, params KeyValuePair<string, string>[] args)
        {
            String content = string.Empty;
            PSDataCollection<PSObject> outputCollection = new PSDataCollection<PSObject>();

            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader(@"C:\Users\ngordat.ACCESSIT\source\Repos\Accessit.Exchange.DroitDeconnexion\Accessit.Exchange.DroitDeconnexion.Presentation\bin\Debug\PowerShell\Scripts\" + scriptName))
                {
                    // Read the stream to a string, and write the string to the console.
                    content = sr.ReadToEnd();
                }

                using (PowerShell PowerShellInstance = PowerShell.Create(RunspaceMode.NewRunspace))
                {
                    // Register callbacks.
                    outputCollection.DataAdded += outputCollection_DataAdded;

                    PowerShellInstance.Streams.Debug.DataAdded += Output_DataAdded;
                    PowerShellInstance.Streams.Error.DataAdded += Output_DataAdded;
                    PowerShellInstance.Streams.Information.DataAdded += Output_DataAdded;
                    PowerShellInstance.Streams.Progress.DataAdded += Output_DataAdded;
                    PowerShellInstance.Streams.Verbose.DataAdded += Output_DataAdded;
                    PowerShellInstance.Streams.Warning.DataAdded += Output_DataAdded;

                    // Set policy to unrestricted.
                    PowerShellInstance.AddScript("$previousPolicy = Get-ExecutionPolicy; Set-ExecutionPolicy Unrestricted; $currentPolicy = Get-ExecutionPolicy; Write-Information \"Previous policy: $previousPolicy, Current Policy: $currentPolicy\"");
                    PowerShellInstance.Invoke();

                    // Prepare the invocation.
                    PowerShellInstance.AddScript(content);
                    PowerShellInstance.AddParameter("ExchangeUsername", "admin@M365x640287.onmicrosoft.com");
                    PowerShellInstance.AddParameter("ExchangePassword", "epobanz@2083");
                    foreach (KeyValuePair<string, string> arg in args)
                    {
                        PowerShellInstance.AddParameter(arg.Key, arg.Value);
                    }

                    // Launch the invocation.
                    IAsyncResult result = PowerShellInstance.BeginInvoke<PSObject, PSObject>(null, outputCollection);

                    // Wait execution.
                    while (result.IsCompleted == false)
                    {
                        Thread.Sleep(500);
                    }

                    Console.WriteLine("Execution has stopped, state: " + PowerShellInstance.InvocationStateInfo.State);
                    return outputCollection;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
                return outputCollection;
            }
        }

        /// <summary>
        /// Event handler for when data is added to the output stream.
        /// </summary>
        /// <param name="sender">Contains the complete PSDataCollection of all output items.</param>
        /// <param name="e">Contains the index ID of the added collection item and the ID of the PowerShell instance this event belongs to.</param>
        void outputCollection_DataAdded(object sender, DataAddedEventArgs e)
        {
            PSDataCollection<PSObject> data = sender as PSDataCollection<PSObject>;
            PSObject last = data.Last<PSObject>();
            // do something when an object is written to the output stream
            Console.WriteLine(last.ToString());
        }

        /// <summary>
        /// Event handler for when Data is added to the a stream.
        /// </summary>
        /// <param name="sender">Contains the complete PSDataCollection of all error output items.</param>
        /// <param name="e">Contains the index ID of the added collection item and the ID of the PowerShell instance this event belongs to.</param>
        void Output_DataAdded(object sender, DataAddedEventArgs e)
        {
            if (sender is PSDataCollection<ErrorRecord>)
            {
                PSDataCollection<ErrorRecord> data = sender as PSDataCollection<ErrorRecord>;
                Console.WriteLine("Error: " + data.Last<ErrorRecord>().ToString());
            }
            else if (sender is PSDataCollection<WarningRecord>)
            {
                PSDataCollection<WarningRecord> data = sender as PSDataCollection<WarningRecord>;
                Console.WriteLine("Warning: " + data.Last<WarningRecord>().Message);
            }
            else if (sender is PSDataCollection<InformationRecord>)
            {
                PSDataCollection<InformationRecord> data = sender as PSDataCollection<InformationRecord>;
                Console.WriteLine("Info: " + data.Last<InformationRecord>().ToString());
            }
            else if (sender is PSDataCollection<DebugRecord>)
            {
                PSDataCollection<DebugRecord> data = sender as PSDataCollection<DebugRecord>;
                Console.WriteLine("Debug: " + data.Last<DebugRecord>().Message);
            }
            else if (sender is PSDataCollection<VerboseRecord>)
            {
                PSDataCollection<VerboseRecord> data = sender as PSDataCollection<VerboseRecord>;
                Console.WriteLine("Verbose: " + data.Last<VerboseRecord>().Message);
            }
            else if (sender is PSDataCollection<ProgressRecord>)
            {
                PSDataCollection<ProgressRecord> data = sender as PSDataCollection<ProgressRecord>;
                ProgressRecord lastRecord = data.Last<ProgressRecord>();
                Console.WriteLine("Progress: " + lastRecord.Activity + ": " + lastRecord.StatusDescription);
            }
        }

        void IDisposable.Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
