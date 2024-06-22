using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float _horizontal;
    private float _speed = 10;
    private bool _isInRightDirection;

    private bool _canDash = true;
    private bool _isDashing;
    public float _dashPower = 1.5f;
    private float _dashTime = 0.6f;
    private float _dashCooldown = 2f;


    private Rigidbody2D _rigidbody;

    private void Start()
    {
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
    }

    private void FixedUpdate()
    {
        if (_isDashing)
        {
            return;
        }
        
        _rigidbody.velocity = new Vector2(_horizontal * _speed, _rigidbody.velocity.y);

        
    }


    private void Flip()
    {
        if (_isInRightDirection &&  _horizontal < 0f || !_isInRightDirection && _horizontal > 0f) 
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
}
