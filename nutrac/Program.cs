using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NuTrace;

namespace nutrac
{
    class Program
    {
        private static string Separator = ";";

        static void Main(string[] args)
        {
            string inputFileName = args[0];
            string outputFileName = Path.ChangeExtension(inputFileName, "csv");
            TextReader inputFile = new StreamReader(inputFileName);
            Queue<Object> parcedData = new Queue<object>();

            ParcerFSM parcer = new ParcerFSM(inputFile, parcedData);
            parcer.EnterState(new FindCounterexampleBegin(parcer));
            parcer.Run();
            inputFile.Close();

            Storage storage = new Storage(parcedData);


            Dictionary<string, List<string>> varsDictionary = new Dictionary<string, List<string>>();
            int statesCounter = 0;
            foreach (NuTraceState state in storage.States)
            {
                statesCounter++;

                IEnumerable<NuTraceVariable> stateVars = storage.Variables.Where(v => v.StateLabel == state.Label);
                //Changed variables
                foreach (NuTraceVariable stateVar in stateVars)
                {
                    try
                    {
                        List<string> curVariableValues = varsDictionary[stateVar.Variable];
                        curVariableValues.Add(stateVar.Value);
                    }
                    catch (KeyNotFoundException e)
                    {
                        List<string> curVariableValues = new List<string>();
                        curVariableValues.Add(stateVar.Value);
                        varsDictionary.Add(stateVar.Variable, curVariableValues);
                    }
                }
                //non-changed variables
                foreach (KeyValuePair<string, List<string>> keyValuePair in varsDictionary.Where(kvp => kvp.Value.Count < statesCounter))
                {
                    string lastValue = keyValuePair.Value.Last();
                    keyValuePair.Value.Add(lastValue);
                }
            }

            WriteOutputFile(outputFileName, statesCounter, varsDictionary, storage.States);
        }

        static void WriteOutputFile(string outputFileName, int statesCount, Dictionary<string, List<string>> varsDictionary, IEnumerable<NuTraceState> states)
        {
            StreamWriter wr = new StreamWriter(outputFileName);
            string firstLine = "";
            foreach (NuTraceState state in states)
            {
                firstLine += Separator + "-" + state.Label;
                if ((state.Loop == ELoopState.LoopBegin || state.Loop == ELoopState.Loop)) firstLine += " L";
            }
            wr.WriteLine(firstLine);
            foreach (KeyValuePair<string, List<string>> keyValuePair in varsDictionary)
            {
                string writeString = keyValuePair.Key;
                foreach (string value in keyValuePair.Value)
                {
                    writeString += Separator + value;
                }
                wr.WriteLine(writeString);
            }
            wr.Close();
        }
    }
}
