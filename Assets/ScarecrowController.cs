using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScarecrowController : MonoBehaviour {

    public OVRLipSyncContext LipSync;
    public OVRLipSyncMicInput MicInput;

    public float LoudnessRotationModifier;
    public float LoudnessYModifier;

    public int SampleDataLength = 1024;

    public Transform BaseTransform;

    private float[] clipSampleData;

    private void Start() {
        clipSampleData = new float[SampleDataLength];
    }

    private void Update() {
        LipSync.audioSource.GetOutputData(clipSampleData, 0);
        float clipLoudness = 0f;
        foreach (var sample in clipSampleData)
        {
            clipLoudness += Mathf.Abs(sample);
        }
        clipLoudness /= SampleDataLength;


        clipLoudness = Mathf.Log(clipLoudness*3f + 1f)/3f;

        BaseTransform.transform.localPosition = new Vector3(0, LoudnessYModifier * clipLoudness, 0);
        BaseTransform.eulerAngles = new Vector3(0, 0, LoudnessRotationModifier * clipLoudness * (1+0.25f*Mathf.Sin(Time.time*2)));
    }
}
