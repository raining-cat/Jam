using UnityEngine;

public class GhostMode : MonoBehaviour
{
    public float ghostGravity = 0.1f;
    public float ghostOpacity = 0.5f;
    public float ghostHardness = 0.1f;
    public Collider2D collider;
    private Collider2D _colliderOther;

    private Rigidbody2D rb;
    private bool isGhostMode = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ToggleGhostMode();
        }
        if(!isGhostMode)
        {
            GetComponent<Renderer>().material.color = Color.white;
            rb.gravityScale = 1;
            rb.drag = 0;
        }
    }

    private void ToggleGhostMode()
    {
        isGhostMode = !isGhostMode;

        if (isGhostMode)
        {
            // Установка прозрачности
            GetComponent<Renderer>().material.color = new Color(1, 1, 1, ghostOpacity);

            // Установка низкой гравитации
            rb.gravityScale = ghostGravity;

            // Установка низкой твёрдости
            rb.drag = ghostHardness;
        }
        else
        {
            // Восстановление обычных параметров
            GetComponent<Renderer>().material.color = Color.white;
            rb.gravityScale = 1;
            rb.drag = 0;
            collider.enabled = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (isGhostMode)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("PassableObject"))
            {
                collision.collider.enabled = false;
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (isGhostMode)
        {
            collider.enabled = true;
        }
        collision.collider.enabled = true;
    }
}