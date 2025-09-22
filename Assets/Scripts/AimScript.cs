using UnityEngine;

public class AimScript : MonoBehaviour
{
    [Header("References")]
    public Transform firePoint;
    public GameObject projectilePrefab;
    public Transform aimReticle;
    public float projectileSpeed = 20f;
    public float aimDistance = 3f;

    [Header("Settings")]
    public bool useMouse = true; 

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
        // Enter aiming mode
        if (Input.GetButtonDown("Fire1"))
        {
            EnterAimingMode();
        }

        // Exit aiming mode and shoot
        if (isAiming && Input.GetButtonUp("Fire1"))
        {
            Vector2 dir = (aimReticle.position - firePoint.position).normalized;
            Shoot(dir);
            ExitAimingMode();
        }

        // Update aim direction
        if (isAiming)
        {
            Vector2 aimDir;

            if (useMouse)
            {
                Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                aimDir = (mouseWorld - transform.position).normalized;
            }
            else
            {
                aimDir = new Vector2(Input.GetAxis("RightStickX"), Input.GetAxis("RightStickY"));
                if (aimDir.magnitude < 0.1f)
                    return; // Don�t update unless there's meaningful input
                aimDir.Normalize();
            }

            // Rotate firePoint
            firePoint.right = aimDir;

            // Move reticle
            aimReticle.position = firePoint.position + (Vector3)(aimDir * aimDistance);
        }
    }
    void EnterAimingMode()
    {
        isAiming = true;
        aimReticle.gameObject.SetActive(true);

        // Freeze player movement
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0f;

        if (movementScript != null)
            enabled = false; // optional: disable other controls
    }

    void ExitAimingMode()
    {
        isAiming = false;
        aimReticle.gameObject.SetActive(false);

        // Restore gravity
        rb.gravityScale = 1f;

        if (movementScript != null)
            enabled = true;
    }

    void Shoot(Vector2 direction)
    {
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        proj.GetComponent<Rigidbody2D>().linearVelocity = direction * projectileSpeed;
    }
}
