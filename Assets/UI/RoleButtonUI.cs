using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RoleButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Assign in Inspector")]
    public RoleSelectUI controller; // 拖 RoleSelectPanel 上的 RoleSelectUI
    public Image iconImage;         // 子物体 Icon 的 Image（用于显示）
    public Sprite roleSprite;       // 角色Sprite（可直接拖）

    [Header("Hover Glow Components (Optional)")]
    public Outline outline;         // Icon 上的 Outline
    public Shadow shadow;           // Icon 上的 Shadow（可选）

    void Awake()
    {
        if (iconImage == null) iconImage = GetComponentInChildren<Image>();

        // 初始化按钮显示（如果你想按钮图=角色图）
        if (iconImage != null && roleSprite != null)
            iconImage.sprite = roleSprite;

        SetGlow(false);
    }

    public void OnPointerEnter(PointerEventData eventData) => SetGlow(true);
    public void OnPointerExit(PointerEventData eventData) => SetGlow(false);

    public void OnPointerClick(PointerEventData eventData)
    {
        if (controller == null) return;

        Sprite s = roleSprite;
        if (s == null && iconImage != null) s = iconImage.sprite;

    }

    void SetGlow(bool on)
    {
        if (outline != null) outline.enabled = on;
        if (shadow != null) shadow.enabled = on;
    }
}
