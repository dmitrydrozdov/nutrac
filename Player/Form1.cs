using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using NuTrace;

namespace Player
{
    public partial class Form1 : Form
    {
        private Storage storage = null;
        private List<NuTraceVariable> unassignedVars = null;
        private List<NuTraceVariable> inputVars = null;
        private List<NuTraceVariable> outputVars = null;

        public Form1()
        {
            InitializeComponent();
            splitContainer3.Panel2Collapsed = true;
        }

        void bindLisboxesData()
        {
            stateVarsListBox.DataSource = null;
            stateVarsListBox.DataSource = unassignedVars;
            stateVarsListBox.DisplayMember = "Variable";

            inputVarsListBox.DataSource = null;
            inputVarsListBox.DataSource = inputVars;
            inputVarsListBox.DisplayMember = "Variable";

            outputVarsListBox.DataSource = null;
            outputVarsListBox.DataSource = outputVars;
            outputVarsListBox.DisplayMember = "Variable";
        }

        private void openTraceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filename = openFileDialog1.FileName;
                TextReader inputFile = new StreamReader(filename);
                Queue<Object> parcedData = new Queue<object>();

                ParcerFSM parcer = new ParcerFSM(inputFile, parcedData);
                parcer.EnterState(new FindCounterexampleBegin(parcer));
                parcer.Run();
                inputFile.Close();

                storage = new Storage(parcedData);

                NuTraceState firstState = storage.States.First();
                unassignedVars = storage.Variables.Where(v => v.StateLabel == firstState.Label).ToList();
                inputVars = new List<NuTraceVariable>();
                outputVars = new List<NuTraceVariable>();

                bindLisboxesData();

                int statesCount = storage.States.Count;
                statesTotalTextBox.Text = statesCount.ToString();
                trackBar1.Maximum = statesCount;

            }
        }

        private void stateVarsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void setAsInputButton_Click(object sender, EventArgs e)
        {
            NuTraceVariable selectedVariable = (NuTraceVariable) stateVarsListBox.SelectedItem;
            if (selectedVariable == null) return;
            inputVars.Add(selectedVariable);
            unassignedVars.Remove(selectedVariable);
            bindLisboxesData();
        }

        private void unsetUnputButton_Click(object sender, EventArgs e)
        {
            NuTraceVariable selectedVariable = (NuTraceVariable)inputVarsListBox.SelectedItem;
            if (selectedVariable == null) return;
            unassignedVars.Add(selectedVariable);
            inputVars.Remove(selectedVariable);
            bindLisboxesData();
        }

        private void setAsOutputButton_Click(object sender, EventArgs e)
        {
            NuTraceVariable selectedVariable = (NuTraceVariable)stateVarsListBox.SelectedItem;
            if (selectedVariable == null) return;
            outputVars.Add(selectedVariable);
            unassignedVars.Remove(selectedVariable);
            bindLisboxesData();
        }

        private void unsetAsOutputButton_Click(object sender, EventArgs e)
        {
            NuTraceVariable selectedVariable = (NuTraceVariable)outputVarsListBox.SelectedItem;
            if (selectedVariable == null) return;
            unassignedVars.Add(selectedVariable);
            outputVars.Remove(selectedVariable);
            bindLisboxesData();
        }

        private void statesVisualizationPanel_Paint(object sender, PaintEventArgs e)
        {
            int offsetLeft = 10;
            int offsetRight = 5;
            int lineWidth = trackBar1.Size.Width - offsetLeft - offsetRight;

            Pen GreenPen = new Pen(Color.Green);
            Pen RedPen = new Pen(Color.Red);
            Point p1 = new Point(trackBar1.Location.X + offsetLeft, 10);
            Point p2 = new Point(trackBar1.Location.X + trackBar1.Size.Width - offsetRight, 10);
            e.Graphics.DrawLine(GreenPen, p1, p2);
        }

        private Socket s = null;
        private void startOpcServer_Click(object sender, EventArgs e)
        {
            splitContainer3.Panel2Collapsed = false;

            IPHostEntry hostEntry = Dns.GetHostEntry("localhost");
            int port = 64700;

            foreach (IPAddress ipAddress in hostEntry.AddressList)
            {
                IPEndPoint ep = new IPEndPoint(ipAddress, port);
                Socket tmpSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    tmpSocket.Connect(ep);
                }
                catch (Exception exception) { }
                if (tmpSocket.Connected)
                {
                    s = tmpSocket;
                    break;
                }
            }
            writeOpcServerConfig("opcserverconfig.xml");
            _startOpcServer();

            stopOpcServerButton.Enabled = true;
            startOpcServer.Enabled = false;
            /*int a = s.Send(Encoding.ASCII.GetBytes("Hello, World!"));
            s.Send(Encoding.ASCII.GetBytes("s1"));
            s.Send(Encoding.ASCII.GetBytes("s2"));
            s.Send(Encoding.ASCII.GetBytes("s3"));
            s.Send(Encoding.ASCII.GetBytes("END"));*/
        }

        private void runExternalTool(string execFile, string parameters)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(execFile, parameters);

            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardInput = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;

            Process proc = Process.Start(startInfo);
            StreamReader nusmvOutput = proc.StandardOutput;
            StreamReader nusmvError = proc.StandardError;
            //StreamWriter nusmvInput = proc.StandardInput;


            messageRichTextBox.Text += nusmvOutput.ReadToEnd();
            //messageRichTextBox.Text += "Error! \n";
            messageRichTextBox.Text += nusmvError.ReadToEnd();

            messageRichTextBox.SelectionStart = messageRichTextBox.Text.Length;
            messageRichTextBox.ScrollToCaret();

            //Process.Start("RegServer.exe", "DANSrvNet4.exe");
        }

        private void _startOpcServer()
        {
            runExternalTool("RegServer.exe", "DANSrvNet4.exe");
        }

        private void _stopOpcServer()
        {
            runExternalTool("UnRegServer.exe", "DANSrvNet4.exe");
        }

        private void writeOpcServerConfig(string fileName)
        {
            OPCServerConfig opcServerConfig = new OPCServerConfig();
            List<string> inputsList = new List<string>();
            List<string> outputsList = new List<string>();

            if (inputVars != null)
            {
                foreach (NuTraceVariable inputVar in inputVars)
                {
                    inputsList.Add(inputVar.Variable);
                }
            }
            if (outputVars != null)
            {
                foreach (NuTraceVariable outputVar in outputVars)
                {
                    outputsList.Add(outputVar.Variable);
                }
            }
            opcServerConfig.InputVariables = inputsList;
            opcServerConfig.OutputVariables = outputsList;
            opcServerConfig.SocketHost = "localhost";
            opcServerConfig.SocketPort = 11000;

            XmlSerializer ser = new XmlSerializer(typeof(OPCServerConfig), "");
            StreamWriter writer = new StreamWriter(fileName);
            ser.Serialize(writer, opcServerConfig);
            writer.Close();
        }

        private void stopOpcServerButton_Click(object sender, EventArgs e)
        {
            _stopOpcServer();
            stopOpcServerButton.Enabled = false;
            startOpcServer.Enabled = true;
        }
    }
}
