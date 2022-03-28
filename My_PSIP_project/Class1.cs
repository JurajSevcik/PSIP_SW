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
        
        public void send(LibPcapLiveDevice device_a, LibPcapLiveDevice device_b, SharpPcap.RawCapture rawPacket, char port, List<MacZaznam> table)  //get packet and where it come from
        {
            var packet = PacketDotNet.Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);
            var ethernetPacket = (EthernetPacket)packet;
            string ToMac = (ethernetPacket.DestinationHardwareAddress).ToString();
            //todo: chack rouls for sending packets 
            char intf = T.WhereDoIGO(table, ToMac);
            
            //send packet to other port 
            if (intf == 'X') //no idead wher to go --> everywhere but home 
            {
                if (port == 'A') { go(device_b, rawPacket); } else { go(device_a, rawPacket); }// other port 
            }
            else if(intf == 'A') {
                go(device_a, rawPacket);
            }
            else if (intf == 'B')
            {
                go(device_b, rawPacket);
            }
        }

        private void go(LibPcapLiveDevice gate, SharpPcap.RawCapture rawPacket)
        {
            gate.Open(mode: DeviceModes.Promiscuous | DeviceModes.DataTransferUdp | DeviceModes.NoCaptureLocal, read_timeout: 1000);   //open send packet close
            try
            {
                gate.SendPacket(rawPacket.Data);
            }
            catch (Exception) { Console.WriteLine("-- error"); }
            gate.Close();
        }
    }
}
