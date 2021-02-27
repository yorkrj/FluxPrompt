using Microsoft.CodeAnalysis.CSharp.Scripting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FluxPrompt
{
    public partial class Form1 : Form
    {
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        HotKeyHandler hotkeyHandler;

        public Form1()
        {
            InitializeComponent();

            ShowInTaskbar = false;
            notifyIcon1.Icon = SystemIcons.Asterisk;
            TopMost = true;

            TransparencyKey = (BackColor);
            notifyIcon1.Visible = true;
            notifyIcon1.Text = "Flux Prompt";

            hotkeyHandler = new HotKeyHandler();

            dataGridView1.Columns.Add(new DataGridViewColumn());
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.RowHeadersVisible = false;
            //dataGridView1.AllowUserToAddRows = false;

            hotkeyHandler.HotKeyPressed += new EventHandler<HotKeyPressedEventArgs>(HandleHotKeyPressed);

            hotkeyHandler.Register(0,
                new HotKeyModifer[] { HotKeyModifer.Alt, HotKeyModifer.NoRepeat },
                Keys.Space.GetHashCode());
        }

        public void HandleHotKeyPressed(object sender, HotKeyPressedEventArgs e)
        {
            //TODO switch for multiple hot key indexes here based on value of e.HotKeyIndex

            WindowState = FormWindowState.Normal;
            TopMost = true;
            Activate();
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            //e.Modifiers
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    string csScript = PromptTextBox.Text.ToString();

                    object result = new object();

                    try
                    {
                        CSharpScript.EvaluateAsync(csScript).ContinueWith(s => result = s.Result).Wait();
                    }
                    catch (Exception ex)
                    {
                        result = ex.Message;
                    }

                    dataGridView1.Rows.Insert(0, csScript + Environment.NewLine + Convert.ToString(result));

                    e.Handled = true;

                    break;
                case Keys.Escape:
                    WindowState = FormWindowState.Minimized;
                    break;
                case Keys.Tab:
                    // TODO autocomplete
                    break;
                default:
                    //dataGridView1.Rows.Add(TextBox1.Text.ToString());
                    break;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            WindowState = FormWindowState.Normal;
            TopMost = true;
            Activate();
        }

        private void OnResize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                TopMost = false;
            }
            else
            {
                TopMost = true;
            }
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            hotkeyHandler.Close();
            notifyIcon1.Visible = false;
        }

        private void TextBox1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void OnActivated(object sender, EventArgs e)
        {
            Opacity = 1;
        }

        private void OnDeactivate(object sender, EventArgs e)
        {
            Opacity = 0.70;
        }

        private void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            //char what = e.KeyChar;

            //switch (e.KeyChar)
            //{
            //    case '\r':
            //        e.Handled = true;
            //        break;
            //}
        }

        /// <summary>
        /// Handle certain key presses to prevent audible alerts and unwanted movement of cursor.
        /// </summary>
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.Up:
                case Keys.Down:
                case Keys.Tab:
                case Keys.Enter:
                case Keys.Escape:
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    break;
                default:
                    break;
            }
        }
    }
}
