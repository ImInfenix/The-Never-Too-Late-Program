using System;
using UnityEngine;

public static class PositionMapper
{
    public static Vector3 GetCubeRealWorldPosition(Vector3 realPosition, bool excludeYAxis = false)
    {
        Vector3 res = GetCubeRealWorldPosition(GetCubePosition(realPosition));
        if (excludeYAxis)
            res.y = realPosition.y;
        return res;
    }

    public static Vector3 GetCubeRealWorldPosition(Vector3Int squarePosition)
    {
        return squarePosition + Vector3.one * 0.5f;
    }

    public static Vector3Int GetCubePosition(Vector3 realPosition)
    {
        return new Vector3Int(
            Mathf.FloorToInt(realPosition.x),
            Mathf.FloorToInt(realPosition.y),
            Mathf.FloorToInt(realPosition.z));
    }

    public static Vector3 GetDecimalPart(Vector3 v)
    {
        float x = v.x - (float)Math.Truncate(v.x);
        float y = v.y - (float)Math.Truncate(v.y);
        float z = v.z - (float)Math.Truncate(v.z);

        return new Vector3(x, y, z);
    }

    public static void Align(MonoBehaviour monoBehaviour, bool excludeY = false)
    {
        Align(monoBehaviour.gameObject);
    }

    public static void Align(GameObject gameObject, bool excludeY = false)
    {
        if (IsAligned(gameObject))
            return;

        gameObject.transform.position = GetCubeRealWorldPosition(gameObject.transform.position, excludeY);
    }

    public static bool IsAligned(Vector3 position, bool excludeY = false)
    {
        Vector3 decimalPart = GetDecimalPart(position);
        if (excludeY)
            return Equals(decimalPart.x, 0.5f) && Equals(decimalPart.z, 0.5f);
        return Equals(decimalPart.x, 0.5f) && Equals(decimalPart.y, 0.5f) && Equals(decimalPart.z, 0.5f);
    }

    public static bool IsAligned(GameObject g, bool excludeY = false)
    {
        return IsAligned(g.transform.position, excludeY);
    }

    public static bool IsAligned(MonoBehaviour monoBehaviour, bool excludeY = false)
    {
        return IsAligned(monoBehaviour.gameObject, excludeY);
    }

    public static bool Equals(float a, float b)
    {
        float epsilon = .0001f;
        return Mathf.Abs(Mathf.Abs(a)- Mathf.Abs(b)) < epsilon;
    }
}
