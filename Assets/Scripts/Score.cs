using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Score : NetworkBehaviour
{
  private int[] score = new int[2];

  public int[] AddScore(int player, int points)
  {
    if (player < 0 || player >= 2)
    {
      Debug.LogError("Cannot add score to player " + player + ": not found.");
      return score;
    }

    score[player] += points;
    return score;
  }

  public int[] IncrementScore(int player)
  {
    int[] newScore = AddScore(player, 1);
    RpcUpdateScore(newScore[0], newScore[1]);
    return newScore;
  }

  public int[] GetCurrentScore()
  {
    return score;
  }

  [ClientRpc]
  public void RpcUpdateScore(int score1, int score2)
  {
    score[0] = score1;
    score[1] = score2;
  }
}
