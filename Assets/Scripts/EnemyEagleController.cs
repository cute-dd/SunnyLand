using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEagleController : EnemyController
{
    public Transform topPoint, bottomPoint;
    public float speed;

    private Rigidbody2D _rb;
    private float _topY, _bottomY, _selfX;
    private bool _flyTop = true;
    
    protected override void Start()
    {
        base.Start();
        _rb = GetComponent<Rigidbody2D>();
        transform.DetachChildren();
        _topY = topPoint.position.y;
        _bottomY = bottomPoint.position.y;
        _selfX = topPoint.position.x;
        Destroy(topPoint.gameObject);
        Destroy(bottomPoint.gameObject);
    }

    void Update()
    {
        Fly();
        transform.position = new Vector2(_selfX, transform.position.y);
    }

    void Fly()
    {
        if (_flyTop)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, speed);
            if (transform.position.y > _topY)
            {
                _flyTop = false;
            }
        }
        else
        {
            _rb.velocity = new Vector2(_rb.velocity.x, -speed);
            if (transform.position.y < _bottomY)
            {
                _flyTop = true;
            }
        }
    }
}
