using UnityEngine;

public class PlayerEntrance : MonoBehaviour
{
    public Vector2 targetPosition;
    public float speed = 2f;

    private Animator animator;
    private Rigidbody2D rb;
    private bool isEntering = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // Face right if needed
        if (targetPosition.x > transform.position.x)
            GetComponent<SpriteRenderer>().flipX = false;
    }

    void Update()
    {
        if (isEntering)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, step);

            float remainingDistance = Vector2.Distance(transform.position, targetPosition);

            animator.SetFloat("Speed", speed);

            if (remainingDistance < 0.05f)
            {
                isEntering = false;
                animator.SetFloat("Speed", 0f); // Switch to idle
            }
        }
    }
}