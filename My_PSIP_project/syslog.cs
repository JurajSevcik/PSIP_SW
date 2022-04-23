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
        public EthernetPacket CreateSyslog()
        {
            var sevenItems = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 }; 
            //construct ethernet packet
            var ethernet = new EthernetPacket(PhysicalAddress.Parse("112233445566"), PhysicalAddress.Parse("665544332211"), EthernetType.IPv4);
            //construct local IPV4 packet
            System.Net.IPAddress ip_from = new System.Net.IPAddress(192166121);
            System.Net.IPAddress ip_to = new System.Net.IPAddress(123123123);
            var ipv4 = new IPv4Packet(ip_from, ip_to);
            ethernet.PayloadPacket = ipv4;
            //construct UDP packet
            var udp = new UdpPacket(514,514); //source and destinatiion port 
            //add data in
            udp.PayloadData = sevenItems;
            ipv4.PayloadPacket = udp;
            System.Console.WriteLine(ethernet);
            return ethernet;
        }
    }
}
