using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using PoGo.RocketBot.Logic;
using PoGo.RocketBot.Logic.Common;
using PoGo.RocketBot.Logic.Event;
using PoGo.RocketBot.Logic.Logging;
using PoGo.RocketBot.Logic.State;
using PoGo.RocketBot.Logic.Tasks;
using PoGo.RocketBot.Logic.Utils;

namespace PoGo.RocketBot.Window
{
    public partial class MainForm : Form
    {

        public static MainForm Instance;

        public MainForm()
        {
            InitializeComponent();
            CenterToScreen();
            Instance = this;

            

            var profilePath = Directory.GetCurrentDirectory();
            var profileConfigPath = Path.Combine(profilePath, "config");
            var configFile = Path.Combine(profileConfigPath, "config.json");

            string subPath = "";
            GlobalSettings settings;
            Boolean boolNeedsSetup = false;

            if (File.Exists(configFile))
            {
                if (!VersionCheckState.IsLatest())
                    settings = Logic.GlobalSettings.Load(subPath, true);
                else
                    settings = Logic.GlobalSettings.Load(subPath);
            }
            else
            {
                settings = new Logic.GlobalSettings();
                settings.ProfilePath = profilePath;
                settings.ProfileConfigPath = profileConfigPath;
                settings.GeneralConfigPath = Path.Combine(Directory.GetCurrentDirectory(), "config");

                boolNeedsSetup = true;
            }
            new ClientSettings(settings);
            var session = new Session(new ClientSettings(settings), new LogicSettings(settings));

            if (boolNeedsSetup)
            {
                if (Logic.GlobalSettings.PromptForSetup(session.Translation) && !settings.isGui)
                    session = Logic.GlobalSettings.SetupSettings(session, settings, configFile);
                else
                {
                    Logic.GlobalSettings.Load(subPath);

                    Logger.Write("Press a Key to continue...",
                        LogLevel.Warning);
                    Console.ReadKey();
                    return;
                }

            }

        }

        public void MainForm_Load(object sender, EventArgs e)
        {
            Text = $"RocketBot v{Assembly.GetExecutingAssembly().GetName().Version}";
        }

        public static void ColoredConsoleWrite(Color color, string text)
        {
            if (Instance.InvokeRequired)
            {
                Instance.Invoke(new Action<Color, string>(ColoredConsoleWrite), color, text);
                return;
            }
            Instance.logTextBox.Select(Instance.logTextBox.Text.Length, 1); // Reset cursor to last
            Instance.logTextBox.SelectionColor = color;
            Instance.logTextBox.AppendText(text);

        }
        public void btnRefreshPokemonList_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public void forceUnbanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public void useLuckyEggToolStripMenuItem_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void todoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var settingsForm = new SettingsForm();
            settingsForm.ShowDialog();
        }

        public void startStopBotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}