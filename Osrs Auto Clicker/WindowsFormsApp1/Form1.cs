using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        [DllImport("User32.dll", SetLastError = true)]
        public static extern int SendInput(int nInputs, ref INPUT pInputs, int cbSize);

        [DllImport("user32.dll")]
        static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        // Get a handle to an application window.
        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName,
            string lpWindowName);

        // Activate an application window.
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        const int MOUSEEVENTF_LEFTDOWN = 0x02;
        const int MOUSEEVENTF_LEFTUP = 0x04;
        const int INPUT_MOUSE = 0;
        private Point mouselocation;
        private const UInt32 WM_KEYDOWN = 0x0100;
        private const int VK_1 = 0x0031;
        private LowLevelKeyboardListener gHook;
        private int clickcount;
        private const int x = 100;
        private const int y = 0;
        private Point pointconst = new Point(x,y);

        public Form1()
        {
            InitializeComponent();
            mouselocation = new Point(x, y);
            gHook = new LowLevelKeyboardListener();
            gHook.KeyDown += new KeyEventHandler(gHook_KeyDown);
            foreach (Keys key in Enum.GetValues(typeof(Keys)))
                gHook.HookedKeys.Add(key);
            timer1.Interval = 3000;

        }

        public void gHook_KeyDown(object sender, KeyEventArgs e)
        {
            if (((char)e.KeyValue).ToString()=="Z")
            {
                timer1.Enabled = false;
                gHook.unhook();
            }
            if (((char)e.KeyValue).ToString() == "F")
            {
                label1.Text = Cursor.Position.X + "  " + Cursor.Position.Y;
                textBox1.Text = Cursor.Position.X.ToString();
                textBox2.Text = Cursor.Position.Y.ToString();
                textBox3.Text = "501";
                gHook.unhook();
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            if(timer1.Interval >=100 && mouselocation != pointconst) { 

            gHook.hook();
            timer1.Enabled = true;
            }
            else
            {
                MessageBox.Show("Set interval 1st");
            }

            clickcount = 0;
            
        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            
            Cursor.Position = mouselocation;
            Process p = Process.GetProcessesByName("JagexLauncher").FirstOrDefault();
            IntPtr h = p.MainWindowHandle;           
            while (clickcount >= 9)
            {

                SetForegroundWindow(h);
                SendKeys.SendWait("1");
                clickcount = 0;
            }
            clickcount++;
           mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 20, 20, 0, 0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            mouselocation.X = Convert.ToInt16(textBox1.Text);
            mouselocation.Y = Convert.ToInt16(textBox2.Text);
            if (Convert.ToInt16(textBox3.Text) >= 100)
            {
                timer1.Interval = Convert.ToInt16(textBox3.Text);
                
            }
            else
            {
                MessageBox.Show("Too Fast Danger");
            }

        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            gHook.unhook();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            gHook.hook();
        }

    }
}
