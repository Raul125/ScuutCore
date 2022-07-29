using System.Linq;
using EventManager.Api;
using Exiled.API.Features;
using GameCore;
using MEC;

namespace EventManager
{
    public class EventHandlers
    {
        private readonly MainClass plugin;
        public EventHandlers(MainClass plugin)
        {
            this.plugin = plugin;
        }

        public static EventStatus Status = EventStatus.Offline;

        public void OnWaitingForPlayers()
        {
            if (Status == EventStatus.NextRound)
            {
                StartEventLobby();
            }
        }

        public void OnStartedRound()
        {
            if (Status == EventStatus.Online)
            {
                Start();
            }
        }

        public static void StartEventLobby()
        {
            Status = EventStatus.Online;
            CustomSpawner.LobbyManager.Events = MainClass.Instance.Config.Events;
            CustomSpawner.LobbyManager.EventVotes = MainClass.Instance.Config.Events.ToDictionary(ev => ev.Key, ev => 0);
            CustomSpawner.LobbyManager.IsEvent = true;
            Map.ClearBroadcasts();
        }

        private static void Start()
        {
            RoundStart.singleton.Timer += 30;
            
            Status = EventStatus.Offline;
            if (CustomSpawner.LobbyManager.EventVotes.Count < 1)
                return;
            
            var max = CustomSpawner.LobbyManager.EventVotes.Max(x => x.Value);
            var ev = CustomSpawner.LobbyManager.EventVotes.First(x => x.Value == max).Key;
            Timing.CallDelayed(1, () => EasyEvents.API.Actions.startEvent(ev));
        }
    }
}