using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using View;

public class FindBookResultModal : Modal
{
    [SerializeField]
    private Text titleText;

    [SerializeField]
    private Button bookButton;

    [SerializeField]
    private Button backButton;

    public void BookResult(int index, string text)
    {
        backButton.onClick.AddListener(() => { Modals.instance.OpenModal<FindBookClassifyModal>(); });
        titleText.text = text;

        bookButton.onClick.AddListener(() =>
        {
            var modal = Modals.instance.OpenModal<BookInfoModal>();
            // TODO: read ro not?
            modal.BookInfo();
        });
    }

    public void MoodResult(int index, string text)
    {
        backButton.onClick.AddListener(() => { Modals.instance.OpenModal<FindBookModal>(); });
        titleText.text = text;

        bookButton.onClick.AddListener(() =>
        {
            var modal = Modals.instance.OpenModal<BookInfoModal>();
            // TODO: read ro not?
            modal.BookInfo();
        });
    }
}
