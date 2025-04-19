using UnityEngine;

public class Asteroid : MonoBehaviour
{
    // Editor Fields
    [SerializeField] private int pointsPerKill = 100;
    [SerializeField] private Sprite[] spriteArray;
    [SerializeField] private float speed = 50.0f;
    [SerializeField] private float maxLifetime = 30.0f;
    [SerializeField] private float spriteSize = 1.0f;
    [SerializeField] private float minSpriteSize = 0.5f;
    [SerializeField] private float maxSpriteSize = 1.5f;
    [SerializeField] private float minSplitFactor = 0.5f;

    // Internal Components
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;


    // Unity Events
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        var randomIndex = Random.Range(0, spriteArray.Length);
        var randomAngle = Random.value * 360.0f;

        // Rotate the sprite so the asteroid looks different
        _spriteRenderer.sprite = spriteArray[randomIndex];
        transform.eulerAngles = new Vector3(0.0f, 0.0f, randomAngle);

        // Set the size of the asteroid
        transform.localScale = Vector3.one * spriteSize;

        // Set the mass of the asteroid
        const float density = 4.0f;
        _rigidbody.mass = spriteSize * density;

        Destroy(gameObject, maxLifetime);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // We only care about asteroid to bullet collisions
        if (!other.gameObject.CompareTag("Bullet")) return;

        AudioManager.Instance.PlayExplosionSfx();

        if (spriteSize * 0.5f > minSpriteSize)
        {
            CreateSpilt();
        }

        // Base the points received on the asteroid size
        var pointsThisKill = Mathf.Floor(pointsPerKill / spriteSize);

        // Destroy the asteroid and call the GameManager to update the score
        GameManager.Instance.AsteroidHasBeenDestroyed((int)pointsThisKill, transform.position);
        Destroy(gameObject);
    }

    // Internal Functions
    private void CreateSpilt(int numberOfSplits = 2)
    {
        for (var i = 0; i < numberOfSplits; i++)
        {
            // Create a new asteroid with a smaller size and position close to the original transform position 
            var newSpriteSize = spriteSize * minSplitFactor;
            Vector2 newPosition = transform.position;
            newPosition += Random.insideUnitCircle * spriteSize * minSplitFactor;
            var splitAsteroid = Instantiate(this, newPosition, transform.rotation);
            splitAsteroid.SetAsteroidSize(newSpriteSize);
            splitAsteroid.SetTrajectory(Random.insideUnitCircle.normalized);
        }

    }


    // External Functions
    public void SetAsteroidSize(float newSize = 0.0f)
    {
        if (newSize < 0.0f)
            Debug.LogError("Asteroid size cannot be negative");

        spriteSize = newSize == 0.0f ? Random.Range(minSpriteSize, maxSpriteSize) : newSize;
    }

    public void SetTrajectory(Vector2 direction)
    {
        // The asteroid only needs a force to be added once since they have no
        // drag to make them stop moving
        _rigidbody.AddForce(direction * speed);
    }

}
