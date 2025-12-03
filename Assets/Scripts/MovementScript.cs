using System.Collections;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    [Header("Player Movement")]
    public float moveSpeed = 5f;
    public Joystick movementJoystick;  // Drag your joystick here
    public Joystick dashJoystick;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    [Header("Dash Settings")]
    private bool canDash = true;
    private bool isDashing;
    private float dashPower = 24f;
    private float dashTime = 0.5f;
    private float dashCooldown = 1f;

    [Header("Camera Control")]
    public Transform cameraTransform;  // Drag your main camera here
    public float cameraFollowSpeed = 3f; // how fast camera follows
    public Vector3 cameraOffset = new Vector3(0f, 0f, -10f); // default camera offset

    [SerializeField] private TrailRenderer tr;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>(); // Reference to Animator component
    }

    private void FixedUpdate()
    {
        if (Time.timeScale == 0f)
            return;

        // --- Player movement ---
        float moveX = movementJoystick.Horizontal;
        rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);

        // Flip sprite for direction
        if (moveX > 0.1f)
            spriteRenderer.flipX = false;
        else if (moveX < -0.1f)
            spriteRenderer.flipX = true;

        // Dash function

        if (jumpJoystick)

        // --- Animation control ---
        bool isWalking = Mathf.Abs(moveX) > 0.1f;
        animator.SetBool("isWalking", isWalking);

        // --- Camera vertical movement ---
        if (cameraTransform != null)
        {
            Vector3 targetPosition = transform.position + cameraOffset;
            cameraTransform.position = Vector3.Lerp(
                cameraTransform.position,
                targetPosition,
                Time.deltaTime * cameraFollowSpeed
            );
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float OriginalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.linearVelocity = new Vector2(transform.localScale.x * dashPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashTime);
        tr.emitting = false;
        rb.gravityScale = OriginalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}