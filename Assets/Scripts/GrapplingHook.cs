using UnityEngine;
using UnityEngine.InputSystem;

public class GrapplingHook : MonoBehaviour
{
    [SerializeField] private LineRenderer line;
    [SerializeField] private Rigidbody playerRb;
    [SerializeField] private float pullSpeed = 20f;

    private Vector3 targetPoint;
    private bool isFired;

    public void Fire(Vector3 target)
    {
        targetPoint = target;
        isFired = true;
        line.enabled = true;
        line.SetPosition(0, transform.position);
        line.SetPosition(1, targetPoint);
    }

    private void FixedUpdate()
    {
        if (isFired)
        {
            Vector3 dir = (targetPoint - playerRb.position).normalized;
            playerRb.AddForce(dir * pullSpeed, ForceMode.Acceleration);

            // optional: stop when close enough
            if (Vector3.Distance(playerRb.position, targetPoint) < 2f)
            {
                isFired = false;
                line.enabled = false;
            }
        }
    }
}
