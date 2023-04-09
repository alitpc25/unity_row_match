using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakPlayButtonController : MonoBehaviour
{
    public Animator playButtonBreakAnim;
    public GameObject playButton;

    // Start is called before the first frame update
    void Start()
    {
        playButton = this.transform.parent.gameObject.GetComponent<LevelSelectRow>().playButton;
        playButtonBreakAnim = playButton.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playButtonBreakLock()
    {
        playButtonBreakAnim.SetBool("isWon", true);
    }
}
