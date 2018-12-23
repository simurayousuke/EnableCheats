using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;

namespace EnableCheats
{

    public class ModEntry : Mod
    {
        public static ModConfig Config;
        private static bool cheat = true;

        private ITranslationHelper i18n;
        private static string STARTUP_INFO_ENABLED = "The cheat mode is enabled , you can type cheats into the console to toggle cheat mode.";
        private static string STARTUP_INFO_DISABLED = "The cheat mode is disabled , you can type cheats into the console to toggle cheat mode.";
        private static string TOGGLE_ENABLED = "The cheat mode is now enabled.";
        private static string TOGGLE_DISABLED = "The cheat mode is now disabled.";
        private static string ILEGAL_ARGUMENT = "Ilegal argument. Use cheats on/off to switch the cheat mode or use cheats to toggle.";
        private static string CONSOLE_COMMAND_DESCRIPTION =  "Toggle cheat mode";
        private static string NOT_INGAME_EXCEPTION = "Toggle failed, please do again when the game is already loaded.";
        private static string UNKNOWN_EXCEPTION = "Toggle failed, unknown exception.";


        public override void Entry(IModHelper helper)
        {
            Config = Helper.ReadConfig<ModConfig>();
            cheat = Config.EnableCheats;

            /*
             * Chinese and Japanese characters will be shown as '?' in the console, so I can't enable i18n before solving that problem.
             * 
            i18n = Helper.Translation;
            STARTUP_INFO_ENABLED = i18n.Get("StartupInfoEnabled");
            STARTUP_INFO_DISABLED = i18n.Get("StartupInfoDisabled");
            TOGGLE_ENABLED = i18n.Get("ToggleEnabled");
            TOGGLE_DISABLED = i18n.Get("ToggleDisabled");
            ILEGAL_ARGUMENT = i18n.Get("IlegalArgument");
            CONSOLE_COMMAND_DESCRIPTION = i18n.Get("ConsoleCommandDescription");
            NOT_INGAME_EXCEPTION = i18n.Get("NotIngameException");
            UNKNOWN_EXCEPTION = i18n.Get("UnknownException");
            */

            SaveEvents.AfterLoad += Init;

            helper.ConsoleCommands.Add("cheats", CONSOLE_COMMAND_DESCRIPTION, Switch);

            //prepare for i18n
            string info = cheat? STARTUP_INFO_ENABLED : STARTUP_INFO_DISABLED;
            Monitor.Log(info, LogLevel.Warn);
        }

        private void Init(object sender, System.EventArgs e)
        {
            try
            {
                StardewValley.Game1.chatBox.enableCheats = cheat;
            }
            catch
            {
                Monitor.Log(UNKNOWN_EXCEPTION, LogLevel.Error);
            }
            finally
            {
                string info = cheat ? TOGGLE_ENABLED : TOGGLE_DISABLED;
                Monitor.Log(info, LogLevel.Info);
            }

        }

        private void ToggleCheat(bool enable)
        {
            try
            {
                if(Game1.gameMode!=(byte)3)
                {
                    Monitor.Log(NOT_INGAME_EXCEPTION, LogLevel.Error);
                    return;
                }
                StardewValley.Game1.chatBox.enableCheats = enable;
                cheat = enable;
            }
            catch
            {
                Monitor.Log(UNKNOWN_EXCEPTION, LogLevel.Error);
            }
            finally
            {
                string info = cheat ? TOGGLE_ENABLED : TOGGLE_DISABLED;
                Monitor.Log(info, LogLevel.Info);
            }
            
        }

        private void Switch(string command, string[] args)
        {
            bool enable = false;
            if (args.Length > 0)
            {
                switch (args[0])
                {
                    case "on": case "true": case "1":
                        enable = true;
                        break;
                    case "off": case "false": case "0":
                        enable = false;
                        break;
                    default:
                        Monitor.Log(ILEGAL_ARGUMENT, LogLevel.Warn);
                        break;
                }
            }
            else
            {
                enable = !cheat;
            }
            ToggleCheat(enable);
        }

    }
}