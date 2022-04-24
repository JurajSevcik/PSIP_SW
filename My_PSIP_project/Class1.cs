using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpPcap;
using SharpPcap.LibPcap;
using PacketDotNet;

namespace My_PSIP_project
{
    internal class PacketSender
    {
        table_class T = new table_class();
        
        public void SendSyslog(PacketDotNet.EthernetPacket ethernet) 
        {
            //If you know where to send this feel free to chose only one device or add annother 
            //I had a little byt of problem with my GNS topology and I wasnt able to add ther a syslog server so 
            //I have no idea if it should be a sepret conection or it is somewhere on the edge of something else ...
            //if you know more than me please do it (message for my future self ...smille )
            // And remember: "Perpose of life is to die young as late as possible "
            try
            {
                Libraly.device_a.SendPacket(ethernet); // send packet all ways
                Libraly.device_b.SendPacket(ethernet);
            }
            catch (Exception)
            {

            }
            
        }


        public void send(LibPcapLiveDevice device_a, LibPcapLiveDevice device_b, SharpPcap.RawCapture rawPacket, char port)  //get packet and where it come from
        {
            
            var packet = PacketDotNet.Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);
            var ethernetPacket = (EthernetPacket)packet;
            string FromMac = (ethernetPacket.SourceHardwareAddress).ToString();
            string ToMac = (ethernetPacket.DestinationHardwareAddress).ToString();
            //Console.WriteLine("From {0} to {1}", FromMac, ToMac);
            //todo: chack rouls for sending packets 
            char intf = T.WhereDoIGO(ToMac);

            
            //send packet to other port 
            if(intf == 'A') {
                go(device_a, rawPacket);
                stat('A', rawPacket);

            }
            else if (intf == 'B')
            {
                go(device_b, rawPacket);
                stat('B', rawPacket);

            }
            else
            {
                //Console.WriteLine((ethernetPacket.SourceHardwareAddress).ToString());
                //Console.WriteLine("Brodcast (Class1/send/else)");
                if (port == 'A') 
                { 
                    go(device_b, rawPacket);
                    stat('B', rawPacket);
                } 
                else { 
                    go(device_a, rawPacket);
                    stat('A', rawPacket);
                }// other port 
            }
        }

        private void stat(char port, SharpPcap.RawCapture rawPacket)
        {
            if(port == 'A')
            {
                var tempPacket = PacketDotNet.Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);
                while (tempPacket.PayloadPacket != null)
                {
                    tempPacket = tempPacket.PayloadPacket;
                }

                if (tempPacket is PacketDotNet.ArpPacket)
                {
                    ST_class.ARP_out_A++;
                }
                else if (tempPacket is PacketDotNet.TcpPacket)
                {
                    ST_class.TCP_out_A++;
                }
                else if (tempPacket is PacketDotNet.UdpPacket)
                {
                    ST_class.UDP_out_A++;
                }
                else if (tempPacket is PacketDotNet.IcmpV4Packet)
                {
                    ST_class.ICMP_out_A++;
                }
                ST_class.packetIndex_out_A++;
            }
            else
            {
                var tempPacket = PacketDotNet.Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);
                while (tempPacket.PayloadPacket != null)
                {
                    tempPacket = tempPacket.PayloadPacket;
                }

                if (tempPacket is PacketDotNet.ArpPacket)
                {
                    ST_class.ARP_out_B++;
                }
                else if (tempPacket is PacketDotNet.TcpPacket)
                {
                    ST_class.TCP_out_B++;
                }
                else if (tempPacket is PacketDotNet.UdpPacket)
                {
                    ST_class.UDP_out_B++;
                }
                else if (tempPacket is PacketDotNet.IcmpV4Packet)
                {
                    ST_class.ICMP_out_B++;
                }
                ST_class.packetIndex_out_B++;
            }
        }

        private void go(LibPcapLiveDevice gate, SharpPcap.RawCapture rawPacket)
        {
            //gate.Open(mode: DeviceModes.Promiscuous, 1000);   //open send packet close
            try
            {
                
                gate.SendPacket(rawPacket);

                //Console.WriteLine("you did it ? (class1/go)");
            }
            catch (Exception) {
                string var = "unable to send packet !!!!!!";
                Console.WriteLine("-- error"); 
                syslog.CreateSyslog(var, 0, "Class1/go");
            }
            //gate.Close();
        }
    }
}
