using System.Collections;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(DistanceJoint2D))]
public class GrappleController : MonoBehaviour
{
    [Header("References")]
    public Transform grappleOrigin;       
    public LineRenderer ropeRenderer;     
    public DistanceJoint2D distanceJoint; 
    public Joystick aimJoystick;          

    [Header("Grapple Settings")]
    public LayerMask grappleLayer;        
    public float maxGrappleDistance = 12f;
    public float reelSpeed = 8f;          
    public float releaseThreshold = 0.6f; 
    public float ropeWidth = 0.05f;

    // internal state
    private Rigidbody2D rb;
    private bool isGrappled = false;
    private Vector2 grapplePoint;
    private float currentDistance;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (distanceJoint == null) distanceJoint = GetComponent<DistanceJoint2D>();

        // configure joint default disabled
        distanceJoint.enabled = false;
        distanceJoint.autoConfigureDistance = false;
        distanceJoint.autoConfigureConnectedAnchor = false;

        if (ropeRenderer != null)
        {
            ropeRenderer.positionCount = 2;
            ropeRenderer.enabled = false;
            ropeRenderer.startWidth = ropeWidth;
            ropeRenderer.endWidth = ropeWidth;
        }
    }

    void Update()
    {
        // Debug / keyboard helpers for testing
        if (Input.GetKeyDown(KeyCode.E)) FireGrapple();
        if (Input.GetKeyDown(KeyCode.Q)) ReleaseGrapple();

        // update rope visual if active
        if (isGrappled)
            UpdateRope();
    }

    // Public: call from UI Button or other script to fire the grapple
    public void FireGrapple()
    {
        // compute aim direction from joystick (if assigned) or mouse direction
        Vector2 aimDir = GetAimDirection();
        if (aimDir.sqrMagnitude < 0.001f) return; // no aim

        Vector2 origin = grappleOrigin != null ? (Vector2)grappleOrigin.position : (Vector2)transform.position;
        RaycastHit2D hit = Physics2D.Raycast(origin, aimDir, maxGrappleDistance, grappleLayer);
        Debug.DrawRay(origin, aimDir * maxGrappleDistance, Color.cyan, 1f);

        if (hit.collider != null)
        {
            AttachGrapple(hit.point);
        }
        else
        {
            // optional: you can play a "miss" feedback
            // Debug.Log("Grapple missed");
        }
    }

    public void ReleaseGrapple()
    {
        if (!isGrappled) return;
        isGrappled = false;
        distanceJoint.enabled = false;
        if (ropeRenderer != null) ropeRenderer.enabled = false;
    }

    private void AttachGrapple(Vector2 point)
    {
        grapplePoint = point;
        isGrappled = true;

        // enable joint and set anchors
        distanceJoint.enabled = true;
        distanceJoint.connectedAnchor = grapplePoint;

        // set current distance from player to point
        currentDistance = Vector2.Distance(rb.position, grapplePoint);
        distanceJoint.distance = currentDistance;
        distanceJoint.enableCollision = false; // don't collide with connected object

        // enable rope visuals
        if (ropeRenderer != null)
        {
            ropeRenderer.enabled = true;
            UpdateRope();
        }
    }

    // Call this each frame while grappled
    private void UpdateRope()
    {
        if (ropeRenderer == null) return;
        ropeRenderer.SetPosition(0, grappleOrigin != null ? grappleOrigin.position : transform.position);
        ropeRenderer.SetPosition(1, grapplePoint);

        // optionally auto-reel in if player presses input (e.g., joystick vertical) or keyboard
        float reelInput = GetReelInput(); // positive -> reel in
        if (Mathf.Abs(reelInput) > 0.01f)
        {
            // reel in/out
            currentDistance -= reelInput * reelSpeed * Time.deltaTime;
            currentDistance = Mathf.Clamp(currentDistance, 0.5f, maxGrappleDistance);
            distanceJoint.distance = currentDistance;
        }
        else
        {
            // small auto-reel to help climb: if player is moving towards the point, optionally reel
            // no automatic behavior by default
        }

        // auto finish if close enough
        float distToPoint = Vector2.Distance(rb.position, grapplePoint);
        if (distToPoint <= releaseThreshold)
        {
            // optional: snap player to point + small offset so player "lands" on the wall
            SnapToGrapplePoint();
            ReleaseGrapple();
        }
    }

    // Helper: get aim direction either from joystick or from mouse position
    private Vector2 GetAimDirection()
    {
        if (aimJoystick != null)
        {
            Vector2 dir = new Vector2(aimJoystick.Horizontal, aimJoystick.Vertical);
            if (dir.sqrMagnitude > 0.001f) return dir.normalized;
        }

        // fallback to mouse position in world (editor/testing)
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dirToMouse = (mouseWorld - transform.position);
        if (dirToMouse.sqrMagnitude < 0.001f) return Vector2.zero;
        return dirToMouse.normalized;
    }

    // Gets reel input: joystick vertical or keyboard W/S arrows
    private float GetReelInput()
    {
        // If you have a separate joystick for vertical, use it. Here we check:
        if (aimJoystick != null)
        {
            // Up = positive (reel in), Down = negative (let out)
            return aimJoystick.Vertical;
        }

        // fallback keyboard
        float inVal = 0f;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) inVal += 1f;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) inVal -= 1f;
        return inVal;
    }

    private void SnapToGrapplePoint()
    {
        // move player slightly away from the grapplePoint along the normal so they don't get stuck inside geometry.
        Vector2 dir = (rb.position - grapplePoint).normalized;
        float snapDistance = 0.6f; // how far from grapplePoint the player should end up
        Vector2 targetPos = grapplePoint + dir * snapDistance;

        // set transform position (if you use physics it may be better to move via Rigidbody2D)
        rb.position = targetPos;
        rb.linearVelocity = Vector2.zero;
    }

    // Optional public helpers to check state
    public bool IsGrappled() => isGrappled;
    public Vector2 GetGrapplePoint() => grapplePoint;
}

