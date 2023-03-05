namespace ScuutCore.Modules.ErrorLogs
{
    using System.Reflection;
    using HarmonyLib;
    using PluginAPI.Core;

    [HarmonyPatch(typeof(Log), nameof(Log.Error), typeof(object))]
    public static class ErrorPatch
    {
        private static void Postfix(object message)
        {
            if (ErrorLogs.Singleton.Config.IsEnabled)
                WebhookSender.AddMessage($"**{Assembly.GetCallingAssembly().GetName().Name}**\n```{message}```");
        }
    }
}