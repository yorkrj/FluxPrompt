using FluxPrompt.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        #endregion

        private readonly HotKeyHandler hotkeyHandler;
        private readonly FileLinksModel fileLinksModel;

        public MainForm()
        {
            InitializeComponent();

            notifyIcon1.Icon = SystemIcons.Asterisk;

            ResultDataGridView.ColumnCount = 2;
            ResultDataGridView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            ResultDataGridView.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            ResultDataGridView.Columns[1].Visible = false; // Column 1 holds key values.
            ResultDataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            hotkeyHandler = new HotKeyHandler();
            fileLinksModel = new FileLinksModel();

            RegisterHotKeys();
        }

        private void RegisterHotKeys()
        {
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
                    break;
                case Keys.Enter:
                    if (ResultDataGridView.Rows.Count > 0)
                    {
                        int selectedRowIndex = ResultDataGridView.SelectedRows[0].Index;
                        Guid selectedKey = (Guid)ResultDataGridView.Rows[selectedRowIndex].Cells[1].Value;
                        FileLink selectedLink = fileLinksModel.GetFileLink(selectedKey);

                        fileLinksModel.SetLaunchHistory(PromptTextBox.Text, selectedKey);

                        PromptTextBox.Clear();
                        ResultDataGridView.Rows.Clear();
                        WindowState = FormWindowState.Minimized;

                        //TODO Error handling for Process.Start since shortcuts my contain bad paths.
                        Process.Start(selectedLink.Path); //TODO Flesh this out to launch w/ paremeters and environment.
                    }

                    break;
                case Keys.Escape:
                    WindowState = FormWindowState.Minimized;
                    break;
                default:
                    ResultDataGridView.Rows.Clear();

                    if (PromptTextBox.Text.Length > 1)
                    {
                        List<FileLink> results = fileLinksModel.GetFileLinks(PromptTextBox.Text);
                        if (results.Count > 0)
                        {
                            foreach (FileLink link in results)
                            {
                                ResultDataGridView.Rows.Add(link.Name, link.Key);
                            }
                            ResultDataGridView.Rows[0].Selected = true;
                        }
                    }
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

        private void ResultDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // TODO launch selected app.
        }
    }
}
