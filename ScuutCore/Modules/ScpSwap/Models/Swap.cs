// -----------------------------------------------------------------------
// <copyright file="Swap.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScuutCore.Modules.ScpSwap
{
    using System.Collections.Generic;
    using PluginAPI.Core;
    using Exiled.Events.EventArgs;
    using Exiled.Events.EventArgs.Player;
    using MEC;
    using PlayerRoles;
    using PluginAPI.Core;
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;
    using PluginAPI.Events;

    /// <summary>
    /// Handles the swapping of players.
    /// </summary>
    public class Swap
    {
        private static readonly List<Swap> Swaps = new List<Swap>();
        private static readonly List<CoroutineHandle> Coroutines = new List<CoroutineHandle>();
        private CoroutineHandle coroutine;

        private Swap(Player sender, Player receiver)
        {
            Sender = sender;
            Receiver = receiver;

            SendRequestMessages();
            coroutine = Timing.RunCoroutine(RunTimeout());
            Coroutines.Add(coroutine);
            EventManager.RegisterEvents(this);
        }

        /// <summary>
        /// Gets the sender of the swap request.
        /// </summary>
        public Player Sender { get; }

        /// <summary>
        /// Gets the person who was sent the swap request.
        /// </summary>
        public Player Receiver { get; }

        /// <summary>
        /// Gets a swap request based on the sender.
        /// </summary>
        /// <param name="player">The sender of the request.</param>
        /// <returns>The sent swap request or null if one isn't found.</returns>
        public static Swap FromSender(Player player)
        {
            foreach (Swap swap in Swaps)
            {
                if (swap.Sender == player)
                    return swap;
            }

            return null;
        }

        /// <summary>
        /// Gets a swap request based on the receiver.
        /// </summary>
        /// <param name="player">The receiver of the request.</param>
        /// <returns>The sent swap request or null if one isn't found.</returns>
        public static Swap FromReceiver(Player player)
        {
            foreach (Swap swap in Swaps)
            {
                if (swap.Receiver == player)
                    return swap;
            }

            return null;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Swap"/> class with the given Sender and Receiver.
        /// </summary>
        /// <param name="sender">The sender of the request.</param>
        /// <param name="receiver">The receiver of the request.</param>
        public static void Send(Player sender, Player receiver)
        {
            Swaps.Add(new Swap(sender, receiver));
        }

        /// <summary>
        /// Clears all active swap requests.
        /// </summary>
        public static void Clear()
        {
            Swaps.Clear();
            foreach (CoroutineHandle coroutine in Coroutines)
                Timing.KillCoroutines(coroutine);

            Coroutines.Clear();
        }

        /// <summary>
        /// Performs the swap between the <see cref="Sender"/> and the <see cref="Receiver"/>.
        /// </summary>
        public void Run()
        {
            PartiallyDestroy();
            SwapData senderData = new SwapData(Sender);
            SwapData receiverData = new SwapData(Receiver);

            senderData.Swap(Receiver);
            receiverData.Swap(Sender);

            ScpSwap.Singleton.Config.SwapSuccessful.SendTo(Sender);
            ScpSwap.Singleton.Config.SwapSuccessful.SendTo(Receiver);
            Swaps.Remove(this);
        }

        /// <summary>
        /// Broadcasts the swap cancellation then destroys the swap.
        /// </summary>
        public void Cancel()
        {
            Sender.SendBroadcast("Swap request cancelled!",5, shouldClearPrevious: true);
            Destroy();
        }

        /// <summary>
        /// Broadcasts the swap decline then destroys the swap.
        /// </summary>
        public void Decline()
        {
            Sender.SendBroadcast($"{Receiver.DisplayNickname ?? Receiver.Nickname} has declined your swap request.",5, shouldClearPrevious: true);
            Destroy();
        }

        private void PartiallyDestroy()
        {
            if (coroutine.IsRunning)
                Timing.KillCoroutines(coroutine);

            EventManager.UnregisterEvents(this);
        }

        private void Destroy()
        {
            PartiallyDestroy();
            Swaps.Remove(this);
        }

        private void SendRequestMessages()
        {
            string consoleMessage = ScpSwap.Singleton.Config.RequestConsoleMessage.Message;
            consoleMessage = consoleMessage.Replace("$SenderName", Sender.DisplayNickname ?? Sender.Nickname);
            consoleMessage = consoleMessage.Replace("$RoleName", ValidSwaps.GetCustom(Sender)?.Name ?? Sender.Role.ToString());
            Receiver.SendConsoleMessage(consoleMessage, ScpSwap.Singleton.Config.RequestConsoleMessage.Color);
            Receiver.SendBroadcast(ScpSwap.Singleton.Config.RequestBroadcast);
        }

        [PluginEvent(ServerEventType.PlayerChangeRole)]
        private void OnChangingRole(Player player, PlayerRoleBase oldRole, RoleTypeId newRole, RoleChangeReason changeReason)
        {
            if (player == Sender || player == Receiver)
                Cancel();
        }

        private IEnumerator<float> RunTimeout()
        {
            yield return Timing.WaitForSeconds(ScpSwap.Singleton.Config.RequestTimeout);
            ScpSwap.Singleton.Config.TimeoutSender.SendTo(Sender);
            ScpSwap.Singleton.Config.TimeoutReceiver.SendTo(Receiver);
            Destroy();
        }
    }
}