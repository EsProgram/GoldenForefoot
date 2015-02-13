using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 円内のオブジェクトを停止させる
/// </summary>
public class Pause : MonoBehaviour
{
  /**************************************************
   * field
   **************************************************/

  [SerializeField]
  private float radius = 10f;

  private SpriteRenderer sprite;
  private Canvas canvas;
  private const string TagNameMine = "Pause";
  private bool pauseSwitch;

  /**************************************************
   * method
   **************************************************/

  public void Awake()
  {
    sprite = GetComponent<SpriteRenderer>();
    canvas = GetComponentInChildren<Canvas>();
    tag = TagNameMine;
  }

  public void Update()
  {
    if(Input.GetButtonDown("Pause"))
    {
      audio.Play();
      if(pauseSwitch)
        OFF();
      else
        ON();
      pauseSwitch = !pauseSwitch;
    }
  }

  public void ON()
  {
    DebugExtension.DrawCircle(transform.position, radius, Color.red, 3f);
    PauseScreen(true);
    SwitchBehaviour(true);
  }

  public void OFF()
  {
    DebugExtension.DrawCircle(transform.position, radius, Color.red, 3f);
    PauseScreen(false);
    SwitchBehaviour(false);
  }

  /// <summary>
  /// ポーズようのスクリーンをセットする
  /// </summary>
  /// <param name="set">trueでポーズ用スクリーンに</param>
  private void PauseScreen(bool set)
  {
    sprite.enabled = set;
    canvas.enabled = set;
  }

  /// <summary>
  /// オブジェクトとして振る舞うかどうか
  /// </summary>
  /// <param name="sw">trueで振る舞うようにする</param>
  private void SwitchBehaviour(bool sw)
  {
    //自分以外の円内のコライダー取得
    var colls = Physics2D.OverlapCircleAll(transform.position, radius).Where(c => { return c.tag != TagNameMine; });
    foreach(var coll in colls)
    {
      //取得したコライダーを持つゲームオブジェクトのコライダー2D以外全部取得
      var spesifyComponents = coll.gameObject.GetComponentsInChildren<Behaviour>()
        .Where(component => { return !(component is Collider2D); });

      //開始
      foreach(var sc in spesifyComponents)
        sc.enabled = !sw;
    }
  }

  private void BehaviourON()
  {
    SwitchBehaviour(true);
  }

  private void BehaviourOFF()
  {
    SwitchBehaviour(false);
  }
}
