using System.Collections;
using System.Linq;
using UnityEngine;

namespace Es.Actor
{
  public abstract class BulletBase : VillainBase
  {
    /**************************************************
     * field
     **************************************************/
    [SerializeField]
    protected float speed = 3f;

    /**************************************************
     * method
     **************************************************/

    public void Update()
    {
      switch(state)
      {
        case State.Play:

          if(HP <= 0)
            state = State.Dead;

          break;

        case State.Dead:
          ExprDead();
          break;

        default:
          state = State.Play;
          break;
      }
    }

    protected abstract void Move();
  }
}
