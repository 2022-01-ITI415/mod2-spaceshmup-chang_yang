using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    [Header("Set in Inspector: Boss")]
    public float leftAndRightEdge = 0.1f;
    public float chanceToChangeDirections = 0.1f;
    public float rollMult = -20;

    public float rotSpeed = 0.02f;

    public bool isready;
    public bool secStage = false;
    public float transformTime = 0;
    private Vector3 bossPos = new Vector3(0, 18, 0);
    private bool transformingOn = true;
    private bool started = false;

    // Start is called before the first frame update
    void Start()
    {
        isready = false;
    }

    // Update is called once per frame
    public override void Move()
    {
        if (isready == false && started == false)
        {
            pos = Vector3.Lerp(pos, bossPos, Time.deltaTime / 2);
            if (Vector3.Distance(bossPos, pos) < 1)
            {
                isready = true;
                started = true;
                this.tag = "Boss";
            }
        }
        else if (isready == true)
        {
            Vector3 pos_ = pos;
            pos_.x += speed * Time.deltaTime;
            pos = pos_;

            if (pos_.x < -leftAndRightEdge)
            {
                speed = Mathf.Abs(speed);
            }
            else if (pos_.x > leftAndRightEdge)
            {
                speed = -Mathf.Abs(speed);
            }
            Quaternion rot = Quaternion.Euler(0, (speed/Mathf.Abs(speed)) * rollMult, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Mathf.Clamp01(rotSpeed));
        }

    }
    private void FixedUpdate()
    {
        if (Random.value < chanceToChangeDirections)
        {
            speed *= -1;
        }

        if (health <= 200)
        {
            if (transformingOn == true)
            {
                transformTime += Time.fixedDeltaTime;
            }
            
            if (secStage == false)
            {
                speed = 25;
                secStage = true;
                isready = false;
                this.tag = "Invincible";
            }
            if (transformTime > 5)
            {
                isready = true;
                this.tag = "Boss";
                transformingOn = false;
            }
        }
    }
}
