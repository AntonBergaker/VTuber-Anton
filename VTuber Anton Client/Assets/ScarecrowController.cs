using System;
using UnityEngine;
using VTuberAnton.Common.Packets;

public class ScarecrowController : MonoBehaviour, IPacketListener<LipsyncPacket>, IPacketListener<PositionPacket> {
    public WebsocketClient Client;
    public Mouth Mouth;
    public BlinkTimer Eyes;

    [SerializeField]
    private LoudnessBobber loudnessBobber;

    void IPacketListener<LipsyncPacket>.HandlePacket(LipsyncPacket packet) {
        Mouth.SetMouthShape(packet.Visemes);
        Eyes.SetHappy(packet.Laughter);
        loudnessBobber.SetVolume(packet.Volume);
    }

    void IPacketListener<PositionPacket>.HandlePacket(PositionPacket packet) {
        transform.position = new Vector3(packet.Position.x, packet.Position.y, transform.position.z);
        transform.localScale = new Vector3(packet.Scale.x, packet.Scale.y, transform.localScale.z);
        transform.eulerAngles = new Vector3(0, 0, packet.Rotation);
    }

    private void Start() {
        Client.Listen<LipsyncPacket>("lipsync", this);
        Client.Listen<PositionPacket>("scare_position", this);


        if (string.IsNullOrWhiteSpace(Application.absoluteURL)) {
            return;
        }
        var query = new Uri(Application.absoluteURL).Query;
        if (string.IsNullOrWhiteSpace(query)) {
            return;
        }
        query = query.TrimStart('?');

        var pairs = query.Split('&');
        foreach (var pair in pairs ) {
            var index = pair.IndexOf('=');
            if (index <= 0) {
                continue;
            }
            var key = pair[..index];
            var value = pair[(index+1)..];
            Debug.Log(key + value);
            if (key == "x") {
                transform.position = new(float.Parse(value), transform.position.y);
            }
            else if (key == "y") {
                transform.position = new(transform.position.x, float.Parse(value));
            }
            else if (key == "scale") {
                transform.localScale = Vector3.one * float.Parse(value);
            }
        }
    }

}