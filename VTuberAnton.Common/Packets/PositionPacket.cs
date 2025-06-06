using UnityEngine;

namespace VTuberAnton.Common.Packets;
public class PositionPacket {
    public Vector2 Position { get; set; }
    public float Rotation { get; set; }
    public Vector2 Scale { get; set; }
}
