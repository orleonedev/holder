using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
public class AccoppiaLeParoleManager : MonoBehaviour
{
	private static AccoppiaLeParoleManager instance;
	public static AccoppiaLeParoleManager Instance { get { return instance; } }

	[SerializeField]
	GameObject AccoppiaParolePrefab;
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
	Dictionary<GameObject, GameObject> selectedWords;
	int selectedWordsCount = 0;
	[SerializeField]
	GameObject canvas;
	[SerializeField]
	GameObject linePrefab;
	GameObject tmp;
	// Start is called before the first frame update
	void Start()
	{
		Application.targetFrameRate = 60;
		DictOfWords = new Dictionary<string, string>();
		ListOfKeys = new List<string>();
		ListOfValues = new List<string>();
		KeysGameObj = new List<GameObject>();
		ValuesGameObj = new List<GameObject>();
		selectedWords = new Dictionary<GameObject, GameObject>();
		wordState = false;
		//Dictionary<string,string> copyOfDictWords = DictOfWords;
		DictOfWords.Add("Muratore", "Cazzuola");
		DictOfWords.Add("Cuoco", "Pentola");
		DictOfWords.Add("Libraio", "Libro");
		DictOfWords.Add("Centralinista", "Telefono");
		DictOfWords.Add("Macchinista", "Treno");
		DictOfWords.Add("Infermiera", "Siringa");
		DictOfWords.Add("Insegnante", "Penna");
		DictOfWords.Add("Biberon", "Latte");
		DictOfWords.Add("Penna", "Inchiostro");
		DictOfWords.Add("Crostata", "Marmellata");
		DictOfWords.Add("Armadio", "Vestiti");
		DictOfWords.Add("Vaso", "Fiore");
		var copyOfDictWords = DictOfWords.ToDictionary(entry => entry.Key,
											   entry => entry.Value);
		for (int i = 0; i < 8; i++)
		{
			string randomWord = copyOfDictWords.ElementAt(Random.Range(0, copyOfDictWords.Count)).Key;
			ListOfKeys.Add(randomWord);
			ListOfValues.Add(copyOfDictWords[randomWord]);
			copyOfDictWords.Remove(randomWord);
		}
		for (int i = 0; i < 8; i++)
		{
			GameObject Word1tmp = Instantiate(AccoppiaParolePrefab, FirstVerticalLayout.GetComponent<RectTransform>().transform);
			GameObject Word2tmp = Instantiate(AccoppiaParolePrefab, SecondVerticalLayout.GetComponent<RectTransform>().transform);
			int firstRandom = Random.Range(0, ListOfKeys.Count);
			int secondRandom = Random.Range(0, ListOfValues.Count);
			Word1tmp.GetComponentInChildren<TMP_Text>().text = ListOfKeys[firstRandom];
			Word1tmp.gameObject.tag = "FirstLayoutAccoppia";
			Word2tmp.GetComponentInChildren<TMP_Text>().text = ListOfValues[secondRandom];
			Word2tmp.gameObject.tag = "SecondLayoutAccoppia";
			KeysGameObj.Add(Word1tmp);
			ValuesGameObj.Add(Word2tmp);
			ListOfKeys.RemoveAt(firstRandom);
			ListOfValues.RemoveAt(secondRandom);
		}
		print(DictOfWords.Count);
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
			selectedWords.TryAdd(obj, null);
			print("Selected");
			//DebugDict();
			tmp = obj;
			wordState = !wordState;
		}
		else
		{
			//GameObject tmp = selectedWords.ElementAt(selectedWordsCount).Key;
			if (obj == tmp)
			{
				print("Deselected");
				selectedWords.Remove(tmp);
				wordState = !wordState;
			}
			else if (obj.gameObject.tag != tmp.gameObject.tag)
			{
				print("QUi");
				selectedWords[tmp] = obj;
				GameObject lineObj = Instantiate(linePrefab, canvas.GetComponent<RectTransform>().transform);
				//lineObj.AddComponent<LineScript>();
				lineObj.GetComponent<LineScript>().gameOrigin = LineGameOrigin.AccoppiaParole;
				lineObj.GetComponent<LineScript>().InitializeLine(tmp, obj);
				tmp.GetComponent<UnityEngine.UI.Button>().enabled = false;
				obj.GetComponent<UnityEngine.UI.Button>().enabled = false;
				selectedWordsCount++;
				wordState = !wordState;
				DebugDict();
				if (selectedWords.Count == 8)
				{
					CheckResult();
				}
			}
		}
	}
	private void CheckResult()
	{
		bool correct = true;
		for (int i = 0; i < selectedWords.Count && correct; i++)
		{
			var firstStepFailed = false;
			var secondStepFailed = false;
			var keyTmp = selectedWords.ElementAt(i).Key.GetComponentInChildren<TMP_Text>().text;
			var valueTmp = selectedWords.ElementAt(i).Value.GetComponentInChildren<TMP_Text>().text;
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
		selectedWords.Remove(key);
		selectedWordsCount--;
		DebugDict();
		key.GetComponent<UnityEngine.UI.Button>().enabled = true;
		value.GetComponent<UnityEngine.UI.Button>().enabled = true;
	}

	void DebugDict(){
		foreach (KeyValuePair<GameObject, GameObject> entry in selectedWords)
		{
			print(entry.Key.GetComponentInChildren<TMP_Text>().text + " : " + entry.Value.GetComponentInChildren<TMP_Text>().text);
		}
		print("Count of words : " + selectedWords.Count);
	}
}
