using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSystem : MonoBehaviour
{
    [Serializable]
    private class SaveData
    {
        public int version = 1;

        public string sceneName;
        public string savedAt;

        public float playerX;
        public float playerY;
    }

    [Header("场景对象")]
    [SerializeField] private PlayerMovement2D playerMovement;
    [SerializeField] private DialogueUI dialogueUI;

    [Header("存档界面")]
    [SerializeField] private GameObject savePanel;

    // 数组顺序对应栏位 1、2、3。
    [SerializeField] private TMP_Text[] slotTexts = new TMP_Text[3];
    [SerializeField] private TMP_Text statusText;

    private bool isOpen;
    private bool movementWasEnabled;

    private void Start()
    {
        savePanel.SetActive(false);
        isOpen = false;

        RefreshSlots();

        Debug.Log("存档目录：" + Application.persistentDataPath);
    }

    private void Update()
    {
        // 原型阶段用 F5 打开或关闭存档菜单。
        // 这里的 F5 不是直接快速保存。
        if (Input.GetKeyDown(KeyCode.F5))
        {
            if (isOpen)
            {
                Close();
            }
            else if (dialogueUI == null || !dialogueUI.IsOpen)
            {
                Open();
            }
        }

        if (isOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            Close();
        }
    }

    public void Open()
    {
        if (isOpen)
        {
            return;
        }

        // 暂时不允许在对话期间打开存档。
        if (dialogueUI != null && dialogueUI.IsOpen)
        {
            return;
        }

        movementWasEnabled = playerMovement.enabled;
        playerMovement.enabled = false;

        isOpen = true;
        savePanel.SetActive(true);

        SetStatus("选择栏位。保存会覆盖该栏位。");
        RefreshSlots();
    }

    public void Close()
    {
        if (!isOpen)
        {
            return;
        }

        isOpen = false;
        savePanel.SetActive(false);

        playerMovement.enabled = movementWasEnabled;
    }

    public void SaveSlot(int slot)
    {
        if (!isOpen || !IsValidSlot(slot))
        {
            return;
        }

        Vector3 position = playerMovement.transform.position;

        SaveData data = new SaveData
        {
            sceneName = SceneManager.GetActiveScene().name,
            savedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
            playerX = position.x,
            playerY = position.y
        };

        try
        {
            string path = GetSlotPath(slot);

            Directory.CreateDirectory(Application.persistentDataPath);

            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(path, json);

            RefreshSlots();
            SetStatus("已保存到栏位 " + slot);
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
            SetStatus("保存失败，请查看 Console。");
        }
    }

    public void LoadSlot(int slot)
    {
        if (!isOpen || !IsValidSlot(slot))
        {
            return;
        }

        try
        {
            string path = GetSlotPath(slot);

            if (!File.Exists(path))
            {
                SetStatus("这个栏位没有存档。");
                return;
            }

            SaveData data = ReadData(path);

            string currentScene = SceneManager.GetActiveScene().name;

            if (data.sceneName != currentScene)
            {
                SetStatus("此存档属于其他场景，当前版本暂不能读取。");
                return;
            }

            Vector3 position = playerMovement.transform.position;
            position.x = data.playerX;
            position.y = data.playerY;

            Rigidbody2D rb =
                playerMovement.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.position = new Vector2(position.x, position.y);
            }
            else
            {
                playerMovement.transform.position = position;
            }

            Close();
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
            SetStatus("读取失败，存档可能损坏。");
        }
    }

    private void RefreshSlots()
    {
        for (int slot = 1; slot <= 3; slot++)
        {
            TMP_Text label = slotTexts[slot - 1];

            if (label == null)
            {
                continue;
            }

            string path = GetSlotPath(slot);

            if (!File.Exists(path))
            {
                label.text = "栏位 " + slot + "\n空存档";
                continue;
            }

            try
            {
                SaveData data = ReadData(path);

                label.text =
                    "栏位 " + slot + " · " + data.sceneName +
                    "\n" + data.savedAt;
            }
            catch
            {
                label.text = "栏位 " + slot + "\n无法读取";
            }
        }
    }

    private SaveData ReadData(string path)
    {
        string json = File.ReadAllText(path);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        if (data == null ||
            data.version != 1 ||
            string.IsNullOrEmpty(data.sceneName) ||
            float.IsNaN(data.playerX) ||
            float.IsNaN(data.playerY) ||
            float.IsInfinity(data.playerX) ||
            float.IsInfinity(data.playerY))
        {
            throw new InvalidDataException("存档格式无效。");
        }

        return data;
    }

    private string GetSlotPath(int slot)
    {
        return Path.Combine(
            Application.persistentDataPath,
            "save_slot_" + slot + ".json"
        );
    }

    private bool IsValidSlot(int slot)
    {
        return slot >= 1 && slot <= 3;
    }

    private void SetStatus(string message)
    {
        if (statusText != null)
        {
            statusText.text = message;
        }
    }
}
