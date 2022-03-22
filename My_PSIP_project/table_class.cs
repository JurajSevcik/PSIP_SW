using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PacketDotNet;
using SharpPcap.LibPcap;
using SharpPcap;
using NHibernate.Mapping;
using SharpPcap;
using SharpPcap.LibPcap;
using PacketDotNet;

/*
class MacZaznam
{
    public string mac_addres = "empty"; //wher packet is from -- and wher i will send next one to this destination
    public char M_interface = 'X'; //on whitch interface did i get packet   
    public string destination = "empty";

    MacZaznam(string x,char y,string z)
    {
        mac_addres = x;
        M_interface = y;
        destination = z;
    }

}
*/
//TODO: pridaj odstranenie / prehodenie kabla ...


namespace My_PSIP_project
{

    internal class table_class
    {
        static int size = 25; //number of addreses in mac table s
        //public MacZaznam[] table = new MacZaznam[size];
        //protected List<MacZaznam> table = new List<MacZaznam>;
        


        public void emmpy(MacZaznam[] table)  //vyprazdny my moju tabulku aby vyzeralal krajšie 
        {
            for(int i = 0; i <= size; i++)
            {
                table[i].mac_addres = "empty";
                table[i].destination = "empty";
                table[i].M_interface = 'X';
            }
        }

        
        public Boolean GiveMeMyPacket(MacZaznam[] table,SharpPcap.RawCapture rawPacket, char port )//get source mac and port 
        {
            //port = 'A'; //TODO: prepisat na parameter 
            string MyMac = "...because c#,  thats why !!!";

            if (rawPacket.LinkLayerType == PacketDotNet.LinkLayers.Ethernet)  //it should be always thrue
            {
                var packet = PacketDotNet.Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);
                var ethernetPacket = (EthernetPacket)packet;
                MyMac =  (ethernetPacket.SourceHardwareAddress).ToString();
                //var destinationMAC = ethernetPacket.DestinationHardwareAddress;
            }
            else
            {
                return false; //it is not ethernet which is weerd ...ther is nothing else it could be !!!
            }

            bool isExisting = Is_ther(table, MyMac, port);
            if (isExisting)
            {
                Console.WriteLine("Jop, not creazy .... yet,   and i also added it to mac table ");
                //mal by som vrátiť kam sa má poslať 
            }
            else //Adresu nepoznam 
            {
                Console.WriteLine("proste, NIE !!!, ");
            }
            return true;
        }


        //TODO: remove this usseles something
        private void AddToTable(MacZaznam[] table, string mac, char port)  //check if mac exist in tabel and if so on what port, if not add to table.
        {
            int i = 0;
            while(table[i].mac_addres != "empty")
            {
                i++;
            }
            table[i].mac_addres = mac;
            table[i].destination = "ja neviem uz";
            table[i].M_interface = port;
        }

        //TODO: add cheange array to dynamic 
        //TODO: add time to table (cheangable)
        //check if mac exist in tabel and if so on what port, if not add to table.
        private bool Is_ther(MacZaznam[] table, string mac, char port) //chack if the address is known 
        {
            
            for(int i = 0; i  < 25; i++)
            {
                if (table[i].mac_addres == mac) // mac address is same 
                {
                    if (table[i].M_interface == port)
                    {
                        //TODO: reset timer
                        return true; //ther is record and i know it 

                    }
                    else
                    {
                        table[i].mac_addres = mac;
                        table[i].destination = "empty";
                        table[i].M_interface = port;
                        Console.WriteLine("There seem to be some misschief going on ( I know mac but ther was wrong port )");
                        return false; 
                        //it's a difrent port and i nead to cheange it 
                        //is from difrent port --delete 
                    }
                }
            }
            int j = 0;  
            while(table[j].mac_addres != "empty")  //add to first empty place //todo: afret dinamic just append
            {
                if (table[j].mac_addres == mac) // mac address is same 
                {
                    if(table[j].M_interface == port) //there alredy is record ..check for interface 
                    {
                        //todo: reset timer
                        return true;
                    }
                    else
                    {
                        break;
                    }
                }
                j++;            
            }
            table[j].mac_addres = mac;
            table[j].destination = "empty";
            table[j].M_interface = port;

            return false;
        }

        public char WhereDoIGO(MacZaznam[] table,string mac) //destination mac address
        //return port where to send packet ...return X sa defolt when unknown --> brodcast
        {
            if (mac == "ff:ff:ff:ff:ff:ff"){
                return 'X';
            }
            for (int i = 0; i < size; i++)
            {
                if (table[i].mac_addres == mac) // mac address is same 
                {
                    return table[i].M_interface;
                }
            }
            return 'X'; //
        }
    }

}
