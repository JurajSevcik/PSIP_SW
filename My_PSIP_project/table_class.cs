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


//TODO: pridaj odstranenie / prehodenie kabla ...


namespace My_PSIP_project
{

    internal class table_class
    {

        public void emmpy(List<MacZaznam> table)  //vyprazdny my moju tabulku aby vyzeralal krajšie 
        {
            foreach(MacZaznam zaznam in table)
            {
                zaznam.M_interface = 'X';
                zaznam.mac_addres = "empty";
                zaznam.destination = "empty";
            }
        }
 
        public Boolean GiveMeMyPacket(List<MacZaznam> table, SharpPcap.RawCapture rawPacket, char port )//get source mac and port 
        {
            //port = 'A'; //TODO: prepisat na parameter 
            string MyMac = "...because c#,  thats why !!!";

            if (rawPacket.LinkLayerType == PacketDotNet.LinkLayers.Ethernet)  //it should be always thrue
            {
                var packet = PacketDotNet.Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);
                var ethernetPacket = (EthernetPacket)packet;
                MyMac =  (ethernetPacket.SourceHardwareAddress).ToString();
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
        private void AddToTable(List<MacZaznam> table, string mac, char port)  //check if mac exist in tabel and if so on what port, if not add to table.
        {
            //table.Append();
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
        private bool Is_ther(List<MacZaznam> table, string mac, char port) //chack if the address is known 
        {
            foreach(MacZaznam zanznem in table)
                if(zanznem.mac_addres == mac )
                {
                    if(zanznem.M_interface == port)
                    {
                        return true;
                    }
                    else
                    {
                        zanznem.mac_addres = mac;
                        zanznem.destination = "empty";
                        zanznem.M_interface = port;
                        Console.WriteLine("There seem to be some misschief going on ( I know mac but ther was wrong port )");
                        return false;
                    }
                }
            MacZaznam zaznam = new MacZaznam() { destination = "empty", mac_addres = mac, M_interface=port};
            table.Add(zaznam);
            return false;
        }

        public char WhereDoIGO(List<MacZaznam> table, string mac) //destination mac address
        //return port where to send packet ...return X sa defolt when unknown --> brodcast
        {
            if (mac == "ff:ff:ff:ff:ff:ff"){
                return 'X';
            }
            foreach(MacZaznam zaznam in table)
            {
                if(zaznam.mac_addres == mac )
                {
                    return zaznam.M_interface;
                }
            }
            return 'X'; //
        }
    }

}
