using UnityEngine;

public class Pig : WeakAnimal
{
    protected override void Reset()
    {
        base.Reset();
        RandomAction();
    }

    protected virtual void RandomAction()
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
        Debug.Log("���");
    }
    private void Eat()
    {
        currentTime = waitTime;
        animator.SetTrigger("Eat");
        Debug.Log("�Ա�");
    }
    private void Peek()
    {
        currentTime = waitTime;
        animator.SetTrigger("Peek");
        Debug.Log("�θ���");
    }
}
