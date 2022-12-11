using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class CSVData : MonoBehaviour
{
    private static CSVData instance;

    public static CSVData Instance {get {return instance;}}
    public TextAsset textAssetData;

    [Serializable]
    public class WordObject {

        public enum Frequency {
            low = 1,
            medium = 2,
            high = 3
        }
        public string Word;
        public string Cat_1;
        public string Cat_2;
        public string Cat_3;
        public string Des_1;
        public string Des_2;
        public string Des_3;
        public Frequency freq;
        public string Concr_Abstr;
        public string Season;
        public string Anello;
        public bool HasImage;

    }

    [Serializable]
    public class WordList {
        public WordObject[] wordobject;

    }

    public WordList holderWordList = new WordList();

    public WordList ImagesWordList = new WordList();

    

    // Start is called before the first frame update
    void Start()
    {
        loadData();
        ImagesWordList.wordobject = FilteredWithImages();
        
        
    }

    private void Awake() {
		instance = this;
	}

    public void loadData(){
        string[] data = textAssetData.text.Split(new string[]{";","\n"}, StringSplitOptions.None);

        int tableSize = data.Length / 13 - 1;
        holderWordList.wordobject = new WordObject[tableSize];

        for (int i = 0; i < tableSize; i++)
        {
            holderWordList.wordobject[i] = new WordObject();
            holderWordList.wordobject[i].Word = data[13 * (i+1) + 1].ToUpper().Trim();
            holderWordList.wordobject[i].Cat_1 = data[13 * (i+1) + 2].ToUpper().Trim();
            holderWordList.wordobject[i].Cat_2 = data[13 * (i+1) + 3].ToUpper().Trim();
            holderWordList.wordobject[i].Cat_3 = data[13 * (i+1) + 4].ToUpper().Trim();
            holderWordList.wordobject[i].Des_1 = data[13 * (i+1) + 5];
            holderWordList.wordobject[i].Des_2 = data[13 * (i+1) + 6];
            holderWordList.wordobject[i].Des_3 = data[13 * (i+1) + 7];
            switch (data[13 * (i+1) + 8]){
                case "bassa":
                holderWordList.wordobject[i].freq = WordObject.Frequency.low;
                break;
                case "media":
                holderWordList.wordobject[i].freq = WordObject.Frequency.medium;
                break;
                case "alta":
                holderWordList.wordobject[i].freq = WordObject.Frequency.high;
                break;
                default:
                holderWordList.wordobject[i].freq = WordObject.Frequency.low;
                break;
            }
            holderWordList.wordobject[i].Concr_Abstr = data[13 * (i+1) + 9].ToUpper().Trim();
            holderWordList.wordobject[i].Season = data[13 * (i+1) + 10].ToUpper().Trim();
            holderWordList.wordobject[i].HasImage = (data[13 * (i+1) + 11].Trim() == "1");
            holderWordList.wordobject[i].Anello = data[13 * (i+1) + 12].ToUpper().Trim();
            
            
            
        }
    }

    public CSVData.WordObject[] FilteredWithImages(){
        CSVData.WordObject[] RealList = Instance.holderWordList.wordobject.Where(c => 
        (c.HasImage == true) &&
        // Use Normalization if working with macos filesystem
        (
            #if UNITY_IOS || UNITY_EDITOR_OSX
            (Resources.Load<Sprite>("Images/abatjour-guidare/"+c.Word.ToLower().Normalize(System.Text.NormalizationForm.FormD)))
            #else
            (Resources.Load<Sprite>("Images/abatjour-guidare/"+c.Word.ToLower()))
            #endif
       )
       ).ToArray();

        return RealList;
    }

    public CSVData.WordObject[] FilteredForAnello(){
        return Instance.holderWordList.wordobject.Where(c=> c.Anello != "0").ToArray();
    }

    public CSVData.WordObject[] FilteredForSeason(){
        return Instance.holderWordList.wordobject.Where(c=> c.Season != "").ToArray();
    }

    public CSVData.WordObject[] FilteredForSeasonWithImages(){
        return Instance.holderWordList.wordobject.Where(c=> c.Season != "" && 
        (c.HasImage == true) &&
        // Use Normalization if working with macos filesystem
        (
            #if UNITY_IOS || UNITY_EDITOR_OSX
            (Resources.Load<Sprite>("Images/abatjour-guidare/"+c.Word.ToLower().Normalize(System.Text.NormalizationForm.FormD)))
            #else
            (Resources.Load<Sprite>("Images/abatjour-guidare/"+c.Word.ToLower()))
            #endif
       )
       ).ToArray();
    }
}
