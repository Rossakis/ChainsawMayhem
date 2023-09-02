using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// A class that enables a static image background to appear constantly moving in a direction (useful for main menus with repeating backgrounds for example)
/// </summary>
public class BackgroundScroller : MonoBehaviour
{
    //MSets the speed at which the backround gets scrolled, but also the direction. For example, -1 will make it go move fast to the left, while 1 will make it do the same to the right.
    //Setting it to 0.5 will make it move at medium speed to the right.
    [Range(-1f, 1f)] public float scrollSpeed = 0.5f;
    private float offset;
    private Material material;
    
    void Start()
    {
        material = GetComponent<Renderer>().material;
    }

    void Update()
    {
        offset += (Time.deltaTime * scrollSpeed) / 10f;
        //The SetTextureOffset() is used to 'offset' or move a texture by a certain vector value.
        material.SetTextureOffset("_MainTex", new Vector2(offset, 0)); //Every material has a "_MainTex" attribute.
    }
}
