using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

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
	private float jumpForce = 25.0F;
    public GameObject respawn;

    private bool isGrounded =false;
    private bool readyShoot = true;


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

    private int FuncState =0;
    private int FucnArg =-1;
    private int FucnArg1 = 0;
    int[] FunctionsArgs;
    int[] FunctionsStates;
    int Stateindex = 0;
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

        if (FucnArg>0)
            FucnArg--;
        if (FucnArg == 0)
        {
            FucnArg1--;
            if (FucnArg1 <= 0)
            {
                FuncState = 0;
                State = CharState.Idle;
            }
            else
            {
                FucnArg = 17;
            }
            
        }
        
            
       // if (isGrounded && Input.GetButton("Jump")) Jump();
        
    }

	private void Update()
	{
        if (isGrounded && readyShoot) State = CharState.Idle;
        if (!readyShoot) State = CharState.FireStay;
        if (FuncState == 1)
        {
            RunBB();
        }
        
        



        /*if(Input.GetButton("Horizontal")) Run();
        if (Input.GetButton("Fire1") && readyShoot)
        {
            State = CharState.Fire;
            StartCoroutine(Shoot());
        }*/

    }
    /* private void Run()
     {
         Vector3 direction = transform.right* Input.GetAxis("Horizontal");
         transform.position = Vector3.MoveTowards(transform.position,transform.position+direction,speed* Time.deltaTime);
         sprite.flipX = direction.x < 0.0F;
         if(isGrounded) State = CharState.Run;
     }*/
    public void CreateFuncArray(int size)
    {
        FunctionsArgs = new int[size];
        Stateindex = 0;
        FunctionsStates = new int[size];      
    }
    private void RunBB()
    {
        Vector3 direction = transform.right;
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
        if (isGrounded) State = CharState.Run;
    }
    public void RBut(int arg)
    {
        FunctionsArgs[Stateindex] = arg;
        FunctionsStates[Stateindex++] = 1;
        //FuncState = 1;
        //FucnArg = 17;
        //FucnArg1 = arg;

        /*
        State = CharState.Run;

        Vector3 direction = transform.right;
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);

        /*Vector3 direction = new Vector3(10.0F,0,0);
        Debug.Log(transform.right);
        for (int i = 0; i < arg; i++)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
            //transform.position += Vector3.right;
        }*/


        //State = CharState.Idle;
    }

    public void JBut(int arg)
    {
        FunctionsArgs[Stateindex] = arg;
        FunctionsStates[Stateindex++] = 2;
        /* if (arg > 5) arg = 5;
        rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);

        rigidbody.AddForce(transform.right * 1.3F*arg, ForceMode2D.Impulse);*/

    }
    public void SBut(int arg)
    {
        FunctionsArgs[Stateindex] = arg;
        FunctionsStates[Stateindex++] = 3;
        /*
        State = CharState.Fire;
        StartCoroutine(Shoot());*/
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

    public IEnumerator Starting()
    {
        for(int i = 0; i < FunctionsStates.Length; i++)
        {
            switch (FunctionsStates[i])
            {
                case 1:
                    FuncState = 1;
                    FucnArg = 17;
                    FucnArg1 = FunctionsArgs[i];
                    yield return new WaitForSeconds(2);
                    break;
                case 2:
                    if (FunctionsArgs[i] > 5) FunctionsArgs[i] = 5;
                    rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);

                    rigidbody.AddForce(transform.right * 1.3F * FunctionsArgs[i], ForceMode2D.Impulse);
                    yield return new WaitForSeconds(2);
                    break;
                case 3:
                    State = CharState.Fire;
                    StartCoroutine(Shoot());
                    yield return new WaitForSeconds(2);
                    break;

            }
        }
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
