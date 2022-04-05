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
        public static Array[] SixPack = new Array[] {};
        public static List<byte[]> b = new List<byte[]>();
        public static List<string> watch = new List<string>();

        public static bool circle(PacketDotNet.Packet raw) // chceck if it's not the same packet ....
        {
            //return false;
            
            var w = raw.Bytes;
            ///var rawPacket = raw.data();
            //var packet = PacketDotNet.Packet.ParsePacket(raw.LinkLayerType, rawPacket.Data);
            //watch.Add(w.ToString());
            if (b.Count == 0)
            {
                b.Add(raw.HeaderData);
                return false;
            }
            //SixPack.Append(raw);
            //byte[] a1 = raw.Data;


            byte[] a1 = raw.Bytes;
            //nt i = 0;
            //return false;
            for (int i = b.Count - 1; i >= 0; i--)
            {
                int control = b[i].Length;
                if (control == a1.Length)
                {
                    for (int j = b[i].Length-1; j > 0; j--)

                        if (b[i][j] != a1[j])//ther is a difreance --> it's not a same one ...he cheanged 
                        {
                            control--;
                        }
                    if (control == b[i].Length)
                    {
                        
                        return true;
                    }
                }
            }
            b.Add(a1);
            return false;
            
            /*
            foreach (byte[] lis in b)
            {
                if (lis[i] == a1[i])
                    i++;
                else
                {
                    b.Add(a1);
                    return false;}
                
            }*/
            //return true;

        }

        public static void rm()
        {
            table.Clear();
        }

        public static void rm_port(char x)
        {
            //remove all information from interface ...if cable was removed f
            for (int i = table.Count - 1; i >= 0; i--)
            {
                if (table[i].M_interface == x)
                    table.RemoveAt(i);
            }            
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
