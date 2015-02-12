using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 円内のオブジェクトを停止させる
/// </summary>
public class Pose : MonoBehaviour
{
  [SerializeField]
  private float radius = 10f;

  private SpriteRenderer sprite;
  private const string TagNameMine = "Pose";

  public void Awake()
  {
    sprite = GetComponent<SpriteRenderer>();
    tag = TagNameMine;
  }

  public void Update()
  {
    /**************************************************
     * ためしにぽーず！！！！
     **************************************************/
    if(Input.GetKeyDown(KeyCode.M))
      On();
    if(Input.GetKeyUp(KeyCode.N))
      Off();
  }

  public void On()
  {
    DebugExtension.DrawCircle(transform.position, radius, Color.red, 3f);

    //ポーズ画像の表示
    sprite.enabled = true;

    //自分以外の円内のコライダー取得
    var colls = Physics2D.OverlapCircleAll(transform.position, radius).Where(c => { return c.tag != TagNameMine; });
    foreach(var coll in colls)
    {
      //取得したコライダーを持つゲームオブジェクトのコライダー2D以外全部取得
      var spesifyComponents = coll.gameObject.GetComponentsInChildren<Behaviour>()
        .Where(component => { return !(component is Collider2D); });

      //停止
      foreach(var sc in spesifyComponents)
      {
        sc.enabled = false;
      }
    }
  }

  public void Off()
  {
    DebugExtension.DrawCircle(transform.position, radius, Color.red, 3f);

    //ポーズ画像の非表示
    sprite.enabled = false;

    //自分以外の円内のコライダー取得
    var colls = Physics2D.OverlapCircleAll(transform.position, radius).Where(c => { return c.tag != TagNameMine; });
    foreach(var coll in colls)
    {
      //取得したコライダーを持つゲームオブジェクトのコライダー2D以外全部取得
      var spesifyComponents = coll.gameObject.GetComponentsInChildren<Behaviour>()
        .Where(component => { return !(component is Collider2D); });

      //停止
      foreach(var sc in spesifyComponents)
      {
        sc.enabled = true;
      }
    }
  }
}
