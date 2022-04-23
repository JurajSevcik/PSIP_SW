using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpPcap;
using PacketDotNet;

namespace My_PSIP_project
{
    internal class filtracia
    {
        public Boolean filtruj_in(RawCapture rawPacket, string pt)
        {
            //get packet --then i should look at it 
            
            var packet = PacketDotNet.Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);
            var ippa = rawPacket.LinkLayerType;
            ip_add(packet, rawPacket);
            var ethernetPacket = (EthernetPacket)packet;

            string FromMac = (ethernetPacket.SourceHardwareAddress).ToString();
            foreach (AddFilter element in ST_class.filtre)
            {
                if (element.port == pt)//pravidlo by sa malo aplikovať na tento port 
                {
                    if (element.way == "in")//ktorím smerom sa má pravidlo použit --> ak má byť von riešim to inde 
                    {
                        //PacketDotNet.ArpPacket  //TODO: pridat porovannie filtroiv + 
                        if (element.protocol == "TCP" || element.protocol =="null")//TODO: nastavot na vsetky protcoly 
                        {
                            if(element.mac_from == (ethernetPacket.SourceHardwareAddress.ToString()) || element.protocol == "null")
                            {
                                if(element.mac_to == (ethernetPacket.DestinationHardwareAddress.ToString()) || element.protocol == "null")
                                {
                                    //TODO: check ip 
                                    //TODO: check port
                                    if(element.YesNo == "deny")//mozem packet poslat 
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        continue;//I dont need to care about that 
                    }
                }
                else// i am loking on difrent port ..Smille 
                {
                    continue;
                    //else I dont care ...
                }
            }
            return false;
        }
        


        private string ip_add(PacketDotNet.Packet tempPacket, RawCapture rawPacket)
        {
            var packet = PacketDotNet.Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);
            var qqe = packet.PayloadPacket.ToString();  // split

            string[] eew = qqe.Split(' ');
            //SenderProtocolAddress  -- SourceAddress
            //TargetProtocolAddress  -- DestinationAddress
            //PacketDotNet.Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data)
            string MyWay = "None of your bisnis";
            while (tempPacket.PayloadPacket != null)
            {
                tempPacket = tempPacket.PayloadPacket;
                
            }

            if (tempPacket is PacketDotNet.ArpPacket)
            {
                //[4] SenderProtocolAddress=10.0.0.1,
                //[5] TargetProtocolAddress=10.0.0.3]

                eew[4].Replace("SenderProtocolAddress=", "");
                eew[4].Replace(",", "");
                string SomethingLikeIPSource = eew[4];
                eew[5].Replace("TargetProtocolAddress=", "");
                eew[5].Replace("]", "");
                string destination = eew[4];
            }
            else if (tempPacket is PacketDotNet.TcpPacket)
            {
                //source: SourceAddress=147.175.176.211,
                //destinatzion: DestinationAddress=52.146.136.48,
                string SomethingLikeIPSource = eew[1].Replace("SourceAddress=", "");
                SomethingLikeIPSource.Replace(",", "");
                string source = eew[1];
                eew[2].Replace("DestinationAddress=", "");
                eew[2].Replace(",", "");
                string destination = eew[2];
            }
            else if (tempPacket is PacketDotNet.UdpPacket)
            {
                //[1] SourceAddress=169.254.62.117,
                //[2] DestinationAddress=239.255.255.250,
                string SomethingLikeIPSource =  eew[1].Replace("SourceAddress=", "");
                SomethingLikeIPSource.Replace(",", "");
                string source = eew[1];
                eew[2].Replace("DestinationAddress=", "");
                eew[2].Replace(",", "");
                string destination = eew[2];
            }
            else if (tempPacket is PacketDotNet.IcmpV4Packet)
            {
                string SomethingLikeIPSource = eew[1].Replace("SourceAddress=", "");
            }
            return MyWay;
        }
    }
}
