using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public Collider2D coll, hard;
    public AudioSource jumpAudio, hurtAudio, rewardAudio;
    public Text cherryNum, gemNum;
    public Transform cellingCheck;
    public LayerMask ground;
    public float speed;
    public float jumpForce;
    public int cherry;
    public int gem;

    private bool _isHurt;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (!_isHurt)
        {
            Movement();   
        }
        SwitchAnimation();
    }

    private void Update()
    {
        Crouch();
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
        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce*Time.fixedDeltaTime);
            jumpAudio.Play();
            anim.SetBool("Jumping", true);
        }
    }

    /// <summary>
    /// 动画切换
    /// </summary>
    void SwitchAnimation()
    {
        //下落动画
        if (rb.velocity.y < 0.1f && !coll.IsTouchingLayers(ground))
        {
            anim.SetBool("Crouch", false);
            anim.SetBool("Falling", true);
        }
        
        //跳跃动画
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

        //受伤动画
        if (_isHurt)
        {
            anim.SetBool("Hurt", true);
            anim.SetFloat("Running", 0f);
            anim.SetBool("Crouch", false);
            if (Mathf.Abs(rb.velocity.x) < 0.1f)
            {
                anim.SetBool("Hurt", false);
                _isHurt = false;
            }
        }
        //下蹲动画
        if (anim.GetBool("Falling") && coll.IsTouchingLayers(ground))
        {
            anim.SetBool("Falling", false);
            anim.SetBool("Crouch", true);
        }
    }

    //收集物品
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Cherry"))
        {
            rewardAudio.Play();
            Destroy(other.gameObject);
            cherry ++;
            cherryNum.text = cherry.ToString();
        }

        if (other.CompareTag("Gem"))
        {
            rewardAudio.Play();
            Destroy(other.gameObject);
            gem ++;
            gemNum.text = gem.ToString();
        }
    }
    
    //消灭敌人
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyController enemy = other.gameObject.GetComponent<EnemyController>();
            
            if (anim.GetBool("Falling"))
            {
                enemy.JumpOn();
                rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.fixedDeltaTime);
                anim.SetBool("Falling", false);
                anim.SetBool("Jumping", true);
            }else if (other.transform.position.x > transform.position.x)
            {
                rb.velocity = new Vector2(-10, rb.velocity.y);
                _isHurt = true;
                hurtAudio.Play();
            }
            else if (other.transform.position.x < transform.position.x)
            { 
                rb.velocity = new Vector2(10, rb.velocity.y);
                _isHurt = true;
                hurtAudio.Play();
            }
        }
    }

    //下蹲
    void Crouch()
    {
        if (!Physics2D.OverlapCircle(cellingCheck.position, 0.2f, ground))
        {
            if (Input.GetButton("Crouch"))
            {
                anim.SetBool("Crouch", true);
                hard.enabled = false;
            }
            else if (!Input.GetButton("Crouch"))
            {
                anim.SetBool("Crouch", false);
                hard.enabled = true;
            }
        }
    }
}
