using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
public class CSVImport : AssetPostprocessor
{
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string str in importedAssets)
        {
            if (str.IndexOf("/GuideData.csv") != -1)
            {
                TextAsset data = AssetDatabase.LoadAssetAtPath<TextAsset>(str);
                string assetfile = str.Replace(".csv", ".asset");
                GuideItemObj guideItemObj = AssetDatabase.LoadAssetAtPath<GuideItemObj>(assetfile);
                if (guideItemObj == null)
                {
                    guideItemObj = new GuideItemObj();
                    AssetDatabase.CreateAsset(guideItemObj, assetfile);
                }

                guideItemObj.guideItems = CSVSerializer.Deserialize<GuideItemObj.GuideItem>(data.text);

                EditorUtility.SetDirty(guideItemObj);
                AssetDatabase.SaveAssets();
#if DEBUG_LOG || UNITY_EDITOR
                Debug.Log("Reimported Asset: " + str);
#endif
            }
            
            if (str.IndexOf("/BookItemsData.csv") != -1)
            {
                TextAsset data = AssetDatabase.LoadAssetAtPath<TextAsset>(str);
                AllItemObj allItemObj = Resources.Load("AllItemsData", typeof(AllItemObj)) as AllItemObj;
                allItemObj.booksItems = CSVSerializer.Deserialize<AllItemObj.BookItem>(data.text);

                EditorUtility.SetDirty(allItemObj);
                AssetDatabase.SaveAssets();
                
#if DEBUG_LOG || UNITY_EDITOR
                Debug.Log("Reimported Asset: " + str);
#endif
            }

            if (str.IndexOf("/MoodItemsData.csv") != -1)
            {
                TextAsset data = AssetDatabase.LoadAssetAtPath<TextAsset>(str);
                AllItemObj allItemObj = Resources.Load("AllItemsData", typeof(AllItemObj)) as AllItemObj;
                allItemObj.moodItems = CSVSerializer.Deserialize<AllItemObj.MoodItem>(data.text);

                EditorUtility.SetDirty(allItemObj);
                AssetDatabase.SaveAssets();

#if DEBUG_LOG || UNITY_EDITOR
                Debug.Log("Reimported Asset: " + str);
#endif
            }

            if (str.IndexOf("/PetsItemsData.csv") != -1)
            {
                TextAsset data = AssetDatabase.LoadAssetAtPath<TextAsset>(str);
                AllItemObj allItemObj = Resources.Load("AllItemsData", typeof(AllItemObj)) as AllItemObj;
                allItemObj.petsItems = CSVSerializer.Deserialize<AllItemObj.PetsItem>(data.text);

                EditorUtility.SetDirty(allItemObj);
                AssetDatabase.SaveAssets();

#if DEBUG_LOG || UNITY_EDITOR
                Debug.Log("Reimported Asset: " + str);
#endif
            }

            if (str.IndexOf("/BookTitleItemData.csv") != -1)
            {
                TextAsset data = AssetDatabase.LoadAssetAtPath<TextAsset>(str);
                AllItemObj allItemObj = Resources.Load("AllItemsData", typeof(AllItemObj)) as AllItemObj;
                allItemObj.booksTitleItems = CSVSerializer.Deserialize<AllItemObj.BookTitleItem>(data.text);

                EditorUtility.SetDirty(allItemObj);
                AssetDatabase.SaveAssets();

#if DEBUG_LOG || UNITY_EDITOR
                Debug.Log("Reimported Asset: " + str);
#endif
            }
        }
    }
}
#endif

