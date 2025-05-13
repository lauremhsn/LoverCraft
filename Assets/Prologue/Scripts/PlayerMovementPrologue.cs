using UnityEngine;

public class PlayerMovementPrologue : MonoBehaviour
{
    public float moveSpeed = 5f; // Adjust in Inspector
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private Animator animator;
    private bool facingRight = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rb.velocity = Vector2.zero;
    }

    private void Update()
    {
        // Get input
        float moveX = Input.GetAxisRaw("Horizontal");

        moveDirection = new Vector2(moveX, 0).normalized;

        // Handle animation
        animator.SetBool("isMoving", moveX != 0);

        // Handle flipping the player
        if (moveX > 0 && !facingRight)
        {
            Flip();
        }
        else if (moveX < 0 && facingRight)
        {
            Flip();
        }
    }

    private void FixedUpdate()
    {
        // Move the player using Rigidbody (smooth, no vibration)
        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}