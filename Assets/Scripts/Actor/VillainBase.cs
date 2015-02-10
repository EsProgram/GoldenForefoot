using System.Collections;
using System.Linq;
using UnityEngine;

namespace Es.Actor
{
  public abstract class VillainBase : ActorBase
  {
    /**************************************************
     * field
     **************************************************/
    [SerializeField, Range(0, 1), Tooltip("攻撃された際に消費させるゲージの値")]
    protected float gageConsume = 0.3f;
    [SerializeField, Tooltip("爆破時に使用するエフェクトプレハブ")]
    private GameObject exprPrefab = null;
    [SerializeField, Range(0, 10), Tooltip("爆破時の吹き飛ばし範囲の半径")]
    private float exprRadius = 0.5f;
    [SerializeField, Range(0f, 3000f), Tooltip("爆破時の吹き飛ばしで周囲のオブジェクトに与える力")]
    private float exprPower = 999f;

    /**************************************************
     * property
     **************************************************/

    /// <summary>
    ///ゲージの消費値を返す
    /// </summary>
    public float GageConsume { get { return gageConsume; } }

    /**************************************************
     * method
     **************************************************/

    /// <summary>
    /// プレイヤーに接触した際のダメージ処理
    /// </summary>
    public virtual void OnCollisionEnter2D(Collision2D coll)
    {
      if(coll.gameObject.tag == "Player")
        DeadOnExpr();
    }

    /// <summary>
    /// 画面からフェードアウトしたら破棄
    /// </summary>
    public void OnBecameInvisible()
    {
      Destroy(gameObject);
    }

    /// <summary>
    /// 爆破時の処理
    /// 周囲のコライダーを取得して四散させ、ダメージを与える
    /// 取得するコライダーはActorBaseがアタッチされたもの
    /// </summary>
    protected void AddExprForceAndDamage()
    {
      //周囲のコライダー取得
      var targets = Physics2D.OverlapCircleAll(transform.position, exprRadius)
        .Where(c =>
        {
          var component = c.GetComponent<ActorBase>();
          if(component == null)
            return false;
          return component.HP > 0;
        })
        .Select(c =>
        {
          Vector2 dir = c.transform.position - transform.position;
          return new { col = c, direction = dir };
        });

      //吹き飛ばしとダメージを与える処理
      foreach(var target in targets)
      {
        AddForce(target.col, target.direction, exprPower);
        target.col.SendMessage("ExprDamaged");
      }
    }

    /// <summary>
    /// 死亡処理
    /// 周囲にダメージ・力を与えない
    /// </summary>
    protected void DeadOnSilent()
    {
      Destroy(Instantiate(exprPrefab, transform.position, Quaternion.identity), 2f);
      Destroy(gameObject);
    }

    /// <summary>
    /// 死亡処理
    /// 周囲に力・ダメージを与える
    /// </summary>
    protected void DeadOnExpr()
    {
      AddExprForceAndDamage();
      DeadOnSilent();
    }
  }
}
