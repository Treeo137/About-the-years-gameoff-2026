using UnityEngine;

public class PlayerInteractor2D : MonoBehaviour
{
    [Header("检测")]
    [SerializeField] private float interactionRadius = 1f;
    [SerializeField] private LayerMask interactionLayers;

    [Header("界面：打开时禁止场景交互")]
    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private InventoryUI inventoryUI;

    private WorldInteractable2D currentTarget;

    private void Update()
    {
        bool blocked =
            (dialogueUI != null && dialogueUI.IsOpen) ||
            (inventoryUI != null && inventoryUI.IsOpen);

        if (blocked ||
            Input.GetKeyDown(KeyCode.F) ||
            Input.GetKeyDown(KeyCode.Escape))
        {
            currentTarget = null;
            return;
        }

        FindNearestTarget();

        bool confirmPressed =
            Input.GetKeyDown(KeyCode.Return) ||
            Input.GetKeyDown(KeyCode.KeypadEnter);

        if (confirmPressed && currentTarget != null)
        {
            currentTarget.Interact();
            currentTarget = null;
        }
    }

    private void FindNearestTarget()
    {
        currentTarget = null;
        float nearestDistance = float.PositiveInfinity;

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            interactionRadius,
            interactionLayers
        );

        foreach (Collider2D hit in hits)
        {
            WorldInteractable2D target =
                hit.GetComponentInParent<WorldInteractable2D>();

            if (target == null || !target.CanInteract)
            {
                continue;
            }

            float distance = (
                target.transform.position - transform.position
            ).sqrMagnitude;

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                currentTarget = target;
            }
        }
    }

    private void OnDisable()
    {
        currentTarget = null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(
            transform.position,
            interactionRadius
        );
    }
}
