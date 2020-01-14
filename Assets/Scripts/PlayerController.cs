using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator anim; 
    public float speed;
    public float jumpForce;
    
    
    void Start()
    {
    }

    void FixedUpdate()
    {
        Movement();
    }

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

    void SwitchAnimation()
    {
         
    }
}
