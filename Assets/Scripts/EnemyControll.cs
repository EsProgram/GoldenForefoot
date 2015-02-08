using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Es.Charactor
{
  /// <summary>
  /// 敵の状態
  /// </summary>
  internal enum EnemyState
  {
    Idle,//停止状態
    Play,//活動状態
    Panched,//猫パンチを喰らって飛ばされている状態
  }

  public class EnemyControll : CharactorBase
  {
    /**************************************************
     * field
     **************************************************/
    [SerializeField]
    private EnemyState state = EnemyState.Idle;
    [SerializeField]
    private GameObject exprPrefab = default(GameObject);//テスト用爆発エフェクト
    [SerializeField, Range(0, 10)]
    private float exprRadius = 1f;//爆破時の吹き飛ばし範囲
    [SerializeField, Range(0f, 999f)]
    private float exprPower = 999f;//爆破時の吹き飛ばしで周囲のオブジェクトに与える力

    private bool timeFixFlagOnPanched = false;//パンチされた際の物理的挙動が適用された後かどうか

    /**************************************************
     * property
     **************************************************/

    internal EnemyState State { get { return state; } set { state = value; } }

    /**************************************************
     * override
     **************************************************/

    public void Update()
    {
      switch(state)
      {
        case EnemyState.Idle:
          Debug.Log(name + ":Idle状態に遷移しました");
          break;

        case EnemyState.Play:

          //HPが0になったら爆発
          if(hp <= 0)
          {
            AddExprForceAndDamage();
            Dead();
          }

          break;

        case EnemyState.Panched:

          //物理演算処理との時間差を吸収できていれば
          if(timeFixFlagOnPanched)
          {
            //吹き飛びの速度が一定以下になった
            if(rigidbody2D.velocity.magnitude < Vector2.one.magnitude)
            {
              //HPが0になっていたら爆発する(周りに力を与え、ダメージメッセージを送信)
              if(hp <= 0)
              {
                #region 爆発処理

                AddExprForceAndDamage();

                //死亡処理
                Dead();
                #endregion 爆発処理
              }
              state = EnemyState.Play;
              timeFixFlagOnPanched = false;
            }
          }

          break;

        default:
          break;
      }
    }

    public void FixedUpdate()
    {
      // 物理パラメータ(velocity等)を参照するUpdateとのズレを考慮し、フラグを変更する
      if(state == EnemyState.Panched)
        timeFixFlagOnPanched = true;
    }

    public void OnCollisionEnter2D(Collision2D coll)
    {
      // 一定の条件下で、velocityをゼロに設定する
      if(hp <= 0 && State == EnemyState.Panched)
      {
        rigidbody2D.velocity = Vector2.zero;
      }
    }

    /**************************************************
     * method
     **************************************************/

    /// <summary>
    /// 爆破時の処理
    /// 周囲のコライダーを取得して四散させ、ダメージを与える
    /// 取得するコライダーはCharactorBaseがアタッチされたもの
    /// </summary>
    private void AddExprForceAndDamage()
    {
      //周囲のコライダー取得
      var targets = Physics2D.OverlapCircleAll(transform.position, exprRadius)
        .Where(c =>
        {
          var component = c.GetComponent<CharactorBase>();
          if(component == null)
            return false;
          return component.GetHP() > 0;
        })//HPが1以上のCharactorBaseをもつものを取得
        .Select(c =>
        {
          Vector2 dir = c.transform.position - transform.position;
          return new { col = c, direction = dir };
        });//吹き飛び方向を加工(追加)した匿名型に射影

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
    /// ダメージを受けた時の処理
    /// </summary>
    protected override void Damaged()
    {
      base.Damaged();
    }
  }
}
