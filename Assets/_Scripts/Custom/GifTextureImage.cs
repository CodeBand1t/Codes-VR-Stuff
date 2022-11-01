using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GifTextureImage : MonoBehaviour
{
    [Space(10)]
    [SerializeField] float fps;
    public List<Sprite> frames;

    Image _image;

    int index;

    void Awake()
    {
        _image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        index = (int)(Time.time * fps);
        index = index % frames.Count;
        // mat.mainTexture = frames[index];
        _image.sprite = frames[index];
    }
}