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
  [SerializeField, Range(0, 10)]
  private float speed = 5f;
  [SerializeField, Tooltip("Animationで変更し、移動方向を制御するための変数")]
  private Vector2 direction = Vector2.zero;

  private Animator animator;
  /**************************************************
   * method
   **************************************************/

  private void Awake()
  {
    if(setTriger != string.Empty)
    {
      animator = GetComponent<Animator>();
      SetTrigger();
    }
  }

  private void Update()
  {
    if(animator.GetCurrentAnimatorStateInfo(0).IsName("None"))
      SetTrigger();

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
