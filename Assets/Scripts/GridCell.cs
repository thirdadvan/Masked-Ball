using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell
{
    private Vector2 _worldPosition;
    private Character _occupant;

    public Vector2 WorldPosition => _worldPosition;
    public Character Occupant => _occupant;

    public bool IsEmpty => _occupant == null;

    public GridCell(Vector2 worldPosition)
    {
        _worldPosition = worldPosition;
    }

    public void Place(Character character)
    {
        _occupant = character;
    }

    public void Clear()
    {
        _occupant = null;
    }

    public Character Take()
    {
        var temp = _occupant;
        _occupant = null;
        return temp;
    }
}