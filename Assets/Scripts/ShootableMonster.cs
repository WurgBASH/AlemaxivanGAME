using UnityEngine;
using System.Collections;

public class ShootableMonster : Monster
{
    [SerializeField]
    private int mLives = 3;
    [SerializeField]
    private float rate = 2.0F;
    [SerializeField]
    private Color bulletColor = Color.white;
    private Bullet bullet;

    protected override void Awake()
    {
        bullet = Resources.Load<Bullet>("SlimeBullet");
    }

    protected override void Start()
    {
        InvokeRepeating("Shoot", rate, rate);
    }

    private void Shoot()
    {
        Vector3 position = transform.position;          position.y += 0.5F;
        Bullet newBullet = Instantiate(bullet, position, bullet.transform.rotation) as Bullet;

        newBullet.Parent = gameObject;
        newBullet.Direction = -newBullet.transform.right;
        newBullet.Color = bulletColor;
    }
    public override void ReceiveDamage()
    {
        mLives--;
        Debug.Log(mLives + ":SMonster");

        if (mLives == 0)
        {
            Destroy(gameObject, .1f);
            Vector3 position = transform.position;
            position.y += 0.5F;
        }
    }
    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        Unit unit = collider.GetComponent<Unit>();

        if (unit && unit is Player)
        {
            if (Mathf.Abs(unit.transform.position.x - transform.position.x) < 1.0F) ReceiveDamage();
            else unit.ReceiveDamage();
        }


        Bullet bullet = collider.gameObject.GetComponent<Bullet>();
        if (bullet && bullet.Parent != gameObject)
        {
            ReceiveDamage();
        }
    }
   
}
