using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 5f;
    public float shootingInterval = 2f;
    public int maxBullets = 2;

    private Transform player;
    private float timeSinceLastShot = 0f;
    private List<GameObject> bullets = new List<GameObject>();

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        timeSinceLastShot += Time.deltaTime;

        if (player != null && timeSinceLastShot >= shootingInterval)
        {
            ShootAtPlayer();
            timeSinceLastShot = 0f;
        }
    }

    void ShootAtPlayer()
    {
        Vector2 direction = (player.position - firePoint.position).normalized;

        if (bullets.Count >= maxBullets)
        {
            Destroy(bullets[0]);
            bullets.RemoveAt(0);
        }

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullets.Add(bullet);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * bulletSpeed;
        }
    }
}
