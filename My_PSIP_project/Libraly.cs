﻿using System;
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
public class MacZaznam
{
    public string mac_addres{ get; set; } //wher packet is from -- and wher i will send next one to this destination
    public char M_interface { get; set; }//on whitch interface did i get packet   
    public string destination { get; set; }
}




namespace My_PSIP_project
{
    internal class Libraly
    {
        public static List<MacZaznam> table = new List<MacZaznam>();
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
            device_b = devices[7];
        }

        public List<MacZaznam> capture()
        {
           
            ChoseDevice_A();
            ChoseDevice_B();

            //F.dataFridView1_update();

            //handler function to the 'packet arrival' event
            device_a.OnPacketArrival += new PacketArrivalEventHandler(device_OnPacketArrival_A);
            device_b.OnPacketArrival += new PacketArrivalEventHandler(device_OnPacketArrival_B);

            // Open device 
            int readTimeoutMilliseconds = 1000;
            device_a.Open(mode: DeviceModes.Promiscuous | DeviceModes.DataTransferUdp | DeviceModes.NoCaptureLocal, read_timeout: readTimeoutMilliseconds);
            device_b.Open(mode: DeviceModes.Promiscuous | DeviceModes.DataTransferUdp | DeviceModes.NoCaptureLocal, read_timeout: readTimeoutMilliseconds);

            // Start capturing 
            device_a.StartCapture();
            device_b.StartCapture();

            return table;
        }

        public void Stop()  //Stop devices   //TODO:add exaption catcher ...if devices are offline 
        {
            F.UpdateTextBox_1("Stop");
            device_a.StopCapture();
            device_b.StopCapture();
            Console.WriteLine("A:\n ARP {0}\n TCP {1}\n UDP {2}\n ICMP {3}\n HTTP {4}\n", ARP_in_A, TCP_in_A, UDP_in_A, ICMP_in_A, HTTP_in_A);
            Console.WriteLine("B:\n ARP {0}\n TCP {1}\n UDP {2}\n ICMP {3}\n HTTP {4}\n", ARP_in_B, TCP_in_B, UDP_in_B, ICMP_in_B, HTTP_in_B);
        }


        private static int packetIndex = 0;
        private static int EthernetII_in_A = 0;
        private static int ARP_in_A  = 0;
        private static int TCP_in_A  = 0;
        private static int UDP_in_A = 0;
        private static int ICMP_in_A = 0;
        private static int HTTP_in_A = 0;
        private static int HTTPS_in_A = 0;

        private static int EthernetII_in_B = 0;
        private static int ARP_in_B = 0;
        private static int TCP_in_B = 0;
        private static int UDP_in_B = 0;
        private static int ICMP_in_B = 0;
        private static int HTTP_in_B = 0;
        private static int HTTPS_in_B = 0;


        private void reset()
        {
            ARP_in_A  = 0;
            TCP_in_A  = 0;
            UDP_in_A = 0;
            ICMP_in_A = 0;
            HTTP_in_A = 0;
            HTTPS_in_A = 0;

            ARP_in_B = 0;
            TCP_in_B = 0;
            UDP_in_B = 0;
            ICMP_in_B = 0;
            HTTP_in_B = 0;
            HTTPS_in_B = 0;
        }

        private void device_OnPacketArrival_A(object sender, PacketCapture e)
        {
            PacketSender S = new PacketSender();
            var rawPacket = e.GetPacket();            
            Console.WriteLine("I got packet A");
            var packet = PacketDotNet.Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);

            //TODO: move to sendnder class 
            //TODO: add calss for sattictics
            //TODO: move after statiscick
            
            T.GiveMeMyPacket(table, rawPacket, 'A'); //chceck mac address table and add or cheange log int there  ;
            S.send(device_a, device_b,   rawPacket, 'A', table);

            device_b.Close();
            var ethernetPacket = (EthernetPacket)packet;
            var type = rawPacket;
            Console.WriteLine("Moj typ je nieco ako neviem co:" + type);

            if (packet is ArpPacket)
            {
                ARP_in_A++;
                F.Label_A_ARP_update(ARP_in_A);
            }
            else if (packet is TcpPacket)
            {
                TCP_in_A++;
                F.Label_A_TCP_update(TCP_in_A);
            }
            else if (packet is UdpPacket)
            {
                UDP_in_A++;
                F.Label_A_UDP_update(UDP_in_A);
            }
            else if (rawPacket is IcmpV4Packet)
            {
                ICMP_in_A++;
                F.Label_A_ICMP_update(ICMP_in_A);
            }
            else if (packet is HttpStyleUriParser)
                HTTP_in_A++;
                F.Label_A_HTTP_update(HTTP_in_A);

            packetIndex++;
            F.dataFridView1_update();
        }

        public void show_table()
        {
            MacZaznam rec = new MacZaznam() { destination = "empty", mac_addres = "MAC", M_interface = 'Q' };
            //table.Add(rec);
            //F.DGW(table);
            int i = 0;
            Console.WriteLine("My MAC table:");
            //F.dataFridView1_update();
            foreach(MacZaznam zaznam in Libraly.table)
            {
                Console.WriteLine("{0}: MAC: {1}, des:{2}, interface:{3}", i, (zaznam.mac_addres), (zaznam.destination), (zaznam.M_interface));
                i++;
            }
        }

        private void device_OnPacketArrival_B(object sender, PacketCapture e)
        {
            PacketSender S = new PacketSender();
            var rawPacket = e.GetPacket();
            Console.WriteLine("I got packet B");
            var packet = PacketDotNet.Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);
            int readTimeoutMilliseconds = 1000;

            T.GiveMeMyPacket(table, rawPacket, 'B'); //chceck mac address table and add or cheange log int there  ;
            S.send(device_a, device_b, rawPacket, 'B', table);
            var ethernetPacket = (EthernetPacket)packet;
            
            var type = ethernetPacket.Type;
            Console.WriteLine("Toto je moj typ pre B : " + type);
            if (packet is ArpPacket)
            {
                ARP_in_B++;
                F.Label_B_ARP_update(ARP_in_B);
            }
            else if (packet is TcpPacket)
            {
                TCP_in_B++;
                F.Label_B_TCP_update(TCP_in_B);
            }
            else if (packet is UdpPacket)
            {
                UDP_in_B++;
                F.Label_B_UDP_update(UDP_in_B);
            }
            else if (rawPacket is IcmpV4Packet)
            {
                ICMP_in_B++;
                F.Label_B_ICMP_update(ICMP_in_B);
            }
            else if (packet is HttpStyleUriParser)
                HTTP_in_B++;
                F.Label_B_HTTP_update(HTTP_in_B);
            F.dataFridView1_update();
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


