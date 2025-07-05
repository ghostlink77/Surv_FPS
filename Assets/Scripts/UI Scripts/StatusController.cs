using UnityEngine;
using UnityEngine.UI;

public class StatusController : MonoBehaviour
{
    [SerializeField] private int maxHp;
    private int currentHp;

    [SerializeField] private int maxSp;
    private int currentSp;
    [SerializeField] private int SpIncreaseSpeed;
    [SerializeField] private int SpRecoveryDelay;
    private int SpCurrentRecoveryTime;
    private bool SpUsed;

    [SerializeField] private int maxDp;
    private int currentDp;

    [SerializeField] private int maxHungry;
    private int currentHungry;
    [SerializeField] private int hungryDecreaseDelay;
    private int hungryCurrentDecreaseTime;

    [SerializeField] private int maxThirsty;
    private int currentThirsty;
    [SerializeField] private int thirstyDecreaseDelay;
    private int thirstyCurrentDecreaseTime;

    [SerializeField] private int maxSatisfy;
    private int currentSatisfy;

    enum StatusType
    {
        HP,
        DP,
        SP,
        Hungry,
        Thirsty,
        Satisfy
    }
    [SerializeField] private Image[] gaugeImages;
    

    void Start()
    {
        currentHp = maxHp;
        currentDp = maxDp;
        currentSp = maxSp;
        currentHungry = maxHungry;
        currentThirsty = maxThirsty;
        currentSatisfy = maxSatisfy;
    }
    void Update()
    {
        Hungry();
        Thirsty();
        GaugeUpdate();
        StaminaRecoverTime();
        StaminaRecover();
    }

    private void Hungry()
    {
        if (currentHungry > 0)
        {
            if (hungryCurrentDecreaseTime <= hungryDecreaseDelay)
            {
                hungryCurrentDecreaseTime++;
            }
            else
            {
                currentHungry--;
                hungryCurrentDecreaseTime = 0;
            }
        }
        else
        {
            Debug.Log("배고픔 수치가 0이 되었습니다.");
        }
    }
    private void Thirsty()
    {
        if (currentThirsty > 0)
        {
            if (thirstyCurrentDecreaseTime <= thirstyDecreaseDelay)
            {
                thirstyCurrentDecreaseTime++;
            }
            else
            {
                currentThirsty--;
                thirstyCurrentDecreaseTime = 0;
            }
        }
        else
        {
            Debug.Log("목마름 수치가 0이 되었습니다.");
        }
    }

    private void StaminaRecoverTime()
    {
        if (SpUsed)
        {
            if (SpCurrentRecoveryTime < SpRecoveryDelay)
            {
                SpCurrentRecoveryTime++;
            }
            else
            {
                SpUsed = false;
            }
        }
    }
    private void StaminaRecover()
    {
        if (!SpUsed && currentSp < maxSp)
        {
            currentSp += SpIncreaseSpeed;
        }
    }

    public void IncreaseHp(int count)
    {
        if (currentHp + count <= maxHp)
        {
            currentHp += count;
        }
        else
        {
            currentHp = maxHp;
        }
    }
    public void IncreaseSp(int count)
    {
        if (currentSp + count <= maxSp)
        {
            currentSp += count;
        }
        else
        {
            currentSp = maxSp;
        }
    }
    public void IncreaseDp(int count)
    {
        if (currentDp + count <= maxDp)
        {
            currentDp += count;
        }
        else
        {
            currentDp = maxDp;
        }
    }
    public void IncreaseHungry(int count)
    {
        if (currentHungry + count <= maxHungry)
        {
            currentHungry += count;
        }
        else
        {
            currentHungry = maxHungry;
        }
    }
    public void IncreaseThirsty(int count)
    {
        if (currentThirsty + count <= maxThirsty)
        {
            currentThirsty += count;
        }
        else
        {
            currentThirsty = maxThirsty;
        }
    }
    public void DecreaseHp(int count)
    {
        if (currentDp > 0)
        {
            DecreaseDp(count);
            return;
        }
        currentHp -= count;
        if(currentHp < 0)
        {
            currentHp = 0;
            Debug.Log("체력이 0이 되었습니다.");
        }
    }
    public void DecreaseDp(int count)
    {
        currentDp -= count;
        if(currentDp < 0)
        {
            currentDp = 0;
            Debug.Log("방어력이 0이 되었습니다.");
        }
    }
    public void DecreaseHungry(int count)
    {
        currentHungry -= count;
        if(currentHungry < 0)
        {
            currentHungry = 0;
        }
    }
    public void DecreaseThirsty(int count)
    {
        currentThirsty -= count;
        if(currentThirsty < 0)
        {
            currentThirsty = 0;
        }
    }
    public void DecreaseStamina(int count)
    {
        SpUsed = true;
        SpCurrentRecoveryTime = 0;

        if (currentSp - count > 0)
        {
            currentSp -= count;
        }
        else
        {

            currentSp = 0;
        }
    }

    private void GaugeUpdate()
    {
        gaugeImages[(int)StatusType.HP].fillAmount = (float)currentHp / maxHp;
        gaugeImages[(int)StatusType.SP].fillAmount = (float)currentSp / maxSp;
        gaugeImages[(int)StatusType.DP].fillAmount = (float)currentDp / maxDp;
        gaugeImages[(int)StatusType.Hungry].fillAmount = (float)currentHungry / maxHungry;
        gaugeImages[(int)StatusType.Thirsty].fillAmount = (float)currentThirsty / maxThirsty;
        gaugeImages[(int)StatusType.Satisfy].fillAmount = (float)currentSatisfy / maxSatisfy;
    }

    public int GetCurrentSp()
    {
        return currentSp;
    }
}
