using UnityEngine;

public class Pig : MonoBehaviour
{
    [SerializeField] private string animalName;
    [SerializeField] private int hp;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private BoxCollider collider;
    [SerializeField] private float walkTime;
    [SerializeField] private float runTime;
    [SerializeField] private float waitTime;

    private float applySpeed;
    private float currentTime;
    private Vector3 direction;

    private bool isAction;
    private bool isWalking = false;
    private bool isRunning = false;
    private bool isDead = false;

    private AudioSource audioSource;
    [SerializeField] private AudioClip[] normalSounds;
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip deadSound;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        currentTime = waitTime;
        isAction = true;
    }

    void Update()
    {
        if (isDead)
        {
            return;
        }
        ElapseTime();
        Move();
        Rotation();
    }

    private void ElapseTime()
    {
        if (isAction)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0f)
            {
                Reset();
            }
        }
    }

    private void Move()
    {
        if (isWalking || isRunning)
        {
            rigid.MovePosition(transform.position + transform.forward * applySpeed * Time.deltaTime);
        }
    }
    private void Rotation()
    {
        Vector3 dir = new Vector3(0f, direction.y, 0f);
        if (isWalking || isRunning)
        {
            Vector3 rotation_Lerp = Vector3.Lerp(transform.eulerAngles, dir, 0.01f);
            rigid.MoveRotation(Quaternion.Euler(rotation_Lerp));
        }
    }
    private void Reset()
    {
        isWalking = false;
        isRunning = false;
        isAction = true;
        applySpeed = walkSpeed;
        animator.SetBool("Walking", isWalking);
        animator.SetBool("Running", isRunning);
        direction.Set(0f, Random.Range(0f, 360f), 0f);
        RandomAction();
    }

    private void RandomAction()
    {
        RandomSound();
        int randomAction = Random.Range(0, 4);

        if (randomAction == 0)
        {
            Wait();
        }
        else if (randomAction == 1)
        {
            Eat();
        }
        else if (randomAction == 2)
        {
            Peek();
        }
        else if (randomAction == 3)
        {
            TryWalk();
        }
    }

    private void Wait()
    {
        currentTime = waitTime;
        Debug.Log("´ë±â");
    }
    private void Eat()
    {
        currentTime = waitTime;
        animator.SetTrigger("Eat");
        Debug.Log("¸Ô±â");
    }
    private void Peek()
    {
        currentTime = waitTime;
        animator.SetTrigger("Peek");
        Debug.Log("µÎ¸®¹ø");
    }
    private void TryWalk()
    {
        isWalking = true;
        animator.SetBool("Walking", isWalking);
        currentTime = walkTime;
        applySpeed = walkSpeed;
        Debug.Log("°È±â");
    }
    public void Run(Vector3 targetPos)
    {
        direction = Quaternion.LookRotation(transform.position - targetPos).eulerAngles;

        currentTime = runTime;
        isWalking = false;
        isRunning = true;
        applySpeed = runSpeed;
        animator.SetBool("Running", isRunning);
    }

    public void Damaged(int damage, Vector3 targetPos)
    {
        if(isDead)
        {
            return;
        }

        hp -= damage;

        if (hp <= 0)
        {
            Dead();
            return;
        }

        PlaySE(hurtSound);
        animator.SetTrigger("Hurt");
        Run(targetPos);
    }

    private void Dead()
    {
        PlaySE(deadSound);
        isWalking = false;
        isRunning = false;
        isDead = true;
        animator.SetTrigger("Dead");
    }

    private void PlaySE(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
    private void RandomSound()
    {
        int randdomIndex = Random.Range(0, 3);
        PlaySE(normalSounds[randdomIndex]);
    }
}
