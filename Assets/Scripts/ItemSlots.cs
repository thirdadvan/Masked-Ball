using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlots : MonoBehaviour
{
    [Header("Auto cache if empty")]
    [SerializeField] private List<Image> itemImages = new(); // 对应每个 ItemSlot 的 Item(Image)

    private void Awake()
    {
        // 如果没手动拖引用，就自动从子物体里找：ItemSlots/ItemSlotXX/Item(Image)
        if (itemImages == null || itemImages.Count == 0)
            CacheFromChildren();
    }

    private void CacheFromChildren()
    {
        itemImages = new List<Image>();

        // 这里按层级顺序抓取：ItemSlot01, ItemSlot02...
        for (int i = 0; i < transform.childCount; i++)
        {
            var slot = transform.GetChild(i);                 // ItemSlotXX
            var itemTf = slot.Find("Item");                   // Item
            if (itemTf == null)
            {
                Debug.LogWarning($"[ItemSlotsUI] {slot.name} 缺少子物体 Item");
                itemImages.Add(null);
                continue;
            }

            var img = itemTf.GetComponent<Image>();
            if (img == null)
            {
                Debug.LogWarning($"[ItemSlotsUI] {slot.name}/Item 缺少 Image 组件");
                itemImages.Add(null);
                continue;
            }

            itemImages.Add(img);
        }
    }

    /// <summary>
    /// 传入 Character：从 character.profile.pocketItems 填充
    /// </summary>
    public void ShowItemsFromCharacter(SoulProfile soul)
    {
        Debug.Log("show item");
        if (soul == null)
        {
            ClearAll();
            return;
        }

        ShowItemsFromProfile(soul);
    }

    /// <summary>
    /// 直接传入 SoulProfile：从 profile.pocketItems 填充
    /// </summary>
    public void ShowItemsFromProfile(SoulProfile profile)
    {
        if (profile == null)
        {
            ClearAll();
            return;
        }

        var items = profile.pocketItems;
        int count = (items == null) ? 0 : items.Count;

        for (int i = 0; i < itemImages.Count; i++)
        {
            var img = itemImages[i];
            if (img == null) continue;

            if (i < count && items[i] != null && items[i].icon != null)
            {
                img.sprite = items[i].icon;
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

    public void ClearAll()
    {
        for (int i = 0; i < itemImages.Count; i++)
        {
            var img = itemImages[i];
            if (img == null) continue;

            img.sprite = null;
            img.enabled = false;
            img.gameObject.SetActive(false);
        }
    }
}