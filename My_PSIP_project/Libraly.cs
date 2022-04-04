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
using System.Timers;
//using System.Timers;
//TODO: kontrola ci sa packet este neodoslal -- hashocvanica tabulka :::ň
//TODO: vlasná kontrola isEuel --- na byty nefunguje .- override 

//TODO: pozrieť sa kde sa nwachádza súbor do ktorého chcem zapoiovaoť 
//TODO: presuńut vystup do okna ... možno skusiť ja pop-up


namespace My_PSIP_project
{
    internal class Libraly
    {
        
        public string TextToDisplay;
        protected LibPcapLiveDevice device_a;  //loopback devices ....
        protected LibPcapLiveDevice device_b;
        static Form1 F = new Form1();
        table_class T = new table_class();
        time_classcs TM = new time_classcs();
        private static System.Timers.Timer aTimer;


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
            //Console.WriteLine(device_a.ToString());
        }

        private void ChoseDevice_B()
        {
            var devices = LibPcapLiveDeviceList.Instance; //list of all devices 
            device_b = devices[7];
            //Console.WriteLine(device_b.ToString());
        }
        
        public static void tim()  //timer to chceck age of mac table content ....
        {
            aTimer = new System.Timers.Timer();
            aTimer.Interval = 1000;

            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;

            // Have the timer fire repeated events (true is the default)
            aTimer.AutoReset = true;

            // Start the timer
            aTimer.Enabled = true;
        }
        private static void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {

            foreach(MacZaznam tab in ST_class.table )
            {
                //TODO: intable as string and and cheange hear and back 

                if(tab.timer < 1)
                {
                    ST_class.table.Remove(tab);
                    break;
                }
                tab.timer--;
               
            }
        }
        public List<MacZaznam> capture()
        {
            tim(); //check every second and sub one second from  age of mac table row
            ChoseDevice_A();
            ChoseDevice_B();

            //TM.start_tiemer(56);  //useles
            //F.dataFridView1_update();

            //handler function to the 'packet arrival' event
            device_a.OnPacketArrival += new PacketArrivalEventHandler(GottaCatchEmAll);
            device_b.OnPacketArrival += new PacketArrivalEventHandler(GottaCatchEmAll);
            
            // Open device 
            int readTimeoutMilliseconds = 1000;
            device_a.Open(mode: DeviceModes.Promiscuous | DeviceModes.DataTransferUdp | DeviceModes.NoCaptureLocal | DeviceModes.NoCaptureRemote, read_timeout: readTimeoutMilliseconds);
            device_b.Open(mode: DeviceModes.Promiscuous | DeviceModes.DataTransferUdp | DeviceModes.NoCaptureLocal | DeviceModes.NoCaptureRemote, read_timeout: readTimeoutMilliseconds);

            // Start capturing 
            device_a.StartCapture();
            device_b.StartCapture();

            return ST_class.table;
        }

        public void Stop()  //Stop devices   //TODO:add exaption catcher ...if devices are offline 
        {
            F.UpdateTextBox_1("Stop");
            device_a.StopCapture();
            device_b.StopCapture();

            Console.WriteLine("IN\nA:\n ARP {0}\n TCP {1}\n UDP {2}\n ICMP {3}\n HTTP {4}\n",
                ST_class.ARP_in_A, ST_class.TCP_in_A, ST_class.UDP_in_A, ST_class.ICMP_in_A, ST_class.HTTP_in_A);
            Console.WriteLine("B:\n ARP {0}\n TCP {1}\n UDP {2}\n ICMP {3}\n HTTP {4}\n", 
                ST_class.ARP_in_B, ST_class.TCP_in_B, ST_class.UDP_in_B, ST_class.ICMP_in_B, ST_class.HTTP_in_B);

            Console.WriteLine("OUT\nA:\n ARP {0}\n TCP {1}\n UDP {2}\n ICMP {3}\n HTTP {4}\n",
                ST_class.ARP_out_A, ST_class.TCP_out_A, ST_class.UDP_out_A, ST_class.ICMP_out_A, ST_class.HTTP_out_A);
            Console.WriteLine("B:\n ARP {0}\n TCP {1}\n UDP {2}\n ICMP {3}\n HTTP {4}\n",
                ST_class.ARP_out_B, ST_class.TCP_out_B, ST_class.UDP_out_B, ST_class.ICMP_out_B, ST_class.HTTP_out_B);
        }

        //useless
        private static int packetIndex = 0;

        private void GottaCatchEmAll(object sender, PacketCapture e)
        {
            PacketSender S = new PacketSender();
            var rawPacket = e.GetPacket();         //zachytenie packetu
            if (sender == device_a)
            {
                device_OnPacketArrival_A(rawPacket);
            }
            if(sender == device_b)
            {
                device_OnPacketArrival_B(rawPacket);
            }

        }

        private void device_OnPacketArrival_A(RawCapture rawPacket)
        {
           
            //TODO: move to sendnder class 
            //TODO: add calss for sattictics
            //TODO: move after statiscick
            PacketSender S = new PacketSender();
            var rrt = rawPacket.Data;
            var packet = PacketDotNet.Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);
            var rrte = packet.Bytes;
            var wwq = packet.GetHashCode();


            
            //filtrovanie packetov len na tiek ktore ma zaujimaju 
            var ethernetPacket = (EthernetPacket)packet;
            List<string> MacDev = new List<String> { "005079666800", "005079666801", "005079666802" };
            string MyMac = (ethernetPacket.SourceHardwareAddress).ToString();
            if (MacDev[0] == MyMac || MacDev[1] == MyMac || MacDev[2] == MyMac)
            {

            }
            else
            {
                return;
            }
            //var time = e.Header.Timeval.Date;
            //ethernetPacket.PrintHex();

            string t = ethernetPacket.PrintHex().ToString();
            ST_class.watch.Add(t); //cheack if it isnt same packet
            if (ST_class.circle(packet)) //it's samo one again 
            {
                return;
            }    


            T.GiveMeMyPacket( rawPacket, 'A'); //chceck mac address table and add or cheange log int there  ;
            Console.WriteLine("Posielam z A");
            S.send(device_a, device_b, rawPacket, 'A');   //odoslanie packetu 
            //device_b.Close();
            //var ethernetPacket = (EthernetPacket)packet;
            
            var type = rawPacket;
            var tempPacket = PacketDotNet.Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);
            while (tempPacket.PayloadPacket != null)
            {
                tempPacket = tempPacket.PayloadPacket;
            }

            if (tempPacket is PacketDotNet.ArpPacket)
            {
                ST_class.ARP_in_A++;
            }
            else if (tempPacket is PacketDotNet.TcpPacket)
            {
                ST_class.TCP_in_A++;
            }
            else if (tempPacket is PacketDotNet.UdpPacket)
            {
                ST_class.UDP_in_A++;
            }
            else if (tempPacket is PacketDotNet.IcmpV4Packet)
            {
                ST_class.ICMP_in_A++;
            }
            packetIndex++;
            F.DGW();
        }

        public void show_table()
        {
            //MacZaznam rec = new MacZaznam() { mac_addres = "MAC", M_interface = 'Q' };
            //table.Add(rec);
            //F.DGW(table);
            int i = 0;
            
            Console.WriteLine("My MAC table:");
            F.dataFridView1_update();

            foreach(MacZaznam zaznam in ST_class.table)
            {
                Console.WriteLine("{0}: MAC: {1}, interface:{2}, timer: {3}", i, (zaznam.mac_addres), (zaznam.M_interface), zaznam.timer.ToString());
                i++;
            }
        }

        private void device_OnPacketArrival_B(RawCapture rawPacket)
        {            
            var packet = PacketDotNet.Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);
            PacketSender S = new PacketSender();
            var ethernetPacket = (EthernetPacket)packet;
            
            List<string> MacDev = new List<String> { "005079666800", "005079666801", "005079666802" };
            string MyMac = (ethernetPacket.SourceHardwareAddress).ToString();
            
                if(MacDev[0] == MyMac || MacDev[1] == MyMac || MacDev[2] == MyMac)
                {
                    
                }
                else
                {
                    return;
                }
            
            
            if (ST_class.circle(packet)) //it's samo one again 
            {
                return;
            }
            //Console.WriteLine(packet.Ethernet.IpV4.Protocol.ToString());
            T.GiveMeMyPacket( rawPacket, 'B'); //chceck mac address table and add or cheange log int there  ;
            Console.WriteLine("Posoelam z B");
            Console.WriteLine("Packet dumped to file.");

            if (rawPacket.LinkLayerType == PacketDotNet.LinkLayers.Ethernet)
            {
                Console.WriteLine("{0} At: {1}:{2}: MAC:{3} -> MAC:{4}",
                                  packetIndex,
                                  rawPacket.Timeval.Date.ToString(),
                                  rawPacket.Timeval.Date.Millisecond,
                                  ethernetPacket.SourceHardwareAddress,
                                  ethernetPacket.DestinationHardwareAddress);
                packetIndex++;
                S.send(device_a, device_b, rawPacket, 'B');
            }
            //device_a.Close();

            //var ethernetPacket = (EthernetPacket)packet;
            //var tcp = packet.PayloadPacket.PayloadPacket.PayloadData;
            //var type = ethernetPacket.Type;

            //Console.WriteLine("Toto je moj typ pre B : " + tcp);
            var pppc = packet.ToString();

            var tempPacket = PacketDotNet.Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);
            while (tempPacket.PayloadPacket != null)
            {
                tempPacket = tempPacket.PayloadPacket;
            }

            if (tempPacket is PacketDotNet.ArpPacket)
            {
                ST_class.ARP_in_B++;
                
            }
            else if (tempPacket is PacketDotNet.TcpPacket)
            {
                ST_class.TCP_in_B++;
                
            }
            else if (tempPacket is PacketDotNet.UdpPacket)
            {
                ST_class.UDP_in_B++;
                
            }
            else if (tempPacket is PacketDotNet.IcmpV4Packet)
            {
                ST_class.ICMP_in_B++;
                
            }

            F.DGW();
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


