using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement2D : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private DialogueUI dialogueUI;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Interactable2D nearbyInteractable;

    private Animator animator;

    // 初始面向下方。
    private Vector2 facingDirection = Vector2.down;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        UpdateAnimation();
    }
    private void UpdateAnimation()
    {
        if (animator == null)
        {
            return;
        }

        if (moveInput.sqrMagnitude > 0.01f)
        {
            // 四方向动画不使用斜向朝向。
            // 同时按两个方向时，这版优先使用上下方向。
            if (Mathf.Abs(moveInput.y) > 0.01f)
            {
                facingDirection = moveInput.y > 0f
                    ? Vector2.up
                    : Vector2.down;
            }
            else
            {
                facingDirection = moveInput.x > 0f
                    ? Vector2.right
                    : Vector2.left;
            }
        }

        animator.SetFloat("MoveX", facingDirection.x);
        animator.SetFloat("MoveY", facingDirection.y);
        animator.SetFloat("Speed", moveInput.magnitude);
    }


    private void Update()
    {
        // F 或 Esc 这一帧，不处理人物移动和交互。
        // 避免关闭菜单时，同一按键又传给人物。
        if (Input.GetKeyDown(KeyCode.F) ||
            Input.GetKeyDown(KeyCode.Escape))
        {
            moveInput = Vector2.zero;

            // Esc 仍然可以退出普通对话。
            if (Input.GetKeyDown(KeyCode.Escape) &&
                dialogueUI != null &&
                dialogueUI.IsOpen)
            {
                dialogueUI.Close();
            }

            return;
        }

        // 对话期间禁止移动，Enter 用于翻页。
        if (dialogueUI != null && dialogueUI.IsOpen)
        {
            moveInput = Vector2.zero;

            if (ConfirmPressed())
            {
                dialogueUI.NextLine();
            }

            return;
        }

        // WASD 或方向键移动。
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        moveInput = new Vector2(
            horizontal,
            vertical
        ).normalized;

        
    }

    private void FixedUpdate()
    {
        if (dialogueUI != null && dialogueUI.IsOpen)
        {
            moveInput = Vector2.zero;
        }

        rb.velocity = moveInput * moveSpeed;

        UpdateAnimation();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        Interactable2D target =
            other.GetComponent<Interactable2D>();

        if (target != null)
        {
            nearbyInteractable = target;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Interactable2D target =
            other.GetComponent<Interactable2D>();

        if (target == nearbyInteractable)
        {
            nearbyInteractable = null;
        }
    }

    private bool ConfirmPressed()
    {
        return Input.GetKeyDown(KeyCode.Return) ||
               Input.GetKeyDown(KeyCode.KeypadEnter);
    }

    private void OnDisable()
    {
        moveInput = Vector2.zero;

        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }

        UpdateAnimation();
    }

}
