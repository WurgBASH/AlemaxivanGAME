using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour {
    public virtual void ReceiveDamage()
    {
        Die();
    }
	protected virtual void Die()
	{
        Destroy(gameObject, .1f);
    }
}
