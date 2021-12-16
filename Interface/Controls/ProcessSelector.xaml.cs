using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Management;
using System.Windows;
using System.Windows.Controls;

namespace InfiniteRuntimeTagViewer.Interface.Controls
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
        public System.Diagnostics.Process? Process { get; set; } = null;
        public string? CommandLine { get; set; }
    }

    /// <summary>
    /// Interaction logic for ProcessSelector.xaml
    /// </summary>
    public partial class ProcessSelector : UserControl
    {
        public ProcessInformation? SelectedProcess { get; private set; } = null;
        public ComboBoxItem? cbxiChooseAny { get; private set; }
		public bool selected = false;
        public bool hookProcess(Memory.Mem m)
        {
            if (SelectedProcess == null)
            {
                selected = m.OpenProcess("HaloInfinite.exe");
				return selected;
            }

            // Check if the process is still alive
            SelectedProcess.Process.Refresh();
            if (SelectedProcess.Process.HasExited)
            {
				System.Diagnostics.Debug.WriteLine("Process not open.");
                MessageBox.Show("Selected halo process closed, Hooking any HI process...");
				selected = m.OpenProcess("HaloInfinite.exe");
                return selected;
            }

			// Attempt to hook PID
			selected = m.OpenProcess(SelectedProcess.ProcessId);
			return selected;

		}

        public ProcessSelector()
        {
            InitializeComponent();
            cbxiChooseAny = (ComboBoxItem) cbxSelector.Items[0];

            // Don't get the processes if in designer
            if (! DesignerProperties.GetIsInDesignMode(this))
			{
				ReloadProcesses();
			}
		}

        private void ReloadProcesses()
        {
            System.Diagnostics.Process hi;

			int si = cbxSelector.SelectedIndex; 
            cbxSelector.Items.Clear();
            cbxSelector.Items.Add(cbxiChooseAny);
            if (si == 0)
			{
				cbxSelector.SelectedIndex = 0;
			}

			List<ProcessInformation> foundProcesses = new List<ProcessInformation>();

            // Find all halo processes and determine if its steam or uwp winstore
            foreach (System.Diagnostics.Process? proc in System.Diagnostics.Process.GetProcesses())
            {
                string? fullPath = null;
                try
                {
                    fullPath = proc.MainModule.FileName;
                    string? exeName = fullPath.Split('\\').Last();

                    if (exeName != "HaloInfinite.exe")
					{
						continue;
					}

					// Look for appxdeployment in modules ? 

					ProcessInformation procInfo = new ProcessInformation();
                    procInfo.ProcessType = ProcessType.WinStore;
                    procInfo.Process = proc;
                    procInfo.ProcessId = proc.Id;

                    foreach (System.Diagnostics.ProcessModule mod in proc.Modules)
					{
						if (mod.ModuleName.ToLower().StartsWith("steamclient64.dll"))
						{
							procInfo.ProcessType = ProcessType.Steam;
						}
					}

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
				int pid = Convert.ToInt32((System.UInt32) mo["ProcessId"]);
				string? commandLine = (string) mo["CommandLine"];

				ProcessInformation? pi = foundProcesses.Where(x => x.ProcessId == pid).First();
                pi.CommandLine = commandLine;

                if (pi.CommandLine.Contains("-server"))
                {
                    pi.ProcessType |= ProcessType.Server;
                }
            }

			bool found = false;

            for (int x = 0; x < foundProcesses.Count; x++)
            {
				ProcessInformation? pi = foundProcesses[x];
				string? time = (DateTime.Now - pi.Process.StartTime).ToString(@"hh\:mm\:ss");

				ComboBoxItem? cbxi = new ComboBoxItem() {
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
			{
				cbxSelector.SelectedIndex = 0;
			}
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

			ComboBoxItem? cbxi = (ComboBoxItem) cbxSelector.SelectedItem;
            if (cbxi?.Tag == null || (cbxi.Tag as ProcessInformation) == null)
			{
				return;
			}

			ProcessInformation? pi = (ProcessInformation) cbxi.Tag;


            this.SelectedProcess = pi;
        }
    }
}
