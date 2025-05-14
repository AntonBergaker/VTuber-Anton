
using UnityEngine;

public class VerletRoot : VerletNode {
    public override void ApplyMovement(float effect) {
        Position = transform.position;
        Rotation = transform.eulerAngles.z;
    }

    private void Start() {
        VerletManager.Instance.RegisterNode(this);
    }
}