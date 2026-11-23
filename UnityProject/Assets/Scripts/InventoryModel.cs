using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryModel : MonoBehaviour
{
    public const int Capacity = 8;

    [Header("初始物品：用于测试，正式游戏可留空")]
    [SerializeField]
    private List<ItemData> items =
        new List<ItemData>();

    [Header("初始信物：用于测试，正式游戏可留空")]
    [SerializeField]
    private List<ItemData> relics =
        new List<ItemData>();

    public event Action Changed;

    public ItemData GetItem(ItemCategory category, int index)
    {
        List<ItemData> list = GetList(category);

        if (index < 0 || index >= list.Count)
        {
            return null;
        }

        return list[index];
    }

    public ItemData CalendarRelic
    {
        get
        {
            foreach (ItemData relic in relics)
            {
                if (relic != null && relic.isCalendarRelic)
                {
                    return relic;
                }
            }

            return null;
        }
    }

    public bool Contains(ItemData item)
    {
        if (item == null)
        {
            return false;
        }

        return ContainsInList(items, item) ||
               ContainsInList(relics, item);
    }

    public bool TryAdd(ItemData item)
    {
        if (item == null || string.IsNullOrWhiteSpace(item.itemId))
        {
            Debug.LogWarning("物品没有配置有效的 Item ID。");
            return false;
        }

        // 当前原型不做叠加，同一 ID 只能获得一次。
        if (Contains(item))
        {
            return false;
        }

        List<ItemData> list = GetList(item.category);

        if (list.Count >= Capacity)
        {
            Debug.LogWarning("该分类的背包已经满了。");
            return false;
        }

        // 添加到末尾，所以会按获得顺序排列。
        list.Add(item);

        Changed?.Invoke();
        return true;
    }

    private List<ItemData> GetList(ItemCategory category)
    {
        return category == ItemCategory.Item ? items : relics;
    }

    private bool ContainsInList(
        List<ItemData> list,
        ItemData target)
    {
        foreach (ItemData item in list)
        {
            if (item == null)
            {
                continue;
            }

            if (item == target ||
                (!string.IsNullOrEmpty(target.itemId) &&
                 item.itemId == target.itemId))
            {
                return true;
            }
        }

        return false;
    }
}
