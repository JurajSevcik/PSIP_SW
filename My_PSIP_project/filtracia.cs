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
            string[] AllINeed = ip_add(packet, rawPacket);
            var ethernetPacket = (EthernetPacket)packet;

            string FromMac = (ethernetPacket.SourceHardwareAddress).ToString();
            //Go throu all roouls and everything have to be same in order
            //to pass .... if everythin g pass I will know that 
            //my roule should apply to packet and I should eather loose a packet or 
            //send it to its destination ... so have fune 
            foreach (AddFilter element in ST_class.filtre)
            {
                if (element.port == pt)//pravidlo by sa malo aplikovať na tento port 
                {
                    if (element.way == "in")//ktorím smerom sa má pravidlo použit --> ak má byť von riešim to inde 
                    {
                        //PacketDotNet.ArpPacket  //TODO: pridat porovannie filtroiv + 
                        if (element.protocol == AllINeed[4] || element.protocol == "ALL" )//TODO: nastavot na vsetky protcoly 
                        {
                            if(element.mac_from == (ethernetPacket.SourceHardwareAddress.ToString()) || element.protocol == "null")
                            {
                                if(element.mac_to == (ethernetPacket.DestinationHardwareAddress.ToString()) || element.protocol == "null")
                                {
                                    if (element.ip_from == AllINeed[0] || element.protocol == "null")  //ip address source
                                    {
                                        if (element.ip_to == AllINeed[1] || element.protocol == "null")  //ip address destination 
                                        {
                                            if (element.port_from == AllINeed[2] || element.protocol == "null") //port spource 
                                            {
                                                if (element.port_to == AllINeed[3] || element.protocol == "null") //port destnination
                                                {
                                                    //TODO: check ip 
                                                    //TODO: check port
                                                    if (element.YesNo == "deny")//mozem packet poslat 
                                                    {
                                                        return true;
                                                    }
                                                }
                                            }
                                        }
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

        //IpV4Datagram ip = packet.Ethernet.IpV4;
        //UdpDatagram udp = ip.Udp;

        // print ip addresses and udp ports
        //Console.WriteLine(ip.Source + ":" + udp.SourcePort+ " -> " + ip.Destination + ":" + udp.DestinationPort);

        private string[] ip_add(PacketDotNet.Packet tempPacket, RawCapture rawPacket)
        {

            if (tempPacket is PacketDotNet.IcmpV6Packet)
            {
                string[] sasas = new string[5];
                sasas[0] = "null";
                sasas[1] = "null";
                sasas[2] = "null";
                sasas[3] = "null";
                sasas[4] = "null";
                return sasas;
            }


            var packet = PacketDotNet.Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);
            var qqe = packet.PayloadPacket.ToString();  // split

            string[] eew = qqe.Split(' ');
            //SenderProtocolAddress  -- SourceAddress
            //TargetProtocolAddress  -- DestinationAddress
            //PacketDotNet.Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data)
            string MyWay = "None of your bisnis";
            string[] MyProtocol = new string[5];
            while (tempPacket.PayloadPacket != null)
            {
                tempPacket = tempPacket.PayloadPacket;
            }

            if (tempPacket is PacketDotNet.ArpPacket)
            {
                //[4] SenderProtocolAddress=10.0.0.1,
                //[5] TargetProtocolAddress=10.0.0.3]

                eew[4].Replace("SenderProtocolAddress=", "");
                MyProtocol[0] = eew[4].Replace(",", "");
                MyProtocol[0] = MyProtocol[0].Replace("SourceAddress=", "");
                MyProtocol[0] = MyProtocol[0].Replace(",", "");
                string SomethingLikeIPSource = eew[4];
                eew[5].Replace("TargetProtocolAddress=", "");
                MyProtocol[1] =  eew[5].Replace("]", "");
                string destination = eew[4];
                //"SourcePort=514,"   [6]
                //"DestinationPort=514]"  [7]
                var tmp = eew[6];
                tmp = tmp.Replace("SourcePort=", "");
                tmp = tmp.Replace(",", "");
                MyProtocol[2] = Int32.Parse(tmp).ToString();
                tmp = eew[7];
                tmp = tmp.Replace("DestinationPort=", "");
                tmp = tmp.Replace("]", "");
                MyProtocol[3] = Int32.Parse(tmp).ToString();
                MyProtocol[4] = "ARP";
            }
            else if (tempPacket is PacketDotNet.TcpPacket)
            {
                //source: SourceAddress=147.175.176.211,
                //destinatzion: DestinationAddress=52.146.136.48,
                string SomethingLikeIPSource = eew[1].Replace("SourceAddress=", "");
                SomethingLikeIPSource.Replace(",", "");
                MyProtocol[0] = eew[1];
                MyProtocol[0] = MyProtocol[0].Replace("SourceAddress=", "");
                MyProtocol[0] = MyProtocol[0].Replace(",", "");
                eew[2] = eew[2].Replace("DestinationAddress=", "");
                eew[2] = eew[2].Replace(",", "");
                MyProtocol[1] = eew[2];
                var tmp = eew[6];
                tmp = tmp.Replace("SourcePort=", "");
                tmp = tmp.Replace(",", "");
                MyProtocol[2] = Int32.Parse(tmp).ToString();
                tmp = eew[7];
                tmp = tmp.Replace("DestinationPort=", "");
                tmp = tmp.Replace("]", "");
                MyProtocol[3] = Int32.Parse(tmp).ToString();
                MyProtocol[4] = "TCP";
            }
            else if (tempPacket is PacketDotNet.UdpPacket)
            {
                if(eew[0] == "[IPv6Packet:")
                {
                    string[] sasas = new string[5];
                    sasas[0] = "null";
                    sasas[1] = "null";
                    sasas[2] = "null";
                    sasas[3] = "null";
                    sasas[4] = "null";
                    return sasas;
                }
                //[1] SourceAddress=169.254.62.117,
                //[2] DestinationAddress=239.255.255.250,
                string SomethingLikeIPSource =  eew[1].Replace("SourceAddress=", "");
                SomethingLikeIPSource.Replace(",", "");
                MyProtocol[0] = eew[1];
                MyProtocol[0] = MyProtocol[0].Replace("SourceAddress=", "");
                MyProtocol[0] = MyProtocol[0].Replace(",", "");
                eew[2] = eew[2].Replace("DestinationAddress=", "");
                eew[2] = eew[2].Replace(",", "");
                MyProtocol[1] = eew[2];

                var tmp = eew[6];
                tmp = tmp.Replace("SourcePort=", "");
                tmp = tmp.Replace(",", "");
                MyProtocol[2] = Int32.Parse(tmp).ToString();
                tmp = eew[7];
                tmp = tmp.Replace("DestinationPort=", "");
                tmp = tmp.Replace("]", "");
                MyProtocol[3] = Int32.Parse(tmp).ToString();

                MyProtocol[4] = "UDP";
            }
            else if (tempPacket is PacketDotNet.IcmpV4Packet)
            {
                string SomethingLikeIPSource = eew[1].Replace("SourceAddress=", "");
                SomethingLikeIPSource.Replace(",", "");
                MyProtocol[0] = eew[1];
                MyProtocol[0] = MyProtocol[0].Replace("SourceAddress=", "");
                MyProtocol[0] = MyProtocol[0].Replace(",", "");
                eew[2] = eew[2].Replace("DestinationAddress=", "");
                eew[2] = eew[2].Replace(",", "");
                MyProtocol[1] = eew[2];
                var tmp = eew[6];
                tmp = tmp.Replace("SourcePort=", "");
                tmp = tmp.Replace(",", "");
                MyProtocol[2] = Int32.Parse(tmp).ToString();
                tmp = eew[7];
                tmp = tmp.Replace("DestinationPort=", "");
                tmp = tmp.Replace("]", "");
                MyProtocol[3] = Int32.Parse(tmp).ToString();
                MyProtocol[4] = "ICMP";
            }
            return MyProtocol;
        }
    }
}
