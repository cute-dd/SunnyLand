using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public Collider2D coll;
    public LayerMask ground;
    public float speed;
    public float jumpForce;
    public int cherry, gem;
    
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        Movement();
        SwitchAnimation();
    }

    /// <summary>
    /// 移动
    /// </summary>
    void Movement()
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        float faceDirection = Input.GetAxisRaw("Horizontal");

        //角色移动
        if (horizontalMove != 0)
        {
            rb.velocity = new Vector2(horizontalMove*speed*Time.fixedDeltaTime, rb.velocity.y);
            anim.SetFloat("Running", Mathf.Abs(horizontalMove));
        }

        //角色转向
        if (faceDirection != 0)
        {
            transform.localScale = new Vector3(faceDirection, 1, 1);
        }

        //角色跳跃
        if (Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce*Time.fixedDeltaTime);
            anim.SetBool("Jumping", true);
        }
    }

    /// <summary>
    /// 动作切换
    /// </summary>
    void SwitchAnimation()
    {
        if (anim.GetBool("Jumping"))
        {
            if (rb.velocity.y < 0)
            {
                anim.SetBool("Jumping", false);
                anim.SetBool("Falling", true);
            }
        }else if (coll.IsTouchingLayers(ground))
        {
            anim.SetBool("Falling", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Cherry"))
        {
            Destroy(other.gameObject);
            cherry += 1;
        }

        if (other.CompareTag("Gem"))
        {
            Destroy(other.gameObject);
            gem += 1;
        }
    }
}
