using UnityEngine;

public class Barrel : MonoBehaviour
{
    private new Rigidbody2D rigidbody;

    public float speed = 5f;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (transform.position.y < -8)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            rigidbody.AddForce(other.transform.right * speed, ForceMode2D.Impulse);
        }
    }
}
