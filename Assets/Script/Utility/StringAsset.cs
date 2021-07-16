public class StringAsset
{
   public class BookRemind
    {
        public const string successToStudy = "這本書已經收進書房了。";
        public const string failToStudy = "哎啊，書房裡有太多還沒看的書了，先去書房看書吧！";
        public const string mood = "你選擇了「{0}」的心情，獲得50宇宙能量。";
        public const string classifySuggest = "你選擇了{0}類中的「{1}」。";
        public const string receiveSuggest = "我們已收到你的建議。";
        public const string moodView = "你看完的心情";
        public const string classifyView = "你覺得哪個分類不適合這本書";
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

    public class BookInfo
    {
        //title
        public const string info = "書本資訊";
        public const string notRead = "未閱讀的書本";
        public const string read = "已閱讀的書本";

        // button
        public const string saveStudy = "收進書房";
        public const string findBook = "找這本書";
        public const string finish = "我讀完了";
        public const string change = "更改分類";
    }

    public class Mine
    {
        public const string achievement = "我的成就";
        public const string study = "我的書房";
    }

    public class Goal
    {
        public const string books = "總共閱讀的書本數";
        public const string happy = "令人「快樂」的書";
        public const string scared = "覺得「可怕」的書";
        public const string wow = "感到「驚奇」的書";
        public const string angry = "令人「生氣」的書";
        public const string hate = "覺得「討厭」的書";
        public const string sad = "感到「悲傷」的書";
        public static string[] goalArray = new string[] { books, happy, scared, wow, angry, hate, sad };
    }

    public class TakePicture
    {
        public const string again = "重新拍攝";
        public const string upload = "上傳照片";

        public const string takePicture = "這本書還沒有封面，你要幫忙拍一張書封的照片嗎？";
        public const string receiveBook = "我們已經收到你拍攝的書封，獲得10宇宙能量。";
        public const string updateMessage = "恭喜，{0}的等級上升囉。";
    }

    public class Domain
    {
        public const string LocalHost = "http://localhost:3000/";
        public const string Host = "https://book2021.azurewebsites.net/books/";
    }

   public class API
    {
        public const string GetBookInfo = "books";
        public const string GetClassify = "classify/{0}";
        public const string Recommend = "recommend";
        public const string MostView = "most_view";
        public const string Search = "search";
    }

    public static string GetFullAPIUrl(string apiUrl)
    {
        return Domain.Host + apiUrl;
    }

    public static string GetSearchAPIUrl(string searchTxt)
    {
        string searchUrl = string.Format("{0}?q={1}", API.Search, searchTxt);
        return Domain.Host + searchUrl;
    }
}
