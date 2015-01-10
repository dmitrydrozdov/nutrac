using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Player
{
    [XmlRoot]
    public class OPCServerConfig
    {
        public List<string> InputVariables;
        public List<string> OutputVariables;
        public string SocketHost;
        public int SocketPort;
    }
}
