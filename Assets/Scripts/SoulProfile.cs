using System;
using System.Collections.Generic;
using UnityEngine;
using MaskedBall.CharacterCore;

[CreateAssetMenu(menuName = "MaskedBall/Soul Profile", fileName = "Soul_Profile")]
public class SoulProfile : ScriptableObject
{
    [Header("Identity")]
    public CharacterID characterID; //例：Soul06
    public string displayName;     // 例：6号灵魂：惯犯

    [Header("Judgement")]
    public Alignment maskAlignment; 
    public Alignment trueAlignment;
    public StrengthTier strength;
    public SinType sin;
    public DeathType deathType;

    [TextArea] public string deathDetail;

    [Header("Story")]
    [TextArea(6, 20)] public string backstory;

    [Header("Pocket Items")]
    public List<EvidenceItem> pocketItems = new();

    [Header("Dialogue")]
    public DialogueTree dialogue;

    [Header("After Investigation")]
    [TextArea] public string skullRemark;
    [TextArea] public string luciferRemark;
}

[Serializable]
public class EvidenceItem
{
    public string id;                 // silver_case_01
    public string displayName;        // 银烟盒
    [TextArea] public string description;
    public Sprite icon;

    public List<string> relatedEvidenceIds = new();
}
