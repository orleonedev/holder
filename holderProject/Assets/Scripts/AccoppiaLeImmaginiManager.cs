using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
public class AccoppiaLeImmaginiManager : MonoBehaviour
{
	private static AccoppiaLeImmaginiManager instance;
	public static AccoppiaLeImmaginiManager Instance { get { return instance; } }

	[SerializeField]
	GameObject AccoppiaImmaginiPrefab;
	[SerializeField]
	GameObject FirstVerticalLayout;
	[SerializeField]
	GameObject SecondVerticalLayout;
	private Dictionary<string, string> DictOfWords;
	private List<string> ListOfKeys;
	private List<string> ListOfValues;
	List<GameObject> KeysGameObj;
	List<GameObject> ValuesGameObj;
	private bool wordState;
	Dictionary<GameObject, GameObject> selectedImages;
	int selectedImagesCount = 0;
	[SerializeField]
	GameObject canvas;
	[SerializeField]
	GameObject linePrefab;
	GameObject tmp;

	[SerializeField]
	GameObject endgame;

	public static int size = 3;
	public static int nSchede = 30;
	// Start is called before the first frame update
	void Start()
	{
		Application.targetFrameRate = 60;
		DictOfWords = CreateDictionary(CSVData.Instance.FilteredWithImages());
		ListOfKeys = new List<string>();
		ListOfValues = new List<string>();
		KeysGameObj = new List<GameObject>();
		ValuesGameObj = new List<GameObject>();
		selectedImages = new Dictionary<GameObject, GameObject>();
		wordState = false;
		
		var copyOfDictWords = DictOfWords.ToDictionary(entry => entry.Key,
											   entry => entry.Value);
		for (int i = 0; i < size; i++)
		{
			string randomWord = copyOfDictWords.ElementAt(Random.Range(0, copyOfDictWords.Count)).Key;
			ListOfKeys.Add(randomWord);
			ListOfValues.Add(copyOfDictWords[randomWord]);
			copyOfDictWords.Remove(randomWord);
		}
		for (int i = 0; i < size; i++)
		{
			GameObject Word1tmp = Instantiate(AccoppiaImmaginiPrefab, FirstVerticalLayout.GetComponent<RectTransform>().transform);
			GameObject Word2tmp = Instantiate(AccoppiaImmaginiPrefab, SecondVerticalLayout.GetComponent<RectTransform>().transform);
			int firstRandom = Random.Range(0, ListOfKeys.Count);
			int secondRandom = Random.Range(0, ListOfValues.Count);
			Word1tmp.GetComponentInChildren<TMP_Text>().text = ListOfKeys[firstRandom];
			var sprite1 = (
				#if UNITY_IOS || UNITY_EDITOR_OSX
            (Resources.Load<Sprite>("Images/abatjour-guidare/" + ListOfKeys[firstRandom].ToLower().Normalize(System.Text.NormalizationForm.FormD)))
            #else
            (Resources.Load<Sprite>("Images/abatjour-guidare/" + ListOfKeys[firstRandom].ToLower()))
            #endif
			);
			Word1tmp.GetComponentInChildren<UnityEngine.UI.Image>().sprite = sprite1;
			Word1tmp.gameObject.tag = "FirstLayoutAccoppia";
			Word2tmp.GetComponentInChildren<TMP_Text>().text = ListOfValues[secondRandom];
			var sprite2 = (
				#if UNITY_IOS || UNITY_EDITOR_OSX
            (Resources.Load<Sprite>("Images/abatjour-guidare/" + ListOfValues[secondRandom].ToLower().Normalize(System.Text.NormalizationForm.FormD)))
            #else
            (Resources.Load<Sprite>("Images/abatjour-guidare/" + ListOfValues[secondRandom].ToLower()))
            #endif
			);
			Word2tmp.GetComponentInChildren<UnityEngine.UI.Image>().sprite = sprite2;
			Word2tmp.gameObject.tag = "SecondLayoutAccoppia";
			KeysGameObj.Add(Word1tmp);
			ValuesGameObj.Add(Word2tmp);
			ListOfKeys.RemoveAt(firstRandom);
			ListOfValues.RemoveAt(secondRandom);
		}
		print(DictOfWords.Count);
		nSchede -= 1;
	}

	// Update is called once per frame
	void Update()
	{

	}

	private void Awake()
	{
		instance = this;
	}

	public void GetClick(GameObject obj)
	{
		if (!wordState)
		{
			selectedImages.TryAdd(obj, null);
			obj.transform.Find("Ring").gameObject.SetActive(true);
			print("Selected");
			//DebugDict();
			tmp = obj;
			wordState = !wordState;
		}
		else
		{
			//GameObject tmp = selectedImages.ElementAt(selectedWordsselectedImagesCountCount).Key;
			if (obj == tmp)
			{
				print("Deselected");
				obj.transform.Find("Ring").gameObject.SetActive(false);
				selectedImages.Remove(tmp);
				wordState = !wordState;
				
			}
			else if (obj.gameObject.tag != tmp.gameObject.tag)
			{
				print("QUi");
				selectedImages[tmp] = obj;
				GameObject lineObj = Instantiate(linePrefab, canvas.GetComponent<RectTransform>().transform);
				//lineObj.AddComponent<LineScript>();
				lineObj.GetComponent<LineScript>().gameOrigin = LineGameOrigin.AccoppiaImmagini;
				lineObj.GetComponent<LineScript>().InitializeLine(tmp, obj);
				tmp.GetComponent<UnityEngine.UI.Button>().enabled = false;
				obj.GetComponent<UnityEngine.UI.Button>().enabled = false;
				tmp.transform.Find("Ring").gameObject.SetActive(false);
				selectedImagesCount++;
				wordState = !wordState;
				DebugDict();
				if (selectedImages.Count == size)
				{
					AnotherCheckResult();
				}
			}
		}
	}

	private void AnotherCheckResult(){
		CSVData.WordObject[] CopyData = CSVData.Instance.FilteredWithImages();

		bool correct = true;
		for (int i = 0; i < selectedImages.Count && correct; i++)
		{
			var first = selectedImages.ElementAt(i).Key.GetComponentInChildren<TMP_Text>().text;
			var second = selectedImages.ElementAt(i).Value.GetComponentInChildren<TMP_Text>().text;
			

			var word1 = CopyData.Where(c => c.Word == first).First();
			var word2 = CopyData.Where(c => c.Word == second).First();

			if (
				( word1.Cat_1 != "" && ( (word1.Cat_1 == word2.Cat_1) || (word1.Cat_1 == word2.Cat_2) || (word1.Cat_1 == word2.Cat_3) )) ||
			( word1.Cat_2 != "" && ( (word1.Cat_2 == word2.Cat_1) || (word1.Cat_2 == word2.Cat_2) || (word1.Cat_2 == word2.Cat_3) )) ||
			( word1.Cat_3 != "" && ( (word1.Cat_3 == word2.Cat_1) || (word1.Cat_3 == word2.Cat_2) || (word1.Cat_3 == word2.Cat_3) )) 
			
			) {
				correct = true;

			} else {
				correct = false;
			}
			print ( first + " - " + second + " = " + correct);
		}

		if (correct) {
			print("tutte esatte");
		} else {
			print("almeno una sbagliata");
		}
		if (nSchede > 0) {
			ResetGame();
		} else {
			nSchede = 2;
			endgame.GetComponent<EndGame>().startEndGame();
			print("FINITA GAME SESSION");
		}
		
	}

	private void ResetGame(){
		CleanElements();
		DictOfWords = CreateDictionary(CSVData.Instance.FilteredWithImages());
		

		var copyOfDictWords = DictOfWords.ToDictionary(entry => entry.Key,
											   entry => entry.Value);
		for (int i = 0; i < size; i++)
		{
			string randomWord = copyOfDictWords.ElementAt(Random.Range(0, copyOfDictWords.Count)).Key;
			ListOfKeys.Add(randomWord);
			ListOfValues.Add(copyOfDictWords[randomWord]);
			copyOfDictWords.Remove(randomWord);
		}
		for (int i = 0; i < size; i++)
		{
			GameObject Word1tmp = Instantiate(AccoppiaImmaginiPrefab, FirstVerticalLayout.GetComponent<RectTransform>().transform);
			GameObject Word2tmp = Instantiate(AccoppiaImmaginiPrefab, SecondVerticalLayout.GetComponent<RectTransform>().transform);
			int firstRandom = Random.Range(0, ListOfKeys.Count);
			int secondRandom = Random.Range(0, ListOfValues.Count);
			Word1tmp.GetComponentInChildren<TMP_Text>().text = ListOfKeys[firstRandom];
			var sprite1 = (
				#if UNITY_IOS || UNITY_EDITOR_OSX
            (Resources.Load<Sprite>("Images/abatjour-guidare/" + ListOfKeys[firstRandom].ToLower().Normalize(System.Text.NormalizationForm.FormD)))
            #else
            (Resources.Load<Sprite>("Images/abatjour-guidare/" + ListOfKeys[firstRandom].ToLower()))
            #endif
			);
			Word1tmp.GetComponentInChildren<UnityEngine.UI.Image>().sprite = sprite1;
			Word1tmp.gameObject.tag = "FirstLayoutAccoppia";
			Word2tmp.GetComponentInChildren<TMP_Text>().text = ListOfValues[secondRandom];
			var sprite2 = (
				#if UNITY_IOS || UNITY_EDITOR_OSX
            (Resources.Load<Sprite>("Images/abatjour-guidare/" + ListOfValues[secondRandom].ToLower().Normalize(System.Text.NormalizationForm.FormD)))
            #else
            (Resources.Load<Sprite>("Images/abatjour-guidare/" + ListOfValues[secondRandom].ToLower()))
            #endif
			);
			Word2tmp.GetComponentInChildren<UnityEngine.UI.Image>().sprite = sprite2;
			Word2tmp.gameObject.tag = "SecondLayoutAccoppia";
			KeysGameObj.Add(Word1tmp);
			ValuesGameObj.Add(Word2tmp);
			ListOfKeys.RemoveAt(firstRandom);
			ListOfValues.RemoveAt(secondRandom);

			
		}
		print(DictOfWords.Count);
		nSchede -= 1;
	}

	private void CheckResult()
	{
		bool correct = true;
		for (int i = 0; i < selectedImages.Count && correct; i++)
		{
			var firstStepFailed = false;
			var secondStepFailed = false;
			var keyTmp = selectedImages.ElementAt(i).Key.GetComponentInChildren<TMP_Text>().text;
			var valueTmp = selectedImages.ElementAt(i).Value.GetComponentInChildren<TMP_Text>().text;
			print(keyTmp);
			print(valueTmp);
			bool containsKey = DictOfWords.ContainsKey(keyTmp);
			bool containsValueAsKey = DictOfWords.ContainsKey(valueTmp);
			if (containsKey)
			{
				if (valueTmp != DictOfWords[keyTmp])
					firstStepFailed = true;
			}
			if (containsValueAsKey && firstStepFailed)
			{
				if (keyTmp != DictOfWords[valueTmp])
					secondStepFailed = true;
			}
			if (firstStepFailed && (secondStepFailed || !containsValueAsKey))
				correct = false;
		}
		print(correct);
	}
	public void DeleteConnection(GameObject key, GameObject value)
	{
		print(key.GetComponentInChildren<TMP_Text>().text);
		print(value.GetComponentInChildren<TMP_Text>().text);
		selectedImages.Remove(key);
		selectedImagesCount--;
		DebugDict();
		key.GetComponent<UnityEngine.UI.Button>().enabled = true;
		value.GetComponent<UnityEngine.UI.Button>().enabled = true;
	}

	void DebugDict(){
		foreach (KeyValuePair<GameObject, GameObject> entry in selectedImages)
		{
			print(entry.Key.GetComponentInChildren<TMP_Text>().text + " : " + entry.Value.GetComponentInChildren<TMP_Text>().text);
		}
		print("Count of words : " + selectedImages.Count);
	}

	public Dictionary<string,string> CreateDictionary(CSVData.WordObject[] Data){
		Dictionary<string, string> filteredDict = new Dictionary<string, string>();
		CSVData.WordObject[] CopyData = Data;
		CSVData.WordObject[] filteredList = CopyData;
		CSVData.WordObject random1;


		for (int i = 0; i < size; i++){
			print("data n "+ CopyData.Count());
			do
			{
				random1 = CopyData.ElementAt(Random.Range(0, CopyData.Count()));
				CopyData = CopyData.Where(val => val != random1).ToArray();
				filteredList = CopyData.Where( c => 
				( c.Cat_1 != "" && ( (c.Cat_1 == random1.Cat_1) || (c.Cat_1 == random1.Cat_2) || (c.Cat_1 == random1.Cat_3) )) ||
				( c.Cat_2 != "" && ( (c.Cat_2 == random1.Cat_1) || (c.Cat_2 == random1.Cat_2) || (c.Cat_2 == random1.Cat_3) )) ||
				( c.Cat_3 != "" && ( (c.Cat_3 == random1.Cat_1) || (c.Cat_3 == random1.Cat_2) || (c.Cat_3 == random1.Cat_3) )) 
				).ToArray();
				print(random1.Word + "cat:" + random1.Cat_1 + " " + random1.Cat_2 +" " + random1.Cat_3 + " filtered n "+filteredList.Count());
			} while ( filteredList.Count() == 0 );
			
			CSVData.WordObject random2 = filteredList.ElementAt(Random.Range(0, filteredList.Count()));
			CopyData = CopyData.Where(val => val != random2).ToArray();
			print(random1.Word + " - " + random2.Word);
			filteredDict.Add(random1.Word,random2.Word);
			
		}

		return filteredDict;
	}

	public void CleanElements(){

		//canvas is gameObject
		for (var i = gameObject.transform.childCount - 1; i >= 0; i--)
        {
            // only destroy tagged object
            if (gameObject.transform.GetChild(i).gameObject.tag == "Line")
            Destroy(gameObject.transform.GetChild(i).gameObject);
        }

		for (var i = FirstVerticalLayout.transform.childCount - 1; i >= 0; i--){
  			Object.Destroy(FirstVerticalLayout.transform.GetChild(i).gameObject);
		}
		for (var i = SecondVerticalLayout.transform.childCount - 1; i >= 0; i--){
  			Object.Destroy(SecondVerticalLayout.transform.GetChild(i).gameObject);
		}
		ListOfKeys = new List<string>();
		ListOfValues = new List<string>();
		KeysGameObj = new List<GameObject>();
		ValuesGameObj = new List<GameObject>();
		selectedImages = new Dictionary<GameObject, GameObject>();
		wordState = false;

	}
}
