
using System.Collections.Generic;
using UnityEngine;

public class VerletRoot : VerletNode {
    private Vector2 _lastPosition;
    private float _lastRotation;

    public override void ApplyMovement(float effect, int progressIndex, int totalProgress) {
        if (progressIndex >= totalProgress - 1) {
            _lastRotation = transform.eulerAngles.z;
            _lastPosition = transform.position;
        }
        
        var progress = (float)progressIndex / totalProgress;
        var diff = ((Vector2)transform.position - _lastPosition) * effect;

        Position = Vector2.Lerp(_lastPosition, transform.position, progress);
        Rotation = Mathf.LerpAngle(_lastRotation, transform.eulerAngles.z, progress);

        foreach (var branch in GetMainBranches(this)) {
            branch.Position += diff * branch.Dampening;
            branch.LastPosition += diff * branch.Dampening;
        }
    }

    private void Start() {
        _lastRotation = transform.eulerAngles.z;
        _lastPosition = transform.position;
        VerletManager.Instance.RegisterNode(this);
    }

    public IEnumerable<VerletBranch> GetMainBranches(VerletNode root) {
        foreach (var connection in root.Connections) {
            var branch = connection.Other;
            if (branch.MainConnection != connection) {
                continue;
            }

            yield return branch;
            foreach (var childBranch in GetMainBranches(branch)) {
                yield return childBranch;
            }
        }
    }
}