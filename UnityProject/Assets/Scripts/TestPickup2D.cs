using UnityEngine;

public class TestPickup2D : WorldInteractable2D
{
    [SerializeField] private InventoryModel inventory;
    [SerializeField] private ItemData item;

    public override string Prompt =>
        item == null
            ? "Enter 获取"
            : "Enter 获取：" + item.displayName;

    public override bool CanInteract =>
        base.CanInteract &&
        inventory != null &&
        item != null;

    private void Start()
    {
        // 如果当前背包已经有该物品，
        // 进入场景时不再显示这份测试拾取物。
        if (inventory != null &&
            item != null &&
            inventory.Contains(item))
        {
            gameObject.SetActive(false);
        }
    }

    public override void Interact()
    {
        if (!CanInteract)
        {
            return;
        }

        if (inventory.Contains(item))
        {
            gameObject.SetActive(false);
            return;
        }

        // 暂时对接现有背包；以后改成对方的数据接口。
        bool success = inventory.TryAdd(item);

        if (!success)
        {
            Debug.Log("无法获得物品：背包满了或配置有误。");
            return;
        }

        Debug.Log("获得：" + item.displayName);

        // 获取成功后才隐藏场景物品。
        gameObject.SetActive(false);
    }
}
