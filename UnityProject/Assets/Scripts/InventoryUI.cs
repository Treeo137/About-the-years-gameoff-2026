using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [Header("逻辑对象")]
    [SerializeField] private InventoryModel inventory;
    [SerializeField] private PlayerMovement2D playerMovement;
    [SerializeField] private DialogueUI dialogueUI;

    [Header("整体界面")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private TMP_Text infoText;

    [Header("栏目：物品、信物、年历")]
    [SerializeField] private TMP_Text[] tabTexts = new TMP_Text[3];

    [Header("八个栏位，按从左到右、从上到下绑定")]
    [SerializeField] private GameObject slotsRoot;
    [SerializeField] private TMP_Text[] slotTexts = new TMP_Text[8];

    [Header("年历")]
    [SerializeField] private GameObject calendarRoot;
    [SerializeField] private Image relicBox;
    [SerializeField] private Image relicIcon;

    [Tooltip("年份十位、年份个位、月份、日期")]
    [SerializeField] private TMP_Text[] digitTexts = new TMP_Text[4];

    [SerializeField] private Image[] digitBoxes = new Image[4];

    [Header("颜色")]
    [SerializeField] private Color normalTextColor = Color.white;

    [SerializeField]
    private Color selectedTextColor =
        new Color(1f, 0.85f, 0.25f);

    [SerializeField]
    private Color normalBoxColor =
        new Color(0.15f, 0.17f, 0.2f, 1f);

    [SerializeField]
    private Color selectedBoxColor =
        new Color(0.5f, 0.4f, 0.12f, 1f);

    public bool IsOpen { get; private set; }

    // 0：物品，1：信物，2：年历
    private int selectedTab;

    // false：左侧栏目；true：右侧内容
    private bool inContent;

    private int selectedSlot;

    // 0：信物框，1～4：四个数字
    private int calendarFocus;

    // 初始四个数字全部为 0。
    private readonly int[] digits = new int[4];

    private bool movementWasEnabled;

    private void Awake()
    {
        inventoryPanel.SetActive(false);
    }

    private void OnEnable()
    {
        if (inventory != null)
        {
            inventory.Changed += HandleInventoryChanged;
        }
    }

    private void OnDisable()
    {
        if (inventory != null)
        {
            inventory.Changed -= HandleInventoryChanged;
        }

        if (IsOpen)
        {
            Close();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (IsOpen)
            {
                Close();
            }
            else
            {
                Open();
            }

            // 打开这帧不再继续处理其他菜单输入。
            return;
        }

        if (!IsOpen)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (inContent)
            {
                inContent = false;
                Refresh();
            }
            else
            {
                Close();
            }

            return;
        }

        if (!inContent)
        {
            HandleTabInput();
        }
        else if (selectedTab == 2)
        {
            HandleCalendarInput();
        }
        else
        {
            HandleSlotInput();
        }
    }

    public void Open()
    {
        if (IsOpen)
        {
            return;
        }

        // 对话期间暂时不允许打开背包。
        if (dialogueUI != null && dialogueUI.IsOpen)
        {
            return;
        }

        if (playerMovement == null)
        {
            Debug.LogError(
                "InventoryUI 没有绑定 Player Movement。",
                this
            );

            return;
        }

        movementWasEnabled = playerMovement.enabled;
        playerMovement.enabled = false;

        selectedTab = 0;
        selectedSlot = 0;
        calendarFocus = 0;
        inContent = false;

        IsOpen = true;
        inventoryPanel.SetActive(true);

        Refresh();
    }


    public void Close()
    {
        if (!IsOpen)
        {
            return;
        }

        IsOpen = false;

        // 退出播放或销毁场景时，面板可能已经不存在。
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(false);
        }

        // 人物也可能已随场景一起销毁。
        if (playerMovement != null)
        {
            playerMovement.enabled = movementWasEnabled;
        }
    }



    private void HandleTabInput()
    {
        if (UpPressed())
        {
            selectedTab = (selectedTab + 2) % 3;
            selectedSlot = 0;
            calendarFocus = 0;
            Refresh();
        }
        else if (DownPressed())
        {
            selectedTab = (selectedTab + 1) % 3;
            selectedSlot = 0;
            calendarFocus = 0;
            Refresh();
        }
        else if (ConfirmPressed())
        {
            inContent = true;
            Refresh();
        }
    }

    private void HandleSlotInput()
    {
        int row = selectedSlot / 2;
        int column = selectedSlot % 2;

        bool changed = false;

        if (UpPressed())
        {
            row = (row + 3) % 4;
            changed = true;
        }
        else if (DownPressed())
        {
            row = (row + 1) % 4;
            changed = true;
        }
        else if (LeftPressed() || RightPressed())
        {
            column = 1 - column;
            changed = true;
        }

        if (changed)
        {
            selectedSlot = row * 2 + column;
            Refresh();
        }

        // 暂时只浏览，不在这里实现使用或送礼。
        if (ConfirmPressed())
        {
            Refresh();
        }
    }

    private void HandleCalendarInput()
    {
        if (LeftPressed())
        {
            calendarFocus = (calendarFocus + 4) % 5;
            Refresh();
        }
        else if (RightPressed())
        {
            calendarFocus = (calendarFocus + 1) % 5;
            Refresh();
        }
        else if (calendarFocus > 0 && UpPressed())
        {
            int index = calendarFocus - 1;
            digits[index] = (digits[index] + 1) % 10;
            Refresh();
        }
        else if (calendarFocus > 0 && DownPressed())
        {
            int index = calendarFocus - 1;
            digits[index] = (digits[index] + 9) % 10;
            Refresh();
        }
        else if (ConfirmPressed())
        {
            if (calendarFocus == 0)
            {
                Refresh();
            }
            else
            {
                CheckDate();
            }
        }
    }

    private void Refresh()
    {
        for (int i = 0; i < tabTexts.Length; i++)
        {
            tabTexts[i].color = i == selectedTab
                ? selectedTextColor
                : normalTextColor;
        }

        bool calendarSelected = selectedTab == 2;

        slotsRoot.SetActive(!calendarSelected);
        calendarRoot.SetActive(calendarSelected);

        if (calendarSelected)
        {
            RefreshCalendar();
        }
        else
        {
            RefreshSlots();
        }
    }

    private void RefreshSlots()
    {
        ItemCategory category = selectedTab == 0
            ? ItemCategory.Item
            : ItemCategory.Relic;

        for (int i = 0; i < slotTexts.Length; i++)
        {
            ItemData item = inventory.GetItem(category, i);

            slotTexts[i].text = item == null
                ? "—"
                : item.displayName;

            slotTexts[i].color =
                inContent && i == selectedSlot
                    ? selectedTextColor
                    : normalTextColor;
        }

        ItemData selectedItem =
            inventory.GetItem(category, selectedSlot);

        infoText.text = selectedItem == null
            ? "这个栏位还没有东西。"
            : selectedItem.description;
    }

    private void RefreshCalendar()
    {
        ItemData relic = inventory.CalendarRelic;

        relicIcon.sprite = relic == null ? null : relic.icon;

        // 没有图标时隐藏，避免 Image 显示一个白方块。
        relicIcon.enabled =
            relic != null && relic.icon != null;

        relicBox.color =
            inContent && calendarFocus == 0
                ? selectedBoxColor
                : normalBoxColor;

        for (int i = 0; i < digits.Length; i++)
        {
            digitTexts[i].text = digits[i].ToString();

            digitBoxes[i].color =
                inContent && calendarFocus == i + 1
                    ? selectedBoxColor
                    : normalBoxColor;

            digitTexts[i].color =
                inContent && calendarFocus == i + 1
                    ? selectedTextColor
                    : normalTextColor;
        }

        infoText.text = relic == null
            ? "等待你往其中放入什么"
            : relic.description;
    }

    private void CheckDate()
    {
        ItemData relic = inventory.CalendarRelic;

        if (relic == null)
        {
            infoText.text = "等待你往其中放入什么";
            return;
        }

        int year = 2000 + digits[0] * 10 + digits[1];
        int month = digits[2];
        int day = digits[3];

        // 排除 0 月、0 日等无效日期。
        try
        {
            DateTime date = new DateTime(year, month, day);
        }
        catch (ArgumentOutOfRangeException)
        {
            infoText.text = "这不是一个有效的日期。";
            return;
        }

        bool matches =
            year == relic.targetYear &&
            month == relic.targetMonth &&
            day == relic.targetDay;

        if (matches)
        {
            infoText.text = "日期正确。穿越功能尚未接入。";

            Debug.Log(
                $"年历校验成功：{year}年{month}月{day}日"
            );
        }
        else
        {
            infoText.text = "这个日期没有发生什么。";
        }
    }

    private void HandleInventoryChanged()
    {
        if (IsOpen)
        {
            Refresh();
        }
    }

    private bool UpPressed()
    {
        return Input.GetKeyDown(KeyCode.W) ||
               Input.GetKeyDown(KeyCode.UpArrow);
    }

    private bool DownPressed()
    {
        return Input.GetKeyDown(KeyCode.S) ||
               Input.GetKeyDown(KeyCode.DownArrow);
    }

    private bool LeftPressed()
    {
        return Input.GetKeyDown(KeyCode.A) ||
               Input.GetKeyDown(KeyCode.LeftArrow);
    }

    private bool RightPressed()
    {
        return Input.GetKeyDown(KeyCode.D) ||
               Input.GetKeyDown(KeyCode.RightArrow);
    }

    private bool ConfirmPressed()
    {
        return Input.GetKeyDown(KeyCode.Return) ||
               Input.GetKeyDown(KeyCode.KeypadEnter);
    }
}
