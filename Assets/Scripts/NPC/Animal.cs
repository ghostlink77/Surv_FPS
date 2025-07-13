using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour
{
    [SerializeField] protected string animalName;
    [SerializeField] protected int hp;

    [SerializeField] protected float walkSpeed;
    [SerializeField] protected float runSpeed;

    [SerializeField] protected Animator animator;
    [SerializeField] protected Rigidbody rigid;
    [SerializeField] protected BoxCollider collider;

    [SerializeField] protected float walkTime;
    [SerializeField] protected float runTime;
    [SerializeField] protected float waitTime;

    protected float currentTime;
    protected Vector3 destination;

    protected bool isAction;
    protected bool isWalking = false;
    protected bool isRunning = false;
    protected bool isDead = false;

    protected NavMeshAgent nav;
    protected AudioSource audioSource;
    [SerializeField] protected AudioClip[] normalSounds;
    [SerializeField] protected AudioClip hurtSound;
    [SerializeField] protected AudioClip deadSound;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        nav = GetComponent<NavMeshAgent>();
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
            //rigid.MovePosition(transform.position + transform.forward * applySpeed * Time.deltaTime);
            nav.SetDestination(transform.position + destination * 5f);
        }
    }
    protected virtual void Reset()
    {
        isWalking = false;
        isRunning = false;
        isAction = true;

        nav.speed = walkSpeed;
        nav.ResetPath();

        animator.SetBool("Walking", isWalking);
        animator.SetBool("Running", isRunning);
        destination.Set(Random.Range(-0.2f, 0.2f), 0, Random.Range(0.5f, 1f));
    }

    
    protected void TryWalk()
    {
        isWalking = true;
        animator.SetBool("Walking", isWalking);
        currentTime = walkTime;
        nav.speed = walkSpeed;
        Debug.Log("°È±â");
    }
    

    public virtual void Damaged(int damage, Vector3 targetPos)
    {
        if (isDead)
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
    }

    protected void Dead()
    {
        PlaySE(deadSound);
        isWalking = false;
        isRunning = false;
        isDead = true;
        animator.SetTrigger("Dead");
    }

    protected void PlaySE(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
    protected void RandomSound()
    {
        int randdomIndex = Random.Range(0, 3);
        PlaySE(normalSounds[randdomIndex]);
    }
}
