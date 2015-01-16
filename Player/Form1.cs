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
using System.Xml;
using System.Xml.Serialization;
using ConfigBuilder;
using NuTrace;

namespace Player
{
    public partial class Form1 : Form
    {
        private Storage storage = null;
        private List<NuTraceVariable> unassignedVars = null;
        private List<NuTraceVariable> inputVars = null;
        private List<NuTraceVariable> outputVars = null;
        private string selectedFbKey = null;

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
                fillFbInstanceTreeView(firstState.Label);


                int statesCount = storage.States.Count;
                statesTotalTextBox.Text = statesCount.ToString();
                curStateTextBox.Text = firstState.Label;
                trackBar1.Maximum = statesCount - 1;

            }
        }

        private void fillFbInstanceTreeView(string firstStateLabel)
        {
            IEnumerable<NuTraceVariable> BasicFbOsms = storage.Variables.Where(v => v.StateLabel == firstStateLabel && v.Variable.EndsWith("S_smv"));

            foreach (NuTraceVariable osmVar in BasicFbOsms)
            {
                string[] splitStrings = osmVar.Variable.Split('.');
                if (splitStrings.Count() < 2) throw new Exception("Wrong OSM variable format: " + osmVar.Variable);

                //TreeNode curNode = new TreeNode(splitStrings[0]);
                string key = "";
                for (int i = 0; i < splitStrings.Count()-1; i++)
                {

                    TreeNode parent = null;
                    if (key != "") parent = treeView1.Nodes.Find(key, true).FirstOrDefault();
                    key += splitStrings[i] + ".";
                    TreeNode curNode = treeView1.Nodes.Find(key, true).FirstOrDefault();

                    if (curNode != null && i < splitStrings.Count() - 2) continue;
                    else if (curNode != null && i == splitStrings.Count() - 2) throw new Exception("Invalid data format!");

                    if (parent != null)
                    {
                        parent.Nodes.Add(key, splitStrings[i]);
                    }
                    else
                    {
                        if (i == 0) treeView1.Nodes.Add(key, splitStrings[i]);
                        else throw new Exception("Invalid data format!");
                    }
                }
            }
        }

        //private TreeNode createTreeNode(List<string> strings) { }

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

            writeOpcServerConfig("DANSrv.Items.xml");
            _startOpcServer();


            string server = "localhost";
            int port = 64700;
            TraceMessage(String.Format("Openning socket connection to OPC Server {0}:{1} ", server, port));

            IPHostEntry hostEntry = Dns.GetHostEntry(server); //TODO: changable address and port
            while (s == null || (!s.Connected))
            {
                foreach (IPAddress ipAddress in hostEntry.AddressList)
                {
                    IPEndPoint ep = new IPEndPoint(ipAddress, port);
                    Socket tmpSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    try
                    {
                        tmpSocket.Connect(ep);
                    }
                    catch (Exception exception)
                    {
                    }
                    if (tmpSocket.Connected)
                    {
                        s = tmpSocket;
                        TraceMessage("Connected!");
                        trackBar1.Enabled = true;
                        break;
                    }
                }
            }

            stopOpcServerButton.Enabled = true;
            startOpcServer.Enabled = false;
            /*int a = s.Send(Encoding.ASCII.GetBytes("Hello, World!"));
            s.Send(Encoding.ASCII.GetBytes("s1"));
            s.Send(Encoding.ASCII.GetBytes("s2"));
            s.Send(Encoding.ASCII.GetBytes("s3"));
            s.Send(Encoding.ASCII.GetBytes("END"));*/
        }

        private void TraceMessage(string message)
        {
            messageRichTextBox.Text += message + "\n";
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
            /*OPCServerConfig opcServerConfig = new OPCServerConfig();
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
            opcServerConfig.SocketPort = 11000;*/

            ConfigBuilder.DefinitionList serverInemsDeflist = new ConfigBuilder.DefinitionList();
            {
                serverInemsDeflist.BranchSeperatorChar = '.';
                ConfigBuilder.BranchElement root = new BranchElement();
                {
                    root.name = "CIROS Connector";
                    ConfigBuilder.ConfigDefs defaultBranchConfig = new ConfigDefs();
                    defaultBranchConfig.activeDef = true;
                    defaultBranchConfig.accRightSpecified = false;
                    defaultBranchConfig.dataTypeSpecified = false;
                    defaultBranchConfig.qualitySpecified = false;
                    defaultBranchConfig.signalTypeSpecified = false;
                    defaultBranchConfig.scanRateSpecified = false;
                    defaultBranchConfig.deviceIDSpecified = false;
                    defaultBranchConfig.deviceAddrSpecified = false;
                    defaultBranchConfig.deviceSubAddrSpecified = false;
                    defaultBranchConfig.user1Specified = false;
                    defaultBranchConfig.user2Specified = false;
                    root.branchDefs = defaultBranchConfig;

                    /*ItemElement dummyItem = new ItemElement();
                    {
                        dummyItem.name = "dummyItem";
                        dummyItem.handle = 0;
                        dummyItem.itemDefs = new ConfigDefs();
                        dummyItem.Value = Convert.ToInt32(0);
                        dummyItem.itemDefs.dataType = drvtypes.Type.SHORT;
                        dummyItem.itemDefs.activeDef = true;
                        dummyItem.itemDefs.accRightSpecified = false;

                        dummyItem.itemDefs.dataTypeSpecified = true;
                        dummyItem.itemDefs.qualitySpecified = false;
                        dummyItem.itemDefs.signalTypeSpecified = false;
                        dummyItem.itemDefs.scanRateSpecified = false;
                        dummyItem.itemDefs.deviceIDSpecified = false;
                        dummyItem.itemDefs.deviceAddrSpecified = false;
                        dummyItem.itemDefs.deviceSubAddrSpecified = false;
                        dummyItem.itemDefs.user1Specified = false;
                        dummyItem.itemDefs.user2Specified = false;
                    }
                    root.items = new ItemElement[1];
                    root.items[0] = dummyItem;*/
                    
                    if (inputVars != null)
                    {
                        root.subBranches = new BranchElement[1]; //TODO: 2
                        root.subBranches[0] = new BranchElement();
                        root.subBranches[0].name = "Inputs";
                        {
                            ConfigBuilder.ConfigDefs branchConfig = new ConfigDefs();
                            branchConfig.activeDef = true;
                            branchConfig.accRightSpecified = true;
                            branchConfig.accRight = OPCAccess.READWRITEABLE;
                            branchConfig.dataTypeSpecified = false;
                            branchConfig.qualitySpecified = true;
                            branchConfig.quality = OPCQuality.GOOD;

                            branchConfig.signalTypeSpecified = false;
                            //branchConfig.signalTypeSpecified = true;
                            //branchConfig.signalType = SignalType.INTERN;

                            branchConfig.scanRate = 100;
                            branchConfig.scanRateSpecified = true;
                            branchConfig.deviceIDSpecified = false;
                            branchConfig.deviceAddrSpecified = false;
                            branchConfig.deviceSubAddrSpecified = false;
                            branchConfig.user1Specified = false;
                            branchConfig.user2Specified = false;

                            root.subBranches[0].branchDefs = branchConfig;
                        }

                        root.subBranches[0].items = new ItemElement[inputVars.Count];
                        int i = 0;
                        foreach (NuTraceVariable inputVar in inputVars)
                        {
                            ItemElement element = new ItemElement();
                            {
                                element.name = inputVar.Variable;
                                element.handle = i+1;
                                element.itemDefs = new ConfigDefs();
                                try
                                {
                                    element.Value = Convert.ToInt32(inputVar.Value);
                                    element.itemDefs.dataType = drvtypes.Type.SHORT;
                                }
                                catch (FormatException e)
                                {
                                    element.Value = Convert.ToBoolean(inputVar.Value);
                                    element.itemDefs.dataType = drvtypes.Type.BOOLEAN;
                                }
                                element.itemDefs.activeDef = true;
                                element.itemDefs.accRightSpecified = true;
                                element.itemDefs.accRight = OPCAccess.READWRITEABLE;
                                
                                element.itemDefs.dataTypeSpecified = true;
                                element.itemDefs.qualitySpecified = true;
                                element.itemDefs.quality = OPCQuality.GOOD;

                                element.itemDefs.signalTypeSpecified = false;
                                element.itemDefs.scanRateSpecified = false;
                                element.itemDefs.deviceIDSpecified = false;
                                element.itemDefs.deviceAddrSpecified = false;
                                element.itemDefs.deviceSubAddrSpecified = false;
                                element.itemDefs.user1Specified = false;
                                element.itemDefs.user2Specified = false;

                                /*element.itemDefs.properties = new PropertyDef[1];
                                {
                                    element.itemDefs.properties[0] = new PropertyDef();
                                    element.itemDefs.properties[0].id = 101;
                                    element.itemDefs.properties[0].name = "Item Description";
                                    element.itemDefs.properties[0].dataType = drvtypes.Type.STRING;
                                    element.itemDefs.properties[0].val = "Property test";

                                }*/

                            }
                            root.subBranches[0].items[i++] = element;
                        }
                    }
                }
                serverInemsDeflist.DefinitionsRoot = root;
            }

            XmlSerializer ser = new XmlSerializer(typeof(ConfigBuilder.DefinitionList));
            StringBuilder sb = new StringBuilder();
            using (TextWriter writer = new StringWriter(sb))
            {
                ser.Serialize(writer, serverInemsDeflist);
            }

            sb.Replace("xsi:nil=\"true\" ", ""); // hack removing xsi:nil="true"

            StreamWriter sw = new StreamWriter(fileName);
            sw.Write(sb);
            //ser.Serialize(writer, serverInemsDeflist);
            sw.Close();
        }

        private void stopOpcServerButton_Click(object sender, EventArgs e)
        {
            s.Close();
            _stopOpcServer();
            stopOpcServerButton.Enabled = false;
            startOpcServer.Enabled = true;
            trackBar1.Enabled = false;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            int stateNum = trackBar1.Value;
            NuTraceState curState = storage.States[stateNum];
            foreach (NuTraceVariable inputVar in inputVars)
            {
                NuTraceVariable curStateVar = storage.Variables.FirstOrDefault(v => v.StateLabel == curState.Label && v.Variable == inputVar.Variable);
                string varString = String.Format("{0}={1}", curStateVar.Variable, curStateVar.Value);

                if (s != null && s.Connected)
                {
                    s.Send(Encoding.ASCII.GetBytes(varString));
                }
                else
                {
                    trackBar1.Enabled = false;
                    TraceMessage("Connection lost!");
                    tabPage1.Show();
                }
            }

            curStateTextBox.Text = curState.Label;
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            selectedFbKey = treeView1.SelectedNode.Name;
            refreshFbShownData();
        }

        private void refreshFbShownData()
        {
            if(selectedFbKey == null || selectedFbKey == "" || curStateTextBox.Text == "") return;

            var fbVariables = storage.Variables.Where(v => v.StateLabel == curStateTextBox.Text && v.Variable.StartsWith(selectedFbKey));
            
            NuTraceVariable osmVariable = fbVariables.First(v => v.Variable.EndsWith("S_smv"));
        }
    }
}
