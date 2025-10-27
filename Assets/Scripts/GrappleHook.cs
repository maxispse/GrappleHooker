using UnityEngine;

public class GrappleHook : MonoBehaviour
{
    public float speed = 15f;             // How fast player is pulled
    public float maxDistance = 10f;       // Max hook range
    public LayerMask wallLayer;           // Wall layer to attach
    public LineRenderer rope;             // Optional visual

    private Vector2 targetPoint;
    private bool isGrappling = false;
    private Rigidbody2D rb;

    public bool IsActive => isGrappling;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rope != null) rope.enabled = false;
    }

    void Update()
    {
        if (isGrappling)
        {
            Vector2 direction = (targetPoint - (Vector2)transform.position).normalized;
            rb.linearVelocity = direction * speed;

            if (rope != null)
            {
                rope.enabled = true;
                rope.SetPosition(0, transform.position);
                rope.SetPosition(1, targetPoint);
            }

            if (Vector2.Distance(transform.position, targetPoint) < 0.2f)
            {
                rb.linearVelocity = Vector2.zero;
                isGrappling = false;
                if (rope != null) rope.enabled = false;
            }
        }
    }

    public void Shoot(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, maxDistance, wallLayer);
        if (hit.collider != null)
        {
            targetPoint = hit.point;
            isGrappling = true;
        }
        else
        {
            isGrappling = false;
            if (rope != null) rope.enabled = false;
        }
    }
}
