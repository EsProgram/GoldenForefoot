using System.Collections;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
  [SerializeField, Tooltip("ロックオン時のカーソルの色を指定する")]
  private Color lockOnColor = default(Color);

  private Color defaultColor;
  private SpriteRenderer sprite;

  public void Awake()
  {
    sprite = GetComponent<SpriteRenderer>();
  }

  private void Start()
  {
    defaultColor = sprite.color;
    Screen.showCursor = false;
  }

  public void OnApplicationFocus(bool focus)
  {
    Screen.showCursor = false;
  }

  public void Update()
  {
    Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    transform.position = pos;
  }

  /// <summary>
  /// ロックオン時のカーソル色に変更する
  /// </summary>
  public void SetLockOnColor()
  {
    sprite.color = lockOnColor;
  }

  /// <summary>
  /// デフォルトのカーソル色に変更する
  /// </summary>
  public void SetNormalColor()
  {
    sprite.color = defaultColor;
  }
}
