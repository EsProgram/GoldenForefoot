using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
  //LoadSceneによって最後に呼び出されたステージを記録
  private static string preLoaded = "Title";

  /// <summary>
  /// シーン名からロードする
  /// </summary>
  public void CallWithName(string sceneName)
  {
    preLoaded = sceneName;
    Application.LoadLevel(sceneName);
  }

  /// <summary>
  /// 今呼び出されているシーンを再ロード
  /// </summary>
  public void CallMine()
  {
    Application.LoadLevel(Application.loadedLevel);
  }

  public void CallPreScene()
  {
    Application.LoadLevel(preLoaded);
  }

  /// <summary>
  /// アプリケーションを終了
  /// </summary>
  public void QuitApplication()
  {
    Application.Quit();
  }
}
