using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using View;

public class MyStudyModal : Modal
{
    [SerializeField]
    private Button backButton;

    private void Awake()
    {
        backButton.onClick.AddListener(() => { Modals.instance.LastModal(); });
    }
}
