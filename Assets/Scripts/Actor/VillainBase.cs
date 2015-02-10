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

    [SerializeField, Tooltip("爆破時に使用するエフェクトプレハブ")]
    private GameObject exprPrefab = default(GameObject);
    [SerializeField, Range(0, 10), Tooltip("爆破時の吹き飛ばし範囲の半径")]
    private float exprRadius = 0.5f;
    [SerializeField, Range(0f, 3000f), Tooltip("爆破時の吹き飛ばしで周囲のオブジェクトに与える力")]
    private float exprPower = 999f;

    /**************************************************
     * method
     **************************************************/

    /// <summary>
    /// プレイヤーに接触したら爆発処理
    /// </summary>
    public virtual void OnCollisionEnter2D(Collision2D coll)
    {
      if(coll.gameObject.tag == "Player")
        ExprDead();
    }

    /// <summary>
    /// プレイヤーに接触したら爆発処理
    /// </summary>
    public virtual void OnTriggerEnter2D(Collider2D other)
    {
      if(other.tag == "Player")
        ExprDead();
    }

    /// <summary>
    /// 爆破時の処理
    /// 周囲のコライダーを取得して四散させ、ダメージを与える
    /// 取得するコライダーはActorBaseがアタッチされたもの
    /// </summary>
    private void AddExprForceAndDamage()
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
        target.col.SendMessage("Damaged");
      }
    }

    /// <summary>
    /// 死亡処理
    /// 死亡エフェクトの表示とゲームオブジェクトの破棄のみを行う
    /// </summary>
    private void Dead()
    {
      Destroy(Instantiate(exprPrefab, transform.position, Quaternion.identity), 2f);
      Destroy(gameObject);
    }

    /// <summary>
    /// 爆発・死亡処理
    /// </summary>
    protected void ExprDead()
    {
      AddExprForceAndDamage();
      Dead();
    }
  }
}
