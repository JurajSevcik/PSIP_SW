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
        
        public void send()
        {
            Console.WriteLine(L.device_a);
        }
    }
}
