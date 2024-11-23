using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAvoidApproach : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float retreatDistance = 3f;
    public float approachDistance = 5f;

    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
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
    }

    void RetreatFromPlayer()
    {
        Vector2 direction = (transform.position - player.position).normalized;
        transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
    }

    void ApproachPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
    }

    void StopMovement()
    {
        transform.position = transform.position;
    }
}
