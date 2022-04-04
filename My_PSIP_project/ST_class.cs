using System;
using System.Collections.Generic;

public class MacZaznam
{
    public string mac_addres { get; set; } //wher packet is from -- and wher i will send next one to this destination
    public char M_interface { get; set; }//on whitch interface did i get packet   
    public int timer = 15;
}

namespace My_PSIP_project
{
    internal class ST_class
    {
        public static List<MacZaznam> table = new List<MacZaznam>();
        public static Array[] SixPack = new Array[] { };
        public static List<string> b = new List<string>();
        public static List<string> watch = new List<string>();

        public static bool circle(PacketDotNet.Packet raw) // chceck if it's not the same packet ....
        {
            return false;
            var w = raw.PayloadPacket;
            
            //watch.Add(w.ToString());
            if (b.Count == 0)
            {
                b.Add(raw.Bytes.ToString());
                return false;
            }
            //SixPack.Append(raw);
            //byte[] a1 = raw.Data;


            string a1 = raw.Bytes.ToString();
            foreach (string lis in b)
            {
                if (lis.Equals(a1))
                {
                    //tento som uz videl ...i de o rovanky packet 
                    return true;
                }
            }
            b.Add(a1);
            return false;

        }

        public static void rm()
        {
            table.Clear();
        }


        public static int packetIndex_in_A = 0;
        public static int EthernetII_in_A = 0;
        public static int ARP_in_A = 0;
        public static int TCP_in_A = 0;
        public static int UDP_in_A = 0;
        public static int ICMP_in_A = 0;
        public static int HTTP_in_A = 0;
        public static int HTTPS_in_A = 0;

        public static int packetIndex_out_A = 0;
        public static int EthernetII_out_A = 0;
        public static int ARP_out_A = 0;
        public static int TCP_out_A = 0;
        public static int UDP_out_A = 0;
        public static int ICMP_out_A = 0;
        public static int HTTP_out_A = 0;
        public static int HTTPS_out_A = 0;

        public static int packetIndex_in_B = 0;
        public static int EthernetII_in_B = 0;
        public static int ARP_in_B = 0;
        public static int TCP_in_B = 0;
        public static int UDP_in_B = 0;
        public static int ICMP_in_B = 0;
        public static int HTTP_in_B = 0;
        public static int HTTPS_in_B = 0;

        public static int packetIndex_out_B = 0;
        public static int EthernetII_out_B = 0;
        public static int ARP_out_B = 0;
        public static int TCP_out_B = 0;
        public static int UDP_out_B = 0;
        public static int ICMP_out_B = 0;
        public static int HTTP_out_B = 0;
        public static int HTTPS_out_B = 0;


    }

}
