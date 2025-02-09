using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform player;
    [SerializeField] float moveSpeed = 3f; // Adjust movement speed
    [SerializeField] int health = 3;
    private Vector2 moveInput;
    private Rigidbody2D rb;
    private Animator animator;

    public void Initialize(Transform player) {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        this.player = player;
    }

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag.Equals("Harmony")) {
            animator.Play("EnemyHit");
            Destroy(collision.gameObject);
            health--;
            if (health <= 0) {

                FindObjectOfType<GameManager>().UpdateScore(10);
                Destroy(gameObject);
            }
        }
    }

    void FixedUpdate() {
        moveInput = (player.position - transform.position).normalized;
        rb.velocity = moveInput * moveSpeed;
    }
}
