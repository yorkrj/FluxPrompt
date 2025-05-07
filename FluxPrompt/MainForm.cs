using FluxPrompt.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace FluxPrompt
{
    public partial class MainForm : Form
    {
        #region Interop for MovableControls_MouseDown - Drag to move window.

        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        #endregion

        private readonly HotKeyHandler hotkeyHandler;
        private readonly FileLinksModel fileLinksModel;
        private readonly AppConfig config;

        public MainForm()
        {
            InitializeComponent();

            config = AppConfig.Load();
            hotkeyHandler = new HotKeyHandler(config);
            fileLinksModel = new FileLinksModel();

            ResultDataGridView.ColumnCount = 2;
            ResultDataGridView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            ResultDataGridView.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            ResultDataGridView.Columns[1].Visible = false; // Column 1 holds key values.
            ResultDataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            RegisterHotKeys();
            AddContextMenuToTray();
            AddHamburgerButtonToPromptPanel();

            // Center the text box vertically within its panel
            this.PromptPanel.Resize += (s, e) =>
                this.PromptTextBox.Top = (this.PromptPanel.Height - this.PromptTextBox.Height) / 2;
            // Initial centering
            this.PromptTextBox.Top = (this.PromptPanel.Height - this.PromptTextBox.Height) / 2;
        }

        private void AddHamburgerButtonToPromptPanel()
        {
            var hamburgerButton = new Button();
            hamburgerButton.Name = "HamburgerButton";
            hamburgerButton.Text = "\u2630";
            hamburgerButton.FlatStyle = FlatStyle.Flat;
            hamburgerButton.FlatAppearance.BorderSize = 0;
            hamburgerButton.Font = new System.Drawing.Font("Segoe UI Variable", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            hamburgerButton.ForeColor = System.Drawing.Color.White;
            hamburgerButton.BackColor = System.Drawing.Color.Transparent;
            hamburgerButton.Size = new System.Drawing.Size(30, 30);
            hamburgerButton.Location = new System.Drawing.Point(this.PromptPanel.Width - 40, (this.PromptPanel.Height - hamburgerButton.Height) / 2);
            hamburgerButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.PromptPanel.Controls.Add(hamburgerButton);

            var hamburgerMenu = new ContextMenuStrip();
            var helpItem = new ToolStripMenuItem("Help");
            helpItem.Click += (s, e) => ShowHelpWindow();
            var settingsItem = new ToolStripMenuItem("Settings");
            settingsItem.Click += (s, e) => ShowSettingsDialog();
            var exitItem = new ToolStripMenuItem("Exit");
            exitItem.Click += (s, e) => this.Close();
            hamburgerMenu.Items.Add(helpItem);
            hamburgerMenu.Items.Add(settingsItem);
            hamburgerMenu.Items.Add(exitItem);

            hamburgerButton.Click += (s, e) => hamburgerMenu.Show(hamburgerButton, new System.Drawing.Point(0, hamburgerButton.Height));
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {            
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    LaunchApplication(e);
                    break;
                case Keys.Up:
                    SelectPreviousResult();
                    break;
                case Keys.Down:
                    SelectNextResult();
                    break;
                case Keys.Escape:
                    HideMainWindow();
                    break;
                default:
                    UpdateResults();
                    break;
            }
        }

        public void HandleHotKeyPressed(object sender, HotKeyPressedEventArgs e)
        {
            //TODO Add a switch for multiple hot key indexes here based on value of e.HotKeyIndex

            WindowState = FormWindowState.Normal;
            TopMost = true;
            PromptTextBox.Focus();
            Activate();
        }

        private void RegisterHotKeys()
        {
            hotkeyHandler.HotKeyPressed += new EventHandler<HotKeyPressedEventArgs>(HandleHotKeyPressed);

            hotkeyHandler.Register(0,
                new HotKeyModifer[] { HotKeyModifer.Alt, HotKeyModifer.NoRepeat },
                Keys.Space.GetHashCode());
        }

        private void AddContextMenuToTray()
        {
            var helpMenuItem = new ToolStripMenuItem("Help");
            helpMenuItem.Click += (s, e) => ShowHelpWindow();

            var restoreMenuItem = new ToolStripMenuItem("Restore");
            restoreMenuItem.Click += (s, e) => {
                WindowState = FormWindowState.Normal;
                TopMost = true;
                PromptTextBox.Focus();
                Activate();
            };

            var exitMenuItem = new ToolStripMenuItem("Exit");
            exitMenuItem.Click += (s, e) => this.Close();
            
            var trayContextMenu = new ContextMenuStrip();
            trayContextMenu.Items.Add(helpMenuItem);
            trayContextMenu.Items.Add(restoreMenuItem);
            trayContextMenu.Items.Add(new ToolStripSeparator());
            trayContextMenu.Items.Add(exitMenuItem);
            
            this.notifyIcon1.ContextMenuStrip = trayContextMenu;
        }

        /// <summary>
        /// Displays a basic help window with usage instructions.
        /// </summary>
        private void ShowHelpWindow()
        {
            string helpText = "FluxPrompt Usage:\n" +
                              "- Alt+Space: Open FluxPrompt.\n" +
                              "- Up/Down: Navigate results.\n" +
                              "- Enter: Launch selected app.\n" +
                              "- Alt+Enter: Launch as administrator.\n" +
                              "- Escape: Minimize to tray.\n" +
                              "- Right-click tray icon: Access Help or Exit.\n";
            MessageBox.Show(helpText, "FluxPrompt Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ShowSettingsDialog()
        {
            using var settingsForm = new SettingsForm(config, hotkeyHandler);
            settingsForm.ShowDialog(this);
        }

        private void HideMainWindow()
        {
            WindowState = FormWindowState.Minimized;
        }

        private void UpdateResults()
        {
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
        }

        private void LaunchApplication(KeyEventArgs e)
        {
            if (ResultDataGridView.Rows.Count > 0)
            {
                bool altPressed = e.Modifiers == Keys.Alt;
                LaunchApplication(altPressed);
            }
        }

        private void SelectNextResult()
        {
            if (ResultDataGridView.Rows.Count > 0)
            {
                int selectedIndex = ResultDataGridView.SelectedRows[0].Index;

                if (selectedIndex < ResultDataGridView.Rows.Count - 1)
                {
                    ResultDataGridView.ClearSelection();
                    ResultDataGridView.Rows[selectedIndex + 1].Selected = true;
                }
            }
        }

        private int SelectPreviousResult()
        {
            int selectedIndex;
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

            return selectedIndex;
        }

        private void LaunchApplication(bool RunAsAdministrator)
        {
            int selectedRowIndex = ResultDataGridView.SelectedRows[0].Index;
            Guid selectedKey = (Guid)ResultDataGridView.Rows[selectedRowIndex].Cells[1].Value;
            FileLink selectedLink = fileLinksModel.GetFileLink(selectedKey);

            fileLinksModel.SetLaunchHistory(PromptTextBox.Text, selectedKey);

            ProcessStartInfo startInfo = new()
            {
                FileName = selectedLink.Path,
                Arguments = selectedLink.Arguments,
                WorkingDirectory = Environment.ExpandEnvironmentVariables(selectedLink.WorkingDirectory),
                UseShellExecute = true
            };

            if (RunAsAdministrator)
            {
                startInfo.Verb = "runas";
            }

            try
            {
                Process.Start(startInfo);
            }
            catch
            {
                // TODO Standardize error handling.
                MessageBox.Show(
                    this,
                    "Could not start this application.\nFluxPrompt is a work in progress.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            PromptTextBox.Clear();
            ResultDataGridView.Rows.Clear();
            WindowState = FormWindowState.Minimized;
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
        /// Suppress certain key presses to prevent audible alerts and unwanted movement of cursor.
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

        private void ResultDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            LaunchApplication(false);
        }
    }
}
