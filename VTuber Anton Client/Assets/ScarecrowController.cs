using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScarecrowController : MonoBehaviour {

    public float LoudnessRotationModifier;
    public float LoudnessYModifier;

    public int SampleDataLength = 1024;

    public Transform BaseTransform;

    private float[] clipSampleData;

    private float smoothedLoudness;

    private void Start() {
        clipSampleData = new float[SampleDataLength];
    }

    private void Update() {
        //LipSync.audioSource.GetOutputData(clipSampleData, 0);
        float clipLoudness = 0f;
        foreach (var sample in clipSampleData)
        {
            clipLoudness += Mathf.Abs(sample);
        }
        clipLoudness /= SampleDataLength;

        clipLoudness = Mathf.Log(clipLoudness*3f + 1f)/3f;

        smoothedLoudness = Mathf.Lerp(smoothedLoudness, clipLoudness, Time.deltaTime * 5);

        BaseTransform.transform.localPosition = new Vector3(0, LoudnessYModifier * smoothedLoudness, 0);
        BaseTransform.eulerAngles = new Vector3(0, 0, LoudnessRotationModifier * smoothedLoudness * (1+0.25f*Mathf.Sin(Time.time*2)));
    }
}
