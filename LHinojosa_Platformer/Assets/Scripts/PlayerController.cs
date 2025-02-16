using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public PlayerSettings _ps;
    
    private Rigidbody2D _rB;
    private SpriteRenderer _sR;
    private CapsuleCollider2D _col;
    private Bounds _bounds;
    private float _xInput;
    private float _yInput;
    private bool _isJumpPressed;
    private bool _isGrounded;

    private Vector2 _btmLocalRayPos;
    private Vector2 _topLocalRayPos;

    private bool _gravityUp;
    
    private Camera _mainCamera;

    void Start()
    {
        _rB = GetComponent<Rigidbody2D>();
        _sR = GetComponentInChildren<SpriteRenderer>();
        _col = GetComponent<CapsuleCollider2D>();
        _bounds = _col.bounds;
        _btmLocalRayPos = new Vector2(0f, _bounds.center.y - _bounds.extents.y);
        _topLocalRayPos = new Vector2(0f, _bounds.extents.y + Mathf.Abs(_col.offset.y));
        _mainCamera = Camera.main;
    }

    void Update()
    {
        _xInput = Input.GetAxis("Horizontal");
        _yInput = Input.GetAxis("Vertical");

        if (_isGrounded)
        {
            if ((!_gravityUp && _rB.velocity.y <= 0) || (_gravityUp && _rB.velocity.y >= 0))
            {
                if (!_isJumpPressed && _yInput > 0)
                    _isJumpPressed = true;
            }

            if (Input.GetButtonDown("Jump"))
            {
                _gravityUp = !_gravityUp;
                _sR.flipY = _gravityUp;
                _col.offset = new Vector2(0, _col.offset.y * -1);
            }
        }
    }
    
    void FixedUpdate()
    {
        //LeftRight
        _rB.velocity = new Vector2(0, _rB.velocity.y);
        _rB.AddForce(Vector2.right * (_xInput * _ps._moveSpeed), ForceMode2D.Impulse);
        
        //Jump
        RaycastHit2D hit;
        if (_gravityUp)
        {
            _rB.gravityScale = -Mathf.Abs(_rB.gravityScale);
            
            hit = Physics2D.Raycast((Vector2)transform.position + _topLocalRayPos, Vector2.up, _ps._raycastDistance,
                LayerMask.GetMask("Ground"));
            
            Debug.DrawRay((Vector2)transform.position + _topLocalRayPos, Vector2.up * _ps._raycastDistance, Color.red);
        }
        else
        {
            _rB.gravityScale = Mathf.Abs(_rB.gravityScale);
            
            hit = Physics2D.Raycast((Vector2)transform.position + _btmLocalRayPos, Vector2.down, _ps._raycastDistance,
                LayerMask.GetMask("Ground"));
            
            Debug.DrawRay((Vector2)transform.position + _btmLocalRayPos, Vector2.down * _ps._raycastDistance, Color.red);
        }
        
        if (hit.collider != null)
            _isGrounded = true;
        else
            _isGrounded = false;

        if (_isGrounded && _isJumpPressed)
        {
            _isJumpPressed = false;
            
            if (_gravityUp)
                _rB.velocity = new Vector2(_rB.velocity.x, -_ps._jumpSpeed);
            else
                _rB.velocity = new Vector2(_rB.velocity.x, _ps._jumpSpeed);
        }
    }
}
