
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VerletRoot : VerletNode {
    private float _lastScale;
    private Vector2 _lastPosition;
    private float _lastRotation;

    public override void ApplyMovement(float effect, int progressIndex, int totalProgress) {
        var scaleOne = GetGlobalScaleOneAxis();
        if (_lastScale != scaleOne) {
            var change = scaleOne / _lastScale;
            
            var all = GetMainBranches(this).SelectMany(x => x.Connections).ToList();
            var unique = all.Distinct().ToList();
            foreach (var connection in Connections) {
                connection.Offset *= change;
            }
            foreach (var branch in GetMainBranches(this)) {
                foreach (var connection in branch.Connections) {
                    connection.Offset *= change;
                }
            }
            _lastScale = scaleOne;
        }

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
        _lastScale = GetGlobalScaleOneAxis();
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

    private float GetGlobalScaleOneAxis() {
        var scale = transform.localScale.x;
        var parent = transform.parent;
        while (parent != null) {
            scale *= parent.localScale.x;
            parent = parent.parent;
        }
        return scale;
    }
}