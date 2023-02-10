namespace ScuutCore.Modules.RainbowTags.Component
{
    using System;
    using System.Collections.Generic;
    using MEC;
    using PluginAPI.Core;
    using UnityEngine;

    public sealed class RainbowTagController : MonoBehaviour
    {
        private Player player;
        private int position;
        private float interval;
        private float intervalInFrames;
        private string[] colors;
        private CoroutineHandle coroutineHandle;

        public string[] Colors
        {
            get => colors;
            set
            {
                colors = value ?? Array.Empty<string>();
                position = 0;
            }
        }

        public float Interval
        {
            get => interval;
            set
            {
                interval = value;
                intervalInFrames = value * 50f;
            }
        }

        private void Awake()
        {
            player = Player.Get(gameObject);
        }

        private void Start()
        {
            coroutineHandle = Timing.RunCoroutine(UpdateColor().CancelWith(player.GameObject).CancelWith(this));
        }

        private void OnDestroy()
        {
            Timing.KillCoroutines(coroutineHandle);
        }

        private string RollNext()
        {
            if (++position >= colors.Length)
                position = 0;

            return colors.Length != 0 ? colors[position] : string.Empty;
        }

        private IEnumerator<float> UpdateColor()
        {
            while (true)
            {
                for (int z = 0; z < intervalInFrames; z++)
                    yield return 0f;

                string nextColor = RollNext();
                if (string.IsNullOrEmpty(nextColor))
                {
                    Destroy(this);
                    break;
                }

                player.ReferenceHub.serverRoles.SetColor(nextColor);
            }
        }
    }
}