using UnityEngine;
using System.Collections;

public class TopDownController : MonoBehaviour {

    [SerializeField] Camera cam;
    [SerializeField] Harmony harmony;
    [SerializeField] Transform pivot;
    [SerializeField] Transform guitar;
    [SerializeField] Animator guitarAnimator;
    public float moveSpeed = 5f; // Adjust movement speed
    private Vector2 moveInput;
    private Rigidbody2D rb;
    public LayerMask hitLayers; // Layers the ray can hit
    public LineRenderer lineRenderer; // Reference to LineRenderer
    public float laserDuration = 0.1f; // How long the laser stays visible
    public float fireRate = 0.2f; // Time between shots (e.g., 0.2s for 5 shots per second)
    private float lastFireTime = 0f; // Tracks the last time a bullet was fired

    private Vector3 facingDirection;


    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        // Get input
        moveInput.x = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right
        moveInput.y = Input.GetAxisRaw("Vertical");   // W/S or Up/Down
        moveInput.Normalize(); // Prevents faster diagonal movement

        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        mousePos = new Vector3(mousePos.x, mousePos.y, 0);

        facingDirection = mousePos - transform.position;

        float angle = Mathf.Atan2(facingDirection.y, facingDirection.x) * Mathf.Rad2Deg; // Convert direction to angle

        if (Time.timeScale != 0f) {
            pivot.rotation = Quaternion.Euler(0, 0, angle);
        }

        if (Mathf.Abs(angle) > 90) {
            pivot.localScale = new Vector3(1, -1, 1);
            guitar.localScale = new Vector3(-1, 1, 1);
        } else {
            pivot.localScale = new Vector3(1, 1, 1);
            guitar.localScale = new Vector3(1, 1, 1);
        }

        if (Input.GetMouseButtonDown(0) && CanFire()) {
            FireNote();
        }
        /*if (Input.GetMouseButtonDown(1)) {
            FireRay();
        }*/
    }
    void FireRay() {

        RaycastHit2D hit = Physics2D.Raycast(transform.position, facingDirection, 50f, hitLayers);
        
        Vector3 endPosition = transform.position + facingDirection * 50f; // Default end point if nothing is hit

        if (hit.collider != null) {
            endPosition = hit.point; // Stop at hit point
            Debug.Log("Hit: " + hit.collider.name);

            // Optionally, apply damage if the target has a health component
            /*EnemyHealth enemy = hit.collider.GetComponent<EnemyHealth>();
            if (enemy != null) {
                enemy.TakeDamage(10);
            }*/
        }

        StartCoroutine(ShowLaser(endPosition));
    }

    bool CanFire() {
        return Time.time >= lastFireTime + fireRate;
    }

    void FireNote() {
        Harmony h = Instantiate(harmony, transform.position, Quaternion.identity);
        h.Initialize(facingDirection);
        guitarAnimator.SetTrigger("Play");
        lastFireTime = Time.time; // Update last fire time
    }

    IEnumerator ShowLaser(Vector3 endPosition) {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, endPosition);
        lineRenderer.enabled = true;

        yield return new WaitForSeconds(laserDuration); // Show for a short time

        lineRenderer.enabled = false;
    }

    void FixedUpdate() {
        rb.velocity = moveInput * moveSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log("ow");
        if (collision.gameObject.tag.Equals("Enemy")) {
            Debug.Log("yowch");
            FindObjectOfType<GameManager>().Lose();
        }
    }
}
