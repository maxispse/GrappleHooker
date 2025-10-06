using UnityEngine;
using UnityEngine.InputSystem;

public class AimScript : MonoBehaviour
{
    [Header("References")]
    public Transform firePoint;
    public Transform aimReticle;
    public float aimDistance = 3f;
    public GrapplingHook grapplingHook;

    private bool isAiming = false;
    private Rigidbody2D rb;
    private WallJump movementScript;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        movementScript = GetComponent<WallJump>();
        aimReticle.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isAiming)
        {
            Vector2 aimDir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
            firePoint.right = aimDir;
            aimReticle.position = firePoint.position + (Vector3)(aimDir * aimDistance);
        }
    }

    // Call this when button is pressed
    public void StartAiming()
    {
        isAiming = true;
        aimReticle.gameObject.SetActive(true);

        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0f;

        if (movementScript != null)
            movementScript.enabled = false;
    }

    // Call this when button is released
    public void StopAimingAndFire()
    {
        if (isAiming)
        {
            grapplingHook.Fire(aimReticle.position);
        }

        isAiming = false;
        aimReticle.gameObject.SetActive(false);

        rb.gravityScale = 1f;

        if (movementScript != null)
            movementScript.enabled = true;
    }
}