using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class VerletManagerInstance : MonoBehaviour {
    public int Iterations = 3;

    [SerializeField]
    [HideInInspector]
    private List<VerletNode> _nodes = new();

    void Update() {
        for (int i = 0; i < Iterations; i++) {
            TickVerlet(Mathf.Clamp01(1f / Iterations), i, Iterations);
        }

        // Fix visuals
        foreach (var node in _nodes) {
            if (node is not VerletBranch branch) {
                continue;
            }

            branch.transform.SetPositionAndRotation(
                new Vector3(branch.Position.x, branch.Position.y, branch.transform.position.z),
                Quaternion.Euler(0, 0, branch.Rotation + branch.RotationOffset)
            );
        }
    }

    private void TickVerlet(float effect, int progressIndex, int totalProgress) {
        foreach (var node in _nodes) {
            node.ApplyMovement(effect, progressIndex, totalProgress);
        }

        foreach (var node in _nodes) {
            foreach (var connection in node.Connections) {
                var other = connection.Other;
                var totalWeight = other.Weight + node.Weight;

                var diff = other.Position - RotateLocalPosition(node.Rotation, connection.Offset) - node.Position;

                diff *= Mathf.Clamp01(effect * connection.Strength);
                node.Position += diff * (other.Weight / totalWeight);
                other.Position -= diff * (node.Weight / totalWeight);
            }
        }

        // Apply distance constraints
        foreach (var node in _nodes) {
            foreach (var connection in node.Connections) {
                var other = connection.Other;
                var diff = other.Position - node.Position;
                var dist = diff.magnitude;
                var expectedDist = connection.Offset.magnitude;
                var distanceWrong = dist - expectedDist;

                var totalWeight = other.Weight + node.Weight;
                var normalDiff = diff.normalized;
                var totalEffect = distanceWrong * effect * connection.DistanceStrength;

                node.Position += normalDiff * totalEffect * (other.Weight / totalWeight);
                other.Position -= normalDiff * totalEffect * (node.Weight / totalWeight);
            }
        }

        // Fix angles
        foreach (var node in _nodes) {
            if (node is not VerletRoot root) {
                continue;
            }
            FixAnglesRecursive(root);
        }
    }

    private void FixAnglesRecursive(VerletNode node) {
        foreach (var connection in node.Connections) {
            if (connection.Other.MainConnection != connection) {
                continue;
            }
            connection.Other.Rotation = AngleFromVector(connection.Other.Position - node.Position);
            FixAnglesRecursive(connection.Other);
        }
    }

    private Vector2 VectorFromAngle(float angle) {
        return new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
    }

    private float AngleFromVector(Vector2 vector) {
        return Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
    }

    private (float Distance, float Angle) GetPolarFromVector(Vector2 vector) {
        return (vector.magnitude, Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg);
    }

    private Vector2 GetVectorFromPolar(float distance, float angle) {
        return VectorFromAngle(angle) * distance;
    }

    public Vector2 RotateLocalPosition(float parentRotation, Vector2 childPosition) {
        var angle = AngleFromVector(childPosition);
        var len = childPosition.magnitude;
        return GetVectorFromPolar(len, angle + parentRotation);
    }

    private List<VerletBranch> _leftToCalculate = new();

    public void RegisterNode(VerletRoot node) {
        AddNode(node);
        DiscoverBranch(node);

        foreach (var branch in _leftToCalculate) {
            foreach (var secondaryConnection in branch.SecondaryConnections) {
                branch.Connections.Add(secondaryConnection);
                secondaryConnection.Offset = CalculateConnection(branch, secondaryConnection.Other);
            }
            
        }
        _leftToCalculate.Clear();
    }

    private void AddNode(VerletNode node) {
        node.Position = node.transform.position;
        node.Rotation = node.transform.eulerAngles.z;
        if (node is VerletBranch branch) {
            branch.LastPosition = branch.Position;
        }
        _nodes.Add(node);
    }

    private void DiscoverBranch(VerletNode node) {
        foreach (Transform child in node.transform) {
            if (!child.TryGetComponent(out VerletBranch branch)) {
                continue;
            }

            AddNode(branch);
            _leftToCalculate.Add(branch);

            var baseDiff = branch.Position - node.Position;

            var connection = new VerletConnectionData() {
                Offset = CalculateConnection(node, branch),
                Other = branch,
                Strength = branch.BaseStrength,
                DistanceStrength = branch.BaseDistanceStrength,
            };

            var rotation = AngleFromVector(baseDiff);
            branch.RotationOffset = branch.Rotation - rotation;
            branch.Rotation = rotation;
            branch.MainConnection = connection;
            node.Connections.Add(connection);

            DiscoverBranch(branch);
        }
    }

    private Vector2 CalculateConnection(VerletNode node, VerletBranch branch) {
        var baseDiff = branch.Position - node.Position;
        var polar = GetPolarFromVector(baseDiff);

        var diff = GetVectorFromPolar(polar.Distance, polar.Angle - node.Rotation);
        return diff;
    }
}

public static class VerletManager {
    private static VerletManagerInstance _instance;
    public static VerletManagerInstance Instance {
        get {
            if (_instance) {
                return _instance;
            }
            _instance = Object.FindObjectOfType<VerletManagerInstance>();
            if (_instance) {
                return _instance;
            }
            var verletObject = new GameObject();
            _instance = verletObject.AddComponent<VerletManagerInstance>();
            return _instance;
        }
    }
}