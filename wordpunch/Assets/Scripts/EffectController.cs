using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class effectController : MonoBehaviour
{
    public MeshRenderer mesh;
    // public VisualEffect vfx;
    public float dissolveRate = 0.0125f;
    public float refreshRate = 0.025f;
    private Material mat;
    // Start is called before the first frame update
    void Start()
    {
        if (mesh != null)
        {
            mat = mesh.material;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if press space, start dissolve
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(DissolveCo());
        }
    }

    IEnumerator DissolveCo()
    {
        // if (vfx != null)
        // {
        //     vfx.Play();
        // }
        float counter = 1;
        while (mat.GetFloat("_DissolveAmount") > 0)
        {
            counter -= dissolveRate;
            mat.SetFloat("_DissolveAmount", counter);
            yield return new WaitForSeconds(refreshRate);
        }
    }
}
