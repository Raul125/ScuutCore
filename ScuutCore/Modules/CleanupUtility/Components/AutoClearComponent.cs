namespace ScuutCore.Modules.CleanupUtility.Components
{
    using System;
    using UnityEngine;

    public class AutoClearComponent : MonoBehaviour
    {
        private float TimeLeft;
        private void Start()
        {
            TimeLeft = CleanupUtility.Singleton.Config.CleanDelay * 50;
        }

        private void FixedUpdate()
        {
            TimeLeft--;
            if (TimeLeft <= 0)
                Destroy(gameObject);
        }
    }
}