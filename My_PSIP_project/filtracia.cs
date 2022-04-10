using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpPcap;

namespace My_PSIP_project
{
    internal class filtracia
    {
        public void filtruj_in(RawCapture rawPacket, string pt)
        {
            //get packet --then i should look at it 
            return;
            var tempPacket = PacketDotNet.Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);
            foreach (AddFilter element in ST_class.filtre)
            {
                if (element.port == pt)//pravidlo by sa malo aplikovať na tento port 
                {
                    if (element.way == "in")
                    {
                        //PacketDotNet.ArpPacket  //TODO: pridat porovannie filtroiv + 

                    }
                }
                else// i am loking on difrent port ..Smille 
                {
                    //else I dont care ...
                }
            }

        }
    }
}
