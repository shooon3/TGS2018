using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class IEnumerableExtensions
{
    /// <summary>
    /// シーケンスの中から、指定した数だけ要素を取り出す
    /// </summary>
    /// <param name="startIndex">始まりの要素</param>
    /// <param name="endIndex">終わりの要素</param>
    /// <returns></returns>
    public static IEnumerable<T> Paging<T>(this List<T> self, int startIndex, int endIndex)
    {
        return self.Skip(startIndex).Take(endIndex);
    }
}
