using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueBtn : MonoBehaviour
{
    private DialogueNode node;
    [SerializeField] private DialogueRunner runner;

    public RoleSelectUI CurrentCharacter;
    public string DialogueID;

    /// <summary>
    /// 由 DialogueRunner 调用，用来给这个按钮“塞”一个节点
    /// </summary>
    /// 

    private void Awake()
    {
        node = new DialogueNode();
        var btn = GetComponent<Button>();
        btn.onClick.AddListener(HandleClick);
    }

    private void HandleClick()
    {

        node.nodeId = CurrentCharacter.soulProfile.characterID.ToString() + "Dia"+ DialogueID;
        Debug.Log(CurrentCharacter.soulProfile.characterID.ToString() + "Dia" + DialogueID);
        if (runner == null || string.IsNullOrEmpty(node.nodeId))
        return;
        runner.OnOptionClicked(node.nodeId, CurrentCharacter.soulProfile);
    }
}