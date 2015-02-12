using System.Collections;
using UnityEngine;

public static class DebugExtension
{
  /// <summary>
  /// 円を表示する
  /// </summary>
  /// <param name="origin">中心座標</param>
  /// <param name="radius">半径</param>
  /// <param name="color">色</param>
  public static void DrawCircle(Vector3 origin, float radius, Color color)
  {
    for(int i = 0; i < 360; ++i)
    {
      var pos = new Vector3(Mathf.Cos(i * Mathf.Deg2Rad), Mathf.Sin(i * Mathf.Deg2Rad), 0) * radius + origin;
      Debug.DrawLine(pos, pos + Vector3.right * 0.01f, color);
    }
  }

  /// <summary>
  /// 指定時間残る円を表示する
  /// </summary>
  /// <param name="origin">中心座標</param>
  /// <param name="radius">半径</param>
  /// <param name="color">色</param>
  /// <param name="duration">継続時間</param>
  public static void DrawCircle(Vector3 origin, float radius, Color color, float duration)
  {
    for(int i = 0; i < 360; ++i)
    {
      var pos = new Vector3(Mathf.Cos(i * Mathf.Deg2Rad), Mathf.Sin(i * Mathf.Deg2Rad), 0) * radius + origin;
      Debug.DrawRay(pos, Vector3.right * 0.01f, color, duration);
    }
  }
}
