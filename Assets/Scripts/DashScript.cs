using UnityEngine;

public class DashScript : MonoBehaviour
{
    [Header("Settings")]
    public float dashSpeed;
    public float dashDistance;

    public Rigidbody2D rb;
    public Vector3 pos;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        
    }
    public void Dash()
    {
        //Upon activation, goes forward
        rb.position = ;
    }
}
