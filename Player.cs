using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private float thrustSpeed = 1.0f;
    [SerializeField] private float turnSpeed = 1.0f;
    [SerializeField] private int enableCollisionsDelay = 3;
    [SerializeField] private Color invincibilityColor = Color.yellow;

    // Internal Components
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;

    // Internal variables
    private const int LeftMouseButton = 0;
    private bool _isThrusting;
    private float _turnDirection;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        gameObject.layer = LayerMask.NameToLayer("IgnoreCollisions");
        _spriteRenderer.color = invincibilityColor;
        Invoke(nameof(EnableCollisions), enableCollisionsDelay);
    }

    private void Update()
    {
        // Pointer is over UI, ignore game input
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        
        _isThrusting = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            _turnDirection = 1f;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            _turnDirection = -1f;
        }
        else
        {
            _turnDirection = 0.0f;
        }

        // Fire the bullet, force the player to press the button for each shot
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(LeftMouseButton))
        {
            Shoot();
        }

    }

    private void FixedUpdate()
    {
        if (_isThrusting)
        {
            _rigidbody.AddForce(transform.up * thrustSpeed);
            // AudioManager.Instance.PlayThrusterSfx();
        }

        if (_turnDirection != 0.0f)
        {
            _rigidbody.AddTorque(_turnDirection * turnSpeed);
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Asteroid")) return;

        _rigidbody.linearVelocity = Vector2.zero;
        _rigidbody.angularVelocity = 0.0f;

        AudioManager.Instance.PlayExplosionSfx();

        GameManager.Instance.PlayerHasDied();

        gameObject.SetActive(false);
    }

    // Internal Functions

    private void EnableCollisions()
    {
        _spriteRenderer.color = Color.white;
        gameObject.layer = LayerMask.NameToLayer("Player");
    }

    private void Shoot()
    {
        var bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        bullet.Project(transform.up);
        AudioManager.Instance.PlayLaserSfx();
    }

}
