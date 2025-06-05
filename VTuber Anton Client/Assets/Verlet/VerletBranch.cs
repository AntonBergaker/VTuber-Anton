using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Serialization;

public class VerletBranch : VerletNode {
    public float BaseStrength = 0.5f;
    public float BaseDistanceStrength = 0f;
    public float Inertia = 0;
    public float Dampening = 0;

    public List<VerletConnectionData> SecondaryConnections = new();

    [HideInInspector]
    public float RotationOffset;
    [HideInInspector]
    public Vector2 LastPosition;

    [HideInInspector]
    public VerletConnectionData MainConnection;

    public override void ApplyMovement(float effect, int progressIndex, int totalProgress) {
        Vector2 position = Position;
        var speed = LastPosition - position;
        LastPosition = position;
        Position += speed;
        LastPosition += speed * Mathf.Clamp01(effect * Inertia);
    }
}
