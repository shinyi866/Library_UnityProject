using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using View;

public class MineModal : Modal
{
    [SerializeField]
    private Text petName;

    [SerializeField]
    private Image petImage;

    [SerializeField]
    private Button changeButton;

    [SerializeField]
    private Transform buttonTransform;

    [SerializeField]
    private Text scoreTxt;

    [SerializeField]
    private Slider slider;

    public bool reset;

    private Button leftButton;
    private Button rightButton;
    private AllItemObj itemData;
    private int targetScore;
    private int score;
    private int level;
    private int mood;
    private int currentPetInt;

    private void Awake()
    {
        mood = 1; //TODO: set mood
        score = PlayerPrefs.GetInt("score");
        level = PlayerPrefs.GetInt("level");
        targetScore = PlayerPrefs.GetInt("targetScore");
        currentPetInt = PlayerPrefs.GetInt("currentPet");
        itemData = MainApp.Instance.itemData;

        float value = (float)score / targetScore; ;
        scoreTxt.text = score.ToString();
        slider.value = value;

        changeButton.onClick.AddListener(() => {
            Modals.instance.OpenModal<PetClassifyModal>();
        });
    }

    private void Start()
    {

        if (reset)
            SetDefault();

        LoadPet(itemData.currentPet);
        CreateButtons(StringAsset.Mine.achievement, StringAsset.Mine.study);

        rightButton.onClick.AddListener(() => {
            Modals.instance.OpenModal<MyStudyModal>();
        });

        leftButton.onClick.AddListener(() => {
            Modals.instance.OpenModal<MyGoalModal>();
        });
    }

    public void LoadPet(AllItemObj.PetsItem data)
    {
        petName.text = data.name;
        petImage.sprite = data.image; //TODO 
    }

    private void CreateButtons(string leftString, string rightString)
    {
        leftButton = ButtonGenerate.Instance.SetModalButton(leftString, TypeFlag.UIColorType.Green, buttonTransform);
        rightButton = ButtonGenerate.Instance.SetModalButton(rightString, TypeFlag.UIColorType.Orange, buttonTransform);
    }

    public void SetLevelDefault()
    {
        if(!PlayerPrefs.HasKey("score"))
        {
            PlayerPrefs.SetInt("score", 0);
            score = PlayerPrefs.GetInt("score");
        }
        
        if (!PlayerPrefs.HasKey("level"))
        {
            PlayerPrefs.SetInt("level", 0);
            level = PlayerPrefs.GetInt("level");
        }
        

        if (!PlayerPrefs.HasKey("targetScore"))
        {
            PlayerPrefs.SetInt("targetScore", 100);
            targetScore = PlayerPrefs.GetInt("targetScore");
        }
            
        if(!PlayerPrefs.HasKey("currentPet"))
        {
            PlayerPrefs.SetInt("currentPet", 5);
            currentPetInt = PlayerPrefs.GetInt("currentPet");
        }

        itemData.currentPet = itemData.petsItems[currentPetInt];
        itemData.currentPet.image = itemData.petsLevelItem[currentPetInt].level[level].mood[mood];
    }

    public void AddScore(int addScore)
    {
        if (score >= 1500) return;
        if (level >= 5) return;

        score += addScore;
        PlayerPrefs.SetInt("score", score);
        Debug.Log("=== score" + score);
        if (score >= targetScore)
            SetTargetScore();
    }

    private void SetTargetScore()
    {
        LeveUpView();

        level++;
        PlayerPrefs.SetInt("level", level);

        var num = level == 1 ? 1 : level / 2;
        var count = num + 1;
        var scoreRange = 50;

        targetScore += scoreRange * count;
        PlayerPrefs.SetInt("targetScore", targetScore);
        Debug.Log("=== num" + num);
        Debug.Log("=== count" + count);
        Debug.Log("=== targetScore" + targetScore);
        Modals.instance.ChangePet(itemData, PlayerPrefs.GetInt("currentPet"));
    }

    private void LeveUpView()
    {
        Debug.Log("level up!!!!");
        var view = Views.instance.OpenView<RemindView>();
        var str = string.Format(StringAsset.TakePicture.updateMessage, itemData.currentPet.name);
        view.ShowOriginRemindView(str);
        view.button.onClick.AddListener(view.DestoryView);
    }

    public void SetDefault()
    {
        PlayerPrefs.SetInt("score", 0);
        score = PlayerPrefs.GetInt("score");

        PlayerPrefs.SetInt("level", 0);
        level = PlayerPrefs.GetInt("level");

        PlayerPrefs.SetInt("targetScore", 100);
        targetScore = PlayerPrefs.GetInt("targetScore");

        PlayerPrefs.SetInt("currentPet", 5);
        currentPetInt = PlayerPrefs.GetInt("currentPet");

        itemData.currentPet = itemData.petsItems[currentPetInt];
        itemData.currentPet.image = itemData.petsLevelItem[currentPetInt].level[level].mood[mood];
    }
}

