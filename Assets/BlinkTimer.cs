using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkTimer : MonoBehaviour {

    public Animator Animator;

    private float blinkCountdown;

    private void Start() {
        blinkCountdown = 0;
    }

    private void Update() {
        blinkCountdown -= Time.deltaTime;
        if (blinkCountdown < 0) {
            blinkCountdown += Random.Range(3f, 10f);
            Animator.SetTrigger("Blink");
        }
    }
}
