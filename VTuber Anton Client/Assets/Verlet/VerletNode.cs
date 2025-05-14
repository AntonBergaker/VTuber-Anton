
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

    public abstract void ApplyMovement(float effect);
}

[Serializable]
public class VerletConnectionData {
    public VerletNode Other;
    public float Strength = 0.5f;

    [HideInInspector]
    public Vector2 Offset;
}