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
    

    private SpriteRenderer sprite;

    protected override void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    protected override void Start()
    {
        direction = transform.right;
    }

    protected override void Update()
    {
        Move();
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
        Unit unit = collider.GetComponent<Unit>();
        if (unit && unit is Player)
        {
            if (Mathf.Abs(unit.transform.position.x - transform.position.x) < 1.5F) ReceiveDamage();
            else unit.ReceiveDamage();
        }
    }

    private void Move()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + transform.up * 0.5F + transform.right * direction.x * 0.5F, 3F);

        if (colliders.Length > 0 && colliders.All(x => !x.GetComponent<Player>())) direction *= -1.0F;
        
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
    }
}
