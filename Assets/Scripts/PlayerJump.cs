using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private Vector2 _gravityVec;


    [SerializeField] private int _jumpPower = 1;
    [SerializeField] private float _fallRatio = 1;

    private float _coyoteTime = 0.2f;
    private float _coyoteTimeCounter;
    private float _bufferTime = 0.2f;
    private float _bufferTimeCounter;

    public Transform groundCheck;
    public LayerMask groundLayer;
    private void Start()
    {
        _gravityVec = new Vector2(0, -Physics2D.gravity.y);
        _rigidbody = GetComponent<Rigidbody2D>();
    }



    private void FixedUpdate()
    {
        if (isGrounded())
        {
            _coyoteTimeCounter = _coyoteTime;
        }
        else
        {
            _coyoteTimeCounter -= Time.fixedDeltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            _bufferTimeCounter = _bufferTime;
        }
        else
        {
            _bufferTimeCounter -= Time.fixedDeltaTime;
        }

        if(_bufferTimeCounter > 0f && _coyoteTimeCounter > 0f)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpPower);
            _bufferTimeCounter = 0f;
        }


        if (Input.GetButtonUp("Jump") && _rigidbody.velocity.y > 0f)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y * 0.5f);
            _coyoteTimeCounter = 0f;
        }

        if (_rigidbody.velocity.y < 0)
        {
            _rigidbody.velocity -= _gravityVec * _fallRatio * Time.fixedDeltaTime;
        }
    }

    private bool isGrounded()
    {
        return Physics2D.OverlapCapsule(groundCheck.position, new Vector2(1f, 0.3f), CapsuleDirection2D.Horizontal, 0, groundLayer);
    }
}
