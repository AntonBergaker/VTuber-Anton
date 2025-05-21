
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
        Position = Vector2.Lerp(_lastPosition, transform.position, progress);
        Rotation = Mathf.LerpAngle(_lastRotation, transform.eulerAngles.z, progress);
    }

    private void Start() {
        _lastRotation = transform.eulerAngles.z;
        _lastPosition = transform.position;
        VerletManager.Instance.RegisterNode(this);
    }
}