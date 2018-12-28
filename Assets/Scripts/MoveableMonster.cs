using UnityEngine;
using System.Collections;
using System.Linq;

public class MoveableMonster : Monster
{
    [SerializeField]
    private int mLives = 1;
    [SerializeField]
    private float speed = 2.0F;
    private Vector3 direction;
    int count = 0;

    private SpriteRenderer sprite;

    protected override void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    protected override void Start()
    {
        direction = transform.right;
    }
    private void FixedUpdate()
    {
        count++;
       
    }
    protected override void Update()
    {
        Move();
        if (count>55)
        {
            count = 0;
            direction *= -1.0F;
            sprite.flipX = !sprite.flipX;
        }
    }
    public override void ReceiveDamage()
    {
        mLives--;
        Debug.Log(mLives + ":Monster");

        if (mLives == 0)
        {
            Destroy(gameObject, .1f);
            Vector3 position = transform.position;
            position.y += 0.5F;
        }
    }
    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        Bullet bullet = collider.gameObject.GetComponent<Bullet>();
        if (bullet && bullet.Parent != gameObject)
        {
            ReceiveDamage();
        }
        Unit unit = collider.GetComponent<Unit>();
        if (unit && unit is Player)
        {
            unit.ReceiveDamage();
        }
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
    }
}
