using System;
using DSharp4Webhook.Core;
using DSharp4Webhook.Core.Constructor;
using Exiled.API.Features;

namespace ScuutCore.Modules.TpsLogger
{
    public class WebhookController : IDisposable
    {
        private static readonly EmbedBuilder EmbedBuilder = ConstructorProvider.GetEmbedBuilder();
        private static readonly EmbedFieldBuilder FieldBuilder = ConstructorProvider.GetEmbedFieldBuilder();
        private static readonly MessageBuilder MessageBuilder = ConstructorProvider.GetMessageBuilder();
        private readonly TpsLogger plugin;
        private readonly IWebhook webhook;
        private bool isDisposed;

        public WebhookController(TpsLogger plugin)
        {
            this.plugin = plugin;
            webhook = WebhookProvider.CreateStaticWebhook(plugin.Config.Url);
        }

        public void Dispose()
        {
            isDisposed = true;
            webhook?.Dispose();
        }

        public void SendTps()
        {
            if (isDisposed)
                throw new ObjectDisposedException(nameof(WebhookController));

            MessageBuilder messageBuilder = PrepareMessage();
            if (messageBuilder is null)
                return;

            webhook.SendMessage(messageBuilder.Build()).Queue((_, isSuccessful) =>
            {
                if (!isSuccessful)
                    Log.Warn("Failed to send the tps webhook.");
            });
        }

        private static string Codeline(object toFormat) => $"```{toFormat}```";

        private MessageBuilder PrepareMessage()
        {
            if (isDisposed)
                throw new ObjectDisposedException(nameof(WebhookController));

            EmbedBuilder.Reset();
            FieldBuilder.Reset();
            MessageBuilder.Reset();

            FieldBuilder.Inline = false;

            FieldBuilder.Name = plugin.Config.Players ?? "Players";
            FieldBuilder.Value = Codeline($"{Server.PlayerCount}/{Server.MaxPlayerCount}");
            EmbedBuilder.AddField(FieldBuilder.Build());

            FieldBuilder.Name = plugin.Config.Tps ?? "TPS";
            FieldBuilder.Value = Codeline(Server.Tps);
            EmbedBuilder.AddField(FieldBuilder.Build());

            if (!string.IsNullOrEmpty(plugin.Config.Header))
                EmbedBuilder.Title = plugin.Config.Header;

            if (!string.IsNullOrEmpty(plugin.Config.Color))
                EmbedBuilder.Color = (uint)DSharp4Webhook.Util.ColorUtil.FromHex(plugin.Config.Color);

            EmbedBuilder.Timestamp = DateTimeOffset.UtcNow;
            MessageBuilder.AddEmbed(EmbedBuilder.Build());

            return MessageBuilder;
        }
    }
}