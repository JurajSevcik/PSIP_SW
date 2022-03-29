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
        public Form1()  
        {
            InitializeComponent();
            //backgroundWorker1.WorkerReportsProgress = true;
            //backgroundWorker1.WorkerSupportsCancellation = true;
            //pridelenie textu na statistiky (TODO:dokoncit --pridat B in/out ----nebud lenivé prasa ...smille ) 
            
            Label_A_ARP.Text = ST_class.ARP_in_A.ToString();
            Label_A_TCP.Text = ST_class.TCP_in_A.ToString();
            dataGridView1.DataSource = ST_class.table;
            //dataGridView1.DataSource = ST_class.table;

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
            L.ControlWrite();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            TextBox.CheckForIllegalCrossThreadCalls = false;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            //this.textBox2.AppendText("Text");
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

        public void Label_B_ARP_update(int Text)
        {
            Label_B_ARP.Text = Text.ToString();
        }

        public void Label_B_TCP_update(int Text)
        {
            Label_B_TCP.Text = Text.ToString();
        }

        public void Label_B_UDP_update(int Text)
        {
            Label_B_UDP.Text = Text.ToString();
        }

        public void Label_B_ICMP_update(int Text)
        {
            Label_B_ICMP.Text = Text.ToString();
        }
        public void Label_B_HTTP_update(int Text)
        {
            Label_B_HTTP.Text = Text.ToString();
        }

        //

        public void Label_A_ARP_update(int Text)
        {
            Label_A_ARP.Text = ST_class.ARP_in_A.ToString();
        }

        public void Label_A_TCP_update(int Text)
        {
            Label_A_TCP.Text = Text.ToString();
        }

        public void Label_A_UDP_update(int Text)
        {
            Label_A_UDP.Text = Text.ToString();
        }

        public void Label_A_ICMP_update(int Text)
        {
            Label_B_ICMP.Text = Text.ToString();
        }
        public void Label_A_HTTP_update(int Text)
        {
            Label_B_HTTP.Text = Text.ToString();
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

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            L.show_table();
            
            int i = 0;
            //dataGridView1.DataSource = ST_class.table.ToList();
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
            dataGridView1.Update();
            dataGridView1.Refresh();

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
    }

}
