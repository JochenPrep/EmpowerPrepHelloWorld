using InterConnectionApp;

Network network = CreateTestSetup();
Console.WriteLine("Network ready");
Console.WriteLine("Calculated route");
Console.WriteLine("Result:"+network.CalculateRoute("UK X20 1", "dualIP", "DE DEC 1", "SDI Out"));

Console.Read();

static Network CreateTestSetup()
{
    Network network = new Network();

    network.AddNetworkSegment(new NetworkSegment("UK BCN"));
    network.AddNetworkSegment(new NetworkSegment("IT BCN"));
    network.AddNetworkSegment(new NetworkSegment("IT RF"));
    network.AddNetworkSegment(new NetworkSegment("DE BCN"));
    network.AddNetworkSegment(new NetworkSegment("DE SDI"));
    network.AddNetworkSegment(new NetworkSegment("DVN"));

    network.AddDevice(new Device("UK X20 1", new List<DeviceInterface> { new DeviceInterface("dualIP"), new DeviceInterface("IPSwitch") }));
    network.AddDevice(new Device("IT X20 1", new List<DeviceInterface> { new DeviceInterface("dualIP"), new DeviceInterface("IPSwitch") }));
    network.AddDevice(new Device("DE X20 1", new List<DeviceInterface> { new DeviceInterface("dualIP"), new DeviceInterface("IPSwitch") }));
    network.AddDevice(new Device("DE DEC 1", new List<DeviceInterface> { new DeviceInterface("IP In"), new DeviceInterface("SDI Out") }));
    network.AddDevice(new Device("DE DEC 2", new List<DeviceInterface> { new DeviceInterface("IP In"), new DeviceInterface("SDI Out") }));
    network.AddDevice(new Device("IT DEMOD 1", new List<DeviceInterface> { new DeviceInterface("RF In"), new DeviceInterface("IP Out") }));

    network.ConnectDeviceInterfacetoNetworkSegment("UK X20 1", "dualIP", "UK BCN");
    network.ConnectDeviceInterfacetoNetworkSegment("UK X20 1", "IPSwitch", "DVN");
    network.ConnectDeviceInterfacetoNetworkSegment("IT X20 1", "dualIP", "IT BCN");
    network.ConnectDeviceInterfacetoNetworkSegment("IT X20 1", "IPSwitch", "DVN");
    network.ConnectDeviceInterfacetoNetworkSegment("DE X20 1", "dualIP", "DE BCN");
    network.ConnectDeviceInterfacetoNetworkSegment("DE X20 1", "IPSwitch", "DVN");
    network.ConnectDeviceInterfacetoNetworkSegment("DE DEC 1", "IP In", "DE BCN");
    network.ConnectDeviceInterfacetoNetworkSegment("DE DEC 1", "SDI Out", "DE SDI");
    network.ConnectDeviceInterfacetoNetworkSegment("DE DEC 2", "IP In", "DE BCN");
    network.ConnectDeviceInterfacetoNetworkSegment("DE DEC 2", "SDI Out", "DE SDI");
    network.ConnectDeviceInterfacetoNetworkSegment("IT DEMOD 1", "RF In", "IT RF");
    network.ConnectDeviceInterfacetoNetworkSegment("IT DEMOD 1", "IP Out", "IT BCN");

    return network;
}