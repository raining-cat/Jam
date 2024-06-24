using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{

    private bool _isInRightDirection = true;
    private bool _isWallSliding;
    private bool _isWallJumping;
    private bool _canDash = true;
    private bool _isDashing;


    [SerializeField] private float _wallSlidingSpeed;
    [SerializeField] private int _jumpPower = 1;
    [SerializeField] private float _fallRatio = 1;

    private Rigidbody2D _rigidbody;

    private Vector2 _gravityVec;
    private Vector2 _wallJumpPower = new Vector2(8f, 16f);

    private float _dashTime = 0.6f;
    private float _dashCooldown = 2f;
    public float _dashPower = 1.5f;
    private float _horizontal;
    private float _speed = 10;
    private float horizontal;
    private float _coyoteTime = 0.2f;
    private float _coyoteTimeCounter;
    private float _bufferTime = 0.2f;
    private float _bufferTimeCounter;
    private float _wallJumpingDirection;
    private float _wallJumpingTime = 0.2f;
    private float _wallJumpingCounter;
    private float _wallJumpingDuration = 0.4f;



    public Transform groundCheck;
    public LayerMask groundLayer;
    public Transform wallCheck;
    public LayerMask wallLayer;

    private void Start()
    {
        _gravityVec = new Vector2(0, -Physics2D.gravity.y);
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (_isDashing)
        {
            return;
        }
        _horizontal = Input.GetAxisRaw("Horizontal");
        Flip();
        if (Input.GetKeyDown(KeyCode.LeftShift) && _canDash)
        {
            StartCoroutine(Dash());
        }

        if (isGrounded())
        {
            _coyoteTimeCounter = _coyoteTime;
        }
        else
        {
            _coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            _bufferTimeCounter = _bufferTime;
        }
        else
        {
            _bufferTimeCounter -= Time.deltaTime;
        }

        if (_bufferTimeCounter > 0f && _coyoteTimeCounter > 0f)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpPower);
            _bufferTimeCounter = 0f;
        }

        if (Input.GetButtonUp("Jump") && _rigidbody.velocity.y > 0f)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y * 0.4f);
            _coyoteTimeCounter = 0f;
        }

        if (_rigidbody.velocity.y < 0)
        {
            _rigidbody.velocity -= _gravityVec * _fallRatio * Time.deltaTime;
        }

        WallSlide();
        WallJump();

        if (!_isWallJumping)
        {
            Flip();
        }
    }

    private void FixedUpdate()
    {
        if (_isDashing)
        {
            return;
        }
        if (!_isWallJumping)
        {
            _rigidbody.velocity = new Vector2(_horizontal * _speed, _rigidbody.velocity.y);
        }



    }


    private void Flip()
    {
        if (_isInRightDirection && _horizontal < 0f || !_isInRightDirection && _horizontal > 0f)
        {
            _isInRightDirection = !_isInRightDirection;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;

        }
    }

    private IEnumerator Dash()
    {
        _canDash = false;
        _isDashing = true;
        float originalGrav = _rigidbody.gravityScale;
        _rigidbody.gravityScale = 0f;
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x * _dashPower, 0f);
        yield return new WaitForSeconds(_dashTime);
        _rigidbody.gravityScale = originalGrav;
        _isDashing = false;
        yield return new WaitForSeconds(_dashCooldown);
        _canDash = true;
    }
    private bool isGrounded()
    {
        return Physics2D.OverlapCapsule(groundCheck.position, new Vector2(0.9f, 0.3f), CapsuleDirection2D.Horizontal, 0, groundLayer);
    }


    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.5f, wallLayer);
    }

    private void WallSlide()
    {

        if (IsWalled() && !isGrounded())
        {
            _isWallSliding = true;

            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, Mathf.Clamp(_rigidbody.velocity.y, -_wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            _isWallSliding = false;
        }
    }
    private void WallJump()
    {
        if (_isWallSliding)
        {
            _isWallJumping = false;
            _wallJumpingDirection = -transform.localScale.x;
            _wallJumpingCounter = _wallJumpingTime;
            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            _wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && _wallJumpingCounter > 0f)
        {
            _isWallJumping = true;
            _rigidbody.velocity = new Vector2(_wallJumpPower.x * _wallJumpingDirection, _wallJumpPower.y);
            _wallJumpingCounter = 0f;
            if (transform.localScale.x != _wallJumpingDirection)
            {
                _isInRightDirection = !_isInRightDirection;
                Vector3 localscale = transform.localScale;
                localscale.x *= -1f;
                transform.localScale = localscale;
            }
            Invoke(nameof(StopWallJumping), _wallJumpingDuration);
        }

    }

    private void StopWallJumping()
    {
        _isWallJumping = false;
    }
}
