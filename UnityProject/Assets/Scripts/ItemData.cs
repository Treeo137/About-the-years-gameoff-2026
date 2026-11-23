using UnityEngine;

public enum ItemCategory
{
    Item,
    Relic
}

[CreateAssetMenu(
    fileName = "NewItem",
    menuName = "Game/Item Data"
)]
public class ItemData : ScriptableObject
{
    [Header("基本信息")]
    public string itemId;

    public string displayName;

    public ItemCategory category;

    public Sprite icon;

    [TextArea(2, 6)]
    public string description;

    [Header("年历特殊信物")]
    public bool isCalendarRelic;

    [Tooltip("当前原型只支持 2000 到 2099 年")]
    public int targetYear = 2001;

    [Range(1, 12)]
    public int targetMonth = 1;

    [Range(1, 31)]
    public int targetDay = 1;
}
