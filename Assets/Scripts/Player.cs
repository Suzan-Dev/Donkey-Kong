
using UnityEngine;

public class Player : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite[] runSprites;
    public Sprite climbSprite;
    private int spriteIndex;

    private new Rigidbody2D rigidbody;
    private new Collider2D collider;
    private Collider2D[] results;

    private Vector2 direction;
    private bool grounded;
    private bool climbing;

    public float moveSpeed = 3f;
    public float jumpStrength = 4f;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();

        results = new Collider2D[4];
    }

    private void OnEnable()
    {
        InvokeRepeating(nameof(AnimateSprite), 1f / 12f, 1f / 12f);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(AnimateSprite));
    }

    private void CheckCollision()
    {
        grounded = false;
        climbing = false;

        Vector2 size = collider.bounds.size;
        size.y += 0.1f;
        size.x /= 5f;

        int amount = Physics2D.OverlapBoxNonAlloc(transform.position, size, 0f, results);

        for (int i = 0; i < amount; i++)
        {
            GameObject hit = results[i].gameObject;

            if (hit.layer == LayerMask.NameToLayer("Ground"))
            {
                grounded = hit.transform.position.y < (transform.position.y - 0.5f);

                Physics2D.IgnoreCollision(collider, results[i], !grounded);
            }
            else if (hit.layer == LayerMask.NameToLayer("Ladder"))
            {
                climbing = true;
            }
        }
    }

    private void Update()
    {
        CheckCollision();

        if (climbing)
        {
            direction.y = Input.GetAxis("Vertical") * moveSpeed;
        }
        else if (grounded && Input.GetButtonDown("Jump"))
        {
            direction = Vector2.up * jumpStrength;
        }
        else
        {
            direction += Physics2D.gravity * Time.deltaTime;
        }

        direction.x = Input.GetAxis("Horizontal") * moveSpeed;

        if (grounded)
        {
            direction.y = Mathf.Max(direction.y, -1f);
        }

        if (direction.x > 0)
        {
            transform.eulerAngles = Vector3.zero;
        }
        else if (direction.x < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }

    private void FixedUpdate()
    {
        rigidbody.MovePosition(rigidbody.position + direction * Time.fixedDeltaTime);
    }

    private void AnimateSprite()
    {
        if (climbing)
        {
            spriteRenderer.sprite = climbSprite;
        }
        else if (direction.x != 0f)
        {
            spriteIndex++;

            if (spriteIndex >= runSprites.Length)
            {
                spriteIndex = 0;
            }

            spriteRenderer.sprite = runSprites[spriteIndex];
        }
        else
        {
            spriteRenderer.sprite = runSprites[0];
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Objective"))
        {
            enabled = false;
            FindObjectOfType<GameManager>().CompleteLevel();
        }
        else if (other.gameObject.CompareTag("Obstacle"))
        {
            enabled = false;
            FindObjectOfType<GameManager>().FailLevel();

        }
    }
}
