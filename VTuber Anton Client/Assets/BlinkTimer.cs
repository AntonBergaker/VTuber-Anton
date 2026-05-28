using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkTimer : MonoBehaviour {

    public Animator Animator;
    public float HappyStartThreshold = 0.4f;
    public float HappyEndThreshold = 0.2f;

    public string HappyParameterString = "HappyClose";
    public string BlinkParameterString = "Blink";

    private float blinkCountdown;
    

    public void SetHappy(float happyValue) {
        if (happyValue > 0.4f) {
            Animator.SetBool(HappyParameterString, true);
        }
        if (happyValue < 0.1f) {
            Animator.SetBool(HappyParameterString, false);
        }
    }

    private void Start() {
        blinkCountdown = 0;
    }

    private void Update() {
        blinkCountdown -= Time.deltaTime;
        if (blinkCountdown < 0) {
            blinkCountdown += Random.Range(3f, 10f);
            Animator.SetTrigger(BlinkParameterString);
        }
    }
}
