﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MacZaznam
{
    public string mac_addres { get; set; } //wher packet is from -- and wher i will send next one to this destination
    public char M_interface { get; set; }//on whitch interface did i get packet   
    public System.Windows.Forms.Timer timer;
}

namespace My_PSIP_project
{
    internal class ST_class
    {
        public static List<MacZaznam> table = new List<MacZaznam>();

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