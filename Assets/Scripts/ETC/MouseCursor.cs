using System.Collections;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
  private void Start()
  {
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
}
