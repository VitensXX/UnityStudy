using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Created by Vitens on 2020/7/25 16:31:43
/// 
/// Description : 
///     格子相关计算扩展
/// </summary>
public static class GridsExtension
{
    //添加元素(如果已经有了则不需要重复添加)
    public static void AddUnique(this List<Vector2Int> self, Vector2Int other)
    {
        if (!self.Contains(other))
        {
            self.Add(other);
        }
    }

    //添加元素(如果已经有了则不需要重复添加)
    public static void AddUnique(this List<Vector2Int> self, List<Vector2Int> others)
    {
        if (others == null)
            return;

        for (int i = 0; i < others.Count; i++)
        {
            if (!self.Contains(others[i]))
            {
                self.Add(others[i]);
            }
        }
    }

    //偏移
    public static void Offset(this List<Vector2Int> self, Vector2Int offset)
    {
        for (int i = 0; i < self.Count; i++)
        {
            self[i] += offset;
        }
    }

    //移除操作
    public static void Except(this List<Vector2Int> self, List<Vector2Int> other)
    {
        if (other == null)
            return;

        for (int i = 0; i < other.Count; i++)
        {
            if (self.Contains(other[i]))
            {
                self.Remove(other[i]);
            }
        }
    }
}
