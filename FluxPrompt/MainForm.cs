using FluxPrompt.Data;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace FluxPrompt
{
    public partial class MainForm : Form
    {
        #region Interop for MovableControls_MouseDown
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        #endregion

        HotKeyHandler hotkeyHandler;
        FileLinksModel fileLinksModel;

        public MainForm()
        {
            InitializeComponent();

            TransparencyKey = (BackColor);
            ShowInTaskbar = false;
            TopMost = true;

            notifyIcon1.Visible = true;
            notifyIcon1.Icon = SystemIcons.Asterisk;
            notifyIcon1.Text = "Flux Prompt";

            ResultDataGridView.ColumnCount = 1;
            ResultDataGridView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            ResultDataGridView.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            ResultDataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            ResultDataGridView.RowHeadersVisible = false;
            ResultDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            RegisterHotKeys();

            fileLinksModel = new FileLinksModel();
        }

        private void RegisterHotKeys()
        {
            hotkeyHandler = new HotKeyHandler();

            hotkeyHandler.HotKeyPressed += new EventHandler<HotKeyPressedEventArgs>(HandleHotKeyPressed);

            hotkeyHandler.Register(0,
                new HotKeyModifer[] { HotKeyModifer.Alt, HotKeyModifer.NoRepeat },
                Keys.Space.GetHashCode());
        }

        public void HandleHotKeyPressed(object sender, HotKeyPressedEventArgs e)
        {
            //TODO Add a switch for multiple hot key indexes here based on value of e.HotKeyIndex

            WindowState = FormWindowState.Normal;
            TopMost = true;
            Activate();
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            //e.Modifiers
            int selectedIndex;
            
            switch (e.KeyCode)
            {
                case Keys.Up:
                    if (ResultDataGridView.SelectedRows.Count > 0)
                    {
                        selectedIndex = ResultDataGridView.SelectedRows[0].Index;
                    }
                    else
                    {
                        selectedIndex = 0;
                    }

                    if (selectedIndex > 0)
                    {
                        ResultDataGridView.ClearSelection();
                        ResultDataGridView.Rows[selectedIndex - 1].Selected = true;
                    }
                    break;
                case Keys.Down:
                    if (ResultDataGridView.Rows.Count > 0)
                    {
                        selectedIndex = ResultDataGridView.SelectedRows[0].Index;

                        if (selectedIndex < ResultDataGridView.Rows.Count - 1)
                        {
                            ResultDataGridView.ClearSelection();
                            ResultDataGridView.Rows[selectedIndex + 1].Selected = true;
                        }
                    }
                    //TODO select next result row.
                    break;
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
                    
                    ResultDataGridView.Rows.Insert(0, csScript + Environment.NewLine + Convert.ToString(result));

                    e.Handled = true;

                    break;
                case Keys.Escape:
                    WindowState = FormWindowState.Minimized;
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

        /// <summary>
        /// Allow user to click and drag on controls, moving the form as if they were clicking on the window's title bar.
        /// </summary>
        private void MovableControls_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
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

        /// <summary>
        /// Handle certain key presses to prevent audible alerts and unwanted movement of cursor.
        /// </summary>
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up
                || e.KeyCode == Keys.Down
                || e.KeyCode == Keys.Tab
                || e.KeyCode == Keys.Enter
                || e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }
    }
}
