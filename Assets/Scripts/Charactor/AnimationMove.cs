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

  [SerializeField]
  private string setTriger = string.Empty;
  [SerializeField, Range(0, 10)]
  private float speed = 5f;
  [SerializeField]
  private Vector2 direction = Vector2.zero;
  [SerializeField]
  private bool isMove = false;

  /**************************************************
   * method
   **************************************************/

  public void MoveStart()
  {
    isMove = true;
    GetComponent<Animator>().Play(setTriger);
  }

  private void Awake()
  {
    if(setTriger != string.Empty)
    {
      GetComponent<Animator>().SetTrigger(setTriger);
    }
  }

  private void Update()
  {
    if(isMove)
      transform.Translate(Time.deltaTime * direction.normalized * speed);
  }

  private void SetTrigger(string triggerName)
  {
    GetComponent<Animator>().SetTrigger(triggerName);
  }
}
