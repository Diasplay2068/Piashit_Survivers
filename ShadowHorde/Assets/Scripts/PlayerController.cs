using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sr;
    Vector2 input;

    void Awake()
    {
        rb   = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();      // precisa de ter Animator no GameObject
        sr   = GetComponent<SpriteRenderer>(); // precisa de ter SpriteRenderer no GameObject
    }

    void Update()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        float x = 0f, y = 0f;
        if (kb.dKey.isPressed || kb.rightArrowKey.isPressed) x += 1f;
        if (kb.aKey.isPressed || kb.leftArrowKey.isPressed) x -= 1f;
        if (kb.wKey.isPressed || kb.upArrowKey.isPressed)   y += 1f;
        if (kb.sKey.isPressed || kb.downArrowKey.isPressed) y -= 1f;

        input = new Vector2(x, y).normalized;

        // --- Animações ---
        bool isMoving = input.magnitude > 0.1f;
        anim.SetBool("isMoving", isMoving);

        // Vira o sprite conforme a direção horizontal
        if (input.x < 0) sr.flipX = true;
        else if (input.x > 0) sr.flipX = false;
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + input * moveSpeed * Time.fixedDeltaTime);
    }
}