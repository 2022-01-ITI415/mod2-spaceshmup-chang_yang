using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossWeaponType
{
    none, // The default / no weapons
    blaster, // A simple blaster
    spread, // Two shots simultaneously
    laser, // [NI] Damage over time
}

public class Boss_Weapon : MonoBehaviour
{
    static public Transform PROJECTILE_E_ANCHOR;
    public BossWeaponType type = BossWeaponType.none;
    public GameObject boom;
    public GameObject line;
    public float boomSpeed = -15;
    public float fireRate = 1f;
    public GameObject boss;
    public GameObject power;

    public float nextFire;
    private float lasTime;
    private bool isready;
    private bool transformed = false;
    private bool lasOn = false;
    private GameObject las;
    // Start is called before the first frame update
    void Start()
    {
        if (PROJECTILE_E_ANCHOR == null)
        {
            GameObject go = new GameObject("_ProjectileAnchor_E");
            PROJECTILE_E_ANCHOR = go.transform;
            PROJECTILE_E_ANCHOR.tag = "ProjectileEnemy";
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (boss.GetComponent<Boss>().secStage == true && transformed == false) 
        {
            fireRate = fireRate / 2;
            boomSpeed = boomSpeed * 1.5f;
            transformed = true;
            lasTime = 0;
            nextFire = 0;
            Destroy(las);
            lasOn = false;
        }
        isready = boss.GetComponent<Boss>().isready;
        if (isready == true )
        {
            nextFire = nextFire + Time.fixedDeltaTime;
            if (nextFire > 8)
            {
                power.GetComponent<Rotation>().LasPre();
            }
            if (nextFire > fireRate)
            {
                Fire();
                nextFire = 0;
            }
            if (lasOn == true)
            {
                lasTime = lasTime + Time.fixedDeltaTime;
                if (lasTime < 5)
                {
                    LaserFollow();
                }
                else
                {
                    Destroy(las);
                    lasOn = false;
                    nextFire = 0;
                    lasTime = 0;
                    power.GetComponent<Rotation>().LasOff();
                }
            }
        }
    }

    public void Fire()
    {

        Boom p;
        Vector3 vel = new Vector3(0, boomSpeed, 0);
        switch (type)
        {
            case BossWeaponType.blaster:
                p = MakeProjectile();
                p.rigid.velocity = vel;
                break;

            case BossWeaponType.spread:
                p = MakeProjectile(); // Make middle Projectile
                p.rigid.velocity = vel;
                p = MakeProjectile(); // Make right Projectile
                p.transform.rotation = Quaternion.AngleAxis(10, Vector3.back);
                p.rigid.velocity = p.transform.rotation * vel;
                p = MakeProjectile(); // Make left Projectile
                p.transform.rotation = Quaternion.AngleAxis(-10, Vector3.back);
                p.rigid.velocity = p.transform.rotation * vel;
                break;

            case BossWeaponType.laser:
                las = Instantiate<GameObject>(line);
                Vector3 weapen_pos = transform.position;
                weapen_pos.y = weapen_pos.y - 30;
                las.transform.position = weapen_pos;
                lasOn = true;
                break;

        }
    }

    public void LaserFollow()
    {
        Vector3 weapen_pos = transform.position;
        weapen_pos.y = weapen_pos.y - 30;
        las.transform.position = weapen_pos;
    }

    public Boom MakeProjectile()
    {
        GameObject go = Instantiate<GameObject>(boom);
        if (transform.parent.gameObject.tag == "Hero")
        {
            go.tag = "ProjectileHero";
            go.layer = LayerMask.NameToLayer("ProjectileHero");
        }
        else
        {
            go.tag = "ProjectileEnemy";
            go.layer = LayerMask.NameToLayer("ProjectileEnemy");
        }
        go.transform.position = transform.position;
        go.transform.SetParent(PROJECTILE_E_ANCHOR, true);
        Boom p = go.GetComponent<Boom>();
        return p;
    }
}
