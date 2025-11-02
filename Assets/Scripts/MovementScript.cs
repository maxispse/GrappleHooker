using UnityEngine;

public class MovementScript : MonoBehaviour
{
    [Header("Player Movement")]
    public float moveSpeed = 5f;
    public Joystick movementJoystick;  // Drag your joystick here
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    [Header("Camera Control")]
    public Transform cameraTransform;  // Drag your main camera here
    public float cameraFollowSpeed = 3f; // how fast camera follows
    public Vector3 cameraOffset = new Vector3(0f, 0f, -10f); // default camera offset

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
}