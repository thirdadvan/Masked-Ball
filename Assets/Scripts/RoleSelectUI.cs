using UnityEngine;
using UnityEngine.UI;


public class RoleSelectUI : MonoBehaviour
{
    [Header("Focus Layer")]
    public GameObject focusLayer;
    public Image dim;
    public Image focusSprite; // FocusIcon 的 Image 组件

    [Header("Close")]
    public Button closeButton;

    [Header("Scale")]
    public float focusedScale = 3f;

    void Awake()
    {
        HideFocus();

        if (closeButton != null)
            closeButton.onClick.AddListener(HideFocus);
    }

    // ✅ 修改：接收 Sprite 参数
    public void ShowRole(Sprite roleSprite)
    {
        if (focusLayer != null) focusLayer.SetActive(true);
        if (dim != null) dim.gameObject.SetActive(true);

        if (focusSprite != null)
        {
            focusSprite.gameObject.SetActive(true);
            focusSprite.sprite = roleSprite; // ✅ 设置传入的 Sprite

            var rt = focusSprite.rectTransform;
            rt.anchoredPosition = Vector2.zero;
            rt.localScale = Vector3.one * focusedScale;
        }

        if (closeButton != null) closeButton.gameObject.SetActive(true);
    }

    public void HideFocus()
    {
        UnityEngine.Debug.Log("hide focus");
        if (focusSprite != null) focusSprite.gameObject.SetActive(false);
        if (dim != null) dim.gameObject.SetActive(false);
        if (focusLayer != null) focusLayer.SetActive(false);

        if (closeButton != null) closeButton.gameObject.SetActive(false);
    }
}