using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSkinDuringSpawn : MonoBehaviour
{
    public List<SkinnedMeshRenderer> meshRenderers = new List<SkinnedMeshRenderer>();

    public List<Material> skins = new List<Material>();

    void Awake()
    {
        //GetComponent<Animator>().enabled = false;
        int randSkin = Random.Range(0,skins.Count);


        foreach(SkinnedMeshRenderer meshRenderer in meshRenderers)
        {
            // Material[] mats = meshRenderer.materials;
            // mats[0] = skins[randSkin];
            meshRenderer.material = skins[randSkin];
        }
        //GetComponent<Animator>().enabled = true;
    }
}
