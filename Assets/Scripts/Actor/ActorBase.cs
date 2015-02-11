using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Es.Actor
{
  public enum State
  {
    Idle,//プレイ停止時
    Play,//プレイ中全般
    Attacked,//攻撃を受けた時
    Dead,//破壊されるとき
  }

  /// <summary>
  /// ゲーム内アクターの基本となるクラス
  /// </summary>
  public abstract class ActorBase : MonoBehaviour
  {
    /**************************************************
     * field
     **************************************************/
    [SerializeField, Range(0, 999)]
    private int hp = 1;
    [SerializeField, Tooltip("爆破時に使用するエフェクトプレハブ")]
    protected GameObject exprPrefab = null;
    public State state = State.Play;

    protected Animator animator;

    protected bool exprDamageTrigger;//爆発によるダメージを受けた際にONになる

    private int maxHP;
    private const int NORMAL_DAMAGE = 1;
    private const int EXPR_DAMAGE = 3;

    /**************************************************
     * property
     **************************************************/

    /// <summary>
    /// 残りHP
    /// </summary>
    public int HP { get { return hp; } set { hp = Mathf.Min(Mathf.Max(0, value), MaxHP); } }

    public int MaxHP { get { return maxHP; } }

    /**************************************************
     * method
     **************************************************/

    public virtual void Awake()
    {
      animator = GetComponent<Animator>();
    }

    public virtual void Start()
    {
      maxHP = hp;
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
          continue;
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
      state = State.Attacked;
      HP -= NORMAL_DAMAGE;
      animator.SetTrigger("Damaged");
    }

    /// <summary>
    /// 爆発によるダメージを受けた時の動作
    /// セットされるトリガーを使って各自で実装
    /// </summary>
    protected virtual void ExprDamaged()
    {
      exprDamageTrigger = true;
      state = State.Attacked;
      HP -= EXPR_DAMAGE;
      animator.SetTrigger("Damaged");
      Invoke("ExprDamageTriggerReset", 0.1f);
    }

    private void ExprDamageTriggerReset()
    {
      exprDamageTrigger = false;
    }
  }
}
