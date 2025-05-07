using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Forms;
using FluxPrompt.Data;

namespace FluxPrompt
{
    public partial class SettingsForm : Form
    {
        private readonly AppConfig config;
        private readonly HotKeyHandler hotKeyHandler;
        private readonly TextBox jsonTextBox;

        public SettingsForm(AppConfig config, HotKeyHandler hotKeyHandler)
        {
            InitializeComponent();
            this.config = config;
            this.hotKeyHandler = hotKeyHandler;

            jsonTextBox = new TextBox
            {
                Multiline = true,
                ScrollBars = ScrollBars.Both,
                Dock = DockStyle.Fill,
                Font = new System.Drawing.Font("Consolas", 10F),
                AcceptsReturn = true,
                AcceptsTab = true,
                WordWrap = false
            };

            var buttonPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                Padding = new Padding(10)
            };

            var saveButton = new Button
            {
                Text = "Save",
                DialogResult = DialogResult.OK,
                Location = new System.Drawing.Point(buttonPanel.Width - 180, 15),
                Width = 80,
                Height = 30
            };

            var cancelButton = new Button
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Location = new System.Drawing.Point(buttonPanel.Width - 90, 15),
                Width = 80,
                Height = 30
            };

            buttonPanel.Controls.Add(saveButton);
            buttonPanel.Controls.Add(cancelButton);

            Controls.Add(jsonTextBox);
            Controls.Add(buttonPanel);

            Text = "FluxPrompt Settings";
            Size = new System.Drawing.Size(600, 400);
            FormBorderStyle = FormBorderStyle.Sizable;
            StartPosition = FormStartPosition.CenterParent;
            MinimumSize = new System.Drawing.Size(400, 300);
            TopMost = true;

            LoadConfig();

            saveButton.Click += (s, e) =>
            {
                try
                {
                    var newConfig = JsonSerializer.Deserialize<AppConfig>(jsonTextBox.Text, GetJsonOptions());
                    if (newConfig == null)
                    {
                        MessageBox.Show("Invalid JSON format", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    config.HotKeys = newConfig.HotKeys;
                    config.Save();

                    hotKeyHandler.Close();
                    hotKeyHandler.CreateHandle(new CreateParams());
                    hotKeyHandler.RegisterHotKeyFromConfig();

                    DialogResult = DialogResult.OK;
                    Close();
                }
                catch (JsonException ex)
                {
                    MessageBox.Show($"Invalid JSON: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
        }

        private void LoadConfig()
        {
            jsonTextBox.Text = JsonSerializer.Serialize(config, GetJsonOptions());
        }

        private static JsonSerializerOptions GetJsonOptions()
        {
            return new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters = 
                {
                    new JsonStringEnumConverter()
                }
            };
        }
    }
} 