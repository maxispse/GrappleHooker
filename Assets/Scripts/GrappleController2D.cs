using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class GrappleController2D : MonoBehaviour
{
    public LayerMask grappleMask; // layers you can grapple to
    public float grappleSpeed = 8f;
    public float maxGrappleDistance = 10f;

    private Rigidbody2D rb;
    private LineRenderer lr;
    private Vector2 grapplePoint;
    private bool isGrappling;
    private bool isPulling;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 0;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryStartGrapple();
        }

        if (Input.GetMouseButtonUp(0))
        {
            StopGrapple();
        }

        if (isPulling)
        {
            PullToGrapple();
        }

        if (isGrappling)
        {
            DrawRope();
        }
    }

    void TryStartGrapple()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mouseWorld - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, maxGrappleDistance, grappleMask);

        if (hit.collider != null)
        {
            grapplePoint = hit.point;
            isGrappling = true;
            lr.positionCount = 2;
            isPulling = true;
        }
    }

    void PullToGrapple()
    {
        Vector2 dir = (grapplePoint - (Vector2)transform.position).normalized;
        rb.linearVelocity = dir * grappleSpeed;

        float dist = Vector2.Distance(transform.position, grapplePoint);
        if (dist < 0.5f)
        {
            StopGrapple();
        }
    }

    void DrawRope()
    {
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, grapplePoint);
    }

    void StopGrapple()
    {
        isGrappling = false;
        isPulling = false;
        lr.positionCount = 0;
        rb.linearVelocity = Vector2.zero;
    }
}