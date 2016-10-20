// file:	Assets\UnityTestTools\Examples\IntegrationTestsFrameworkExamples\AngryBotsTests\Scripts\GameScore.cs
//
// summary:	Implements the game score class

using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>   A game score. </summary>
///
/// <remarks>    . </remarks>

public class GameScore : MonoBehaviour
{
    /// <summary>   The instance. </summary>
    static GameScore s_Instance;

    /// <summary>   Gets the instance. </summary>
    ///
    /// <value> The instance. </value>

    static GameScore Instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = (GameScore)FindObjectOfType(typeof(GameScore));
            }

            return s_Instance;
        }
    }

    /// <summary>   Executes the application quit action. </summary>
    ///
 

    public void OnApplicationQuit()
    {
        s_Instance = null;
    }


    /// <summary>   Name of the enemy layer. </summary>
    public string playerLayerName = "Player", enemyLayerName = "Enemies";


    /// <summary>   The deaths. </summary>
    int m_Deaths;
    /// <summary>   The kills. </summary>
    readonly Dictionary<string, int> m_Kills = new Dictionary<string, int>();
    /// <summary>   The start time. </summary>
    float m_StartTime;

    /// <summary>   Gets the deaths. </summary>
    ///
    /// <value> The deaths. </value>

    public static int Deaths
    {
        get
        {
            if (Instance == null)
            {
                return 0;
            }

            return Instance.m_Deaths;
        }
    }


    #if !UNITY_FLASH

    /// <summary>   Gets a list of types of the kills. </summary>
    ///
    /// <value> A list of types of the kills. </value>

    public static ICollection<string> KillTypes
    {
        get
        {
            if (Instance == null)
            {
                return new string[0];
            }

            return Instance.m_Kills.Keys;
        }
    }
    #endif  // if !UNITY_FLASH

    /// <summary>   Gets the kills. </summary>
    ///
 
    ///
    /// <param name="type"> The type. </param>
    ///
    /// <returns>   The kills. </returns>

    public static int GetKills(string type)
    {
        if (Instance == null || !Instance.m_Kills.ContainsKey(type))
        {
            return 0;
        }

        return Instance.m_Kills[type];
    }

    /// <summary>   Gets the game time. </summary>
    ///
    /// <value> The game time. </value>

    public static float GameTime
    {
        get
        {
            if (Instance == null)
            {
                return 0.0f;
            }

            return Time.time - Instance.m_StartTime;
        }
    }

    /// <summary>   Registers the death described by deadObject. </summary>
    ///
 
    ///
    /// <param name="deadObject">   The dead object. </param>

    public static void RegisterDeath(GameObject deadObject)
    {
        if (Instance == null)
        {
            Debug.Log("Game score not loaded");
            return;
        }

        int
            playerLayer = LayerMask.NameToLayer(Instance.playerLayerName),
            enemyLayer = LayerMask.NameToLayer(Instance.enemyLayerName);

        if (deadObject.layer == playerLayer)
        {
            Instance.m_Deaths++;
        }
        else if (deadObject.layer == enemyLayer)
        {
            Instance.m_Kills[deadObject.name] = Instance.m_Kills.ContainsKey(deadObject.name) ? Instance.m_Kills[deadObject.name] + 1 : 1;
        }
    }

    /// <summary>   Executes the level was loaded action. </summary>
    ///
 
    ///
    /// <param name="level">    The level. </param>

    public void OnLevelWasLoaded(int level)
    {
        if (m_StartTime == 0.0f)
        {
            m_StartTime = Time.time;
        }
    }
}
