using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MaskedBall.CharacterCore;

public class Character : MonoBehaviour
{

    public Vector2Int CellIndex { get; private set; }

    public SoulProfile profile;

    public RoleSelectUI CharacterFocus;

    public void Awake()
    {
    }
    public void OnClick()
    {
        Debug.Log($"Clicked character: {profile.displayName}");

        CharacterFocus.ShowRole();
        // TODO:
        // 1. 打开角色面板
        // 2. 显示道具 / 穿着
        // 3. 高亮 / 选中
    }

    public void SetCellIndex(Vector2Int idx)
    {
        CellIndex = idx;
        Debug.Log(idx);
    }
}
