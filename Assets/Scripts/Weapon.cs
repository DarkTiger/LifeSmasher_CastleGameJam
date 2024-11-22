using UnityEngine;

public enum WeaponType { Melee, Ranged, Spread, RangedReturn, Area }
public enum WeaponItem { Base, Mace, Staff, Flower, Hat }

public class Weapon : MonoBehaviour
{
    [SerializeField] WeaponItem weaponItem = WeaponItem.Base;
    [SerializeField] WeaponType weaponType = WeaponType.Area;
    [SerializeField] bool continuousAttack = true;
    [SerializeField] float damage = 15f;
    [SerializeField] float damageLv2 = 15f;
    [SerializeField] float damageLv3 = 15f;
    [SerializeField] float attackRate = 2f;
    [SerializeField] float attackRateLv2 = 2f;
    [SerializeField] float attackRateLv3 = 2f;
    [SerializeField] Vector2 shakeForce = new Vector2(0.05f, 0.025f);
    [SerializeField] GameObject hitVfxPrefab = null;
    [SerializeField] GameObject[] bulletPrefabs = null;
    [SerializeField] float bulletSpeed = 5f;
    [SerializeField] float bulletSpeedLv2 = 5f;
    [SerializeField] float bulletSpeedLv3 = 5f;
    [SerializeField] float bulletLifetime = 5f;
    [SerializeField] float bulletLifetimeLv2 = 5f;
    [SerializeField] float bulletLifetimeLv3 = 5f;
    [SerializeField] Sprite sprite = null;
    [SerializeField] string archetype = null;

    public WeaponItem WeaponItem => weaponItem;
    public WeaponType WeaponType => weaponType;
    public Sprite Sprite => sprite;
    public string Archetype => archetype;
    public float AttackRate => attackRate;
    public bool ContinuousAttack => continuousAttack;

    Transform cameraTransform;
    CameraShake[] cameraShakes;


    private void Awake()
    {
        cameraTransform = Camera.main.transform; 
        cameraShakes = FindObjectsByType<CameraShake>(FindObjectsSortMode.None);
    }

    //CALLED BY ANIMATION
    public void Attack()
    {
        if (weaponType == WeaponType.Area)
        {
            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, 2f, LayerMask.GetMask("Enemy"), QueryTriggerInteraction.Ignore))
            {
                PlayShakes();
                Destroy(Instantiate(hitVfxPrefab, hit.point, Quaternion.identity), 5f);
                hit.collider.GetComponent<Enemy>().SetDamage(damage, 0f);
            }
        }
        else if (weaponType == WeaponType.Melee)
        {
            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, 1f, LayerMask.GetMask("Enemy"), QueryTriggerInteraction.Ignore))
            {
                PlayShakes();
                Destroy(Instantiate(hitVfxPrefab, hit.point, Quaternion.identity), 5f);

                float finalDamage = damage;
                switch (GameManager.Instance.RebelLevel)
                {
                    case 2: finalDamage = damageLv2; break;
                    case 3: finalDamage = damageLv3; break;
                }

                hit.collider.GetComponent<Enemy>().SetDamage(finalDamage, 0f);

            }
        }
        else if (weaponType == WeaponType.Ranged)
        {
            PlayShakes();
            Bullet bullet = Instantiate(bulletPrefabs[Random.Range(0,bulletPrefabs.Length)], cameraTransform.position + cameraTransform.forward * 0.5f, Quaternion.identity).GetComponent<Bullet>();

            float finalDamage = damage;
            float finalBulletSpeed = bulletSpeed;
            float finalBulletLifetime = bulletLifetime;
            switch (GameManager.Instance.RebelLevel)
            {
                case 2: finalDamage = damageLv2; break;
                case 3: finalDamage = damageLv3; break;
            }

            bullet.SetBulletStats(finalDamage, finalBulletSpeed, finalBulletLifetime, hitVfxPrefab);
        }
        else if (weaponType == WeaponType.Spread)
        {
            PlayShakes();
            for (int i = 0; i < bulletPrefabs.Length; i++)
            {
                Bullet bullet = Instantiate(bulletPrefabs[Random.Range(0, bulletPrefabs.Length)], cameraTransform.position + cameraTransform.forward * 0.5f, Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360))).GetComponent<Bullet>();

                float finalDamage = damage;
                float finalBulletSpeed = bulletSpeed;
                float finalBulletLifetime = bulletLifetime;
                switch (GameManager.Instance.RebelLevel)
                {
                    case 2: finalDamage = damageLv2; break;
                    case 3: finalDamage = damageLv3; break;
                }

                bullet.SetBulletStats(finalDamage, finalBulletSpeed, finalBulletLifetime, hitVfxPrefab, true);
            } 
        }
        else if (weaponType == WeaponType.RangedReturn)
        {
            PlayShakes();
            Bullet bullet = Instantiate(bulletPrefabs[Random.Range(0, bulletPrefabs.Length)], cameraTransform.position + cameraTransform.forward * 0.5f, Quaternion.identity).GetComponent<Bullet>();

            float finalDamage = damage;
            float finalBulletSpeed = bulletSpeed;
            float finalBulletLifetime = bulletLifetime;
            switch (GameManager.Instance.RebelLevel)
            {
                case 2: finalDamage = damageLv2; break;
                case 3: finalDamage = damageLv3; break;
            }

            bullet.SetBulletStats(finalDamage, finalBulletSpeed, finalBulletLifetime, hitVfxPrefab, false, true);
        }
    }

    void PlayShakes()
    {
        for (int i = 0; i < cameraShakes.Length; i++)
        {
            cameraShakes[i].Shake(shakeForce.x, shakeForce.y, 0f);
        }
    }
}
