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
            catch (Exception) { Console.WriteLine("-- error"); }
            //gate.Close();
        }
    }
}
