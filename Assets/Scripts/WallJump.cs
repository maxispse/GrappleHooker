using UnityEngine;

public class WallJump : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Wall Jumping")]
    public float wallJumpForceX = 8f;
    public float wallJumpForceY = 12f;

    [Header("Checks")]
    public Transform wallCheck;
    public float wallCheckRadius = 0.2f;
    public LayerMask wallLayer;

    private Rigidbody2D rb;
    private bool isTouchingWall;
    private int wallDirection; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    
    void Update()
    {
        // Wall detection
        Vector2 leftCheck = wallCheck.position + Vector3.left * 0.1f;
        Vector2 rightCheck = wallCheck.position + Vector3.right * 0.1f;

        bool onLeftWall = Physics2D.OverlapCircle(leftCheck, wallCheckRadius, wallLayer);
        bool onRightWall = Physics2D.OverlapCircle(rightCheck, wallCheckRadius, wallLayer);

        isTouchingWall = onLeftWall || onRightWall;
        wallDirection = onLeftWall ? -1 : (onRightWall ? 1 : 0);

        // Wall Jump Input
        if (Input.GetButtonDown("Jump") && isTouchingWall)
        {
            rb.linearVelocity = new Vector2(-wallDirection * wallJumpForceX, wallJumpForceY);
        }
    }

    void FixedUpdate()
    {
        float inputX = Input.GetAxisRaw("Horizontal");

        if (isTouchingWall)
        {
            
            rb.linearVelocity = new Vector2(inputX * moveSpeed, 0f);
            rb.gravityScale = 0f;
        }
        else
        {
            
            rb.linearVelocity = new Vector2(inputX * moveSpeed, rb.linearVelocity.y);
            rb.gravityScale = 1f;
        }
    }
}
