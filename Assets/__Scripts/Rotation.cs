using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    public bool lasReady = false;

    public Color[] originalColors;
    public Material[] materials;
    // Start is called before the first frame update
    void Start()
    {
        materials = Utils.GetAllMaterials(gameObject);
        originalColors = new Color[materials.Length];
        for (int i = 0; i < materials.Length; i++)
        {
            originalColors[i] = materials[i].color;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (lasReady == true)
        {
            transform.Rotate(new Vector3(0, 180, 0) * Time.fixedDeltaTime);
        }
        else
        {
            transform.Rotate(new Vector3(0, 45, 0) * Time.fixedDeltaTime);
        }
    }

    public void LasPre()
    {
        lasReady = true;
        foreach (Material m in materials)
        {
            m.color = Color.yellow;
        }
    }

    public void LasOff()
    {
        lasReady = false;
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].color = originalColors[i];
        }
    }
}
