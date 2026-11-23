using TMPro;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text speakerText;
    [SerializeField] private TMP_Text contentText;

    private string[] currentLines;
    private int currentIndex;

    public bool IsOpen { get; private set; }

    private void Awake()
    {
        Close();
    }

    public void Open(string speaker, string[] lines)
    {
        if (lines == null || lines.Length == 0)
        {
            return;
        }

        currentLines = lines;
        currentIndex = 0;

        speakerText.text = speaker;
        contentText.text = currentLines[currentIndex];

        IsOpen = true;
        dialoguePanel.SetActive(true);
    }

    public void NextLine()
    {
        if (!IsOpen)
        {
            return;
        }

        currentIndex++;

        if (currentIndex >= currentLines.Length)
        {
            Close();
            return;
        }

        contentText.text = currentLines[currentIndex];
    }

    public void Close()
    {
        IsOpen = false;
        currentLines = null;
        currentIndex = 0;

        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
    }
}
