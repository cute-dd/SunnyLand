using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class EnemyForgContorller : EnemyController
{
    public float speed;
    public float jumpForce;
    public Transform leftPoint, rightPoint;
    public LayerMask ground;

    private int _faceNum = -1;
    private float _leftX, _rightX;
    private Rigidbody2D _rb;
    private Collider2D _coll;
    
    protected override void Start()
    { 
        base.Start();
        _rb = GetComponent<Rigidbody2D>();
        _coll = GetComponent<Collider2D>();
        transform.DetachChildren();
        _leftX = leftPoint.position.x;
        _rightX = rightPoint.position.x;
        Destroy(leftPoint.gameObject);
        Destroy(rightPoint.gameObject);
    }

    void Update()
    {
        Direction();
        SwitchAnimation();
    }
    
    //移动
    void Movement()
    {
        _rb.velocity = new Vector2(_faceNum*speed, jumpForce);
    }

    //转向
    void Direction()
    {
        if (_faceNum==-1 && _coll.IsTouchingLayers(ground))
        {
            if (transform.position.x < _leftX)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                _faceNum = 1;
            }
        }
        else if (_faceNum==1 && _coll.IsTouchingLayers(ground))
        {
            if (transform.position.x > _rightX)
            {
                transform.localScale = new Vector3(1, 1, 1);
                _faceNum = -1;
            }
        }
    }

    //动作切换
    void SwitchAnimation()
    {
        if (!_coll.IsTouchingLayers(ground))
        {
            Anim.SetBool("Jumping", true);
        }
    
        if (_rb.velocity.y < 0.1f && Anim.GetBool("Jumping"))
        {
            Anim.SetBool("Jumping", false);
            Anim.SetBool("Falling", true);
        }
    
        if (_coll.IsTouchingLayers(ground))
        {
            Anim.SetBool("Falling", false);
        }
    }

    
}
