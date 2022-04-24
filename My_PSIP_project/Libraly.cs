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
        //ako dlho musí byť port neaktívny aby sa jeho tabulka vymazala  
        private static int TimeOut = 10;
        private static int cabel_a = TimeOut;
        private static int cabel_b = TimeOut;
        private static int repete = 10;
        public static int timer = 20;
        private static int a = 7; //index of devices that will be used (list of devices in consoe after start )
        private static int b = 8;
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
            device_a = devices[a];
            
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
            device_b = devices[b];
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
            
            repete --;
            if (repete ==0 )
            {
                //ST_class.b.Clear();
                repete = 5;
            }
            foreach(MacZaznam tab in ST_class.table )
            {
                //TODO: intable as string and and cheange hear and back 

                if(tab.timer < 1)
                {
                    ST_class.table.Remove(tab);
                    ST_class.b.Clear();
                    break;
                }
                tab.timer--;
            }
            

            if (cabel_a < 1) // if cabel dead ... remove 
            {
                string var = "No trafic on port A";
                syslog.CreateSyslog(var, 3, "Libraly/On*TimeEvent");
                ST_class.rm_port('A');
                ST_class.b.Clear();
            }
            if (cabel_b < 1)
            {
                string var = "No trafic on port B";
                syslog.CreateSyslog(var, 3, "Libraly/On*TimeEvent");
                ST_class.rm_port('B');
                ST_class.b.Clear();
            }
            cabel_a--;
            cabel_b--;

            F.update_text_stat(); //update satistic every second
            F.DGW();
            Application.DoEvents();
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
            string var = "Switch was rurnd off ";
            syslog.CreateSyslog(var, 2, "Libraly/Stop");
            F.UpdateTextBox_1("Stop");
            device_a.StopCapture();
            device_b.StopCapture();

        }

        //useless
        private static int packetIndex = 0;

        private void GottaCatchEmAll(object sender, PacketCapture e)
        {
            //syslog syslog = new syslog();// TODO : remove after testig
            string var = "toto je moj testvaci vypis";
            device_a.SendPacket(syslog.CreateSyslog(var, 2, "Libraly/GottaCatchEmAll"));

            var rawPacket = e.GetPacket();         //zachytenie packetu
            if (sender == device_a)
            {
                cabel_a = TimeOut; // interface is UP 

                filtracia fi = new filtracia();
                fi.filtruj_in(rawPacket, "A");

                device_OnPacketArrival_A(rawPacket);
                rawPacket = null;
            }
            if(sender == device_b)
            {
                filtracia fi = new filtracia();
                fi.filtruj_in(rawPacket, "B");

                cabel_b = TimeOut; // interface is UP 
                device_OnPacketArrival_B(rawPacket);
                rawPacket = null;
            }
        }

        private void device_OnPacketArrival_A(RawCapture rawPacket)
        {
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

            string t = ethernetPacket.PrintHex().ToString();
            ST_class.watch.Add(t); //cheack if it isnt same packet
            if (ST_class.circle(packet)) //it's samo one again 
            {
                Console.WriteLine("duplicitny packet");
                return;
            }    

            T.GiveMeMyPacket( rawPacket, 'A'); //chceck mac address table and add or cheange log int there  ;
            S.send(device_a, device_b, rawPacket, 'A');   //odoslanie packetu 
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
            F.update_text_stat();
            F.DGW();
            F.dataFridView1_update();
            //Application.DoEvents();
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
                Console.WriteLine("Duplicitny packet  - B");
                return;
            }
            //Console.WriteLine(packet.Ethernet.IpV4.Protocol.ToString());
            T.GiveMeMyPacket( rawPacket, 'B'); //chceck mac address table and add or cheange log int there  ;
            //Console.WriteLine("Posoelam z B");
            

            if (rawPacket.LinkLayerType == PacketDotNet.LinkLayers.Ethernet)
            {
                /*Console.WriteLine("{0} At: {1}:{2}: MAC:{3} -> MAC:{4}",
                                  packetIndex,
                                  rawPacket.Timeval.Date.ToString(),
                                  rawPacket.Timeval.Date.Millisecond,
                                  ethernetPacket.SourceHardwareAddress,
                                  ethernetPacket.DestinationHardwareAddress);*/
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
            F.update_text_stat();
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

