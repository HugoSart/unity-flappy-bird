using UnityEngine;

public class Bird : MonoBehaviour {
    
    // Properties
    [SerializeField]
    private float initialSpeed = 0.7f;
    
    [SerializeField]
    private float jumpSpeed = 1f;

    [SerializeField]
    private float maximumFallSpeed = 1f;

    [SerializeField]
    private float timeToStartFallingRotation = 0.75f;
    
    [SerializeField]
    private float topRotationAngle = 20f;
    
    [SerializeField]
    private float bottomRotationAngle = -90f;

    [SerializeField]
    private float fallingRotationDuration = 1f;
    
    // References
    private GameEvents _gameEvents;
    private Camera _camera;
    private float _currentSpeed;
    private bool _isDead;
    private float _initialGravityScale;
    private bool _isStarted;
    private Quaternion _topQuaternion;
    private Quaternion _bottomQuaternion;
    private float _fallingRotationElapsedTime;

    // Components
    private Rigidbody2D _rigidbody2D;
    private CapsuleCollider2D _capsuleCollider2D;
    private Floating _floating;
    private Animator _animator;


    // =================================================================================================================
    // Unity Lifecycle
    // =================================================================================================================
    private void Start() {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        _floating = GetComponent<Floating>();
        _animator = GetComponent<Animator>();
        
        _gameEvents = GameEvents.main;
        _camera = Camera.main;
        _currentSpeed = initialSpeed;
        _initialGravityScale = _rigidbody2D.gravityScale;
        _isStarted = false;
        _isDead = false;
        _topQuaternion = Quaternion.Euler(0, 0, topRotationAngle);
        _bottomQuaternion = Quaternion.Euler(0, 0, bottomRotationAngle);
        
        _rigidbody2D.velocity = new Vector2(_currentSpeed, 0);

        if (_floating.enabled) {
            _rigidbody2D.gravityScale = 0;
        }
        
    }
    
    private void Update() {
        var pos = transform.position;
        
        if (!_isDead && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0))) {
            _floating.enabled = false;
            _rigidbody2D.gravityScale = _initialGravityScale;
            if (!_isStarted) {
                _StartGame();
                _isStarted = true;
            }
            _Jump();
        }

        // Limit maximum fall speed
        if (_rigidbody2D.velocity.y < -maximumFallSpeed) {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, -maximumFallSpeed);
        }
        
        // Update camera to follow bird on x-axis
        if (_camera is not null) {
            var camPos = _camera.transform.position;
            _camera.transform.position = new Vector3(pos.x, camPos.y, camPos.z);
        }
        
        // Other updates
        _UpdateBirdRotation();

    }
    
    private void OnCollisionEnter2D(Collision2D other) {
        _Die();
    }


    // =================================================================================================================
    // Utilities
    // =================================================================================================================
    private void _StartGame() {
        _gameEvents.TriggerOnGameStart();
        _animator.speed = 2f;
    }

    private void _UpdateBirdRotation() {
        if (!_isStarted) return;
        if (_fallingRotationElapsedTime < fallingRotationDuration + timeToStartFallingRotation) {
            if (_fallingRotationElapsedTime >= timeToStartFallingRotation) {
                var fraction = (_fallingRotationElapsedTime - timeToStartFallingRotation) / fallingRotationDuration;
                transform.rotation = Quaternion.Lerp(_topQuaternion, _bottomQuaternion, fraction);
            }
            _fallingRotationElapsedTime += Time.deltaTime;
        }
    }
    
    private void _Jump() {
        if (transform.position.y >= 1.5) return;
        _rigidbody2D.velocity = new Vector2(_currentSpeed, jumpSpeed);
        _animator.Rebind();
        transform.rotation = _topQuaternion;
        _fallingRotationElapsedTime = 0;
    }

    private void _Die() {
        _isDead = true;
        _isStarted = false;
        _rigidbody2D.velocity = new Vector2(0, 0);
        _animator.speed = 0;
        // _rigidbody2D.simulated = false;
        _gameEvents.TriggerOnGameEnd();
    }
    
}
