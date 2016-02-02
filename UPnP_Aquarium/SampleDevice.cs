// UPnP .NET Framework Device Stack, Device Module
// Device Builder Build#1.0.5329.22110

using System;
using OpenSource.UPnP;
using Aquarium;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Aquarium
{
    /// <summary>
    /// Summary description for SampleDevice.
    /// </summary>
    class SampleDevice
    {
        private UPnPDevice device;

        public SampleDevice()
        {
            device = UPnPDevice.CreateRootDevice(1800, 1.0, "\\");

            device.FriendlyName = "Aquarium";
            device.Manufacturer = "Aquarium";
            device.ManufacturerURL = "http://polytech.unice.fr";
            device.ModelName = "Aquarium";
            device.ModelDescription = "Aquarium";
            device.ModelNumber = "AQUA1";
            device.HasPresentation = false;
            device.DeviceURN = "urn:schemas-upnp-org:device:Aquarium:1";
            Aquarium.DvaquariumService aquariumService = new Aquarium.DvaquariumService();
            aquariumService.External_getBrightness = new Aquarium.DvaquariumService.Delegate_getBrightness(aquariumService_getBrightness);
            aquariumService.External_getTemperature = new Aquarium.DvaquariumService.Delegate_getTemperature(aquariumService_getTemperature);
            aquariumService.External_setBrightness = new Aquarium.DvaquariumService.Delegate_setBrightness(aquariumService_setBrightness);
            aquariumService.External_setTemperature = new Aquarium.DvaquariumService.Delegate_setTemperature(aquariumService_setTemperature);
            device.AddService(aquariumService);

            // Setting the initial value of evented variables
            aquariumService.Evented_brightness = 0;
            aquariumService.Evented_temperature = 0;
        }

        public void Start()
        {
            device.StartDevice();
        }

        public void Stop()
        {
            device.StopDevice();
        }

        public System.Int32 aquariumService_getBrightness()
        {
            Console.WriteLine("[UPnP] Get Brightness");
            var client = new UdpClient();
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5000);
            client.Connect(ep);

            // send data
            Byte[] data = Encoding.ASCII.GetBytes("getLight#0");
            client.Send(data, data.Length);

            var receivedData = client.Receive(ref ep);
            String value = Encoding.ASCII.GetString(receivedData);
            Console.Write("[UPnP] Get Brightness | Value : " + value);

            String s = value.Split('#')[1];
            System.Int32 result = 0;
            try
            {
                float o = float.Parse(s);
                result = Convert.ToInt32(o);
            }
            catch (Exception)
            {
            }


            client.Close();

            return result;
        }

        public System.Int32 aquariumService_getTemperature()
        {
            Console.WriteLine("[UPnP] Get Temperature");
            var client = new UdpClient();
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5000);
            client.Connect(ep);

            // send data
            Byte[] data = Encoding.ASCII.GetBytes("getTemperature#0");
            client.Send(data, data.Length);

            var receivedData = client.Receive(ref ep);
            String value = Encoding.ASCII.GetString(receivedData);
            Console.Write("[UPnP] Get Temperature | Value : " + value);

            String s = value.Split('#')[1];
            System.Int32 result = 0;
            try
            {
                float o = float.Parse(s);
                result = Convert.ToInt32(o);
            }
            catch (Exception)
            {
            }

            client.Close();

            return result;
        }

        public void aquariumService_setBrightness(System.Int32 inputBrightness)
        {
            Console.WriteLine("[UPnP] Set Brightness");
            var client = new UdpClient();
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5000);
            client.Connect(ep);
            Byte[] data = Encoding.ASCII.GetBytes("setUPnPLight#" + inputBrightness);
            client.Send(data, data.Length);
        }

        public void aquariumService_setTemperature(System.Int32 inputTemperature)
        {
            Console.WriteLine("[UPnP] Set Temperature");
            var client = new UdpClient();
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5000);
            client.Connect(ep);
            Byte[] data = Encoding.ASCII.GetBytes("setUPnPTemperature#" + inputTemperature);
            client.Send(data, data.Length);
        }

    }
}

