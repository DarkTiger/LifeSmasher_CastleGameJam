using UnityEngine;

public class Bullet : MonoBehaviour
{
    float lifeTime = 5f;
    float speed = 1f;
    float damage = 15f;
    float startedTime = 0f;
    Vector3 direction;
    GameObject hitVfxPrefab;
    bool hasReturn = false;
    bool returning = false;


    public void SetBulletStats(float damage, float speed, float lifeTime, GameObject hitVfxPrefab, bool isSpread = false, bool hasReturn = false)
    {
        this.damage = damage;
        this.speed = speed;
        this.lifeTime = lifeTime;
        this.hitVfxPrefab = hitVfxPrefab;
        this.hasReturn = hasReturn;

        startedTime = Time.time;

        if (!hasReturn)
        {
            Destroy(gameObject, lifeTime);
        }

        direction = Camera.main.transform.forward;

        if (isSpread)
        {
            direction += Camera.main.transform.right * Random.Range(-0.2f, 0.2f) + Camera.main.transform.up * Random.Range(-0.2f, 0.2f);
        }
    }

    void Update()
    {
        if (hasReturn && !returning && startedTime + (lifeTime / 2f) < Time.time)
        {
            returning = true;
        }

        if (returning)
        {
            if ((Camera.main.transform.position - transform.position).magnitude < 0.5f)
            {
                Destroy(gameObject);
            }

            direction = ((Camera.main.transform.position - (Vector3.up * 0.2f)) - transform.position).normalized;
        }

        transform.position += direction * speed * Time.deltaTime;
        transform.Rotate(Vector3.up * 1000f * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            Destroy(Instantiate(hitVfxPrefab, transform.position, Quaternion.identity), 5f);
            enemy.SetDamage(damage, 0f);
        }

        if ((!hasReturn || !enemy) && !other.GetComponent<Player>() && !other.GetComponent<Bullet>())
        {
            Destroy(gameObject);
        }
    }
}
