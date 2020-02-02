using System;
using UnityEngine;



[CreateAssetMenu(fileName = "HighScore", menuName = "ScriptableObject/HighScore", order = 1)]

 public class HighScore : ScriptableObject
 {
    public string[] joueur;
    public int[] score;
 }

