using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FrameImageManager : MonoBehaviour
{
    [SerializeField] private List<Sprite> _spritesList;
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private TMP_Dropdown _dropdown;
    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangedDropdownImage()
    {
        _spriteRenderer.sprite = _spritesList[_dropdown.value];
    }
}
