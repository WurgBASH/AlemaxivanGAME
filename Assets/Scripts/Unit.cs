using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour {
    public virtual void ReceiveDamage()
    {
        Die();
    }
    protected virtual IEnumerator Die()
	{
        Destroy(gameObject, .1f);
        yield return new WaitForSeconds(2);
    }
}
