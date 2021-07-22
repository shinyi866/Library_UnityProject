using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using View;

public class MainBarController : MonoBehaviour
{
    private GameObject BarObject;

    private Button findButton;
    private Button friendButton;
    private Button homeButton;
    private Button missionButton;
    private Button mineButton;

    private void Awake()
    {
        BarObject = this.transform.GetChild(0).GetChild(0).gameObject;

        homeButton = BarObject.transform.GetChild(0).gameObject.GetComponent<Button>();
        friendButton = BarObject.transform.GetChild(1).gameObject.GetComponent<Button>();
        findButton = BarObject.transform.GetChild(2).gameObject.GetComponent<Button>();
        missionButton = BarObject.transform.GetChild(3).gameObject.GetComponent<Button>();
        mineButton = BarObject.transform.GetChild(4).gameObject.GetComponent<Button>();

        homeButton.onClick.AddListener(() => { Modals.instance.OpenModal<MainModal>(); Modals.instance.GetModel<FindBookResultModal>().isCurrentPage = false; });
        findButton.onClick.AddListener(() => { Modals.instance.OpenModal<FindBookModal>(); Modals.instance.GetModel<FindBookResultModal>().isCurrentPage = false; });
        mineButton.onClick.AddListener(() => { Modals.instance.OpenModal<MineModal>(); Modals.instance.GetModel<FindBookResultModal>().isCurrentPage = false; });
    }

    public void CloseBar(bool isClose)
    {
        if (isClose)
            this.gameObject.SetActive(!isClose);
        else
            this.gameObject.SetActive(isClose);
    }
}
