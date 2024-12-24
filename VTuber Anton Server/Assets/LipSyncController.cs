using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class LipSyncController : MonoBehaviour {

    private OVRLipSyncMicInput micInput;
    private string lastSavedDevice = "";

    private string filePath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AntonVTuber", "mic.txt");

    private int pollMicTimer = 2;

    void Start()
    {
        micInput = GetComponent<OVRLipSyncMicInput>();
    }

    void Update() {
        pollMicTimer--;
        if (pollMicTimer == 0) {
            if (File.Exists(filePath))
            {
                string newMic = File.ReadAllText(filePath);
                if (Microphone.devices.Contains(newMic)) {
                    micInput.SetMic(newMic);
                }
                
            }

            lastSavedDevice = micInput.selectedDevice;
        }

        if (pollMicTimer < -2) {
            if (micInput.selectedDevice != lastSavedDevice) {
                string directoryName = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directoryName)) {
                    Directory.CreateDirectory(directoryName);
                }

                File.WriteAllText(filePath, micInput.selectedDevice);
                lastSavedDevice = micInput.selectedDevice;
            }
        }
    }

}