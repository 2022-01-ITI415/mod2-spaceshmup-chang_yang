using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public GameObject hero;
    public float easing = 0.05f;
    public float skillTime;
    public float acc = 0.1f;

    public delegate void WeaponFireDelegate();
    public WeaponFireDelegate fireDelegate;

    public float currentTime = 0;

    private BoundsCheck bndCheck;
    // Start is called before the first frame update
    void Start()
    {
        bndCheck = GetComponent<BoundsCheck>();
        hero = GameObject.Find("_Hero");
        skillTime = hero.GetComponent<Hero>().skillTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentTime += Time.fixedDeltaTime;
        if (currentTime <= skillTime)
        {
            Vector3 pos = hero.transform.position;

            pos = Vector3.Lerp(transform.position, pos, easing);
            transform.position = pos;

            if (Input.GetAxis("Jump") == 1 && fireDelegate != null)
            {
                fireDelegate();
            }
        }
        else
        {
            acc += 0.05f;
            Vector3 pos = transform.position;
            pos.y += acc;
            transform.position = pos;
        }
        if (bndCheck.offUp)
        {
            hero.GetComponent<Hero>().skillOn = false;
            Destroy(gameObject);           
        }
    }
}
