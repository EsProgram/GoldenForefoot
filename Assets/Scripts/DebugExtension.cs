using System.Collections;
using UnityEngine;

public static class DebugExtension
{
  public static void DrawCircle(Vector3 origin, float radius, Color color)
  {
    for(int i = 0; i < 360; ++i)
    {
      var pos = new Vector3(Mathf.Cos(i * Mathf.Deg2Rad), Mathf.Sin(i * Mathf.Deg2Rad), 0) * radius + origin;
      Debug.DrawLine(pos, pos + Vector3.right * 0.01f, color);
    }
  }
}
