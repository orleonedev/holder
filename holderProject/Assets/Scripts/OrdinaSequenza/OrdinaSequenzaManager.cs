using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class OrdinaSequenzaManager : MonoBehaviour
{
	private static OrdinaSequenzaManager instance;
	public static OrdinaSequenzaManager Instance { get { return instance; } }
	[SerializeField]
	List<Sprite> imagesToOrder;
	[SerializeField]
	GameObject imagesPrefab;
	List<GameObject> imagesToOrderList;
	[SerializeField]
	GameObject ImageLayout;
	[SerializeField]
	GameObject AnswerLayout;
	[SerializeField]
	GameObject answerSequencePrefab;
	List<GameObject> answersList;
	int imageInsertedCount;
	List<ImageShuffleClass> shuffleList;
	//used for handling position inserted and shuffled
	Dictionary<int,int> positionDict;
    // Start is called before the first frame update
    void Start()
    {
		imageInsertedCount = 0;
        imagesToOrderList = new List<GameObject>();
		answersList = new List<GameObject>();
		shuffleList = new List<ImageShuffleClass>();
		positionDict = new Dictionary<int, int>();
		//Shuffling images
		System.Random rng = new System.Random();
		var shuffledImages = imagesToOrder.OrderBy(a => rng.Next()).ToList();
		
		for(int i = 0; i < imagesToOrder.Count; i++){
			GameObject imageHolder = Instantiate(imagesPrefab, ImageLayout.GetComponent<RectTransform>());
			imageHolder.GetComponent<ImageOrderScript>().NumberInSequence = i;
			imageHolder.transform.GetChild(0).GetComponentInChildren<UnityEngine.UI.Image>().sprite = shuffledImages[i];
			imagesToOrderList.Add(imageHolder);
			GameObject answerHolder = Instantiate(answerSequencePrefab, AnswerLayout.GetComponent<RectTransform>());
			answerHolder.GetComponentInChildren<TMP_Text>().text = (i+1).ToString();
			answerHolder.GetComponent<AnswerSequenceScript>().ImagePosition = i;
			answerHolder.GetComponent<UnityEngine.UI.Button>().enabled = false;
			answersList.Add(answerHolder);
			ImageShuffleClass tmp = new ImageShuffleClass(i, imagesToOrder[i]);
			shuffleList.Add(tmp);
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void Awake() {
		instance = this;
	}

	public void AssignImageToAvailableSpot(int numberInSequence, Sprite sprite){
		bool positionFound = false;
		if(imageInsertedCount != imagesToOrder.Count){
			for(int i = 0; i < imagesToOrder.Count && !positionFound; i++){
				if(answersList[i].transform.GetChild(1).GetComponent<UnityEngine.UI.Image>().sprite == null) {
					answersList[i].transform.GetChild(1).GetComponent<UnityEngine.UI.Image>().sprite = sprite;
					answersList[i].transform.GetChild(1).GetComponent<UnityEngine.UI.Image>().color = Color.white;
					answersList[i].GetComponent<UnityEngine.UI.Button>().enabled = true;
					positionDict.Add(i, numberInSequence);
					positionFound = true;
				}
			}
			imageInsertedCount++;
			if(imageInsertedCount == imagesToOrder.Count){
				bool imageOutOfOrder = false;
				for(int i = 0; i < imagesToOrder.Count && !imageOutOfOrder; i++){
					if(answersList[i].transform.GetChild(1).GetComponent<UnityEngine.UI.Image>().sprite != shuffleList[i].SpritePublic)
						imageOutOfOrder = true;
				}
				if(imageOutOfOrder){
					print("sbagliato");
				}
				else{
					print("Bravo");
				}
			}
		}
	}
	public void DeleteImageFromTappedSpot(int position){
		answersList[position].transform.GetChild(1).GetComponent<UnityEngine.UI.Image>().color = Color.clear;
		answersList[position].transform.GetChild(1).GetComponent<UnityEngine.UI.Image>().sprite = null;
		answersList[position].GetComponent<UnityEngine.UI.Button>().enabled = false;
		imagesToOrderList[positionDict[position]].transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().color = Color.white;
		imagesToOrderList[positionDict[position]].GetComponent<UnityEngine.UI.Button>().enabled = true;
		positionDict.Remove(position);
		if(imageInsertedCount > 0){
			imageInsertedCount--;
		}
	}
}
