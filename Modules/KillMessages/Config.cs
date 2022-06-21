using Exiled.API.Features;
using ScuutCore.API;
using System.ComponentModel;
using System.IO;

namespace ScuutCore.Modules.KillMessages
{
    public class Config : IModuleConfig
    {
        public bool IsEnabled { get; set; } = true;

        [Description("The folder where the database file will be stored in")]
        public string DatabaseFolder { get; set; } = Path.Combine(Paths.Plugins, "KillMessage");

        [Description("Whether or not to send a message to a player that joins to tell them about the plugin")]
        public bool SendConsoleMessage { get; set; } = true;

        [Description("Kill message's character limit")]
        public int CharLimit { get; set; } = 32;

        [Description("A list of blacklisted words")]
        public string[] BlacklistedWords { get; set; } = { "your", "blacklisted", "words", "go", "here" };

        [Description("Size of the message that's shown to the killed player")]
        public int MessageSize { get; set; } = 30;

        [Description("Duration of the message that's shown to the killed player")]
        public ushort MessageDuration { get; set; } = 3;

        [Description("If broadcasts should be used instead of hints")]
        public bool UseBroadcast { get; set; } = false;

        [Description("List of available colors. MAKE SURE TO USE SCPSL WIKI COLORS")]
        public string[] AvailableColors { get; set; } = {
            "pink", "red", "brown", "silver", "light_green", "crimson", "cyan", "aqua", "deep_pink", "tomato", "yellow", "magenta", "blue_green",
            "orange"
        };

        [Description("Whether or not to send the message if the player suicides")]
        public bool ShowOnSuicide { get; set; } = true;

        // Translations

        [Description("Console message sent to players when they join. %helpmsg will be replaced with the help message")]
        public string ConsoleMessage { get; set; } = "\n<b>KillMessage</b>\n" +
                                                     "A plugin that shows a message to players you kill\n$helpmsg";

        [Description(
            "Message shown to killer player. $message will be replaced with the message. $author will be replaced with the killer")]
        public string Message { get; set; } = "$message - <b>$author</b>";
        [Description("Help message. $current will be replaced with the current message")]
        public string HelpMessage { get; set; } = "\nUsage:\n" +
                                                  "· kmsg set - Sets your kill message\n" +
                                                  "· kmsg delete - Deletes your kill message\n" +
                                                  "· kmsg toggle - Toggles whether or not you can see kill messages\n" +
                                                  "· kmsg color - Sets your kill message color\n" +
                                                  "· Your current message and color: $current / $color";

        [Description("Message not set translation (shown on help message)")]
        public string MessageNotSet { get; set; } = "NOT SET";

        [Description("Message sent to players without permissions to use the command")]
        public string NoPerms { get; set; } = "No permission.";

        [Description("Delete command response")]
        public string DeleteCmd { get; set; } = "Kill message deleted.";

        [Description("Max characters error. $limit will be replaced with character limit")]
        public string MaxChars { get; set; } = "You can only enter up to $limit characters";

        [Description("Empty message error response")]
        public string EmptyMessage { get; set; } =
            "You can't set an empty message, if you want to delete your message, use kmsg delete";

        [Description("Set command response")]
        public string SetCmd { get; set; } = "Kill message set";

        [Description("Message shown when you try to set your message with blacklisted words")]
        public string BlacklistedWordsMessage { get; set; } = "There are blacklisted words in your message";

        [Description("Color not found error. $color will be replaced with the color")]
        public string ColorNotFound { get; set; } = "Could not find color $color";

        [Description("Color command response")]
        public string ColorCmd { get; set; } = "Color updated";

        [Description("Color empty error reponse")]
        public string ColorEmpty { get; set; } =
            "The color you're setting can't be empty, if you want to set your color to white, use default or white";

        [Description("Messages hidden response")]
        public string MessagesHidden { get; set; } = "Kill messages are now hidden";
        [Description("Messages no longer hidden response")]
        public string MessagesNotHidden { get; set; } = "Kill messages are no longer hidden";
    }
}