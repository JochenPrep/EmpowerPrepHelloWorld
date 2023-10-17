using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterConnectionApp
{
    internal class Device
    {
        public string Name { get; set; }
        List<DeviceInterface> Interfaces {  get; set; }

        public Device(string name, List<DeviceInterface> interfaces) 
        {
            Name = name;
            Interfaces = interfaces;
            foreach (var itf in Interfaces)
            {
                itf.Device = this;
            }
        }
        public DeviceInterface GetInterface(string itfName)
        {
            return Interfaces.First(itf => itf.Name.Equals(itfName));
        }

        public bool HasInterface(string itfName)
        {
            return Interfaces.Any(itf => itf.Name.Equals(itfName));
        }
    }
}
