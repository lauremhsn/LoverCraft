using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PC_Walking : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    
    }
}
