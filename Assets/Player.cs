using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public float jumpForce = 5f;

    private float movement;
    public float moveSpeed = 5f;
    private bool facingRight = true;

    void Start()
    {

    }

    void Update()
    {
        movement = 0f;

        // MOVEMENT
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
        {
            movement = -1f;
        }
        else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
        {
            movement = 1f;
        }

        // FLIP
        if (movement < 0f && facingRight)
        {
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
            facingRight = false;
        }
        else if (movement > 0f && !facingRight)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            facingRight = true;
        }

        // JUMP
        if (Keyboard.current.spaceKey.isPressed && Mathf.Abs(rb.linearVelocity.y) < 0.001f)
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(movement, 0f, 0f)
            * Time.fixedDeltaTime * moveSpeed;
    }

    void Jump()
    {
        rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
    }
}