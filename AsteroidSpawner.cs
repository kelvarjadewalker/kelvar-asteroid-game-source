using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public static AsteroidSpawner Instance { get; private set; }

    // Editor Fields
    [Tooltip("Asteroid Prefab to spawn")]
    [SerializeField] private Asteroid asteroidPrefab;

    [Tooltip("Cone for the angle the asteroid will take towards the center")]
    [SerializeField]
    private float trajectoryVariance = 15.0f;

    [Tooltip("Rate at which the asteroids will spawn")]
    [SerializeField] private float spawnRate = 2.0f;

    [Tooltip("Amount of asteroids to spawn each time")]
    [SerializeField] private int spawnAmount = 1;

    [Tooltip("Radius of the circle where the asteroids will spawn to create distance from the center")]
    [SerializeField] private float spawnRadius = 15.0f;

    // Unity Events
    private void Awake()
    {
        Instance = this;
    }

    // Internal Functions
    private void SpawnAsteroid()
    {
        for (var i = 0; i < spawnAmount; i++)
        {
            // Randomize the spawn position to be outside the screen
            Vector3 spawnDirection = Random.insideUnitCircle.normalized * spawnRadius;
            var spawnPoint = transform.position + spawnDirection;

            // Randomize the trajectory of the asteroid
            var variance = Random.Range(-trajectoryVariance, trajectoryVariance);
            var rotation = Quaternion.AngleAxis(variance, Vector3.forward);

            var asteroid = Instantiate(asteroidPrefab, spawnPoint, transform.rotation);
            asteroid.SetAsteroidSize();

            asteroid.SetTrajectory(rotation * -spawnDirection);
        }

    }

    public void StartSpawning()
    {
        InvokeRepeating(nameof(SpawnAsteroid), spawnRate, spawnRate);
    }

    public void StopSpawning()
    {
        CancelInvoke(nameof(SpawnAsteroid));
    }
}
