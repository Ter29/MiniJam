using UnityEngine;
using System.Collections;      

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float dashSpeed = 10f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 3f;

    private Vector2 movement;
    private Vector2 moveDirection;
    private bool isDashing = false;
    private bool isCasting = false; // Додаємо прапорець для перевірки кастування
    private float dashTime = 0f;
    private float lastDashTime = 0f;

    private float lastWPressTime = 0f;
    private float lastAPressTime = 0f;
    private float lastSPressTime = 0f;
    private float lastDPressTime = 0f;

    private float doubleTapTimeLimit = 0.3f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Vector3 lastPosition;
    private bool isMooving;
    public int cast;
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        lastPosition = transform.position;
    }

    void Update()
    {
        if (!isCasting) // Забороняємо рух під час кастування
        {
            Movement();
            HandleMovementInput();
            UpdateAnimation();
            CheckingOfMoving();
        }

        // Додаємо перевірку на натискання Space для касту
        if (Input.GetKeyDown(KeyCode.Space) && !isCasting)
        {
            if(!isMooving){
                CastSpell();
            }
        }
        Debug.Log(isMooving);
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
        else if (!isCasting)
        {
            rb.velocity = movement * moveSpeed;
        }
    }

    void Movement()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
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

    void UpdateAnimation()
    {
        // Перевірка напрямку руху для вибору анімації
        if (movement.y > 0)
        {
            // Рух вгору
            animator.SetTrigger("walk_back");
        }
        else if (movement.y < 0)
        {
            // Рух вниз
            animator.SetTrigger("walk_front");
        }
        else if (movement.x != 0)
        {
            // Рух вправо або вліво
            animator.SetTrigger("walk_right");

            // Визначаємо, в яку сторону треба flip
            if (movement.x > 0)
            {
                spriteRenderer.flipX = false; // Рух вправо
            }
            else
            {
                spriteRenderer.flipX = true; // Рух вліво
            }
        }
        else
        {
            // Якщо персонаж не рухається, вибір idle анімації
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk_Back"))
            {
                animator.SetTrigger("idle_back");
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk_Front"))
            {
                animator.SetTrigger("idle_front");
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk_Right"))
            {
                animator.SetTrigger("idle_right");
            }
        }
    }
    void CastSpell()
    {
        // Визначаємо напрямок миші
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - transform.position;
        direction.z = 0; // Ignore the z-axis for 2D

        // Визначаємо напрямок і вибір анімації
        if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x))
        {
            if (direction.y > 0)
            {
                animator.SetTrigger("cast_back");
            }
            else
            {
                animator.SetTrigger("cast_front");
            }
        }
        else
        {
            animator.SetTrigger("cast_right");

            if (direction.x > 0)
            {
                spriteRenderer.flipX = false; // Рух вправо
            }
            else
            {
                spriteRenderer.flipX = true; // Рух вліво
            }
        }

        isCasting = true;
        StartCoroutine(EndCastAfterAnimation());
    }

    IEnumerator EndCastAfterAnimation()
    {
        // Wait until the casting animation is finished
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }

        EndCast();
    }
    void EndCast()
    {
        isCasting = false;

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Cast_Back"))
        {
            animator.SetTrigger("idle_back");
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Cast_Front"))
        {
            animator.SetTrigger("idle_front");
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Cast_Right"))
        {
            animator.SetTrigger("idle_right");
        }
    }
    
    void CheckingOfMoving()
    {
        if (transform.position != lastPosition)
        {
            lastPosition = transform.position;
            isMooving = true;
        }
        else{
            isMooving = false;
        }
    }
}
