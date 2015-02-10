using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Es.Charactor
{
  /// <summary>
  /// 全てのキャラクターの基本となるクラス
  /// </summary>
  public abstract class CharactorBase : MonoBehaviour
  {
    /**************************************************
     * field
     **************************************************/
    [SerializeField, Range(0, 999)]
    protected int hp;

    /**************************************************
     * method
     **************************************************/

    /// <summary>
    /// 残りHPを返す
    /// </summary>
    /// <returns>残りHP</returns>
    public int GetHP()
    {
      return hp;
    }

    /// <summary>
    /// 複数のコライダーのrigidbody2Dに指定した方向に力を加える
    /// 全てのコライダーは同じ方向に力を与えられる
    /// </summary>
    /// <param name="cols">力を与える対象の列挙可能コライダー</param>
    /// <param name="dir">力の向き</param>
    /// <param name="power">力の大きさ</param>
    protected void AddForce(IEnumerable<Collider2D> cols, Vector2 dir, float power)
    {
      dir = dir.normalized;
      foreach(var col in cols)
      {
        if(col.rigidbody2D == null)
        {
          Debug.LogWarning(col.name + "にrigidbody2Dが適用されていません");
          continue;
        }
        col.rigidbody2D.AddForce(dir * power);
      }
    }

    /// <summary>
    /// 1つのコライダーのrigidbody2Dに指定した方向に力を加える
    /// </summary>
    /// <param name="col">力を与える対象のコライダー</param>
    /// <param name="dir">力の向き</param>
    /// <param name="power">力の大きさ</param>
    protected void AddForce(Collider2D col, Vector2 dir, float power)
    {
      List<Collider2D> cols = new List<Collider2D>();
      cols.Add(col);
      AddForce(cols, dir, power);
    }

    /// <summary>
    /// ダメージを受けた時の動作
    /// </summary>
    protected virtual void Damaged()
    {
      --hp;
    }
  }
}
