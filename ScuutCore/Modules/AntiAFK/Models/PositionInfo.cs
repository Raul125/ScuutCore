namespace ScuutCore.Modules.AntiAFK.Models
{
    using System;
    using PluginAPI.Core;
    using UnityEngine;

    public readonly struct PositionInfo : IEquatable<PositionInfo>
    {
        private readonly Vector3 position;
        private readonly Vector2 rotation;
        private readonly Quaternion cameraRotation;

        public PositionInfo(Player player)
        {
            position = player.Position;
            rotation = player.Rotation;
            cameraRotation = player.Camera.rotation;
        }

        public static bool operator ==(PositionInfo x, PositionInfo y) => x.Equals(y);

        public static bool operator !=(PositionInfo x, PositionInfo y) => !(x == y);

        public bool Equals(PositionInfo other)
        {
            return position.Equals(other.position) &&
                   rotation.Equals(other.rotation) &&
                   cameraRotation.Equals(other.cameraRotation);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            return obj is PositionInfo other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = position.GetHashCode();
                hashCode = (hashCode * 397) ^ rotation.GetHashCode();
                hashCode = (hashCode * 397) ^ cameraRotation.GetHashCode();
                return hashCode;
            }
        }
    }
}