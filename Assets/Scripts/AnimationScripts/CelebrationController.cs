using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelebrationController : MonoBehaviour
{
    public Animator celebrateAnim;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void celebrate()
    {
        celebrateAnim.SetBool("isWon", true);
    }
}
