using MaskedBall.CharacterCore;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Character : MonoBehaviour
{
    public Vector2Int CellIndex { get; private set; }

    public SoulProfile profile;
    public RoleSelectUI CharacterFocus;

    private SpriteRenderer spriteRenderer; // ✅ 添加

    public void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // ✅ 获取 SpriteRenderer
    }

    public void OnClick()
    {
        Debug.Log($"Clicked character: {profile.displayName}");

        // ✅ 传递当前角色的 Sprite
        if (CharacterFocus != null && spriteRenderer != null)
        {
            CharacterFocus.ShowRole(spriteRenderer.sprite);
        }

        // TODO:
        // 1. 打开角色面板 ✅ 已实现
        // 2. 显示道具 / 穿着
        // 3. 高亮 / 选中
    }

    public void SetCellIndex(Vector2Int idx)
    {
        CellIndex = idx;
        Debug.Log(idx);
    }
}