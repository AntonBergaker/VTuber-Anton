using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoudnessBobber : MonoBehaviour
{
    public float LoudnessRotationModifier;
    public float LoudnessYModifier;

    public Transform BaseTransform;

    public float LongVolumePeriod;
    public float LongVolumeModifier;
    public float LongVolumeDampening = 1;

    public float ShortVolumePeriod;
    public float ShortVolumeModifier;
    public float ShortVolumeDampening = 1;

    private float _volume;
    private float _shortVolume;
    private float _longVolume;

    internal void SetVolume(float volume) {
        _volume = volume;
    }


    void Update() {
        _shortVolume = Mathf.Lerp(_shortVolume, _volume, Time.deltaTime / ShortVolumePeriod);
        _longVolume = Mathf.Lerp(_longVolume, _volume, Time.deltaTime / LongVolumePeriod);

        var averageVolume =
            Mathf.Log(1 + _shortVolume * ShortVolumeDampening, 2) / ShortVolumeDampening * ShortVolumeModifier + 
            Mathf.Log(1 + _longVolume * LongVolumeDampening, 2) / LongVolumeDampening * LongVolumeModifier;

        BaseTransform.localPosition = new Vector3(0, LoudnessYModifier * averageVolume, 0);
        BaseTransform.localEulerAngles = new Vector3(0, 0, LoudnessRotationModifier * averageVolume * (1 + 0.25f * Mathf.Sin(Time.time * 2)));
    }
}
