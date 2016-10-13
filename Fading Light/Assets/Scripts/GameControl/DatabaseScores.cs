﻿using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using System.Collections.Generic;

/// <summary>
/// Used to manage database scores.
/// </summary>
public class DatabaseScores : MonoBehaviour
{
    private string SecretKey = "2W_0Yc:p_~oU}(P1?]P98)1]0894J0"; // Edit this value and make sure it's the same as the one stored on the server
    private string AddScoreURL = "http://jackbarker.co/306/API/AddScores.php?"; //be sure to add a ? to your url
    private string HighscoreURL = "http://jackbarker.co/306/API/GetScores.php";
    List<HighScore> _results = new List<HighScore>();
    public bool IsDone = false;
    /// <summary>
    /// Posts the scores to the webserver.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="gold">The gold.</param>
    /// <param name="Player1DamageGiven">The player1 damage given.</param>
    /// <param name="Player2DamageGiven">The player2 damage given.</param>
    /// <param name="Player1DamageTaken">The player1 damage taken.</param>
    /// <param name="Player2DamageTaken">The player2 damage taken.</param>
    /// <param name="Player1Accuracy">The player1 accuracy.</param>
    /// <param name="Player2Accuracy">The player2 accuracy.</param>
    /// <returns></returns>
    /// Example: StartCoroutine(PostScores("jack", 100, 99, 99, 99, 99, 99, 99));
    public IEnumerator PostScores(string name, int gold, float Player1DamageGiven, float Player2DamageGiven, float Player1DamageTaken, float Player2DamageTaken, float Player1Accuracy, float Player2Accuracy)
    {
        //Create hash to ensure things that arent this game arent posting scores
        var md5 = MD5.Create();
        var asciiBytes = System.Text.Encoding.ASCII.GetBytes(name + gold + SecretKey);
        var hashedBytes = MD5.Create().ComputeHash(asciiBytes);
        var stringHash = System.BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

        var post_url = string.Format("{0}name={1}&gold={2}&p1damagegiven={3}&p2damagegiven={4}&p1damagetaken={5}&p2damagetaken={6}&p1accuracy={7}&p2accuracy={8}&hash={9}",
            AddScoreURL, name, gold, Player1DamageGiven, Player2DamageGiven, Player1DamageTaken, Player2DamageTaken, Player1Accuracy, Player2Accuracy, stringHash);
        //Post scores to php page
        var post = new WWW(post_url);
        yield return post; 

        if (post.error != null)
        {
            Debug.Log("ERROR");
            Debug.Log("There was an error posting the high score: " + post.error);
        }
    }


    /// <summary>
    /// Gets the scores.
    /// </summary>
    /// <returns></returns>
    /// Example: Example: StartCoroutine(GetScores());
    public IEnumerator GetScores()
    {
        WWW hs_get = new WWW(HighscoreURL);

        yield return hs_get;

        if (hs_get.error != null)
        {
            Debug.Log("There was an error getting the high score: " + hs_get.error);
        }

        var splitText = System.Text.RegularExpressions.Regex.Split(hs_get.text, "\r\n|\r|\n");

        _results = new List<HighScore>();

        foreach(var s in splitText)
        {
            var tmp = HighScore.CreateFromJSON(s);
            _results.Add(tmp);
        }

        IsDone = true;

    }

    public List<HighScore> GetResults()
    {
        return _results;
    }
}