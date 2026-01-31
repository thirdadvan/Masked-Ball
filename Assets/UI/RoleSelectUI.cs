using UnityEngine;
using UnityEngine.UI;

public class RoleSelectUI : MonoBehaviour
{
    [Header("Focus Layer")]
    public GameObject focusLayer;   // FocusLayer
    public Image dim;               // FocusLayer/Dim
    public Image focusIcon;         // FocusLayer/FocusIcon

    [Header("Close")]
    public Button closeButton;      // CloseButton

    [Header("Scale")]
    public float focusedScale = 3f; // 放大倍率，可调

    void Awake()
    {
        HideFocus();

        if (closeButton != null)
            closeButton.onClick.AddListener(HideFocus);
    }

    public void ShowRole()
    {

        if (focusLayer != null) focusLayer.SetActive(true);
        if (dim != null) dim.gameObject.SetActive(true);

        if (focusIcon != null)
        {
            focusIcon.gameObject.SetActive(true);
            focusIcon.sprite = null; // placebo

            // 居中 + 放大（FocusIcon 要是 Middle Center Anchor）
            var rt = focusIcon.rectTransform;
            rt.anchoredPosition = Vector2.zero;
            rt.localScale = Vector3.one * focusedScale;
        }

        if (closeButton != null) closeButton.gameObject.SetActive(true);
    }

    public void HideFocus()
    {
        if (focusIcon != null) focusIcon.gameObject.SetActive(false);
        if (dim != null) dim.gameObject.SetActive(false);
        if (focusLayer != null) focusLayer.SetActive(false);

        if (closeButton != null) closeButton.gameObject.SetActive(false);
    }
}
