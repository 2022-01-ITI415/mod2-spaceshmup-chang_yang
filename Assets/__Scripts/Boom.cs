using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom : MonoBehaviour
{
    public Rigidbody rigid;

    private BoundsCheck bndCheck;
    // Start is called before the first frame update
    private void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (bndCheck.offDown)
        {
            Destroy(gameObject);
        }
    }
}
