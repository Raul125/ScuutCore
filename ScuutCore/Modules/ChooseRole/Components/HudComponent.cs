using GameCore;
using Hints;
using NorthwoodLib.Pools;
using PluginAPI.Core;
using UnityEngine;
using StringBuilder = System.Text.StringBuilder;

namespace ScuutCore.Modules.ChooseRole
{
    public class HudComponent : MonoBehaviour
    {
        public string Team = ChooseRole.Singleton.Config.Random;
        private ReferenceHub _hub;

        public void Init(ReferenceHub hub)
        {
            _hub = hub;
        }

        public void SetTeam(string team)
        {
            Team = team;
        }

        private float _counter;

        private void Update()
        {
            _counter += Time.deltaTime;

            if (_counter < 0.5f)
                return;

            if (Round.IsRoundStarted)
            {
                _hub.hints.Show(new TextHint(string.Empty));
                Destroy(this);
                return;
            }

            _counter = 0;
            var text = Build();
            _hub.hints.Show(new TextHint(text, new HintParameter[] { new StringHintParameter(text) }, null, 4));
        }

        private string Build()
        {
            StringBuilder builder = StringBuilderPool.Shared.Rent();
            builder.AppendLine(ChooseRole.Singleton.Config.StartingSoon);
            builder.AppendLine(GetStatus());
            int count = Player.Count;

            builder.AppendLine(count == 1
                ? ChooseRole.Singleton.Config.PlayerHasConnected
                : ChooseRole.Singleton.Config.PlayersHaveConnected.Replace("%players%", count.ToString()));

            builder.Append("\n\n\n\n\n\n\n\n\n\n\n\n\n");
            builder.AppendLine(ChooseRole.Singleton.Config.SelectedTeam);
            builder.AppendLine("<size=60>" + Team + "</size>");
            builder.Append("\n\n\n");
            builder.AppendLine(ChooseRole.Singleton.Config.DiscordInv);
            builder.AppendLine(ChooseRole.Singleton.Config.UpperText);
            builder.AppendLine(ChooseRole.Singleton.Config.BottomText);
            builder.Append("\n\n\n\n\n\n\n");

            return StringBuilderPool.Shared.ToStringReturn(builder);
        }

        private static string GetStatus()
        {
            short timer = RoundStart.singleton.NetworkTimer;
            return timer switch
            {
                -2 => ChooseRole.Singleton.Config.ServerPaused,
                -1 => ChooseRole.Singleton.Config.StartingRound,
                0 => ChooseRole.Singleton.Config.StartingRound,
                _ => ChooseRole.Singleton.Config.SecondsLeft.Replace("%seconds", timer.ToString())
            };
        }
    }
}