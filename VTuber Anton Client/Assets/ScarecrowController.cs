using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScarecrowController : MonoBehaviour, IPacketListener<ScarecrowController.LipsyncData> {
    public WebsocketClient Client;
    public Mouth Mouth;
    public BlinkTimer Eyes;

    [SerializeField]
    private LoudnessBobber loudnessBobber;

    void IPacketListener<LipsyncData>.HandlePacket(LipsyncData packet) {
        Mouth.SetMouthShape(packet.Vismes);
        Eyes.SetHappy(packet.Laughter);
        loudnessBobber.SetVolume(packet.Volume);
    }

    private void Start() {
        Client.Listen("lipsync", this);


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


    private class LipsyncData {
        public string Vismes { get; set; }
        public float Volume { get; set; }
        public float Laughter { get; set; }
    }

}