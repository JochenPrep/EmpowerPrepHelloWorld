using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterConnectionApp
{
    internal class Network
    {
        private Dictionary<string, NetworkSegment> _networkSegments;
        private Dictionary<string, Device> _devices;

        public Network()
        {
            _networkSegments = new Dictionary<string, NetworkSegment>();
            _devices = new Dictionary<string, Device>();
        }

        public void AddNetworkSegment(NetworkSegment networkSegment)
        {
            if(!_networkSegments.ContainsKey(networkSegment.Name))
                _networkSegments.Add(networkSegment.Name, networkSegment);
        }

        public void AddDevice(Device device)
        {
            if (!_devices.ContainsKey(device.Name))
                _devices.Add(device.Name, device);
        }

        public void ConnectDeviceInterfacetoNetworkSegment(string deviceName, string interfaceName, string networkSegmentName)
        {
            if (!_devices.ContainsKey(deviceName))
                throw new InvalidOperationException($"Device '{deviceName}' not found");

            if (!_devices[deviceName].HasInterface(interfaceName))
                throw new InvalidOperationException($"Interface '{interfaceName}' not found on device '{deviceName}'");

            if (!_networkSegments.ContainsKey(networkSegmentName))
                throw new InvalidOperationException($"Network segment '{networkSegmentName}' not found");

            _devices[deviceName].GetInterface(interfaceName).Connect(_networkSegments[networkSegmentName]);

        }

        public string CalculateRoute(string fromDevice, string fromInterface, string toDevice, string toInterface)
        {

            return null;
        }
    }
}
