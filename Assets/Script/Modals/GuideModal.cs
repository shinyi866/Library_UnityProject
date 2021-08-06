using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using View;

public class GuideModal : Modal
{
    [SerializeField]
    private GameObject BarObject;
    
    private GuideItemObj.GuideItem data;
    private Image guideImage;
    private Button nextButton;
    private int count = 0;

    // camera
    private bool camAvailble;
    private WebCamTexture backCamera;
    private Texture defaultBackground;

    public RawImage background;
    public AspectRatioFitter fit;

    private void Awake()
    {
        guideImage = this.transform.GetChild(0).gameObject.GetComponent<Image>();
        nextButton = this.transform.GetChild(1).gameObject.GetComponent<Button>();
    }

    private void OpenCamera()
    {
        background.enabled = true;

        defaultBackground = background.texture;
        WebCamDevice[] devices = WebCamTexture.devices;

        if(devices.Length == 0)
        {
            Debug.Log("No Camera");
            camAvailble = false;
            return;
        }

        for(int i = 0; i < devices.Length; i++)
        {
            if(!devices[i].isFrontFacing)
            {
                backCamera = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
            }
        }

        if(backCamera == null)
        {
            Debug.Log("Unable to find back camera");
            return;
        }

        backCamera.Play();
        background.texture = backCamera;
        camAvailble = true;
    }

    private void Update()
    {
        if (!camAvailble)
            return;

        float ratio = (float)backCamera.width / (float)backCamera.height;
        fit.aspectRatio = ratio;

        float scaleY = backCamera.videoVerticallyMirrored ? -1f : 1f;
        background.rectTransform.localScale = new Vector3(1, scaleY, 1);

        int orient = -backCamera.videoRotationAngle;
        background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
    }

    public void ShowView(TypeFlag.GuideType type)
    {
        data = MainApp.Instance.guideData.guideItems[(int)type];
        guideImage.sprite = data.image1;

        nextButton.onClick.AddListener(()=> {
            count++;
            ButtonClick(type);
        });
    }

    private void ButtonClick(TypeFlag.GuideType type)
    {
        //var data = MainApp.Instance.guideData.guideItems[(int)type];
        //guideImage.sprite = data.image1;

        if (count == 1)
            guideImage.sprite = data.image2;

        if (count == 2)
            guideImage.sprite = data.image3;

        if (count == 3)
        {
            switch (type)
            {
                case TypeFlag.GuideType.main:
                    Modals.instance.OpenModal<MainModal>();
                    break;
                case TypeFlag.GuideType.ARfindBook:
                    //Modals.instance.CloseAllModal();
                    BarObject.SetActive(false);
                    OpenCamera();
                    // TODO: open AR camera
                    break;
                case TypeFlag.GuideType.classify:
                    Modals.instance.OpenModal<BookClassifyModal>();
                    break;
            }
        }
    }
}
