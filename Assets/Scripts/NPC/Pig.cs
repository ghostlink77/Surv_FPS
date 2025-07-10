using UnityEngine;

public class Pig : MonoBehaviour
{
    [SerializeField] private string animalName;
    [SerializeField] private int hp;
    [SerializeField] private float walkSpeed;
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private BoxCollider collider;
    [SerializeField] private float walkTime;
    [SerializeField] private float waitTime;

    private float currentTime;
    private Vector3 direction;

    private bool isAction;
    private bool isWalking = false;

    void Start()
    {
        currentTime = waitTime;
        isAction = true;
    }

    void Update()
    {
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
        if (isWalking)
        {
            rigid.MovePosition(transform.position + transform.forward * walkSpeed * Time.deltaTime);
        }
    }
    private void Rotation()
    {
        if (isWalking)
        {
            Vector3 rotation_Lerp = Vector3.Lerp(transform.eulerAngles, direction, 0.01f);
            rigid.MoveRotation(Quaternion.Euler(rotation_Lerp));
        }
    }
    private void Reset()
    {
        isWalking = false;
        isAction = true;
        animator.SetBool("Walking", isWalking);
        direction.Set(0f, Random.Range(0f, 360f), 0f);
        RandomAction();
    }

    private void RandomAction()
    {
        isAction = true;

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
        Debug.Log("°È±â");
    }
}
