using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterConnectionApp
{
    internal class NetworkSegment
    {
        public string Name { get; set; }
        List<DeviceInterface> ConnectedInterfaces { get; set; }

        public NetworkSegment(string name)
        {
            Name = name;
            ConnectedInterfaces = new List<DeviceInterface>();
        }

        public void AddConnectedInterface(DeviceInterface itf)
        {
            ConnectedInterfaces.Add(itf);
        }
    }
}
