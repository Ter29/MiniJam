/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAvoidApproach : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float retreatDistance = 3f;
    public float approachDistance = 5f;

    private Transform player;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Vector2 movement;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer < retreatDistance)
            {
                RetreatFromPlayer();
            }
            else if (distanceToPlayer > approachDistance)
            {
                ApproachPlayer();
            }
            else
            {
                StopMovement();
            }
        }
        Anim();
    }

    void RetreatFromPlayer()
    {
        Vector2 direction = (transform.position - player.position).normalized;
        movement = direction; // Оновлюємо напрям руху
        MoveInDirection(direction);
    }

    void ApproachPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        movement = direction; // Оновлюємо напрям руху
        MoveInDirection(direction);
    }

    void StopMovement()
    {
        movement = Vector2.zero; // Зупинка руху
    }

    void MoveInDirection(Vector2 direction)
    {
        transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
    }

    void Anim()
    {
        // Перевіряємо напрям руху
        if (movement.y > 0.1f)
        {
            animator.SetTrigger("Back"); // Рух вгору
        }
        else if (movement.y < -0.1f)
        {
            animator.SetTrigger("Front"); // Рух вниз
        }
        else if (movement.x > 0.1f)
        {
            animator.SetTrigger("Right"); // Рух вправо
            spriteRenderer.flipX = false; // Спрайт не перевертаємо
        }
        else if (movement.x < -0.1f)
        {
            animator.SetTrigger("Right"); // Рух вліво, але використовуємо той самий тригер
            spriteRenderer.flipX = true; // Перевертаємо спрайт вліво
        }
        else
        {
            animator.SetTrigger("Front"); // Якщо відсутній рух
        }
    }
}
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAvoidApproach : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float retreatDistance = 3f;
    public float approachDistance = 5f;

    private Transform player;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    private Vector2 movement;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>(); // Додаємо Rigidbody2D
    }

    void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer < retreatDistance)
            {
                RetreatFromPlayer();
            }
            else if (distanceToPlayer > approachDistance)
            {
                ApproachPlayer();
            }
            else
            {
                StopMovement();
            }
        }
        Anim();
    }
    void FixedUpdate()
    {
        Move();
    }

    void RetreatFromPlayer()
    {
        Vector2 direction = (transform.position - player.position).normalized;
        movement = direction; // Оновлюємо напрям руху
    }

    void ApproachPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        movement = direction; // Оновлюємо напрям руху
    }

    void StopMovement()
    {
        movement = Vector2.zero; // Зупинка руху
    }

    void Move()
    {
        Vector2 targetVelocity = movement * moveSpeed;
        rb.velocity = Vector2.Lerp(rb.velocity, targetVelocity, 0.1f); // Згладжування зміни швидкості
    }

    void Anim()
    {
        // Перевіряємо напрям руху
        if (movement.y > 0.1f)
        {
            animator.SetTrigger("Back"); // Рух вгору
        }
        else if (movement.y < -0.1f)
        {
            animator.SetTrigger("Front"); // Рух вниз
        }
        else if (movement.x > 0.1f)
        {
            animator.SetTrigger("Right"); // Рух вправо
            spriteRenderer.flipX = false; // Спрайт не перевертаємо
        }
        else if (movement.x < -0.1f)
        {
            animator.SetTrigger("Right"); // Рух вліво, але використовуємо той самий тригер
            spriteRenderer.flipX = true; // Перевертаємо спрайт вліво
        }
        else
        {
            animator.SetTrigger("Front"); // Якщо відсутній рух
        }
    }
}