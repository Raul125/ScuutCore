namespace ScuutCore.Modules.ErrorLogs;

using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using PluginAPI.Core;
using PluginAPI.Loader;

[HarmonyPatch(typeof(Log), nameof(Log.Error))]
public static class ErrorPatch
{
    private static void Postfix(object message)
    {
        if (ErrorLogs.Singleton?.Config.IsEnabled ?? true)
            return;
        StackTrace stackTrace = new StackTrace();
        StackFrame[] stackFrames = stackTrace.GetFrames() ?? Array.Empty<StackFrame>();
        MethodBase? method = stackFrames.Length > 1 ? stackFrames[2].GetMethod() : null;
        string pluginInfo = "unknown";
        if (method != null && method.DeclaringType != null && AssemblyLoader.Plugins.TryGetValue(method.DeclaringType.Assembly, out var value) && value.Any())
        {
            pluginInfo = string.Join(", ", value.Select(x => x.Value.PluginName + " v" + x.Value.PluginVersion));
        }
        string caller = method != null ? $"{method.DeclaringType?.Assembly.GetName().Name} {method.DeclaringType?.FullName ?? "null"}::{method!.Name} Plugin: {pluginInfo}" : "Unknown";
        WebhookSender.AddMessage($"**{caller}**\n```{message}```");
    }
}