using UnityEngine;
using System.Collections;

public class ButtonSound : MonoBehaviour {

    public AudioSource source;
    public AudioClip buttonPress;

    // Use this for initialization
    void Start () {
        source = GameObject.Find("SFX Manager").GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PlaySound()
    {
        if(source != null){
            source.PlayOneShot(buttonPress, 0.5f);
        }
    }
}
