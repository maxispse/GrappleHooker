using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum PlayerState { Normal, Aiming, Shooting }

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float wallSlideSpeed = 2f;
    public float wallJumpForce = 8f;

    [Header("References")]
    public Transform aimPivot;
    public GrappleHook grapplingHook;
    public Joystick movementJoystick;
    public Joystick aimJoystick;

    private Rigidbody2D rb;
    private PlayerState state = PlayerState.Normal;
    private bool isTouchingWall;

    private Vector2 moveInput;
    private Vector2 aimDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        switch (state)
        {
            case PlayerState.Normal:
                HandleMovement();

                // Enter aiming if right joystick is moved
                if (aimJoystick.Horizontal != 0 || aimJoystick.Vertical != 0)
                {
                    state = PlayerState.Aiming;
                }
                break;

            case PlayerState.Aiming:
                Aim();

                // Shoot grappling hook when aim joystick is released
                if (aimJoystick.Horizontal == 0 && aimJoystick.Vertical == 0 && aimDirection.sqrMagnitude > 0.1f)
                {
                    state = PlayerState.Shooting;
                    grapplingHook.Shoot(aimDirection);
                }

                // Cancel aim if you want (optional)
                if (movementJoystick.Horizontal != 0 || movementJoystick.Vertical != 0)
                {
                    // Optional: allow canceling aim when moving
                    // state = PlayerState.Normal;
                }
                break;

            case PlayerState.Shooting:
                // Return to normal when grappling hook finishes
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
        moveInput = new Vector2(movementJoystick.Horizontal, 0);
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
    }

    private void Aim()
    {
        aimDirection = new Vector2(aimJoystick.Horizontal, aimJoystick.Vertical);

        if (aimDirection.sqrMagnitude > 0.1f)
        {
            aimDirection.Normalize();
            aimPivot.right = aimDirection;
        }
    }

    private void WallCheck()
    {
        // Raycast to detect walls on the side
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * Mathf.Sign(transform.localScale.x), 0.6f, LayerMask.GetMask("Wall"));
        isTouchingWall = hit.collider != null;

        // Wall slide logic
        if (isTouchingWall && rb.linearVelocity.y < 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -wallSlideSpeed);

            // Wall jump when pressing joystick up
            if (movementJoystick.Vertical > 0.5f)
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
