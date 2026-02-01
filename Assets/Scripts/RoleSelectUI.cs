using UnityEngine;
using UnityEngine.UI;

public class RoleSelectUI : MonoBehaviour
{
    [Header("Focus Layer")]
    public GameObject focusLayer;   // FocusLayer
    public Image dim;               // FocusLayer/Dim
    public Image focusSprite;         // FocusLayer/FocusIcon

    [Header("Close")]
    public Button closeButton;      // CloseButton

    [Header("Scale")]
    public float focusedScale = 3f; // 放大倍率，可调

    private SoulProfile soulProfile;
    void Awake()
    {
        HideFocus();

        if (closeButton != null)
            closeButton.onClick.AddListener(HideFocus);
    }

    public void ShowRole(SoulProfile sp, Sprite cs)
    {
        soulProfile = sp;
        if (focusLayer != null) focusLayer.SetActive(true);
        if (dim != null) dim.gameObject.SetActive(true);

        if (focusSprite != null)
        {
            focusSprite.gameObject.SetActive(true);
            focusSprite.sprite = cs;

            // 居中 + 放大（FocusIcon 要是 Middle Center Anchor）
            var rt = focusSprite.rectTransform;
            rt.anchoredPosition = Vector2.zero;
            rt.localScale = Vector3.one * focusedScale;
        }

        if (closeButton != null) closeButton.gameObject.SetActive(true);
    }

    public void HideFocus()
    {
        Debug.Log("hide focus");
        if (focusSprite != null) focusSprite.gameObject.SetActive(false);
        if (dim != null) dim.gameObject.SetActive(false);
        if (focusLayer != null) focusLayer.SetActive(false);

        if (closeButton != null) closeButton.gameObject.SetActive(false);
    }
}
