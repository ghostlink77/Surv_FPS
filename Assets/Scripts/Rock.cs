using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField] int hp;
    [SerializeField] int destroyTime;
    [SerializeField] int itemCount; // Number of items to drop
    [SerializeField] SphereCollider collider;
    [SerializeField] GameObject go_rock;
    [SerializeField] GameObject go_debris;
    [SerializeField] GameObject go_effectPrefab;
    [SerializeField] GameObject go_rockItemPrefab;      // item

    [SerializeField] string strikeSound;
    [SerializeField] string destroySound;

    public void Mining()
    {
        SoundManager.instance.PlaySE(strikeSound);
        var clone = Instantiate(go_effectPrefab, collider.bounds.center, Quaternion.identity);
        Destroy(clone, destroyTime);

        hp--;
        if (hp <= 0)
        {
            Destruction();
        }
    }

    private void Destruction()
    {
        SoundManager.instance.PlaySE(destroySound);

        collider.enabled = false;
        for (int i = 0; i < itemCount; i++)
        {
            Instantiate(go_rockItemPrefab, go_rock.transform.position, Quaternion.identity);
        }
        Destroy(go_rock);

        go_debris.SetActive(true);
        Destroy(go_debris, destroyTime);
    }
}
