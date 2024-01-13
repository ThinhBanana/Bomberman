using System;
using UnityEngine;

public class MovementController : MonoBehaviour
{


    public Rigidbody2D rigidBody { get; private set; }
    public float movingSpeed = 5f;

    public KeyCode inputUp = KeyCode.W;
    public KeyCode inputDown = KeyCode.S;
    public KeyCode inputRight = KeyCode.D;
    public KeyCode inputLeft = KeyCode.A;

    public AnimatedSpriteRenderer spriteRendererUp;
    public AnimatedSpriteRenderer spriteRendererDown;
    public AnimatedSpriteRenderer spriteRendererLeft;
    public AnimatedSpriteRenderer spriteRendererRight;
    public AnimatedSpriteRenderer spriteRendererDeath;


    private Vector2 direction = Vector2.down;
    private AnimatedSpriteRenderer activeSpriteRenderer;


    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        activeSpriteRenderer = spriteRendererDown;
        spriteRendererDeath.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKey(inputUp))
        {
            SetDirection(Vector2.up, spriteRendererUp);
        }
        else if (Input.GetKey(inputDown))
        {
            SetDirection(Vector2.down, spriteRendererDown);
        }
        else if (Input.GetKey(inputLeft))
        {
            SetDirection(Vector2.left, spriteRendererLeft);
        }
        else if (Input.GetKey(inputRight))
        {
            SetDirection(Vector2.right, spriteRendererRight);
        }
        else
        {
            SetDirection(Vector2.zero, activeSpriteRenderer);
        }
    }

    private void FixedUpdate()
    {
        Vector2 position = rigidBody.position;
        Vector2 translation = direction * movingSpeed * Time.fixedDeltaTime;

        rigidBody.MovePosition(position + translation);
    }

    private void SetDirection(Vector2 newDirection, AnimatedSpriteRenderer animatedSpriteRenderer)
    {
        direction = newDirection;

        // Change sprite when move
        spriteRendererUp.enabled = animatedSpriteRenderer == spriteRendererUp;
        spriteRendererDown.enabled = animatedSpriteRenderer == spriteRendererDown;
        spriteRendererLeft.enabled = animatedSpriteRenderer == spriteRendererLeft;
        spriteRendererRight.enabled = animatedSpriteRenderer == spriteRendererRight;

        // When no input, keep the idle anim of the current direction
        activeSpriteRenderer = animatedSpriteRenderer;
        activeSpriteRenderer.idle = direction == Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Explosion"))
        {
            PlayerDie();
        }
    }

    private void PlayerDie()
    {
        enabled = false;
        GetComponent<BombController>().enabled = false;

        spriteRendererUp.enabled = false;
        spriteRendererDown.enabled = false;
        spriteRendererLeft.enabled = false;
        spriteRendererRight.enabled = false;
        spriteRendererDeath.enabled = true;

        Invoke(nameof(OnDeathAnimEnded), 1.25f);
    }

    private void OnDeathAnimEnded()
    {
        gameObject.SetActive(false);
        FindObjectOfType<GameManager>().CheckWinState();
    }


}
