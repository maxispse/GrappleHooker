using UnityEngine;

public class ShootingScript : MonoBehaviour
{
    public Transform firePoint;
    public GameObject projectilePrefab; 
    public float projectileSpeed = 15f;
    public bool useMouseAiming = true;


    void Update()
    {
        Vector2 aimDirection;

        if (useMouseAiming)
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            aimDirection = (mouseWorld - transform.position).normalized;
        }
        else
        {
            aimDirection = new Vector2(Input.GetAxis("RightStickX"), Input.GetAxis("RightStickY")).normalized;
        }

        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        firePoint.rotation = Quaternion.Euler(0, 0, angle);

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot(aimDirection);
        }
    }

    void Shoot(Vector2 direction)
    {
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * projectileSpeed;
    }
}
