using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoardController : MonoBehaviour
{

  public Text scoreJ1;
  public Text scoreJ2;

  //public Score score;

  private Score score;

  void Start()
  {
    score = GameObject.Find("Score").GetComponent<Score>();
  }

  // Update is called once per frame
  void Update()
  {
    scoreJ1.text = score.GetCurrentScore()[0].ToString();
    scoreJ2.text = score.GetCurrentScore()[1].ToString();
  }
}
