using UnityEngine;

public class StringAsset
{
   public class BookRemind
    {
        public const string successToStudy = "這本書已經收進書房了。";
        public const string failToStudy = "哎啊，書房裡有太多還沒看的書了，先去書房看書吧！";
        public const string mood = "你選擇了「{0}」的心情，獲得50宇宙能量。";
        public const string receiveSuggest = "我們已收到你的建議。";
        public const string takePicture = "這本書還沒有封面，你要幫忙拍一張書封的照片嗎？";
        public const string receiveBook = "我們已經收到你拍攝的書封，獲得10宇宙能量。";
        public const string updateMessage = "恭喜，{0}的等級上升囉。";
    }

    public class RemindButton
    {
        public const string nextTime = "下次再說";
        public const string confirm = "確定";
        public const string success = "好的";
        public const string back = "回上一頁";
        public const string takePicture = "去拍照";
        public const string goToStudy = "去書房";
        public const string next = "下一步";
        public const string start = "開始";
    }

    public class Main
    {
        public const string recommend = "為你推薦";
        public const string top = "最多人看";
    }
}
