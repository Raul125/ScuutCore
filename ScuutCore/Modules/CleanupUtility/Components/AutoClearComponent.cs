namespace ScuutCore.Modules.CleanupUtility.Components
{
    using UnityEngine;

    public class AutoClearComponent : MonoBehaviour
    {
        public float TimeLeft;
        public bool Enabled = false;

        private void FixedUpdate()
        {
            if (!Enabled)
                return;
            TimeLeft--;
            if (TimeLeft <= 0)
                Destroy(gameObject);
        }
    }
}