using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    public float jumpForce = 5f;

    private float movement;
    public float moveSpeed = 5f;
    // Sprite bawaan menghadap ke kiri (scale.x positif = hadap kiri, scale.x negatif = hadap kanan)
    private bool facingRight = false;

    private void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        // Tentukan facingRight berdasarkan arah awal scale.x
        // Karena sprite bawaan menghadap kiri saat scale.x > 0:
        facingRight = transform.localScale.x < 0;
    }

    void Update()
    {
        movement = 0f;

        // MOVEMENT INPUT (Mendukung New Input System dan Legacy Input)
        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            {
                movement = -1f;
            }
            else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            {
                movement = 1f;
            }
        }
        else
        {
            movement = Input.GetAxisRaw("Horizontal");
        }

        // FLIP:
        // Sprite bawaan menghadap kiri.
        // Bergerak ke kanan (movement > 0) -> flip scale.x menjadi negatif agar menghadap kanan
        // Bergerak ke kiri (movement < 0) -> flip scale.x menjadi positif agar menghadap kiri
        if (movement > 0f && !facingRight)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            facingRight = true;
        }
        else if (movement < 0f && facingRight)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            facingRight = false;
        }

        // ANIMATION
        if (animator != null)
        {
            bool isWalking = Mathf.Abs(movement) > 0.01f;
            animator.SetBool("isWalking", isWalking);
        }

        // JUMP
        bool jumpPressed = (Keyboard.current != null && Keyboard.current.spaceKey.isPressed) || Input.GetKeyDown(KeyCode.Space);
        if (jumpPressed && rb != null && Mathf.Abs(rb.linearVelocity.y) < 0.001f)
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(movement * moveSpeed, rb.linearVelocity.y);
        }
    }

    void Jump()
    {
        if (rb != null)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
    }
}
