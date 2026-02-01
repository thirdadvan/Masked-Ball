using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MaskedBall.CharacterCore;

public class Character : MonoBehaviour
{

    public Vector2Int CellIndex { get; private set; }

    public SoulProfile profile;

    public RoleSelectUI CharacterFocus;

    private Sprite characterSprite;

    public ItemSlots ItemSlots;

    public void Awake()
    {
        characterSprite = GetComponent<SpriteRenderer>().sprite;
    }
    public void OnClick()
    {
        Debug.Log($"Clicked character: {profile.displayName}");

        CharacterFocus.ShowRole(profile, characterSprite);
        ItemSlots.ShowItemsFromCharacter(profile);

    }

    public void SetCellIndex(Vector2Int idx)
    {
        CellIndex = idx;

        if (idx.y == 0)
        {
            GetComponent<SpriteRenderer>().sortingOrder = 0;
        }
        else if(idx.y == 1)
        {
            GetComponent<SpriteRenderer>().sortingOrder = 6;
        }
        else if (idx.y == 1)
        {
            GetComponent<SpriteRenderer>().sortingOrder = 6;
        }
    }
}
