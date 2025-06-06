using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigBomb : MonoBehaviour
{
    public Camera Camera;

    public float Speed;
    public float Duration;

    private float _rotationalSpeed;
    private float _timeOutside = 0;

    private Vector2 _speed;

    private Rect _rectangle;

    private void Awake() {
        var camSize = new Vector2(Camera.orthographicSize * Screen.width / Screen.height, Camera.orthographicSize);
        _rectangle = new Rect((Vector2)Camera.transform.position - camSize, camSize*2);

        transform.position = _rectangle.position + new Vector2(
            Random.Range(0, _rectangle.size.x),
            Random.Range(0, _rectangle.size.y)
        );
        _rotationalSpeed = Random.Range(-30, 30);
        _speed = Vector2.zero;
        while (_speed.magnitude < 0.01f) {
            _speed = new Vector2(
                Random.Range(-1, 1),
                Random.Range(-1, 1)
            );
        } 
        _speed*=Speed;

    }

    private void Update() {
        Duration -= Time.deltaTime;
        transform.position = (Vector2)transform.position + _speed * Time.deltaTime;
        transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + _rotationalSpeed * Time.deltaTime);

        var x = transform.position.x;
        var y = transform.position.y;
        var xSpd = _speed.x;
        var ySpd = _speed.y;

        bool outside = false;

        if (x > _rectangle.xMax) {
            x = _rectangle.xMax;
            xSpd = -Mathf.Abs(xSpd);
            outside = true;
        }
        if (x < _rectangle.xMin) { 
            x = _rectangle.xMin;
            xSpd = Mathf.Abs(xSpd);
            outside = true;
        }
        if (y > _rectangle.yMax) {
            y = _rectangle.yMax;
            ySpd = -Mathf.Abs(ySpd);
            outside = true;
        }
        if (y < _rectangle.yMin) {
            y = _rectangle.yMin;
            ySpd = Mathf.Abs(-ySpd);
            outside = true;
        }

        if (outside && Duration < 0) {
            _timeOutside += Time.deltaTime;
            if (_timeOutside > 2f) {
                Destroy(gameObject);
            }
            return;
        }

        transform.position = new Vector2(x, y);
        _speed = new Vector2(xSpd, ySpd);
    }
}
