using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public int jumpCount = 2;
    private int _jumpRemain;
    public float jumpForce = 15;
    public float groundCheckRadius = 0.5f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _jumpRemain = jumpCount;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump") && (isGrounded || _jumpRemain > 0))
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            _jumpRemain -= 1;
        }

        if(isGrounded)
        {
           _jumpRemain = jumpCount;
        }
        
        //isGrounded = Physics2D.OverlapCircle(transform.position, groundCheckRadius, groundLayer);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        isGrounded = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        isGrounded = false;
    }
}