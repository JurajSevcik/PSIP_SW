using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpPcap;
using System.Net.NetworkInformation;
using System.Collections.Generic;
//using SharpPcap.Packets;
using PacketDotNet;


namespace My_PSIP_project
{
    internal class syslog
    {
        private static int poradie = 0; // poradie syslog správy a
        public static String GetTimestamp(DateTime value)
        {
            return value.ToString("yyyy:MM:ddTHH:mm:ss:ffff"); // cheange timestamp to string format 
        }
        public static EthernetPacket CreateSyslog(string var, int level , string app)
        {
            //syslog.CreateSyslog(var, 2, "Libraly/On*TimeEvent");§
            string order = poradie.ToString();
            poradie++; // number of message 
            String timeStamp = GetTimestamp(DateTime.Now);
            string message = "<" + level + ">2 " + timeStamp +
                " MyAsomeSwitch " + app + " 111 " + 
                 order + " - BOM " + var;  //message format
            byte[] mess = Encoding.ASCII.GetBytes(message);  //cheange message to bytes

            //construct ethernet packet
            string MyMac = GetMacAddress().ToString(); //mac adresa tohoto zariadenia
            var ethernet = new EthernetPacket(PhysicalAddress.Parse(MyMac), PhysicalAddress.Parse("ffffffffffff"), EthernetType.IPv4);
            //construct local IPV4 packet
            System.Net.IPAddress ip_from = new System.Net.IPAddress(int.Parse(ST_class.switch_ip));  // my ip addres 
            System.Net.IPAddress ip_to = new System.Net.IPAddress(int.Parse(ST_class.syslog_ip));   //syslog server ip addres
            var ipv4 = new IPv4Packet(ip_from, ip_to);  
            ethernet.PayloadPacket = ipv4;
            //construct UDP packet
            var udp = new UdpPacket(514,514); //source and destinatiion port (514 - for syslog )
            udp.PayloadData = mess;
            ipv4.PayloadPacket = udp;
            PacketSender S = new PacketSender();
            S.SendSyslog(ethernet); //send packert 
            //Libraly.device_a.SendPacket(ethernet); // send packet all ways
            //Libraly.device_b.SendPacket(ethernet);
            System.Console.WriteLine(ethernet);
            
            return ethernet;
        }

        private static PhysicalAddress GetMacAddress() // geet my mac address 
        {
            var myInterfaceAddress = NetworkInterface.GetAllNetworkInterfaces()
                .Where(n => n.OperationalStatus == OperationalStatus.Up && n.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .OrderByDescending(n => n.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                .Select(n => n.GetPhysicalAddress())
                .FirstOrDefault();

            return myInterfaceAddress;
        }
    }
}
