using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Assembly69.Interface.Controls
{

    [Flags]
    public enum ProcessType
    {
        None = 0,
        WinStore = 1 << 0,
        Steam = 1 << 1,
        Server = 1 << 2
    }

    public class ProcessInformation
    {
        public int ProcessId { get; set; }
        public ProcessType ProcessType { get; set; }
        public System.Diagnostics.Process Process { get; set; } = null;
        public string CommandLine { get; set; }
    }

    /// <summary>
    /// Interaction logic for ProcessSelector.xaml
    /// </summary>
    public partial class ProcessSelector : UserControl
    {
        public ProcessInformation SelectedProcess { get; private set; } = null;
        public ComboBoxItem cbxiChooseAny { get; private set; }

        public void hookProcess(Memory.Mem m)
        {
            if (SelectedProcess == null)
            {
                m.OpenProcess("HaloInfinite.exe");
                return;
            }

            // Check if the process is still alive
            SelectedProcess.Process.Refresh();
            if (SelectedProcess.Process.HasExited)
            {
                MessageBox.Show("Selected halo process closed, Hooking any HI process...");
                m.OpenProcess("HaloInfinite.exe");
                return;
            }

            // Attempt to hook PID
            m.OpenProcess(SelectedProcess.ProcessId);
        }

        public ProcessSelector()
        {
            InitializeComponent();
            cbxiChooseAny = (ComboBoxItem) cbxSelector.Items[0];

            // Don't get the processes if in designer
            if (! DesignerProperties.GetIsInDesignMode(this))
                ReloadProcesses();
        }

        private void ReloadProcesses()
        {
            System.Diagnostics.Process hi;

            var si = cbxSelector.SelectedIndex; 
            cbxSelector.Items.Clear();
            cbxSelector.Items.Add(cbxiChooseAny);
            if (si == 0) cbxSelector.SelectedIndex = 0;

            List<ProcessInformation> foundProcesses = new List<ProcessInformation>();

            // Find all halo processes and determine if its steam or uwp winstore
            foreach (var proc in System.Diagnostics.Process.GetProcesses())
            {
                string fullPath = null;
                try
                {
                    fullPath = proc.MainModule.FileName;
                    string exeName = fullPath.Split('\\').Last();

                    if (exeName != "HaloInfinite.exe")
                        continue;

                    // Look for appxdeployment in modules ? 

                    ProcessInformation procInfo = new ProcessInformation();
                    procInfo.ProcessType = ProcessType.WinStore;
                    procInfo.Process = proc;
                    procInfo.ProcessId = proc.Id;

                    foreach (System.Diagnostics.ProcessModule mod in proc.Modules)
                        if (mod.ModuleName.ToLower().StartsWith("steamclient64.dll"))
                            procInfo.ProcessType = ProcessType.Steam;

                    foundProcesses.Add(procInfo);
                    hi = proc;
                }
                catch (Exception ex)
                {
                    //Console.WriteLine("Failed to read proc: " + ex.Message);
                    continue;
                }
            }

            if (foundProcesses.Count == 0)
            {
                SelectedProcess = null;
                return;
            }

            // Query WMI for commandline info
            string whereQuery = string.Join(" OR ", foundProcesses.Select(x => $" ProcessId = {x.ProcessId} "));

            string query = 
                $"SELECT Name, CommandLine, ProcessId, Caption, ExecutablePath " +
                $"FROM Win32_Process " + 
                $"WHERE {whereQuery}";


            string wmiScope = $@"\\{Environment.MachineName}\root\cimv2";
            int a = 0;

            ManagementObjectSearcher searcher = new ManagementObjectSearcher(wmiScope, query);
            foreach (ManagementObject mo in searcher.Get())
            {
                var pid = Convert.ToInt32((System.UInt32) mo["ProcessId"]);
                var commandLine = (string) mo["CommandLine"];

                var pi = foundProcesses.Where(x => x.ProcessId == pid).First();
                pi.CommandLine = commandLine;

                if (pi.CommandLine.Contains("-server"))
                {
                    pi.ProcessType |= ProcessType.Server;
                }
            }

            var found = false;

            for (int x = 0; x < foundProcesses.Count; x++)
            {
                var pi = foundProcesses[x];
                var time = (DateTime.Now - pi.Process.StartTime).ToString(@"hh\:mm\:ss");

                var cbxi = new ComboBoxItem() {
                    Content = $"Halo Infinite, {pi.ProcessType}, {pi.ProcessId}, {time}",
                    Tag = pi
                };

                cbxSelector.Items.Add(cbxi);

                if (SelectedProcess?.ProcessId == pi.ProcessId)
                {
                    cbxSelector.SelectedIndex = x + 1;
                    found = true;
                }
            }

            if (!found)
                cbxSelector.SelectedIndex = 0;
        }

        private void cbxSelector_DropDownOpened(object sender, EventArgs e)
        {
            ReloadProcesses();
        }

        private void cbxSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxSelector.SelectedIndex == 0)
            {
                SelectedProcess = null;
                return;
            }

            var cbxi = (ComboBoxItem) cbxSelector.SelectedItem;
            if (cbxi?.Tag == null || (cbxi.Tag as ProcessInformation) == null)
                return;

            var pi = (ProcessInformation) cbxi.Tag;


            this.SelectedProcess = pi;
        }
    }
}
