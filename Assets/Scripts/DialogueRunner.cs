using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueRunner : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text dialogueText;

    [Header("Typewriter")]
    private float charInterval = 0.005f; // 每个字符间隔
    [SerializeField] private bool richText = true;                    // TMP富文本是否保留
    [SerializeField] private bool allowSkip = true;                   // 再点一次是否直接显示全文

    // 缓存：每棵 DialogueTree 建一次 nodeId->node 的表
    private readonly Dictionary<DialogueTree, Dictionary<string, DialogueNode>> _treeMaps
        = new Dictionary<DialogueTree, Dictionary<string, DialogueNode>>();

    private Coroutine _typingCo;
    private string _currentFullText = "";
    private bool _isTyping;

    public void OnOptionClicked(string optionNodeId, SoulProfile soulProfile)
    {
        if (string.IsNullOrWhiteSpace(optionNodeId))
        {
            Debug.LogError("[DialogueRunner] optionNodeId is empty");
            return;
        }

        if (soulProfile == null)
        {
            Debug.LogError("[DialogueRunner] soulProfile is null");
            return;
        }

        if (soulProfile.dialogue == null)
        {
            Debug.LogError($"[DialogueRunner] soulProfile.dialogue is null (characterID={soulProfile.characterID})");
            return;
        }

        if (dialogueText == null)
        {
            Debug.LogError("[DialogueRunner] dialogueText is null (Inspector 没拖 TMP_Text)");
            return;
        }

        // 如果正在打字，再点（或点别的选项）时：先处理“跳过”逻辑
        if (_isTyping && allowSkip)
        {
            SkipTypewriter();
            // 注意：这里不 return，让这次点击继续切换到新的回复
            // 如果你希望“打字时点击只负责跳过，不切换新对话”，就在这里 return;
        }

        DialogueTree tree = soulProfile.dialogue;
        var map = GetOrBuildMap(tree);

        string optId = optionNodeId.Trim();

        if (!map.TryGetValue(optId, out var optionNode))
        {
            Debug.LogError($"[DialogueRunner] Option node not found: '{optId}' in character '{soulProfile.characterID}'");
            return;
        }

        if (optionNode.next == null || optionNode.next.Count == 0)
        {
            Debug.LogWarning($"[DialogueRunner] Option '{optId}' has no next");
            return;
        }

        // 固定回复：next[0]
        string replyId = optionNode.next[0]?.Trim();
        if (string.IsNullOrEmpty(replyId))
        {
            Debug.LogWarning($"[DialogueRunner] Option '{optId}' next[0] is empty");
            return;
        }

        if (!map.TryGetValue(replyId, out var replyNode))
        {
            Debug.LogError($"[DialogueRunner] Reply node not found: '{replyId}' (from option '{optId}')");
            return;
        }

        // ✅ 用打字机显示 replyNode.text
        ShowTextTypewriter(replyNode.text);

        Debug.Log($"[DialogueRunner] {soulProfile.characterID}: '{optId}' -> '{replyId}' typing");
    }

    // ===== Typewriter API =====

    public void ShowTextTypewriter(string fullText)
    {
        _currentFullText = fullText ?? "";

        // 停掉上一段
        if (_typingCo != null)
            StopCoroutine(_typingCo);

        _typingCo = StartCoroutine(TypeRoutine(_currentFullText));
    }

    public void SkipTypewriter()
    {
        if (!_isTyping) return;

        if (_typingCo != null)
            StopCoroutine(_typingCo);

        _isTyping = false;
        dialogueText.maxVisibleCharacters = int.MaxValue;
        dialogueText.text = _currentFullText;
    }

    private IEnumerator TypeRoutine(string fullText)
    {
        _isTyping = true;

        // 富文本：用 maxVisibleCharacters 更稳，不会把 <color> 之类标签打断
        dialogueText.text = fullText;

        if (richText)
        {
            dialogueText.ForceMeshUpdate();
            int totalChars = dialogueText.textInfo.characterCount;

            dialogueText.maxVisibleCharacters = 0;

            for (int i = 0; i <= totalChars; i++)
            {
                dialogueText.maxVisibleCharacters = i;
                yield return new WaitForSeconds(charInterval);
            }
        }
        else
        {
            // 非富文本：逐字拼接
            dialogueText.maxVisibleCharacters = int.MaxValue;
            dialogueText.text = "";

            for (int i = 0; i < fullText.Length; i++)
            {
                dialogueText.text += fullText[i];
                yield return new WaitForSeconds(charInterval);
            }
        }

        _isTyping = false;
        _typingCo = null;
    }

    // ===== Map build =====

    private Dictionary<string, DialogueNode> GetOrBuildMap(DialogueTree tree)
    {
        if (_treeMaps.TryGetValue(tree, out var cached))
            return cached;

        var map = new Dictionary<string, DialogueNode>();

        if (tree.nodes != null)
        {
            foreach (var n in tree.nodes)
            {
                if (n == null) continue;
                if (string.IsNullOrWhiteSpace(n.nodeId)) continue;

                map[n.nodeId.Trim()] = n;
            }
        }

        _treeMaps[tree] = map;
        Debug.Log($"[DialogueRunner] Build map for tree, nodes={map.Count}");
        return map;
    }
}