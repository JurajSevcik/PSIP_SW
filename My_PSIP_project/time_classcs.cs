using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_PSIP_project
{
    internal class time_classcs  //class for checking thing i need to control in time loop
    {
        private System.Windows.Forms.Timer timer1;
        private int counter = 5; // time on timer 
        public void start_tiemer(int id)
        {
            timer1 = new System.Windows.Forms.Timer();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 1000; // 1 sekunda 
            timer1.Start();
           
        }

        public void timer1_Tick(object sender, EventArgs e)  
        {
            counter--;
            if (counter == 0)
                timer1.Stop();
           
        }
    }
}
