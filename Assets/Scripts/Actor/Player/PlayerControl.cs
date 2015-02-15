using Es.Actor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Es.Actor
{
  /// <summary>
  /// どちらの手か
  /// </summary>
  public enum WhichHand
  {
    Left,
    Right,
  }

  /// <summary>
  /// プレイヤー制御
  /// </summary>
  public class PlayerControl : ActorBase
  {
    /**************************************************
     * field
     **************************************************/

    [SerializeField, Tooltip("攻撃範囲(円)の原点(中心座標)になる")]
    private Transform attackOrigin = null;
    [SerializeField, Tooltip("吹き飛ばし方向を決定するためのヘルパー")]
    private Transform dirHelper = null;
    [SerializeField, Range(0, 10)]
    private float moveSpeed = 5f;
    [SerializeField, Range(0, 1)]
    private float slowSpeedRate = 1f;
    [SerializeField, Range(0, 999)]
    private float attackPower = 999f;
    [SerializeField, Range(0, 10)]
    private float attackRadius = 0.5f;
    [SerializeField, Range(0, 1), Tooltip("1秒でのゲージ上昇値(1で1秒につきゲージマックスまで貯まる)")]
    private float gageUp = 0.5f;

    private float horizontalInput;
    private float verticalInput;
    private bool leftHandAttackInput;//"左手"での攻撃
    private bool rightHandAttackInput;//"右手"での攻撃
    private bool lockOnInput;//ターゲットのロックオン
    private float leftGage;
    private float rightGage;
    private bool clearFlag;

    private const float MIN_ATTACK_GAGE = 0.3f;//攻撃に必要なゲージの最小値
    private const float WIFF_CONSUME_GAGE = 0.3f;//空振りで消費するゲージ

    private Vector2 moveDir;//移動方向
    private GameObject lockOnEnemy;//ロックオンしたエネミーを保持しておく
    private Transform defaultDirHelper;//dirHelperがデフォルトで持つTranslate情報
    private MouseCursor cursor;//マウスカーソル(ロックオンで色を変更)
    private Transform target;//照準が持つTransform
    private SpriteRenderer targetSprite;//照準が持つスプライト

    /**************************************************
     * property
     **************************************************/

    public float LeftGage { get { return leftGage; } }
    public float RightGage { get { return rightGage; } }
    public float MinAttackGage { get { return MIN_ATTACK_GAGE; } }
    private float SlowSpeed
    {
      get { return slowSpeedRate; }
      set { slowSpeedRate = Mathf.Min(Mathf.Max(value, 0), 1); }
    }

    /**************************************************
     * method
     **************************************************/

    public override void Awake()
    {
      cursor = GameObject.FindGameObjectWithTag("Cursor").GetComponent<MouseCursor>();
      target = GameObject.FindGameObjectWithTag("Target").transform;
      targetSprite = target.GetComponent<SpriteRenderer>();
      base.Awake();
    }

    public override void Start()
    {
      defaultDirHelper = dirHelper.transform;
      targetSprite.enabled = false;
      base.Start();
    }

    public void Update()
    {
      //ユーザー入力情報の取得
      GetUserInput();

      //State別処理
      switch(state)
      {
        case State.Idle:
          break;

        case State.Play:

          #region 移動
          moveDir = new Vector2(horizontalInput, verticalInput);
          if(moveDir.magnitude > 1f)
            moveDir = moveDir.normalized;

          transform.Translate(Time.deltaTime * moveDir * moveSpeed * SlowSpeed, Space.World);

          var arrowDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
          var angle = Mathf.Atan2(arrowDir.y, arrowDir.x) * Mathf.Rad2Deg;
          attackOrigin.transform.eulerAngles = new Vector3(0f, 0f, angle);
          #endregion 移動

          #region ロックオン

          //ロックオンボタンでロックオンする敵を更新
          if(lockOnInput)
            lockOnEnemy = LockOn();

          //敵がいなかったら元に戻す
          if(lockOnEnemy == null)
          {
            dirHelper.position = defaultDirHelper.position;
            cursor.SetNormalColor();
            targetSprite.enabled = false;
          }

          //敵がいたらそこにロックオン設定する
          else
          {
            dirHelper.position = lockOnEnemy.transform.position;
            cursor.SetLockOnColor();
            target.position = lockOnEnemy.transform.position;
            targetSprite.enabled = true;
          }

          #endregion ロックオン

          #region 攻撃

          DebugExtension.DrawCircle(attackOrigin.position, attackRadius, Color.green);
          if(leftHandAttackInput && leftGage > MIN_ATTACK_GAGE)
          {
            animator.SetTrigger("LeftAttack");
            Attack(WhichHand.Left);
          }
          if(rightHandAttackInput && rightGage > MIN_ATTACK_GAGE)
          {
            animator.SetTrigger("RightAttack");
            Attack(WhichHand.Right);
          }
          #endregion 攻撃

          //ゲージUP
          GageUP();

          //死んだら状態をDeadに
          if(HP <= 0)
            state = State.Dead;

          break;

        case State.Attacked:
          if(rigidbody2D.velocity.magnitude < Vector2.one.magnitude)
            state = State.Play;
          break;

        case State.Dead:

          //ゲームオーバーへ移行
          if(rigidbody2D.velocity.magnitude < Vector2.one.magnitude)
          {
            Instantiate(exprPrefab, transform.position, Quaternion.identity);
            GetComponentsInChildren<SpriteRenderer>().ToList().ForEach(s => { s.enabled = false; });
            GetComponentsInChildren<Collider2D>().ToList().ForEach(s => { s.enabled = false; });
            Invoke("LoadGameOverScene", 3f);
            state = State.Idle;
          }
          break;

        default:
          state = State.Play;
          break;
      }
    }

    public void OnCollisionEnter2D(Collision2D coll)
    {
      if(coll.gameObject.tag == "Enemy")
        audio.PlayOneShot(FindAudioWithName("接触"));
    }

    /// <summary>
    /// ユーザーからの入力を取得する
    /// </summary>
    private void GetUserInput()
    {
      horizontalInput = Input.GetAxis("Horizontal");
      verticalInput = Input.GetAxis("Vertical");
      leftHandAttackInput = Input.GetButtonDown("Fire1");
      rightHandAttackInput = Input.GetButtonDown("Fire2");
      SlowSpeed += Input.GetAxis("Mouse ScrollWheel");

      lockOnInput = Input.GetButtonDown("Fire3");
    }

    /// <summary>
    /// ロックオンキー入力時にマウス座標に存在する
    /// Enemyタグをもつオブジェクトを取得する
    /// 取得できなかった場合Null
    /// </summary>
    private GameObject LockOn()
    {
      var lockon = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition))
        .FirstOrDefault(coll => { return coll.tag == "Enemy"; });
      return lockon == null ? null : lockon.gameObject;
    }

    /// <summary>
    /// 攻撃関数
    /// </summary>
    /// <param name="which">どちらの手か</param>
    private void Attack(WhichHand which)
    {
      audio.PlayOneShot(FindAudioWithName("パンチ"));
      var colls = FindVillainCollider2D();

      #region 空振り
      if(colls.Count() == 0)
      {
        Consume(which, WIFF_CONSUME_GAGE);
        return;
      }
      #endregion 空振り

      #region ゲージの消費
      foreach(var coll in colls)
        Consume(which, coll.GetComponent<VillainBase>().GageConsume);
      #endregion ゲージの消費

      #region コライダーの吹き飛ばし
      var exprDir = dirHelper.position - attackOrigin.position;
      AddForce(colls, exprDir, attackPower);
      #endregion コライダーの吹き飛ばし

      #region コライダーのダメージ処理
      foreach(var coll in colls)
        coll.gameObject.SendMessage("Damaged");
      #endregion コライダーのダメージ処理
    }

    /// <summary>
    /// 攻撃範囲内のVillainBaseを継承したアクターを取得する
    /// </summary>
    private IEnumerable<Collider2D> FindVillainCollider2D()
    {
      return Physics2D.OverlapCircleAll(attackOrigin.position, attackRadius)
              .Where(c =>
              {
                var villain = c.GetComponent<VillainBase>();
                if(villain == null)
                  return false;

                return villain.HP > 0;
              });
    }

    /// <summary>
    /// ゲージの上昇
    /// </summary>
    private void GageUP()
    {
      leftGage += gageUp * Time.deltaTime;
      rightGage += gageUp * Time.deltaTime;

      leftGage = Mathf.Min(1f, leftGage);
      rightGage = Math.Min(1f, rightGage);
    }

    /// <summary>
    /// ゲージを消費する
    /// </summary>
    /// <param name="which">どちらの手か</param>
    /// <param name="consume">消費する値</param>
    private void Consume(WhichHand which, float consume)
    {
      switch(which)
      {
        case WhichHand.Left:
          leftGage -= consume;
          break;

        case WhichHand.Right:
          rightGage -= consume;
          break;

        default:
          break;
      }

      leftGage = Mathf.Max(0f, leftGage);
      rightGage = Mathf.Max(0f, rightGage);
    }

    /// <summary>
    /// HPを回復する
    /// シーン開始時のHP以上にはならない
    /// </summary>
    /// <param name="healValue">回復量</param>
    public void Heal(int healValue)
    {
      HP += healValue;
    }

    /// <summary>
    /// クリア後ダメージを喰らわなくする処理
    /// </summary>
    public void GameClear()
    {
      clearFlag = true;
    }

    protected override void Damaged()
    {
      if(state == State.Play)
      {
        if(!clearFlag)
          base.Damaged();
        audio.PlayOneShot(FindAudioWithName("猫"));
      }
    }

    protected override void ExprDamaged()
    {
      if(state == State.Play)
      {
        if(!clearFlag)
          base.ExprDamaged();
        audio.PlayOneShot(FindAudioWithName("猫"));
      }
    }

    private void LoadGameOverScene()
    {
      Application.LoadLevel("GameOver");
    }
  }
}
