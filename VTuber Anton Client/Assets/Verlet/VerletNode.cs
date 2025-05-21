
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class VerletNode : MonoBehaviour {
    public float Weight = 1;

    [HideInInspector]
    public Vector2 Position;
    [HideInInspector]
    public float Rotation;

    [HideInInspector]
    public List<VerletConnectionData> Connections = new();

    public abstract void ApplyMovement(float effect, int progressIndex, int totalProgress);
}

[Serializable]
public class VerletConnectionData {
    public VerletBranch Other;
    public float Strength = 0.5f;
    public float DistanceStrength = 0f;

    [HideInInspector]
    public Vector2 Offset;
}