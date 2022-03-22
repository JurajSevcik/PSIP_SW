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


//TODO: pozrieť sa kde sa nwachádza súbor do ktorého chcem zapoiovaoť 
//TODO: presuńut vystup do okna ... možno skusiť ja pop-up
class MacZaznam
{
    public string mac_addres = "empty"; //wher packet is from -- and wher i will send next one to this destination
    public char M_interface = 'X'; //on whitch interface did i get packet   
    public string destination = "empty";
    
    /*
    MacZaznam(string x, char y, string z)
    {
        this.mac_addres = x;
        M_interface = y;
        destination = z;
    }*/

}




namespace My_PSIP_project
{
    internal class Libraly
    {
        public MacZaznam[] table = new MacZaznam[25];

        //public MacZaznam[] table = new MacZaznam[25];
        public string TextToDisplay;
        protected internal LibPcapLiveDevice device_a;  //loopback devices ....
        protected internal LibPcapLiveDevice device_b;
        static Form1 F = new Form1();
        table_class T = new table_class();

        public Array Devices()
        {
            var devices = CaptureDeviceList.Instance;
            string[] array = new string[5];
            foreach (var dev in devices)
                 array.Append(dev.ToString());
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
            device_b = devices[9];
        }

        public void capture()
        {
            ChoseDevice_A();
            ChoseDevice_B();

            //handler function to the 'packet arrival' event
            device_a.OnPacketArrival +=
                new PacketArrivalEventHandler(device_OnPacketArrival_A);

            device_b.OnPacketArrival +=
                new PacketArrivalEventHandler(device_OnPacketArrival_B);

            // Open device 
            int readTimeoutMilliseconds = 1000;
            device_a.Open(mode: DeviceModes.Promiscuous | DeviceModes.DataTransferUdp | DeviceModes.NoCaptureLocal, read_timeout: readTimeoutMilliseconds);
            device_b.Open(mode: DeviceModes.Promiscuous | DeviceModes.DataTransferUdp | DeviceModes.NoCaptureLocal, read_timeout: readTimeoutMilliseconds);

            // Start capturing 
            device_a.StartCapture();
            device_b.StartCapture();
        }

        public void Stop()  //Stop devices   //TODO:add exaption catcher ...if devices are offline 
        {
            F.UpdateTextBox_1("Stop");
            device_a.StopCapture();
            device_b.StopCapture();
            Console.WriteLine("A:\n ARP {0}\n TCP {1}\n UDP {2}\n ICMP {3}\n HTTP {4}\n", TypARP_in_A, TypTCP_in_A, TypUDP_in_A, TypICMP_in_A, TypHTTP_in_A);
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

        private void device_OnPacketArrival_A(object sender, PacketCapture e)
        {
            
            PacketSender S = new PacketSender();
            
            //var device = (ICaptureDevice)sender;

            // write the packet to the file
            var rawPacket = e.GetPacket();
            //add MAC to table (source)
            
            
            Console.WriteLine("I got packet A");
            var packet = PacketDotNet.Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);

            //TODO: move to sendnder class 
            //TODO: add calss for sattictics
            //TODO: move after statiscick
            
            T.GiveMeMyPacket(table, rawPacket, 'A'); //chceck mac address table and add or cheange log int there  ;
            S.send(rawPacket, 'A', table);

            device_b.Open(mode: DeviceModes.Promiscuous | DeviceModes.DataTransferUdp | DeviceModes.NoCaptureLocal, read_timeout: 1000);   //open send packet close
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
            
            var type = rawPacket;
            Console.WriteLine("Moj typ je nieco ako neviem co:" + type);

            if (packet is ArpPacket)
            {
                TypARP_in_A++;
                F.Label_A_ARP_update(TypARP_in_A);
            }
            else if (packet is TcpPacket)
            {
                TypTCP_in_A++;
                F.Label_A_TCP_update(TypTCP_in_A);
            }
            else if (packet is UdpPacket)
            {
                TypUDP_in_A++;
                F.Label_A_UDP_update(TypUDP_in_A);
            }
            else if (rawPacket is IcmpV4Packet)
            {
                TypICMP_in_A++;
                F.Label_A_ICMP_update(TypICMP_in_A);
            }
            else if (packet is HttpStyleUriParser)
                TypHTTP_in_A++;
                F.Label_A_HTTP_update(TypHTTP_in_A);

            packetIndex++;
        }

        public void show_table()
        {
            //for (int i = 0; i <= size; i++)
            int i = 0;
            Console.WriteLine("My MAC table:");
            while (table[i].mac_addres != "empty")
            {
                Console.WriteLine("{0}:\n MAC: {1}, des:{2}, interface:{3}", i, (table[i].mac_addres), (table[i].destination), (table[i].M_interface));
                i++;
            }
            if (i == 0)
            {
                Console.WriteLine("empty");
            }
        }

        private void device_OnPacketArrival_B(object sender, PacketCapture e)
        {
            var rawPacket = e.GetPacket();
            Console.WriteLine("I got packet B");
            var packet = PacketDotNet.Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);
            int readTimeoutMilliseconds = 1000;
            device_a.Open(mode: DeviceModes.Promiscuous | DeviceModes.DataTransferUdp | DeviceModes.NoCaptureLocal);   //open send packet close
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
            Console.WriteLine("Toto je moj typ pre B : " + type);
            if (packet is ArpPacket)
            {
                TypARP_in_B++;
                F.Label_B_ARP_update(TypARP_in_B);
            }
            else if (packet is TcpPacket)
            {
                TypTCP_in_B++;
                F.Label_B_TCP_update(TypTCP_in_B);
            }
            else if (packet is UdpPacket)
            {
                TypUDP_in_B++;
                F.Label_B_UDP_update(TypUDP_in_B);
            }
            else if (rawPacket is IcmpV4Packet)
            {
                TypICMP_in_B++;
                F.Label_B_ICMP_update(TypICMP_in_B);
            }
            else if (packet is HttpStyleUriParser)
                TypHTTP_in_B++;
                F.Label_B_HTTP_update(TypHTTP_in_B);

        }

        public async void ControlWrite()
        {
            Console.WriteLine("I am working and i love it ");
        }

        private static byte[] GetRandomPacket()
        {
            byte[] packet = new byte[200];
            Random rand = new Random();
            rand.NextBytes(packet);
            return packet;
        }

    }
}
//prevzate z: 
//https://github.com/dotpcap/sharppcap/blob/master/Examples/CreatingCaptureFile/Program.cs --github interessting code


