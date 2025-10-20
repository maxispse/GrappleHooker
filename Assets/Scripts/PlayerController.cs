using UnityEngine;
using UnityEngine.Playables;
public class PlayerController : MonoBehaviour
{
    public enum PlayerState
    {
        Normal,
        Aiming,
        Shooting
    }
    public float moveSpeed = 5f;
    public float wallSlideSpeed = 2f;
    public float wallJumpForce = 8f;

    public Transform aimPivot;
    public GrapplingHook grapplingHook;

    private Rigidbody2D rb;
    private PlayerState state = PlayerState.Normal;
    private bool isTouchingWall;

    private Vector2 moveInput;
    private Vector2 aimDirection;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case PlayerState.Normal:
                HandleMovement();
                if (Input.GetMouseButtonDown(1)) // Right click to aim
                {
                    state = PlayerState.Aiming;
                    rb.linearVelocity = Vector2.zero;
                }
                break;

            case PlayerState.Aiming:
                Aim();
                if (Input.GetMouseButtonDown(0)) // Left click to shoot
                {
                    state = PlayerState.Shooting;
                    grapplingHook.Shoot(aimDirection);
                }
                if (Input.GetMouseButtonDown(1)) // Cancel aim
                {
                    state = PlayerState.Normal;
                }
                break;

            case PlayerState.Shooting:
                if (!grapplingHook.IsActive)
                {
                    state = PlayerState.Normal;
                }
                break;
        }
        WallCheck();
    }
    private void HandleMovement()
    {
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
    }

    private void Aim()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        aimDirection = (mousePos - aimPivot.position).normalized;
        aimPivot.right = aimDirection;
    }

    private void ShootGrapple()
    {
        state = PlayerState.Shooting;
        Debug.Log("Shoot!");

        // Calculate the direction from the aimPivot toward the mouse
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - aimPivot.position).normalized;

        grapplingHook.Shoot(direction);
    }
    private void WallCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * Mathf.Sign(transform.localScale.x), 0.6f, LayerMask.GetMask("Wall"));
        isTouchingWall = hit.collider != null;

        // Wall slide
        if (isTouchingWall && rb.linearVelocity.y < 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -wallSlideSpeed);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                WallJump();
            }
        }
    }

    private void WallJump()
    {
        Vector2 jumpDir = new Vector2(-Mathf.Sign(transform.localScale.x), 1).normalized;
        rb.linearVelocity = jumpDir * wallJumpForce;
    }
}
