using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 7f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    private Rigidbody2D rb;
    private Animator animator;
    private float moveInput;
    private bool isGrounded;
    private bool jumpRequested;
    private SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb.constraints |= RigidbodyConstraints2D.FreezeRotation;
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        if (moveInput != 0) spriteRenderer.flipX = moveInput < 0;
        jumpRequested |= Input.GetButtonDown("Jump");
    }
    void FixedUpdate()
    {
        isGrounded = groundCheck != null
            && rb.linearVelocity.y <= 0.01f
            && Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
        if (jumpRequested && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
        }
        jumpRequested = false;
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);
        animator.SetBool("isJumping", !isGrounded);
        animator.SetBool("isMoving", moveInput != 0);
    }


}
