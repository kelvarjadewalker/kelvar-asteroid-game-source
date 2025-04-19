using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 500.0f;
    [SerializeField] private float lifeTime = 5.0f;

    // Internal Components
    private Rigidbody2D _rigidbody;

    // Unity Events
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
    }

    // External Methods
    public void Project(Vector2 direction)
    {
        _rigidbody.AddForce(direction * speed);
        Destroy(gameObject, lifeTime);
    }

}
