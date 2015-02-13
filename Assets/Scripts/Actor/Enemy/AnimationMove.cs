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
    SetTrigger();
  }

  private void Update()
  {
    transform.Translate(Time.deltaTime * direction.normalized * speed);
  }

  public void SetTrigger(string triggerName)
  {
    GetComponent<Animator>().SetTrigger(triggerName);
    if(triggerName == "Damaged")
      Invoke("SetTrigger", 2f);
  }

  private void SetTrigger()
  {
    if(setTriger != string.Empty)
      SetTrigger(setTriger);
  }
}
