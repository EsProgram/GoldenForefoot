using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
  /// <summary>
  /// シーン名からロードする
  /// </summary>
  public void CallWithName(string sceneName)
  {
    Application.LoadLevel(sceneName);
  }

  /// <summary>
  /// 今呼び出されているシーンを再ロード
  /// </summary>
  public void CallMine()
  {
    Application.LoadLevel(Application.loadedLevel);
  }

  /// <summary>
  /// アプリケーションを終了
  /// </summary>
  public void QuitApplication()
  {
    Application.Quit();
  }
}
