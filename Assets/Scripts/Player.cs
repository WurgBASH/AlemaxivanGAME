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
    
    private int stars = 3;
    public int Stars
    {
        get { return stars; }
        set
        {
            if (value < 3) stars = value;
            starBar.Refresh();
        }
    }
    private StarBar starBar;

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
    bool PlayerDie;
    public bool PlayerWin;

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
        
    }

	private void Update()
	{
        if (isGrounded && readyShoot) State = CharState.Idle;
        if (!readyShoot) State = CharState.FireStay;
        if (FuncState == 1)
        {
            RunBB();
        }
    }
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
    }

    public void JBut(int arg)
    {
        FunctionsArgs[Stateindex] = arg;
        FunctionsStates[Stateindex++] = 2;
    }
    public void SBut(int arg)
    {
        FunctionsArgs[Stateindex] = arg;
        FunctionsStates[Stateindex++] = 3;
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
        if (Lives == 0)
        {
            Lose();

        }
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
                    yield return new WaitForSeconds(1+FunctionsArgs[i] / 3);
                    break;
                case 2:
                    if (FunctionsArgs[i] > 5) FunctionsArgs[i] = 5;
                    rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);

                    rigidbody.AddForce(transform.right * 1.3F * FunctionsArgs[i], ForceMode2D.Impulse);
                    yield return new WaitForSeconds(3);
                    break;
                case 3:
                    State = CharState.Fire;
                    StartCoroutine(Shoot());
                    yield return new WaitForSeconds(2);
                    break;

            }
            if (PlayerDie)
            {
                break;
            }
        }
        if (!PlayerWin)
        {
            Lose();
        }
    }

    public void Lose()
    {
        if (!PlayerWin)
        {           
            PlayerDie = true;
            gameOverPanel.SetActive(true);
            starBar = FindObjectOfType<StarBar>();
            Stars = 0;
            starBar.Refresh();
            gameOverText.text = "YOU LOSE!";
            gameOverPanel.GetComponentInChildren<Button>();
            var next = GameObject.Find("NextBut");
            next.GetComponent<Button>().interactable = false;
            Lives = 5;
            livesBar.Refresh();
        }
        
    }
    public void Win()
    {
        sprite.enabled = !sprite.enabled;
        if (SceneManager.GetActiveScene().name == LevelManager.countUnlockedLevel.ToString())
            LevelManager.countUnlockedLevel++;
        PlayerWin = true;
        gameOverPanel.SetActive(true);
        starBar = FindObjectOfType<StarBar>();
        gameOverText.text = "YOU WIN!";
        switch (FunctionsStates.Length)
        {
            case 3:
                Stars = 3;
                break;
            case 4:
            case 5:
                Stars = 2;
                break;
            case 6:
            case 7:
            case 8:
                Stars = 1;
                break;
        }
        starBar.Refresh();
        var next = GameObject.Find("NextBut");
        next.GetComponent<Button>().interactable = true;
        Lives = 5;
        livesBar.Refresh();
    }
    private void CheckGround()
	{
		Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.3F);
		isGrounded = colliders.Length>1;
		if(!isGrounded) State = CharState.Jump;
	}
    public void AgainBut()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);    
    }
    public void NextBut()
    {       
        SceneManager.LoadScene("levels");
    }
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
