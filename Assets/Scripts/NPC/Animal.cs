using UnityEngine;

public class Animal : MonoBehaviour
{
    [SerializeField] protected string animalName;
    [SerializeField] protected int hp;

    [SerializeField] protected float walkSpeed;
    [SerializeField] protected float runSpeed;
    [SerializeField] protected float turnSpeed;

    [SerializeField] protected Animator animator;
    [SerializeField] protected Rigidbody rigid;
    [SerializeField] protected BoxCollider collider;

    [SerializeField] protected float walkTime;
    [SerializeField] protected float runTime;
    [SerializeField] protected float waitTime;

    protected float applySpeed;
    protected float currentTime;
    protected Vector3 direction;

    protected bool isAction;
    protected bool isWalking = false;
    protected bool isRunning = false;
    protected bool isDead = false;

    protected AudioSource audioSource;
    [SerializeField] protected AudioClip[] normalSounds;
    [SerializeField] protected AudioClip hurtSound;
    [SerializeField] protected AudioClip deadSound;

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
    protected void Rotation()
    {
        Vector3 dir = new Vector3(0f, direction.y, 0f);
        if (isWalking || isRunning)
        {
            Vector3 rotation_Lerp = Vector3.Lerp(transform.eulerAngles, dir, turnSpeed);
            rigid.MoveRotation(Quaternion.Euler(rotation_Lerp));
        }
    }
    protected virtual void Reset()
    {
        isWalking = false;
        isRunning = false;
        isAction = true;

        applySpeed = walkSpeed;

        animator.SetBool("Walking", isWalking);
        animator.SetBool("Running", isRunning);
        direction.Set(0f, Random.Range(0f, 360f), 0f);
    }

    
    protected void TryWalk()
    {
        isWalking = true;
        animator.SetBool("Walking", isWalking);
        currentTime = walkTime;
        applySpeed = walkSpeed;
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
