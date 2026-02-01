using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlots : MonoBehaviour
{
    [Header("External")]
    [SerializeField] private DialogueRunner dialogueRunner;

    [Header("Slot Cache (Auto)")]
    [SerializeField] private List<ItemSlotButton> slotButtons = new();
    [SerializeField] private List<Image> slotItemImages = new(); // 每个 Slot 里用于显示 icon 的 Image

    private void Awake()
    {
        CacheSlotsFromChildren();
        ApplyRunnerToSlots();
        ClearAllSlots();
    }

    private void CacheSlotsFromChildren()
    {
        slotButtons.Clear();
        slotItemImages.Clear();

        // ItemSlots 下每个子物体：ItemSlot01, ItemSlot02...
        for (int i = 0; i < transform.childCount; i++)
        {
            var slot = transform.GetChild(i); // ItemSlotXX

            // 1) Button脚本在 ItemSlotXX 上
            var btn = slot.GetComponent<ItemSlotButton>();
            if (btn == null)
            {
                Debug.LogWarning($"[ItemSlotsUI] {slot.name} 没挂 ItemSlotButton");
            }
            slotButtons.Add(btn);

            // 2) icon 的 Image：优先找 slot/Item 这个子物体上的 Image
            Image img = null;
            var itemTf = slot.Find("Item"); // 你截图里就是 Item
            if (itemTf != null)
            {
                img = itemTf.GetComponent<Image>();
                if (img == null)
                {
                    // 如果 Item 上没有 Image，就向下找
                    img = itemTf.GetComponentInChildren<Image>(true);
                }
            }
            else
            {
                // 如果没找到 Item，就直接在 slot 子层级里找 Image（兜底）
                img = slot.GetComponentInChildren<Image>(true);
                Debug.LogWarning($"[ItemSlotsUI] {slot.name} 找不到子物体 'Item'，已用兜底 Image 搜索");
            }

            if (img == null)
                Debug.LogWarning($"[ItemSlotsUI] {slot.name} 找不到用于显示 icon 的 Image");

            slotItemImages.Add(img);
        }
    }

    private void ApplyRunnerToSlots()
    {
        for (int i = 0; i < slotButtons.Count; i++)
        {
            if (slotButtons[i] != null)
                slotButtons[i].SetDialogueRunner(dialogueRunner);
        }
    }

    // ===== Public API =====

    public void ShowItemsFromCharacter(SoulProfile soul)
    {
        if (soul == null)
        {
            ClearAllSlots();
            return;
        }

        ShowItemsFromSoulProfile(soul);
    }

    public void ShowItemsFromSoulProfile(SoulProfile profile)
    {
        if (profile == null)
        {
            ClearAllSlots();
            return;
        }

        var items = profile.pocketItems;
        int itemCount = (items == null) ? 0 : items.Count;

        for (int i = 0; i < slotButtons.Count; i++)
        {
            EvidenceItem item = (i < itemCount) ? items[i] : null;

            // 1) 给 SlotButton 绑定 item（点击后显示 description）
            var btn = slotButtons[i];
            if (btn != null)
                btn.SetItem(item);

            // 2) 设置 icon
            var img = slotItemImages[i];
            if (img == null) continue;

            if (item != null && item.icon != null)
            {
                img.sprite = item.icon;
                img.enabled = true;
                img.gameObject.SetActive(true);
            }
            else
            {
                img.sprite = null;
                img.enabled = false;
                img.gameObject.SetActive(false);
            }
        }
    }

    public void ClearAllSlots()
    {
        for (int i = 0; i < slotButtons.Count; i++)
        {
            if (slotButtons[i] != null)
                slotButtons[i].Clear();

            var img = slotItemImages[i];
            if (img == null) continue;

            img.sprite = null;
            img.enabled = false;
            img.gameObject.SetActive(false);
        }
    }

    public void SetDialogueRunner(DialogueRunner runner)
    {
        dialogueRunner = runner;
        ApplyRunnerToSlots();
    }
}