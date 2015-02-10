using System.Collections;
using UnityEngine;

/// <summary>
/// 子ゲームオブジェクトが全てお亡くなりになったら
/// 親も心中してお亡くなりになる
/// </summary>
public class FamilySuicide : MonoBehaviour
{
  private const float initWaitTime = 1f;
  private const float repeatRate = 1f;

  public void Start()
  {
    InvokeRepeating("CheckChildren", initWaitTime, repeatRate);
  }

  private void CheckChildren()
  {
    if(transform.childCount == 0)
      Destroy(gameObject);
  }
}
