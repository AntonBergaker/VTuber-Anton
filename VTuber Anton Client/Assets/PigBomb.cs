using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigBomb : MonoBehaviour
{
    public float MaxX;
    public float MaxY;

    public float Speed;
    public float Duration;

    private float rotationalSpeed;

    private Vector2 speed;

    private void Awake() {
        transform.position = new Vector2(
            Random.Range(-MaxX, MaxX),
            Random.Range(-MaxY, MaxY)
        );
        rotationalSpeed = Random.Range(-30, 30);
        speed = Vector2.zero;
        while (speed.magnitude < 0.01f) {
            speed = new Vector2(
                Random.Range(-1, 1),
                Random.Range(-1, 1)
            );
        } 
        speed*=Speed;

    }

    private void Update() {
        Duration -= Time.deltaTime;
        transform.position = (Vector2)transform.position + speed * Time.deltaTime;
        transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + rotationalSpeed * Time.deltaTime);

        var x = transform.position.x;
        var y = transform.position.y;
        var xSpd = speed.x;
        var ySpd = speed.y;

        bool outside = false;

        if (x > MaxX) {
            x = MaxX;
            xSpd = -Mathf.Abs(xSpd);
            outside = true;
        }
        if (x < -MaxX) { 
            x = -MaxX;
            xSpd = Mathf.Abs(xSpd);
            outside = true;
        }
        if (y > MaxY) {
            y = MaxY;
            ySpd = -Mathf.Abs(ySpd);
            outside = true;
        }
        if (y < -MaxY) {
            y = -MaxY;
            ySpd = Mathf.Abs(-ySpd);
            outside = true;
        }

        if (outside && Duration < 0) {
            Destroy(this.gameObject);
        }

        transform.position = new Vector2(x, y);
        speed = new Vector2(xSpd, ySpd);
    }
}
