using UnityEngine;
using VTuberAnton.Common.Packets;

public class ObjectPositioner : MonoBehaviour
{
    public WebsocketServer Server;
    public string Channel;

    private Vector3 _lastPosition;
    private Vector3 _lastScale;
    private float _lastRotation;

    // Start is called before the first frame update
    void Start() {
        UpdateLastPositions();
    }

    private void UpdateLastPositions() {
        _lastPosition = transform.position;
        _lastScale = transform.lossyScale;
        _lastRotation = transform.eulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (_lastPosition != transform.position ||
            _lastScale != transform.localScale ||
            _lastRotation != transform.eulerAngles.z) {
            
            UpdateLastPositions();

            var packet = new PositionPacket() {
                Position = transform.position,
                Scale = transform.lossyScale,
                Rotation = transform.eulerAngles.z,
            };
            
            Server.BroadcastData(Channel, packet);
        }
    }
}
