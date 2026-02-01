using UnityEngine;
using UnityEngine.UI;

public class ItemSlotButton : MonoBehaviour
{
    [Header("Runtime")]
    public EvidenceItem currentItem;

    [Header("Refs")]
    [SerializeField] private DialogueRunner dialogueRunner;

    private Button _btn;

    private void Awake()
    {
        _btn = GetComponent<Button>();
        if (_btn == null)
        {
            Debug.LogError($"[ItemSlotButton] No Button on {name}");
            enabled = false;
            return;
        }

        _btn.onClick.AddListener(OnClick);
    }

    public void SetDialogueRunner(DialogueRunner runner)
    {
        dialogueRunner = runner;
    }

    public void SetItem(EvidenceItem item)
    {
        currentItem = item;
        if (_btn != null) _btn.interactable = (item != null);
    }

    public void Clear()
    {
        currentItem = null;
        if (_btn != null) _btn.interactable = false;
    }

    private void OnClick()
    {
        if (currentItem == null)
        {
            Debug.Log("[ItemSlotButton] Slot empty");
            return;
        }

        if (dialogueRunner == null)
        {
            Debug.LogError("[ItemSlotButton] DialogueRunner not assigned");
            return;
        }

        // ✅ 直接用 DialogueRunner 的“显示对话文字”的方法
        // 显示物品注释（description）
        dialogueRunner.ShowTextTypewriter(currentItem.description);
    }
}