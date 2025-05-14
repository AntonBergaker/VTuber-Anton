using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;

public class LipsyncServer : MonoBehaviour {
    public WebsocketServer server;
    private VismesBest vismesBest;
    private OVRLipSyncContext lipSync;


    public int SampleDataLength = 1024;
    private float[] clipSampleData;
    private float smoothedLoudness;

    private void Awake() {
        clipSampleData = new float[SampleDataLength];
        lipSync = GetComponent<OVRLipSyncContext>();
        vismesBest = GetComponent<VismesBest>();
    }

    private void Update() {
        lipSync.audioSource.GetOutputData(clipSampleData, 0);
        float clipLoudness = 0f;
        foreach (var sample in clipSampleData) {
            clipLoudness += Mathf.Abs(sample);
        }
        clipLoudness /= SampleDataLength;

        clipLoudness = Mathf.Log(clipLoudness * 3f + 1f) / 3f;

        smoothedLoudness = Mathf.Lerp(smoothedLoudness, clipLoudness, Time.deltaTime * 5);

        server.BroadcastData("lipsync", new LipsyncData() {
            Vismes = vismesBest.CurrentVisme,
            Volume = smoothedLoudness,
            Laughter = lipSync.laughterScore,
        });
    }

    private class LipsyncData {
        public string Vismes { get; set; }
        public float Volume { get; set; }
        public float Laughter { get; set; }
    }


}
