using UnityEngine;
using VTuberAnton.Common.Packets;

public class LipsyncServer : MonoBehaviour {
    public WebsocketServer server;
    private VismesBest vismesBest;
    private OVRLipSyncContext lipSync;


    public int SampleDataLength = 1024;
    private float[] clipSampleData;

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

        server.BroadcastData("lipsync", new LipsyncPacket() {
            Visemes = vismesBest.CurrentVisme,
            Volume = clipLoudness,
            Laughter = lipSync.laughterScore,
        });
    }
}
