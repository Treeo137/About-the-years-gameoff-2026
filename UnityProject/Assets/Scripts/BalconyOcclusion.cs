using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BalconyOcclusion : MonoBehaviour
{
    [Header("阳台门窗的 Sprite Renderer")]
    [SerializeField] private SpriteRenderer _doorWindowRenderer;

    [Header("门窗显示顺序")]
    [SerializeField] private int _outsideOrder = 10;
    [SerializeField] private int _insideOrder = -1;

    // 支持玩家身上存在多个 Collider 的情况。
    private readonly HashSet<Collider2D> _playerColliders =
        new HashSet<Collider2D>();

    private void Awake()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
        SetWindowOrder(_outsideOrder);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsPlayer(other))
        {
            return;
        }

        _playerColliders.Add(other);
        SetWindowOrder(_insideOrder);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!_playerColliders.Remove(other))
        {
            return;
        }

        if (_playerColliders.Count == 0)
        {
            SetWindowOrder(_outsideOrder);
        }
    }

    private bool IsPlayer(Collider2D other)
    {
        return other.GetComponentInParent<PlayerMovement2D>() != null;
    }

    private void SetWindowOrder(int order)
    {
        if (_doorWindowRenderer != null)
        {
            _doorWindowRenderer.sortingOrder = order;
        }
    }

    private void OnDisable()
    {
        _playerColliders.Clear();
        SetWindowOrder(_outsideOrder);
    }
}
