using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using MaskedBall.CharacterCore;

public class GameManager : MonoBehaviour
{
    public GridManager grid;
    public Transform characterParent; // 指向层级里的 Character 父物体

    private Camera cam;
    private Character dragging;
    private Vector3 dragOffset;

    private Vector3 mouseDownWorldPos;
    private bool isDragging;
    private float clickThreshold = 0.15f; // 世界坐标下的点击容差

    void Awake()
    {
        cam = Camera.main;
    }

    void Start()
    {
        // 初始化：把 1..9 角色按“最近格子”注册进 grid
        foreach (Transform child in characterParent)
        {
            Character c = child.GetComponent<Character>();
            if (c == null) continue;

            Vector2Int idx = grid.FindNearestCellIndex(child.position);
            if (!grid.IsInBounds(idx)) continue;

            // 如果出现两个角色都最近同一个格子：你需要确保初始摆放不要重叠
            if (grid.GetOccupant(idx) != null) continue;

            grid.RegisterCharacter(c, idx);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            TryPick();

        if (Input.GetMouseButton(0) && dragging != null)
            Drag();

        if (Input.GetMouseButtonUp(0) && dragging != null)
            Drop();
    }

    void TryPick()
    {
        Vector2 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mouseWorld, Vector2.zero);

        if (hit.collider == null) return;

        dragging = hit.collider.GetComponent<Character>();
        if (dragging == null) return;

        mouseDownWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseDownWorldPos.z = dragging.transform.position.z;

        Vector3 m = mouseDownWorldPos;
        dragOffset = dragging.transform.position - m;

        isDragging = false;
    }

    void Drag()
    {
        Vector3 m = cam.ScreenToWorldPoint(Input.mousePosition);
        m.z = dragging.transform.position.z;

        if (!isDragging)
        {
            float dist = Vector3.Distance(m, mouseDownWorldPos);
            if (dist > clickThreshold)
                isDragging = true;
        }

        if (isDragging)
            dragging.transform.position = m + dragOffset;
    }

    void Drop()
    {
        if (!isDragging)
        {
            // ✅ 当作点击
            dragging.OnClick();
            dragging = null;
            return;
        }

        // ✅ 当作拖拽交换
        Vector2Int from = dragging.CellIndex;
        Vector2Int to = grid.FindNearestCellIndex(dragging.transform.position);

        bool swapped = grid.TrySwap(from, to);

        if (!swapped)
            grid.SnapToCell(dragging);

        dragging = null;
    }
    [ContextMenu("DEBUG / Evaluate Survivors (Good)")]
    public void DebugEvaluateGoodSurvivors()
    {
        Debug.Log("Evaluate");
        if (grid == null)
        {
            Debug.LogError("GridManager is null.");
            return;
        }

        // 1) 获取网格尺寸（优先读 grid.width / grid.height；读不到就默认 3x3）
        int width = 3, height = 3;
        TryGetGridSize(grid, ref width, ref height);

        // 2) 同步判定：哪些 Good 会被杀（先收集 toDie，再统一结算）
        var toDie = new HashSet<Character>();

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                var idx = new Vector2Int(x, y);
                var c = grid.GetOccupant(idx);
                if (c == null || c.profile == null) continue;

                // 只处理 trueAlignment 为 Good 的
                if (c.profile.trueAlignment != Alignment.Good) continue;

                // 上下左右邻居
                CheckNeighborKill(idx + Vector2Int.up, c, toDie);
                CheckNeighborKill(idx + Vector2Int.down, c, toDie);
                CheckNeighborKill(idx + Vector2Int.left, c, toDie);
                CheckNeighborKill(idx + Vector2Int.right, c, toDie);
            }

        // 3) 统计剩余强善 / 弱善（按你说的规则，只统计 Strong 和 Weak）
        int strongGood = 0;
        int weakGood = 0;

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                var idx = new Vector2Int(x, y);
                var c = grid.GetOccupant(idx);
                if (c == null || c.profile == null) continue;

                if (c.profile.trueAlignment != Alignment.Good) continue;
                if (toDie.Contains(c)) continue;

                if (c.profile.strength == StrengthTier.Strong) strongGood++;
                else if (c.profile.strength == StrengthTier.Weak) weakGood++;
            }

        Debug.Log($"[Evaluate] Survived -> StrongGood: {strongGood}, WeakGood: {weakGood}");
    }

    private void CheckNeighborKill(Vector2Int nIdx, Character targetGood, HashSet<Character> toDie)
    {
        if (!grid.IsInBounds(nIdx)) return;

        var n = grid.GetOccupant(nIdx);
        if (n == null || n.profile == null) return;

        // 只关心“邻居是 Evil”
        if (n.profile.trueAlignment != Alignment.Evil) return;

        var goodStrength = targetGood.profile.strength;
        var evilStrength = n.profile.strength;

        // 规则：
        // 强善不受弱恶影响
        // 弱善被所有恶杀
        if (goodStrength == StrengthTier.Weak)
        {
            toDie.Add(targetGood);
        }
        if (goodStrength == StrengthTier.Strong && evilStrength == StrengthTier.Strong)
        {
            toDie.Add(targetGood);
        }
    }

    private void TryGetGridSize(GridManager gm, ref int w, ref int h)
    {
        var t = gm.GetType();

        // 优先读 public field：width/height
        var fW = t.GetField("width");
        var fH = t.GetField("height");
        if (fW != null && fH != null)
        {
            try
            {
                w = (int)fW.GetValue(gm);
                h = (int)fH.GetValue(gm);
                return;
            }
            catch { }
        }

        // 再读 public property：Width/Height 或 width/height
        var pW = t.GetProperty("width") ?? t.GetProperty("Width");
        var pH = t.GetProperty("height") ?? t.GetProperty("Height");
        if (pW != null && pH != null)
        {
            try
            {
                w = (int)pW.GetValue(gm);
                h = (int)pH.GetValue(gm);
                return;
            }
            catch { }
        }

        // 读不到就保持默认 3x3
    }

}
