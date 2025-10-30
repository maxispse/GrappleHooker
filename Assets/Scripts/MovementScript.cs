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
    public float cameraMoveSpeed = 2f;
    public float cameraYClamp = 3f;    // How far camera can move up/down from player

    private float defaultCameraY;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>(); // Reference to Animator component

        if (cameraTransform != null)
            defaultCameraY = cameraTransform.position.y;
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
            float moveY = movementJoystick.Vertical;

            float targetY = Mathf.Clamp(
                defaultCameraY + moveY * cameraYClamp,
                defaultCameraY - cameraYClamp,
                defaultCameraY + cameraYClamp
            );

            Vector3 camPos = cameraTransform.position;
            camPos.y = Mathf.Lerp(camPos.y, targetY, Time.deltaTime * cameraMoveSpeed);
            cameraTransform.position = camPos;
        }
    }
}