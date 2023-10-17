using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterConnectionApp
{
    internal class DeviceInterface
    {
        public string Name { get; set; }
        public Device Device { get; set; }
        public NetworkSegment ConnectedNetworkSegment { get; set; }

        public DeviceInterface(string name)
        {
            Name = name;
        }

        public void Connect(NetworkSegment connectedNetworkSegment) 
        {
            ConnectedNetworkSegment = connectedNetworkSegment;
            connectedNetworkSegment.AddConnectedInterface(this);
        }
    }
}
