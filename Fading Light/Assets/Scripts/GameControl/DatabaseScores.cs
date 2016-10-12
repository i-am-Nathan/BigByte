using UnityEngine;
using System.Collections;
using System.Security.Cryptography;

/// <summary>
/// Used to manage database scores.
/// </summary>
public class DatabaseScores : MonoBehaviour
{
    private string SecretKey = "2W_0Yc:p_~oU}(P1?]P98)1]0894J0"; // Edit this value and make sure it's the same as the one stored on the server
    private string AddScoreURL = "http://jackbarker.co/306/API/AddScores.php?"; //be sure to add a ? to your url
    private string HighscoreURL = "http://localhost/unity_test/display.php";


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
    IEnumerator PostScores(string name, int gold, float Player1DamageGiven, float Player2DamageGiven, float Player1DamageTaken, float Player2DamageTaken, float Player1Accuracy, float Player2Accuracy)
    {

        //Create hash to ensure things that arent this game arent posting scores
        MD5 md5 = MD5.Create();
        byte[] asciiBytes = System.Text.Encoding.ASCII.GetBytes(name + gold + SecretKey);
        byte[] hashedBytes = MD5.Create().ComputeHash(asciiBytes);
        string stringHash = System.BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

        //Update the url with the scores
        string post_url = AddScoreURL + "name=" + name + "&gold=" + gold + "&p1damagegiven=" + Player1DamageGiven + "&p2damagegiven=" + Player2DamageGiven + "&p1damagetaken=" + Player1DamageTaken + "&p2damagetaken=" + Player2DamageTaken + "&p1accuracy=" + Player1Accuracy + "&p2accuracy=" + Player2Accuracy + "&hash=" + stringHash;

        //Post scores to php page
        WWW post = new WWW(post_url);
        yield return post; 

        if (post.error != null)
        {
            Debug.Log("ERROR");
            Debug.Log("There was an error posting the high score: " + post.error);
        }
    }
}