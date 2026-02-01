using UnityEngine;
using UnityEngine.UI;

public class DebugEvaluateButton : MonoBehaviour
{
    public GameManager gameManager;

    void Awake()
    {
        var btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(OnClick);
        }
    }

    void OnClick()
    {
        if (gameManager == null)
        {
            Debug.LogError("GameManager reference is null.");
            return;
        }

        gameManager.DebugEvaluateGoodSurvivors();
    }
}