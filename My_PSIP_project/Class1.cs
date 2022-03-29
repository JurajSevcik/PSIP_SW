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
            string ToMac = (ethernetPacket.DestinationHardwareAddress).ToString();
     
            //todo: chack rouls for sending packets 
            char intf = T.WhereDoIGO(ToMac);

            
            //send packet to other port 
            if(intf == 'A') {
                go(device_b, rawPacket);
            }
            else if (intf == 'B')
            {
                go(device_a, rawPacket);
            }
            else
            {
                //Console.WriteLine((ethernetPacket.SourceHardwareAddress).ToString());
                if (port == 'A') { go(device_b, rawPacket); } else { go(device_a, rawPacket); }// other port 
            }
        }

        private void go(LibPcapLiveDevice gate, SharpPcap.RawCapture rawPacket)
        {
            //gate.Open(mode: DeviceModes.Promiscuous, 1000);   //open send packet close
            try
            {
                gate.SendPacket(rawPacket);
                //Console.WriteLine("you did it ? ");
            }
            catch (Exception) { Console.WriteLine("-- error"); }
            //gate.Close();
        }
    }
}
