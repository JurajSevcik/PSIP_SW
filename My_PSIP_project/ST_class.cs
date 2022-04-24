using System;
using System.Collections.Generic;

public class MacZaznam
{
    public string mac_addres { get; set; } //wher packet is from -- and wher i will send next one to this destination
    public char M_interface { get; set; }//on whitch interface did i get packet   
    public int timer = 300;
}
public class AddFilter
{
    public string port { get; set; }
    public string way { get; set; }
    public string YesNo { get; set; }
    public string protocol { get; set; }
    public string ip_from { get; set; }
    public string mac_from { get; set; }
    public string port_from { get; set; }
    public string ip_to { get; set; }
    public string mac_to { get; set; }
    public string port_to { get; set; }
}

namespace My_PSIP_project
{
    internal class ST_class
    {
        public static List<MacZaznam> table = new List<MacZaznam>();
        public static Array[] SixPack = new Array[] {};
        public static List<byte[]> b = new List<byte[]>();
        public static List<string> watch = new List<string>();
        public static List<AddFilter> filtre = new List<AddFilter>();

        public static bool circle(PacketDotNet.Packet raw) // chceck if it's not the same packet ....
        {
            var w = raw.Bytes;
            if (b.Count == 0)
            {
                b.Add(raw.HeaderData);
                return false;
            }
            byte[] a1 = raw.Bytes;
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

        //_________________________________________ Statisticks
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

        //_______________________________________    syslog   //defaulte valiu for syslog 
        public static string switch_ip = "111222111";
        public static string syslog_ip = "222111222";

        public static int MyTimerIsTop = 20;
        //public static var liingua = LibPcapLiveDeviceList.Instance;

    }
}
