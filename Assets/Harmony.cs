using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harmony : MonoBehaviour
{
    [SerializeField] float speed = 6f; // Adjust movement speed
    [SerializeField] float oscillationSpeed = 100f; // Adjust movement speed
    [SerializeField] float oscillationDistance = 1f; // Adjust movement speed
    public Vector3 direction;
    public Vector3 perpendicularDirection;
    private float elapsedTime = 0f;

    public void Initialize(Vector3 dir) {
        direction = dir;
        perpendicularDirection = Vector3.Cross(direction.normalized, Vector3.forward);
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        // Move forward at a constant speed
        transform.position += direction.normalized * speed * Time.deltaTime;

        // Calculate perpendicular oscillation using Sin wave
        float oscillation = Mathf.Sin(elapsedTime * oscillationSpeed) * oscillationDistance;

        // Apply perpendicular movement
        transform.position += perpendicularDirection * oscillation * Time.deltaTime;
    }
}
