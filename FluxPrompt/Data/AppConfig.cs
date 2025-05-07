using System;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace FluxPrompt.Data
{
    public class AppConfig
    {
        private static readonly string configPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "FluxPrompt",
            "config.json");

        public HotKeyConfig HotKeys { get; set; } = new HotKeyConfig();

        public static AppConfig Load()
        {
            try
            {
                if (File.Exists(configPath))
                {
                    string json = File.ReadAllText(configPath);
                    return JsonSerializer.Deserialize<AppConfig>(json) ?? new AppConfig();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error loading config file. Please check your FluxPrompt directory.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return new AppConfig();
        }

        public void Save()
        {
            try
            {
                string directory = Path.GetDirectoryName(configPath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                string json = JsonSerializer.Serialize(this, new JsonSerializerOptions 
                { 
                    WriteIndented = true 
                });
                File.WriteAllText(configPath, json);
            }
            catch (Exception)
            {
                MessageBox.Show("Error saving config file. Please check your FluxPrompt directory.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public class HotKeyConfig
    {
        public HotKeyModifier[] Modifiers { get; set; } = new[] { HotKeyModifier.Alt, HotKeyModifier.NoRepeat };
        public Keys Key { get; set; } = Keys.Space;
    }

    public enum HotKeyModifier
    {
        Alt = 0x0001,
        Control = 0x0002,
        Shift = 0x0004,
        Win = 0x0008,
        NoRepeat = 0x4000
    }
} 