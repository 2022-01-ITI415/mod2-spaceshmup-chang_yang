using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hero : MonoBehaviour {
    static public Hero S; // Singleton
    
    [Header("Set in Inspector")]
    // These fields control the movement of the ship
    public float speed = 30;
    public float rollMult = -45;
    public float pitchMult = 30;
    public float gameRestartDelay = 2f;
    public GameObject projectilePrefab;
    public float projectileSpeed = 40;
    public Weapon[] weapons;
    [Header("Set Dynamically")]
    [SerializeField]
    public float _shieldLevel = 1;
    [Header("Set Skill")]
    public GameObject skillPrefab;
    public float skillTime = 5;
    
    private bool invincible = false;
    public bool skillOn = false;

    // This variable holds a reference to the last triggering GameObject
    private GameObject lastTriggerGo = null;

    // Declare a new delegate type WeaponFireDelegate
    public delegate void WeaponFireDelegate();
    // Create a WeaponFireDelegate field named fireDelegate.
    public WeaponFireDelegate fireDelegate;

    public float time = 0;

    public Color[] originalColors;
    public Material[] materials;

    void Start()
    {
        if (S == null)
        {
            S = this; // Set the Singleton
        }
        else
        {
            Debug.LogError("Hero.Awake() - Attempted to assign second Hero.S!");
        }
        //fireDelegate += TempFire;

        // Reset the weapons to start _Hero with 1 blaster
        ClearWeapons();
        weapons[0].SetType(WeaponType.blaster);

        materials = Utils.GetAllMaterials(gameObject);
        originalColors = new Color[materials.Length];
        for (int i = 0; i < materials.Length; i++)
        {
            originalColors[i] = materials[i].color;
        }
    }
	
	// Update is called once per frame
	void Update()
    {
        // Pull in information from the Input class
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        // Change transform.position based on the axes
        Vector3 pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;
        transform.position = pos;

        // Rotate the ship to make it feel more dynamic
        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);

        // Use the fireDelegate to fire Weapons
        // First, make sure the button is pressed: Axis("Jump")
        // Then ensure that fireDelegate isn't null to avoid an error
        if (Input.GetAxis("Jump") == 1 && fireDelegate != null)
        {
            fireDelegate();
        }
        
        if (Input.GetKeyDown(KeyCode.E) && Main.S.skill > 0 && skillOn == false)
        {
            UseSkill();
            skillOn = true;
            invincible = true;
            IsInvincible();
            time = 10 - skillTime;
            Main.S.skill -= 1;
            int wingIndex = Main.S.wingList.Count - 1;
            GameObject wingGO = Main.S.wingList[wingIndex];
            Main.S.wingList.RemoveAt(wingIndex);
            Destroy(wingGO);
        }

        if (invincible == true)
        {
            time += Time.deltaTime;
            if (time >= 10)
            {
                NotInvincible();
                time = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;
        print("Triggered: " + go.name);

        // Make sure it's not the same triggering go as last time
        if (go.tag != "Laser" )
        {
            if (go == lastTriggerGo)
            {
                return;
            }
        }
        lastTriggerGo = go;

        if(go.tag == "Enemy" || go.tag == "ProjectileEnemy")
        {
            if (invincible == true)
            {
                Main.S.GetScore();
                Destroy(go);
            }
            else
            {
                shieldLevel--;
                Destroy(go);
            }
        }
        else if (go.tag == "Boss" || go.tag == "Invincible")
        {
            if (invincible == false)
            {
                shieldLevel -= 5;
            }
        }
        else if (go.tag == "Laser")
        {
            if (invincible == false)
            {
                IsInvincible();
                time = 8;
                shieldLevel--;
            }
        }
        else if (go.tag == "PowerUp")
        {
            // If the shield was triggered by a PowerUp
            AbsorbPowerUp(go);
        }
        else
        {
            print("Triggered by non-Enemy: " + go.name);
        }
    }

    public void AbsorbPowerUp(GameObject go)
    {
        PowerUp pu = go.GetComponent<PowerUp>();
        switch (pu.type)
        {
            case WeaponType.shield:
                if (_shieldLevel == 4)
                {          
                    IsInvincible();
                    time = 0;
                }
                shieldLevel++;
                break;

            default:
                if(pu.type == weapons[0].type)
                {
                    Weapon w = GetEmptyWeaponSlot();
                    if(w != null)
                    {
                        // Set it to pu.type
                        w.SetType(pu.type);
                    }
                }
                else
                {
                    //If this is a different weapon type
                    ClearWeapons();
                    weapons[0].SetType(pu.type);
                }
                break;
        }
        pu.AbsorbedBy(gameObject);
    }

    public float shieldLevel
    {
        get
        {
            return (_shieldLevel);
        }
        set
        {
            _shieldLevel = Mathf.Min(value, 4);
            // If the shield is going to be set to less than zero
            if (value < 0)
            {
                Destroy(this.gameObject);
                // Tell Main.S to restart the game after a delay
                Main.S.DelayedRestart(gameRestartDelay);
            }
        }
    }

    Weapon GetEmptyWeaponSlot()
    {
        for (int i=0; i<weapons.Length; i++)
        {
            if (weapons[i].type == WeaponType.none)
            {
                return (weapons[i]);
            }
        }
        return (null);
    }

    void ClearWeapons()
    {
        foreach (Weapon w in weapons)
        {
            w.SetType(WeaponType.none);
        }
    }

    public void IsInvincible()
    {
        invincible = true;
        foreach (Material m in materials)
        {
            m.color = Color.yellow;
        }
    }

    void NotInvincible()
    {
        invincible = false;
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].color = originalColors[i];
        }
    }

    void UseSkill()
    {
        GameObject go = Instantiate<GameObject>(skillPrefab);

        Vector3 pos = transform.position;
        pos.y = pos.y -80;
        go.transform.position = pos;
    }
}
