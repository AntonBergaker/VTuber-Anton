using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScarecrowController : MonoBehaviour, IPacketListener<ScarecrowController.LipsyncData> {
    public WebsocketClient Client;
    public Mouth Mouth;

    public float LoudnessRotationModifier;
    public float LoudnessYModifier;

    public Transform BaseTransform;

    private float volume = 0;

    void IPacketListener<LipsyncData>.HandlePacket(LipsyncData packet) {
        Mouth.SetMouthShape(packet.Vismes);
        volume = packet.Volume;
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

    private void Update() {
        

        BaseTransform.transform.localPosition = new Vector3(0, LoudnessYModifier * volume, 0);
        BaseTransform.eulerAngles = new Vector3(0, 0, LoudnessRotationModifier * volume * (1+0.25f*Mathf.Sin(Time.time*2)));
    }

    private class LipsyncData {
        public string Vismes { get; set; }
        public float Volume { get; set; }
    }

}