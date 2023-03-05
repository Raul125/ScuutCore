namespace ScuutCore.Modules.ErrorLogs
{
    using System;
    using System.Collections.Generic;
    using MEC;
    using UnityEngine.Networking;
    using Utf8Json;

    public static class WebhookSender
    {
        public static void AddMessage(string content)
        {
            content = $"<t:{((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds()}:T> " + content;
            MsgQueue.Add(content);
        }

        private static IEnumerator<float> SendMessage(Message message)
        {
            UnityWebRequest webRequest = new(ErrorLogs.Singleton.Config.Url, UnityWebRequest.kHttpVerbPOST);
            UploadHandlerRaw uploadHandler = new(JsonSerializer.Serialize(message));
            uploadHandler.contentType = "application/json";
            webRequest.uploadHandler = uploadHandler;

            yield return Timing.WaitUntilDone(webRequest.SendWebRequest());
        }

        public static IEnumerator<float> ManageQueue()
        {
            while (true)
            {
                foreach (var webhook in MsgQueue)
                    yield return Timing.WaitUntilDone(Timing.RunCoroutine(SendMessage(new Message(webhook))));

                yield return Timing.WaitForSeconds(10);
            }
        }

        private static readonly List<string> MsgQueue = new();
    }
}