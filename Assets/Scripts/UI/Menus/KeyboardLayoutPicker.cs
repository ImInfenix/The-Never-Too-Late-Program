using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardLayoutPicker : MonoBehaviour
{
    public static List<KeyboardLayoutPicker> layoutPickers;

    [SerializeField]
    private KeyboardLayout attachedLayout;
    [SerializeField]
    private bool isDefault = false;

    private void Awake()
    {
        if (layoutPickers == null)
            layoutPickers = new List<KeyboardLayoutPicker>();
        layoutPickers.Add(this);

        if (isDefault)
            DefineAsLayout();
    }

    [SerializeField]
    private Image isSelectedImage;

    public void DefineAsLayout()
    {
        GameManager.Instance.keyboardLayout = attachedLayout;
        foreach (KeyboardLayoutPicker klp in layoutPickers)
            klp.isSelectedImage.gameObject.SetActive(false);
        isSelectedImage.gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        layoutPickers.Remove(this);
    }
}
