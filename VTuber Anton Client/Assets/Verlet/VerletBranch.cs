using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class VerletBranch : VerletNode {
    public float BaseStrength = 0.5f;
    public float Dampening = 0;

    [HideInInspector]
    public float RotationOffset;
    [HideInInspector]
    public Vector2 LastPosition;

    public override void ApplyMovement(float effect) {
        Vector2 position = Position;
        var speed = LastPosition - position;
        LastPosition = position;
        Position += speed;
        LastPosition += speed * effect * Dampening;
    }
}
