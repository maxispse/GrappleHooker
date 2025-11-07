using UnityEngine;

public class WallClimbAndThruster2D : MonoBehaviour
{
    [Header("References")]
    public MovementScript movementScript;
    public Joystick movementJoystick;   // left joystick (move/climb)
    public Joystick thrusterJoystick;   // right joystick (charge + launch)
    public Rigidbody2D rb;
    public LayerMask climbableLayer;

    [Header("Climbing Settings")]
    public float climbSpeed = 3f;
    public float wallCheckDistance = 0.5f;
    public float normalGravity = 3f;
    public float climbGravity = 0f;

    [Header("Thruster Settings")]
    public float minChargeTime = 0.3f;
    public float maxChargeTime = 1.2f;
    public float minLaunchForce = 6f;
    public float maxLaunchForce = 14f;
    public float launchHorizontalMultiplier = 1f;
    public float launchVerticalMultiplier = 1f;
    public float wallPushForce = 4f; // extra push away from the wall

    private bool isTouchingWall;
    private bool isGrounded;
    private bool isClimbing;
    private bool isCharging;
    private float chargeTimer;
    private float directionFacing = 1f;

    private void Start()
    {
        if (!rb) rb = GetComponent<Rigidbody2D>();
        if (!movementScript) movementScript = GetComponent<MovementScript>();
        if (!movementJoystick) movementJoystick = movementScript.movementJoystick;
    }

    private void Update()
    {
        CheckGround();
        CheckWall();

        // Stick and climb on wall (even if grounded)
        if (isTouchingWall)
        {
            StartClimbing();
        }
        else if (isClimbing && !isTouchingWall)
        {
            StopClimbing();
        }

        if (isClimbing)
        {
            HandleClimbing();
            HandleThrusterCharge();
        }
    }

    private void FixedUpdate()
    {
        float moveX = movementJoystick.Horizontal;
        if (Mathf.Abs(moveX) > 0.1f)
            directionFacing = Mathf.Sign(moveX);
    }

    private void CheckGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, LayerMask.GetMask("Ground"));
        isGrounded = hit.collider != null;
    }

    private void CheckWall()
    {
        Vector2 direction = new Vector2(directionFacing, 0);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, wallCheckDistance, climbableLayer);
        isTouchingWall = hit.collider != null;

        Debug.DrawRay(transform.position, direction * wallCheckDistance, isTouchingWall ? Color.green : Color.red);
    }

    private void StartClimbing()
    {
        if (!isClimbing)
        {
            isClimbing = true;
            rb.gravityScale = climbGravity;
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void StopClimbing()
    {
        if (isClimbing)
        {
            isClimbing = false;
            rb.gravityScale = normalGravity;
            isCharging = false;
            chargeTimer = 0f;
        }
    }

    private void HandleClimbing()
    {
        // Move up/down along the wall with movement joystick
        float vertical = movementJoystick.Vertical;
        rb.linearVelocity = new Vector2(0f, vertical * climbSpeed);
    }

    private void HandleThrusterCharge()
    {
        Vector2 input = new Vector2(thrusterJoystick.Horizontal, thrusterJoystick.Vertical);
        float magnitude = input.magnitude;

        // Begin charging if joystick pushed far enough
        if (magnitude > 0.5f && !isCharging)
        {
            isCharging = true;
            chargeTimer = 0f;
        }

        // Continue charging while held
        if (isCharging && magnitude > 0.5f)
        {
            chargeTimer += Time.deltaTime;
            chargeTimer = Mathf.Clamp(chargeTimer, 0f, maxChargeTime);
        }

        // Release if let go or reached full charge
        if (isCharging && (magnitude < 0.4f || chargeTimer >= maxChargeTime))
        {
            LaunchThruster(input);
        }
    }

    private void LaunchThruster(Vector2 inputDirection)
    {
        // Always detach from wall before launching
        StopClimbing();

        // Normalize input direction
        Vector2 launchDir = inputDirection.normalized;
        if (launchDir == Vector2.zero) launchDir = Vector2.up;

        float chargePercent = Mathf.InverseLerp(minChargeTime, maxChargeTime, chargeTimer);
        float force = Mathf.Lerp(minLaunchForce, maxLaunchForce, chargePercent);

        // Base launch direction
        Vector2 totalForce = new Vector2(
            launchDir.x * force * launchHorizontalMultiplier,
            launchDir.y * force * launchVerticalMultiplier
        );

        // Add push AWAY from wall
        Vector2 wallPushDir = new Vector2(-directionFacing, 0f) * wallPushForce;
        totalForce += wallPushDir;

        // Apply impulse
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(totalForce, ForceMode2D.Impulse);

        // Reset charge
        isCharging = false;
        chargeTimer = 0f;
    }
}
