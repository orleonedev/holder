using UnityEngine;
public class ImageShuffleClass : MonoBehaviour {
	private int numberInSequence;
	public int NumberInSequence { get { return numberInSequence; } set { numberInSequence = value; } }
	private Sprite sprite;
	public Sprite SpritePublic { get { return sprite; } set { sprite = value; } }
	public ImageShuffleClass(int NumberInSequence, Sprite sprite){
		this.numberInSequence = NumberInSequence;
		this.sprite = sprite;
	}
}