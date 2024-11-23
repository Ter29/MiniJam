using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float dashSpeed = 10f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 3f;

    private Vector2 movement;
    private Vector2 moveDirection;
    private bool isDashing = false;
    private float dashTime = 0f;
    private float lastDashTime = 0f;

    private float lastWPressTime = 0f;
    private float lastAPressTime = 0f;
    private float lastSPressTime = 0f;
    private float lastDPressTime = 0f;

    private float doubleTapTimeLimit = 0.3f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        HandleMovementInput();
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            rb.velocity = moveDirection * dashSpeed;
            dashTime += Time.deltaTime;

            if (dashTime >= dashDuration)
            {
                isDashing = false;
                dashTime = 0f;
                rb.velocity = Vector2.zero;
            }
        }
        else
        {
            rb.velocity = movement * moveSpeed;
        }
    }

    void HandleMovementInput()
    {
        if (Time.time - lastDashTime >= dashCooldown)
        {
            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
            {
                MovePlayer(Vector2.up, ref lastWPressTime);
            }
            else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.LeftShift))
            {
                MovePlayer(Vector2.left, ref lastAPressTime);
            }
            else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.LeftShift))
            {
                MovePlayer(Vector2.down, ref lastSPressTime);
            }
            else if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.LeftShift))
            {
                MovePlayer(Vector2.right, ref lastDPressTime);
            }
        }
    }

    void MovePlayer(Vector2 direction, ref float lastPressTime)
    {
        if (Time.time - lastPressTime <= doubleTapTimeLimit)
        {
            isDashing = true;
            moveDirection = direction;
            lastDashTime = Time.time;
        }
        else
        {
            isDashing = false;
        }

        lastPressTime = Time.time;
    }
}
