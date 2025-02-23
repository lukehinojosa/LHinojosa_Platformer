using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public PlayerSettings _ps;
    
    private Rigidbody2D _rB;
    private SpriteRenderer _sR;
    private BoxCollider2D _col;
    private Bounds _bounds;
    private float _xInput;
    private bool _isJumpPressed;
    private bool _isGravityPressed;
    private bool _isGrounded;
    public float _addVelocity;
    private bool _jumpAvailable;
    private bool _jumpBuffer;
    private bool _gravityBuffer;
    private float _inputBufferTimer = 0.2f;
    private float _coyoteTime = 0.1f;
    private bool _coyoteCoroutineRunning;
    private float _initialGravity;
    private bool _doJump;
    private bool _doFlipGravity;

    private bool _gravityUp;

    public bool GetGravityUp()
    {return _gravityUp;}
    
    private Camera _mainCamera;

    private Animator _animator;

    void Start()
    {
        _rB = GetComponent<Rigidbody2D>();
        _sR = GetComponentInChildren<SpriteRenderer>();
        _col = GetComponent<BoxCollider2D>();
        _mainCamera = Camera.main;
        _animator = GetComponentInChildren<Animator>();
        _initialGravity = _rB.gravityScale;
    }

    void Update()
    {
        _xInput = Input.GetAxis("Horizontal");
        
        _animator.SetBool("TryMoving", true);
        if (_xInput < 0)
            _sR.flipX = true;
        else if (_xInput > 0)
            _sR.flipX = false;
        else
            _animator.SetBool("TryMoving", false);
        
        //Jump Input
        if (Input.GetKeyDown(KeyCode.UpArrow))
            _isJumpPressed = true;

        //Flips Gravity related stuff.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isGravityPressed = true;
        }
        
        if ((!_gravityUp && _rB.velocity.y > 0.1f) || (_gravityUp && _rB.velocity.y < -0.1f))
            _animator.SetBool("Jumping", true);
        else if ((!_gravityUp && _rB.velocity.y < 0.1f) || (_gravityUp && _rB.velocity.y > -0.1f))
            _animator.SetBool("Jumping", false);
    }
    
    void FixedUpdate()
    {
        //LeftRight
        _rB.velocity = new Vector2(_addVelocity, _rB.velocity.y);//Reset Velocity
        _rB.AddForce(Vector2.right * (_xInput * _ps._moveSpeed), ForceMode2D.Impulse);

        IsJumpAvailable();
        
        if (_isJumpPressed)
        {
            _isJumpPressed = false;
            
            if (_jumpAvailable)
                _doJump = true;
            else
            {
                _jumpBuffer = true;
                StartCoroutine(JumpBufferTimer());
            }
        }

        if (_doJump)
        {
            Jump();
            _doJump = false;
        }
        
        if (_isGravityPressed)
        {
            _isGravityPressed = false;
            
            _gravityBuffer = true;
            StartCoroutine(GravityBufferTimer());
        }

        if (_doFlipGravity)
        {
            FlipGravity();
            _doFlipGravity = false;
        }
    }

    void FlipGravity()
    {
        _gravityUp = !_gravityUp;
        _sR.flipY = _gravityUp;
        _col.offset = new Vector2(0, _col.offset.y * -1);
                
        StopCoroutine(CoyoteTimer());
        _coyoteCoroutineRunning = false;
        _jumpAvailable = false;
    }

    void IsJumpAvailable()
    {
        RaycastHit2D hit;
        if (_gravityUp)
        {
            Vector2 topLocalRayPos = new Vector2(transform.position.x, _col.bounds.max.y + 0.01f);
            
            hit = Physics2D.Raycast(topLocalRayPos, Vector2.up, _ps._raycastDistance,
                LayerMask.GetMask("Ground", "Player"));
            
            Debug.DrawRay(topLocalRayPos, Vector2.up * _ps._raycastDistance, Color.red);
        }
        else
        {
            Vector2 btmLocalRayPos = new Vector2(transform.position.x, _col.bounds.min.y - 0.01f);
            
            hit = Physics2D.Raycast(btmLocalRayPos, Vector2.down, _ps._raycastDistance,
                LayerMask.GetMask("Ground", "Player"));
            
            Debug.DrawRay(btmLocalRayPos, Vector2.down * _ps._raycastDistance, Color.red);
        }
        
        if (hit.collider != null)
            _isGrounded = true;
        else
            _isGrounded = false;

        if (!_isGrounded)
        {
            if (_jumpAvailable)
            {
                _rB.gravityScale = 0f;
                
                if (!_coyoteCoroutineRunning)
                    StartCoroutine(CoyoteTimer());
            }
            else
            {
                //Do Gravity
                if (_gravityUp)
                    _rB.gravityScale = -_initialGravity;
                else
                    _rB.gravityScale = _initialGravity;
            }
        }
        else
        {
            StopCoroutine(CoyoteTimer());
            _coyoteCoroutineRunning = false;
            
            _jumpAvailable = true;
            
            if (_jumpBuffer)
            {
                _doJump = true;
                _jumpBuffer = false;
            }
            
            if (_gravityBuffer)
            {
                _doFlipGravity = true;
                _gravityBuffer = false;
            }
        }
    }

    void Jump()
    {
        if (_gravityUp)
            _rB.velocity = new Vector2(_rB.velocity.x, -_ps._jumpSpeed);
        else
            _rB.velocity = new Vector2(_rB.velocity.x, _ps._jumpSpeed);
        
        _jumpAvailable = false;
    }
    
    private IEnumerator CoyoteTimer()
    {
        _coyoteCoroutineRunning = true;
        yield return new WaitForSeconds(_coyoteTime);
        _jumpAvailable = false;
        _coyoteCoroutineRunning = false;
    }

    private IEnumerator JumpBufferTimer()
    {
        yield return new WaitForSeconds(_inputBufferTimer);
        _jumpBuffer = false;
    }
    
    private IEnumerator GravityBufferTimer()
    {
        yield return new WaitForSeconds(_inputBufferTimer);
        _gravityBuffer = false;
    }
}
