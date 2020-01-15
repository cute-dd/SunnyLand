using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyForgContorller : MonoBehaviour
{
    public float speed;
    
    public Transform leftPoint, rightPoint;
    
    private Rigidbody2D rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Movement();
    }

    void Movement()
    {
        rb.velocity = new Vector2(speed * Time.fixedDeltaTime, rb.velocity.y);
    }
}
