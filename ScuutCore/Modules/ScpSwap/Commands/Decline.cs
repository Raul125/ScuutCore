// -----------------------------------------------------------------------
// <copyright file="Decline.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScuutCore.Modules.ScpSwap
{
    using System;
    using CommandSystem;
    using PluginAPI.Core;

    /// <summary>
    /// Rejects an active swap request.
    /// </summary>
    public class Decline : ICommand
    {
        /// <inheritdoc />
        public string Command { get; set; } = "decline";

        /// <inheritdoc />
        public string[] Aliases { get; set; } = { "no", "d" };

        /// <inheritdoc />
        public string Description { get; set; } = "Rejects an active swap request.";

        /// <inheritdoc />
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player playerSender = Player.Get(sender);
            if (playerSender == null)
            {
                response = "This command must be from the game level.";
                return false;
            }

            Swap swap = Swap.FromReceiver(playerSender);
            if (swap == null)
            {
                response = "You do not have an active swap request.";
                return false;
            }

            swap.Decline();
            response = "Swap request cancelled!";
            return true;
        }
    }
}