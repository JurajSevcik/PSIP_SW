using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpPcap;
using SharpPcap.LibPcap;
using PacketDotNet;
using System.Threading;
using System.Windows.Forms;


//TODO: pozrieť sa kde sa nachádza súbor do ktorého chcem zapoiovaoť 
//TODO: presuńut vystup do okna ... možnoskusiť ja pop-up

namespace My_PSIP_project
{
    internal class Libraly
    {
        public string TextToDisplay;
        protected LibPcapLiveDevice device_a;
        protected LibPcapLiveDevice device_b;

        static Form1 F = new Form1();

        private static CaptureFileWriterDevice captureFileWriter;
        public Array Devices()
        {
            var devices = CaptureDeviceList.Instance;
            string[] array = new string[5];
            foreach (var dev in devices)
                 array.Append(dev.ToString());
            //Console.WriteLine("{0}\n", dev.ToString());
            return array;
        }

        private void ChoseDevice_A()   //TODO: make dinamic 
        {
            var devices = LibPcapLiveDeviceList.Instance; //list of all devices 
            device_a = devices[8];
            int i = 0;
            foreach (var dev in devices)
            {
                Console.WriteLine("{0}) {1}", i, dev.Description);
                i++;
            }
        }

        private void ChoseDevice_B()
        {
            var devices = LibPcapLiveDeviceList.Instance; //list of all devices 
            device_b = devices[7];
        }

        public void capture()
        {
            //F.SetTextCallback("Start");

            //TextToDisplay.Invoke((MethodInvoker)(() => F.textBox1_TextChanged.Text = "My new text"));
            //F.UpdateTextBox_1_2("Start\r\n");
            //TextToDisplay += "Start";
            //string capFile = ("CaptureFile.pcap");
            ChoseDevice_A();
            ChoseDevice_B();

            // Register our handler function to the 'packet arrival' event
            device_a.OnPacketArrival +=
                new PacketArrivalEventHandler(device_OnPacketArrival_A);

            device_b.OnPacketArrival +=
                new PacketArrivalEventHandler(device_OnPacketArrival_B);

            // Open the device for capturing
            int readTimeoutMilliseconds = 1000;
            device_a.Open(mode: DeviceModes.Promiscuous | DeviceModes.DataTransferUdp | DeviceModes.NoCaptureLocal, read_timeout: readTimeoutMilliseconds);
            device_b.Open(mode: DeviceModes.Promiscuous | DeviceModes.DataTransferUdp | DeviceModes.NoCaptureLocal, read_timeout: readTimeoutMilliseconds);

            

            // Start the capturing process
            device_a.StartCapture();
            device_b.StartCapture();

            // Print out the device statistics
            //Console.WriteLine(device_a.Statistics.ToString());
        }

        public void Stop()  //Stop devices   //TODO:add exaption catcher ...if devices are offline 
        {
            F.UpdateTextBox_1("Stop");
            device_a.StopCapture();
            device_b.StopCapture();
            Console.WriteLine("A:\nARP {0}\nTCP {1}\n UDP {2}\n ICMP {3}\n HTTP {4}\n", TypARP_in_A, TypTCP_in_A, TypUDP_in_A, TypICMP_in_A, TypHTTP_in_A);
            Console.WriteLine("B:\n ARP {0}\n TCP {1}\n UDP {2}\n ICMP {3}\n HTTP {4}\n", TypARP_in_B, TypTCP_in_B, TypUDP_in_B, TypICMP_in_B, TypHTTP_in_B);
        }


        private static int packetIndex = 0;
        private static int TypARP_in_A  = 0;
        private static int TypTCP_in_A  = 0;
        private static int TypUDP_in_A = 0;
        private static int TypICMP_in_A = 0;
        private static int TypHTTP_in_A = 0;
        private static int TypHTTPS_in_A = 0;

        private static int TypARP_in_B = 0;
        private static int TypTCP_in_B = 0;
        private static int TypUDP_in_B = 0;
        private static int TypICMP_in_B = 0;
        private static int TypHTTP_in_B = 0;
        private static int TypHTTPS_in_B = 0;


        private void reset()
        {
            TypARP_in_A  = 0;
            TypTCP_in_A  = 0;
            TypUDP_in_A = 0;
            TypICMP_in_A = 0;
            TypHTTP_in_A = 0;
            TypHTTPS_in_A = 0;

            TypARP_in_B = 0;
            TypTCP_in_B = 0;
            TypUDP_in_B = 0;
            TypICMP_in_B = 0;
            TypHTTP_in_B = 0;
            TypHTTPS_in_B = 0;
        }

        /// <summary>
        /// Prints the time and length of each received packet
        /// </summary>
        private void device_OnPacketArrival_A(object sender, PacketCapture e)
        {
            //var device = (ICaptureDevice)sender;

            // write the packet to the file
            var rawPacket = e.GetPacket();
            Console.WriteLine("I got packet A");
            var packet = PacketDotNet.Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);
            device_b.Open();   //open send packet close
            try
            {
                //Send the packet out the network device
                device_b.SendPacket(rawPacket.Data);
                
                //Console.WriteLine("-- Packet sent successfuly.");
            }
            catch (Exception )
            {
                Console.WriteLine("-- error");
            }
            
            device_b.Close();
            //captureFileWriter.Write(rawPacket);
            //Console.WriteLine("Packet dumped to file.");
            //var packet = PacketDotNet.Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);
            var ethernetPacket = (EthernetPacket)packet;
            

            /*
            if (rawPacket.LinkLayerType == PacketDotNet.LinkLayers.Ethernet)
            {
                
                //
                
                string text = (packetIndex + " At: " 
                    + rawPacket.Timeval.Date.ToString() + ":"
                    + rawPacket.Timeval.Date.Millisecond + " : MAC: "
                    +  + "-> MAC: "
                    + ethernetPacket.DestinationHardwareAddress);
                F.UpdateTextBox_1(text);
                
                //int TypARP = 0;

                
                string arp = "ARP";
                if (
                
                Console.WriteLine("{0} At: {1}:{2}: MAC:{3} -> MAC:{4}",
                                  packetIndex,
                                  rawPacket.Timeval.Date.ToString(),
                                  rawPacket.Timeval.Date.Millisecond,
                                  ethernetPacket.SourceHardwareAddress,
                                  ethernetPacket.DestinationHardwareAddress);                
            }*/
            
            
            var type = rawPacket;
            Console.WriteLine(type);

            
            if (packet is ArpPacket)
            {
                TypARP_in_A++;
            }
            else if (packet is TcpPacket)
            {
                TypTCP_in_A++;
            }
            else if (packet is UdpPacket)
            {
                TypUDP_in_A++;
            }
            else if (rawPacket is IcmpV4Packet)
            {
                TypICMP_in_A++;
            }
            else if (packet is HttpStyleUriParser)
                TypHTTP_in_A++;
            
            packetIndex++;
        }

        private void device_OnPacketArrival_B(object sender, PacketCapture e)
        {
            var rawPacket = e.GetPacket();
            Console.WriteLine("I got packet B");
            var packet = PacketDotNet.Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);
            device_a.Open();   //open send packet close
            try
            {
                //Send the packet out the network device
                device_a.SendPacket(rawPacket.Data);
                Console.WriteLine("-- Packet sent successfuly.");
            }
            catch (Exception)
            {
                Console.WriteLine("-- error");
            }
            device_a.Close();

            var ethernetPacket = (EthernetPacket)packet;
            
            
            var type = ethernetPacket.Type;
            Console.WriteLine("my type is ethernet : " + type);
            if (packet is ArpPacket)
            {
                TypARP_in_B++;
            }
            else if (packet is TcpPacket)
            {
                TypTCP_in_B++;
            }
            else if (packet is UdpPacket)
            {
                TypUDP_in_B++;
            }
            else if (rawPacket is IcmpV4Packet)
            {
                TypICMP_in_B++;
            }
            else if (packet is HttpStyleUriParser)
                TypHTTP_in_B++;
            
        }

            public async void ControlWrite()
            {
            //F.textBox1_TextChanged();
                F.label1_update("fucking work");
                //F.UpdateTextBox_1_2("This is my favorit text, folow him!\n\r");
            }
        
    }
}
//prevzate z: 
//https://github.com/dotpcap/sharppcap/blob/master/Examples/CreatingCaptureFile/Program.cs --github interessting code


