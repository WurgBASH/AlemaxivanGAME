using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : Unit {
	[SerializeField]
	private int lives =5;

    public int Lives
    {
        get { return lives; }
        set
        {
            if (value < 5) lives = value;
            livesBar.Refresh();
        }
    }
    private LivesBar livesBar;

    [SerializeField]
    private int points = 0;
    public int Points
    {
        get { return points; }
        set
        {
            points = value;
        }
    }
    private PointsBar pointsBar;

    [SerializeField]
	private float speed = 3.0F;
	[SerializeField]
	private float jumpForce = 7.0F;
    public GameObject respawn;

    private bool isGrounded =false;
    public bool readyShoot = true;


    private Bullet bullet;

	private CharState State
	{
		get{return (CharState)animator.GetInteger("State"); }
		set{ animator.SetInteger("State",(int)value);}
	}

	new private Rigidbody2D rigidbody;
	private Animator animator;
	private SpriteRenderer sprite;
    public GameObject gameOverPanel;
    public Text gameOverText;
    private void Awake()
	{
        gameOverPanel.SetActive(false);
        livesBar = FindObjectOfType<LivesBar>();
        rigidbody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		sprite =GetComponentInChildren<SpriteRenderer>();
        bullet = Resources.Load<Bullet>("Bullet");

    }

	private void FixedUpdate()
	{
		CheckGround();
        if(points == 425)
        {
            StartCoroutine(Win());
        }
	}

	private void Update()
	{
		if(isGrounded && readyShoot) State = CharState.Idle;
        if(!readyShoot) State = CharState.FireStay;
        if (Input.GetButton("Fire1") && readyShoot)
        {
            State = CharState.Fire;
            StartCoroutine(Shoot());
        }
		if(Input.GetButton("Horizontal")) Run();
		if(isGrounded && Input.GetButton("Jump")) Jump();
	}

    private void Run()
	{
		Vector3 direction = transform.right* Input.GetAxis("Horizontal");

		transform.position = Vector3.MoveTowards(transform.position,transform.position+direction,speed* Time.deltaTime);
		sprite.flipX = direction.x < 0.0F;
		if(isGrounded) State = CharState.Run;
	}
    private void Jump()
	{
		rigidbody.AddForce(transform.up*jumpForce, ForceMode2D.Impulse);
	}

    IEnumerator Shoot()
	{
        
        readyShoot = false;
        Vector3 position = transform.position; position.y+=0.8F; 
		if(sprite.flipX)  position.x -=1.8F; else position.x +=1.8F;
        
        Bullet newBullet = Instantiate(bullet,position,bullet.transform.rotation) as Bullet;
        State = CharState.FireStay;
        newBullet.Direction = newBullet.transform.right *(sprite.flipX ? -1.0F:1.0F);
        yield return new WaitForSeconds(1);
        readyShoot = true;

    }
    public override void ReceiveDamage()
    {
        Lives--;
        /*rigidbody.velocity = Vector3.zero;
        rigidbody.AddForce(transform.up * 8.0F, ForceMode2D.Impulse);*/
        if (Lives == 0)
        {
            StartCoroutine(Die());

        }
        Debug.Log(lives);
    }
    protected override IEnumerator Die()
    {

        gameOverPanel.SetActive(true);
        gameOverText.text = "GAME OVER!";
        yield return new WaitForSeconds(2);
        Lives = 5;
        livesBar.Refresh();
        gameOverPanel.SetActive(false);
        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
    }
    IEnumerator Win()
    {

        gameOverPanel.SetActive(true);
        gameOverText.text = "YOU WIN!";
        yield return new WaitForSeconds(2);
        Lives = 5;
        livesBar.Refresh();
         gameOverPanel.SetActive(false);
        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
    }
    private void CheckGround()
	{
		Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.3F);

		isGrounded = colliders.Length>1;

		if(!isGrounded) State = CharState.Jump;
	}

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        Unit unit = collision.gameObject.GetComponent<Unit>();
        if (unit) ReceiveDamage();

        Bullet bullet = collision.gameObject.GetComponent<Bullet>();
        if (bullet) ReceiveDamage();
    }*/
    private void OnTriggerEnter2D(Collider2D collider)
    {

        Bullet bullet = collider.gameObject.GetComponent<Bullet>();
        if (bullet && bullet.Parent != gameObject)
        {
            ReceiveDamage();
        }
    }

}

public enum CharState
{
	Idle,
	Run,
    Jump,
    Fire,
    FireStay
}
