using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using SharpPcap;
using SharpPcap.LibPcap;





namespace My_PSIP_project
{
    public partial class Form1 : Form
    {
        table_class T = new table_class();
        Libraly L = new Libraly();

        //private Thread thread2 = null;
        private delegate void SafeCallDelegate(string text);
        delegate void SetTextCallback(string text);
        public static BindingSource bindingSource1 = new BindingSource();
        private static System.Timers.Timer bTimer;
        public Form1()  
        {
            InitializeComponent();
            //backgroundWorker1.WorkerReportsProgress = true;
            //backgroundWorker1.WorkerSupportsCancellation = true;
            //pridelenie textu na statistiky (TODO:dokoncit --pridat B in/out ----nebud lenivé prasa ...smille ) 

            dataGridView1.DataSource = ST_class.table;
            dataGridView2.DataSource = ST_class.filtre;
            /*
            int i = 0;
            foreach(MacZaznam mac in ST_class.table)
            {
            listView3.Items.Add(ST_class.table[i].mac_addres.ToString());
           
            listView3.Items.Add(ST_class.table[i].M_interface.ToString());
            listView3.Items.Add(ST_class.table[i].timer.ToString());
             i++;
            }*/



            //dataGridView1.DataSource = ST_class.table;

            // TExtbox ...
            textBox_B_in_ARP.Text = ST_class.ARP_in_B.ToString();
            textBox_B_in_TCP.Text = ST_class.TCP_in_B.ToString();
            textBox_B_in_UDP.Text = ST_class.UDP_in_B.ToString();
            textBox_B_in_ICMP.Text = ST_class.ICMP_in_B.ToString();
            textBox_B_in_HTTP.Text = ST_class.HTTPS_in_B.ToString();

            textBox_A_in_ARP.Text = ST_class.ARP_in_A.ToString();
            textBox_A_in_TCP.Text = ST_class.TCP_in_A.ToString();
            textBox_A_in_DUP.Text = ST_class.UDP_in_A.ToString();
            textBox_A_in_ICMP.Text = ST_class.ICMP_in_A.ToString();
            textBox_A_in_HTTP.Text = ST_class.HTTPS_in_A.ToString();

            textBox_A_out_ARP.Text = ST_class.ARP_out_A.ToString();
            textBox_A_out_TCP.Text = ST_class.TCP_out_A.ToString();
            textBox_A_out_ICMP.Text = ST_class.ICMP_out_A.ToString();
            textBox_A_out_HTTP.Text = ST_class.HTTP_out_A.ToString();
            textBox_A_out_UDP.Text = ST_class.UDP_out_A.ToString();

            textBox_B_out_ARP.Text = ST_class.ARP_out_B.ToString();
            textBox_B_out_TCP.Text = ST_class.TCP_out_B.ToString();
            textBox_B_out_ICMP.Text = ST_class.ICMP_out_B.ToString();
            textBox_B_out_HTTP.Text = ST_class.HTTP_out_B.ToString();
            textBox_B_out_UDP.Text = ST_class.UDP_out_B.ToString();

        }
        private void tim()  //timer to chceck age of mac table content ....
        {
            bTimer = new System.Timers.Timer();
            bTimer.Interval = 1000;

            // Hook up the Elapsed event for the timer. 
            bTimer.Elapsed += OnTimedEvent;

            // Have the timer fire repeated events (true is the default)
            bTimer.AutoReset = true;

            // Start the timer
            bTimer.Enabled = true;
        }
        private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            //do something 
            update_text_stat();
            DGW();

        }


        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        public void lblHelloWorld_Click(object sender, EventArgs e)
        {
        }

        public void textBox1_TextChanged(object sender, EventArgs e)
        {
            //this.textBox1.AppendText("Text");
        }


        private void button2_Click(object sender, EventArgs e)//capture 
        {
            //thread2 = new Thread(new ThreadStart(L.capture));
            //thread2.Start();
            tim();
            var devices = LibPcapLiveDeviceList.Instance; //list of all devices 
            //device_a = devices[8];
            //T.emmpy();
            List<MacZaznam> t = L.capture();
            int i = 0;
            foreach (var dev in devices)
            {
                textBox1.AppendText(i +  dev.Description);
                textBox1.AppendText("\n");
                i++;
            }
            
            //dataGridView1.DataSource = ST_class.table.ToList();
            //dataGridView1.DataBi
            //L.capture();
        }

        private void button4_Click(object sender, EventArgs e) //clear -- vycisty textBox
        {
            textBox1.Clear();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            L.Stop();
        }


        public void UpdateTextBox_1(string text)
        {
            textBox1.Text = L.TextToDisplay;
 
        }

        public void UpdateTextBox_1_2(string Text)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(UpdateTextBox_1_2), Text);
                MessageBox.Show("invoke required");
            }
            else
            {
                textBox1.AppendText(Text);
                MessageBox.Show("invoke not required");
            }
            //textBox1.Text = L.TextToDisplay;
            textBox1.AppendText(Text); // TODO: add BackgroundWorker 
            textBox1.Text += Text;
            //textBox2_TextChanged();
        }

        private void BlaBla(string Text)
        {
            textBox2.AppendText(Text);
        }

        private void button3_Click(object sender, EventArgs e) //read  from capture file 
        {
            syslog syslog = new syslog();
            L.ControlWrite();
            syslog.CreateSyslog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            TextBox.CheckForIllegalCrossThreadCalls = false;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        public void label1_update(string Text)
        {
            this.label1.Text += Text;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            
        }

        public void update_text_stat()
        {

            textBox_B_in_ARP.Text = ST_class.ARP_in_B.ToString();
            textBox_B_in_TCP.Text = ST_class.TCP_in_B.ToString();
            textBox_B_in_UDP.Text = ST_class.UDP_in_B.ToString();
            textBox_B_in_ICMP.Text = ST_class.ICMP_in_B.ToString();
            textBox_B_in_HTTP.Text = ST_class.HTTPS_in_B.ToString();

            textBox_A_in_ARP.Text = ST_class.ARP_in_A.ToString();
            textBox_A_in_TCP.Text = ST_class.TCP_in_A.ToString();
            textBox_A_in_DUP.Text = ST_class.UDP_in_A.ToString();
            textBox_A_in_ICMP.Text = ST_class.ICMP_in_A.ToString();
            textBox_A_in_HTTP.Text = ST_class.HTTPS_in_A.ToString();

            textBox_A_in_ARP.Update();
            textBox_A_in_TCP.Update();
            textBox_A_in_DUP.Update();
            textBox_A_in_ICMP.Update();
            textBox_A_in_HTTP.Update();

            textBox_A_in_ARP.Refresh();
            textBox_A_in_TCP.Refresh();
            textBox_A_in_DUP.Refresh();
            textBox_A_in_ICMP.Refresh();
            textBox_A_in_HTTP.Refresh();
            //textBox_A_in_ARP.BeginInvoke();

            textBox_A_out_ARP.Text = ST_class.ARP_out_A.ToString();
            textBox_A_out_TCP.Text = ST_class.TCP_out_A.ToString();
            textBox_A_out_ICMP.Text = ST_class.ICMP_out_A.ToString();
            textBox_A_out_HTTP.Text = ST_class.HTTP_out_A.ToString();
            textBox_A_out_UDP.Text = ST_class.UDP_out_A.ToString();

            textBox_B_out_ARP.Text = ST_class.ARP_out_B.ToString();
            textBox_B_out_TCP.Text = ST_class.TCP_out_B.ToString();
            textBox_B_out_ICMP.Text = ST_class.ICMP_out_B.ToString();
            textBox_B_out_HTTP.Text = ST_class.HTTP_out_B.ToString();
            textBox_B_out_UDP.Text = ST_class.UDP_out_B.ToString();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void Label_B_UDP_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void Label_A_ARP_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void label21_Click(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            //rull
            AddFilter Fi = new AddFilter() {};
            Fi.port = comboBox1.SelectedItem.ToString();
            Fi.way = comboBox2.SelectedItem.ToString();
            Fi.YesNo = comboBox3.SelectedItem.ToString();
            Fi.protocol = comboBox4.SelectedItem.ToString();
            Fi.ip_from = textBox3.Text;
            Fi.mac_from = textBox4.Text;
            Fi.port_from = textBox5.Text;
            Fi.ip_to = textBox6.Text;
            Fi.mac_to = textBox7.Text;
            Fi.port_to = textBox8.Text;
            ST_class.filtre.Add(Fi);
            dataGridView2.Invoke(new Action(() => dataGridView2.DataSource = ST_class.filtre.ToList()));
            dataGridView2.Update();
            dataGridView2.Refresh();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            L.show_table();

            //int i = 0;

            //dataGridView1.DataSource = ST_class.table.ToList();
            dataGridView1.Invoke(new Action(() => dataGridView1.DataSource = ST_class.table.ToList()));
            dataGridView1.Update();
            
            
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        public void DGW()
        {

            //L.show_table();
            //dataGridView1.DataSource = null;
            //int i = 0;
            //dataGridView1.DataSource = ST_class.table.ToList();
            //dataGridView1.Invoke(new Action(() => dataGridView1.DataSource = null));
            dataGridView1.Invoke(new Action(() => dataGridView1.DataSource = ST_class.table.ToList()))  ;
            dataGridView1.Update();

        }

        public void dataFridView1_update()
        {
            //dataGridView1.DataSource = ST_class.table;
            dataGridView1.Update();
            dataGridView1.Refresh();
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //dataGridView1.DataSource = ST_class.table;
            
            
            
        }

        private void label23_Click(object sender, EventArgs e)
        {

        }

        private void label22_Click(object sender, EventArgs e)
        {

        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            ST_class.rm();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void domainUpDown1_SelectedItemChanged(object sender, EventArgs e)
        {

        }


        private void MacTimerUpdate_Click(object sender, EventArgs e)
        {
            //TODO: tu som skoncit ... napojit timer na cas 
            var time = timer.Text;
            
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

        }
    }

}
