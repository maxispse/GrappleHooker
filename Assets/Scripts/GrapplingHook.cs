using UnityEngine;
using System.Collections;

public class GrapplingHook : MonoBehaviour
{
    public LineRenderer line;        // Draws the rope
    public float hookSpeed = 15f;
    public float pullSpeed = 10f;
    public LayerMask grappleLayer;

    private Vector2 hookTarget;
    private bool isPulling;
    private Transform player;
    private Rigidbody2D playerRb;

    public bool IsActive => isPulling;

    void Start()
    {
        player = transform.parent;
        playerRb = player.GetComponent<Rigidbody2D>();
        line.positionCount = 0;
    }

    public void Shoot(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 20f, grappleLayer);

        if (hit.collider)
        {
            hookTarget = hit.point;
            StartCoroutine(PullPlayerToTarget());
        }
        else
        {
            Debug.Log("No grapple target hit!");
        }
    }

    private IEnumerator PullPlayerToTarget()
    {
        isPulling = true;
        line.positionCount = 2;
        line.SetPosition(1, hookTarget);

        playerRb.gravityScale = 0f;

        while (Vector2.Distance(player.position, hookTarget) > 1f)
        {
            Vector2 dir = (hookTarget - (Vector2)player.position).normalized;
            playerRb.linearVelocity = dir * pullSpeed;

            line.SetPosition(0, transform.position);
            yield return null;
        }

        playerRb.linearVelocity = Vector2.zero;
        playerRb.gravityScale = 3f;
        line.positionCount = 0;
        isPulling = false;
    }
}
