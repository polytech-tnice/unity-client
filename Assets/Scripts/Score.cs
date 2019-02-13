using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    private int[] score = new int[2];

    public int[] AddScore(int player, int points) {
        if (player < 0 || player >= 2) {
            Debug.LogError("Cannot add score to player " + player + ": not found.");
            return score;
        }

        score[player] += points;
        return score;
    }

    public int[] IncrementScore(int player) {
        return AddScore(player, 1);
    }

    public int[] GetCurrentScore() {
        return score;
    }
}
