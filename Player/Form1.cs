using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
    }
}
