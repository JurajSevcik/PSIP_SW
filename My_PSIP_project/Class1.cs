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
        Libraly L = new Libraly();
        table_class T = new table_class();
        
        public void send(SharpPcap.RawCapture rawPacket, char port, MacZaznam[] table)  //get packet and where it come from
        {
            var packet = PacketDotNet.Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);
            var ethernetPacket = (EthernetPacket)packet;
            string ToMac = (ethernetPacket.DestinationHardwareAddress).ToString();
            //todo: chack rouls for sending packets 
            //Console.WriteLine(L.device_a);
            char intf = T.WhereDoIGO(table, ToMac);
            LibPcapLiveDevice gate = L.device_a;
            //send packet to other port 
            if (intf == 'X') //no idead wher to go --> everywhere but home 
            {
                if (port == 'A') { gate = L.device_b; } else { gate = L.device_a; }// other port 
                go(gate, rawPacket);
            }
            else if(intf == 'A') { 
                gate = L.device_a;
                go(gate, rawPacket);
            }
            else if (intf == 'B')
            {
                gate = L.device_b;
                go(gate, rawPacket);
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
