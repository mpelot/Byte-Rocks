using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public int score = 0;

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI endScoreText;
    [SerializeField] GameObject loseScreen;
    [SerializeField] float enemySpawnTime = 3f;
    [SerializeField] Transform player;
    [SerializeField] Enemy enemy;
    [SerializeField] Bounds enemySpawnBounds;
    [SerializeField] Camera cam;
    [SerializeField] float spawnCheckPadding = 2f; // Extra padding outside the camera view
    private bool spawnEnemies = true;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy() {

        while (spawnEnemies) {

            bool foundPoint = true;

            Vector2 spawnPoint;
            int attempts = 0;
            do {
                spawnPoint = GetRandomPointInBounds();
                attempts++;

                if (attempts > 100) // Prevent infinite loops
                {
                    Debug.LogWarning("Couldn't find a valid spawn point after 100 attempts.");
                    foundPoint = false;
                    break;
                }

            } while (IsPointInCameraView(spawnPoint));

            if (foundPoint) {
                Enemy e = Instantiate(enemy, spawnPoint, Quaternion.identity);
                e.Initialize(player);
            }

            yield return new WaitForSeconds(enemySpawnTime);

            if (enemySpawnTime > 0.7f) {
                enemySpawnTime -= 0.1f;
            }

        }
    }

    Vector2 GetRandomPointInBounds() {
        Bounds bounds = enemySpawnBounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);
        return new Vector2(x, y);
    }

    bool IsPointInCameraView(Vector2 point) {
        Vector3 viewportPoint = cam.WorldToViewportPoint(point);
        return viewportPoint.x > -spawnCheckPadding && viewportPoint.x < 1 + spawnCheckPadding &&
               viewportPoint.y > -spawnCheckPadding && viewportPoint.y < 1 + spawnCheckPadding;
    }

    public void UpdateScore(int amount) {
        score += amount;
        scoreText.text = score.ToString();
    }

    public void Lose() {
        Debug.Log("lost");
        Time.timeScale = 0f;
        loseScreen.SetActive(true);
        endScoreText.text = "Final Score: " + score.ToString();
    }

    public void Reset() {
        Time.timeScale = 1f;
        score = 0;
        SceneManager.LoadScene(0);
    }
}
