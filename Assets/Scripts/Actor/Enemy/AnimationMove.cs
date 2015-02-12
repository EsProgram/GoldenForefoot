using System.Collections;
using UnityEngine;

/// <summary>
/// アニメーションで変更し移動を実現するためのクラス
/// </summary>
public class AnimationMove : MonoBehaviour
{
  /**************************************************
   * field
   **************************************************/

  [SerializeField, Tooltip("Awakeメソッドでアニメーションを変更する際のトリガー名")]
  private string setTriger = string.Empty;
  [Range(0, 10)]
  public float speed = 5f;
  [Tooltip("Animationで変更し、移動方向を制御するための変数")]
  public Vector2 direction = Vector2.zero;

  /**************************************************
   * method
   **************************************************/

  private void Awake()
  {
    if(setTriger != string.Empty)
    {
      SetTrigger();
    }
  }

  private void Update()
  {
    transform.Translate(Time.deltaTime * direction.normalized * speed);
  }

  private void SetTrigger(string triggerName)
  {
    GetComponent<Animator>().SetTrigger(triggerName);
  }

  private void SetTrigger()
  {
    SetTrigger(setTriger);
  }
}
