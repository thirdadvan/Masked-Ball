using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int width = 3;
    public int height = 3;
    public Transform gridParent;

    private GridCell[,] grid;

    void Awake()
    {
        grid = new GridCell[width, height];

        foreach (Transform child in gridParent)
        {
            string[] parts = child.name.Split('-');
            int x = int.Parse(parts[0]) - 1;
            int y = int.Parse(parts[1]) - 1;

            grid[x, y] = new GridCell(child.position);
        }
    }

    public void RegisterCharacter(Character c, Vector2Int idx)
    {
        grid[idx.x, idx.y].Place(c);
        c.SetCellIndex(idx);
        c.transform.position = grid[idx.x, idx.y].WorldPosition;
    }

    public Vector2Int FindNearestCellIndex(Vector2 worldPos)
    {
        float best = float.MaxValue;
        Vector2Int bestIdx = new Vector2Int(-1, -1);

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                float d = ((Vector2)grid[x, y].WorldPosition - worldPos).sqrMagnitude;
                if (d < best)
                {
                    best = d;
                    bestIdx = new Vector2Int(x, y);
                }
            }
        return bestIdx;
    }

    public bool IsInBounds(Vector2Int idx)
    {
        return idx.x >= 0 && idx.x < width && idx.y >= 0 && idx.y < height;
    }

    public Character GetOccupant(Vector2Int idx) => grid[idx.x, idx.y].Occupant;

    public void SnapToCell(Character c)
    {
        var idx = c.CellIndex;
        c.transform.position = grid[idx.x, idx.y].WorldPosition;
    }

    public bool TrySwap(Vector2Int a, Vector2Int b)
    {
        if (!IsInBounds(a) || !IsInBounds(b)) return false;
        if (a == b) return false;

        Character ca = grid[a.x, a.y].Occupant;
        Character cb = grid[b.x, b.y].Occupant;

        // 你说“两个 character 交换”，所以这里要求目标格必须有人
        if (ca == null || cb == null) return false;

        // 数据层交换
        grid[a.x, a.y].Place(cb);
        grid[b.x, b.y].Place(ca);

        // 更新角色索引
        cb.SetCellIndex(a);
        ca.SetCellIndex(b);

        // 表现层对齐
        cb.transform.position = grid[a.x, a.y].WorldPosition;
        ca.transform.position = grid[b.x, b.y].WorldPosition;

        return true;
    }
}
