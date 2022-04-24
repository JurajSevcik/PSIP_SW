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
using System.Timers;

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
               
            }
        }
 
        public Boolean GiveMeMyPacket( SharpPcap.RawCapture rawPacket, char port )//get source mac and port 
        {
            //port = 'A'; //TODO: prepisat na parameter 
            string MyMac = "...because c#,  thats why !!!";
            List<string> MacDev = new List<String> { "005079666800", "005079666801", "005079666802" };

            if (rawPacket.LinkLayerType == PacketDotNet.LinkLayers.Ethernet)  //it should be always thrue
            {
                var packet = PacketDotNet.Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);
                var ethernetPacket = (EthernetPacket)packet;
                MyMac =  (ethernetPacket.SourceHardwareAddress).ToString();
                if (MacDev[0] == MyMac || MacDev[1] == MyMac || MacDev[2] == MyMac) //I know this mac ...go ahead
                {

                }
                else   //nope 
                {
                    return false;
                }

            }
            else
            {
                return false; //it is not ethernet which is weerd ...ther is nothing else it could be !!!
            }
            bool isExisting = Is_ther( MyMac, port);
            if (isExisting)
            {
                //Console.WriteLine("Jop, not creazy .... yet,   and i also added it to mac table ");
                //mal by som vrátiť kam sa má poslať 
            }
            else //Adresu nepoznam 
            {
                //Console.WriteLine("proste, NIE !!!, ");
            }
            return true;
        }

 

        //TODO: remove this usseles something
        private void AddToTable(List<MacZaznam> table, string mac, char port)  //check if mac exist in tabel and if so on what port, if not add to table.
        {
            int i = 0;

            while(table[i].mac_addres != "empty")
            {
                i++;
            }
            table[i].mac_addres = mac;
            //[i].timer.Start();
            //table[i].destination = "ja neviem uz";
            table[i].M_interface = port;
        }


        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            //Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}", e.SignalTime);
        }

        //TODO: add cheange array to dynamic 
        //TODO: add time to table (cheangable)
        //check if mac exist in tabel and if so on what port, if not add to table.
        private bool Is_ther( string mac, char port) //chack if the address is known 
        {
            //time_classcs Tm = new time_classcs();
            //TODO: check the logic .... i am not shure about that ...!
            foreach(MacZaznam zanznem in ST_class.table)
                if(zanznem.mac_addres == mac ) //našiels som mac 
                {
                    if(zanznem.M_interface == port) // it shouls 
                    {
                        return true;
                    }
                    else  //mam rozdoelny port .....it's aproblem babe ..
                    {
                        RM_CB(port);
                        zanznem.mac_addres = mac;
                        zanznem.M_interface = port;
                        string var = "MAC addres"+ mac.ToString() +  " moved to port : " + port.ToString() + " ";
                        syslog.CreateSyslog(var, 2, "table_class/RM_CB");
                        //TODO: remove all on interface
                        //Console.WriteLine("There seem to be some misschief going on ( I know mac but ther was wrong port )");
                        return false;
                    }
                }
            MacZaznam zaznam = new MacZaznam() { mac_addres = mac, M_interface=port};
            ST_class.table.Add(zaznam);
            return false;
        }

        private void RM_CB(char port)// someone removed my cable .... no touchy !
        {
            
            //Console.WriteLine("Mazem vsetko");
            for (int i = ST_class.table.Count - 1; i >= 0; i--)
            {
                if (ST_class.table[i].M_interface == port)
                    ST_class.table.RemoveAt(i);
            }
        }

        private bool IsEmpty<T>(List<T> list)
        {
            if (list == null)
            {
                return true;
            }

            return !list.Any();
        }

        public char WhereDoIGO(string mac) //destination mac address
        //return port where to send packet ...return X sa defolt when unknown --> brodcast
        {
            if (mac == "FFFFFFFFFFFF")
            {
                return 'X';
            }
            bool isEmpty = IsEmpty(ST_class.table);
            if(isEmpty)
            {
                string var = "Unknown MAC arres: " + mac.ToString() + " "; 
                syslog.CreateSyslog(var, 4, "table_class/WhereDoIGo");
                return 'X';
            }
            foreach(MacZaznam zaznam in ST_class.table)
            {
                if(zaznam.mac_addres == mac )
                {
                    //Console.WriteLine("From {0}, Going to: {1}, (table_class/WhereDoIGO/foreach)",mac,  zaznam.M_interface);
                    return zaznam.M_interface;
                }
            }
            return 'X'; //
        }
    }

}
