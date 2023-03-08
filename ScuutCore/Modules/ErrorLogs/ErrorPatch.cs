namespace ScuutCore.Modules.ErrorLogs
{
    using HarmonyLib;
    using PluginAPI.Core;

    [HarmonyPatch(typeof(Log), nameof(Log.Error))]
    public static class ErrorPatch
    {
        private static void Postfix(object message)
        {
            if (ErrorLogs.Singleton.Config.IsEnabled)
                WebhookSender.AddMessage($"**ScuutCore**\n```{message}```");
        }
    }
}