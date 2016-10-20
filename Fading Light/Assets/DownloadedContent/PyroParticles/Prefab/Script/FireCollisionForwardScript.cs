// file:	Assets\DownloadedContent\PyroParticles\Prefab\Script\FireCollisionForwardScript.cs
//
// summary:	Implements the fire collision forward script class

using UnityEngine;
using System.Collections;

namespace DigitalRuby.PyroParticles
{
    /// <summary>   Interface for collision handler. </summary>
    ///
 

    public interface ICollisionHandler
    {
        /// <summary>   Handles the collision. </summary>
        ///
        /// <param name="obj">  The object. </param>
        /// <param name="c">    The Collision to process. </param>

        void HandleCollision(GameObject obj, Collision c);
    }

    /// <summary>
    /// This script simply allows forwarding collision events for the objects that collide with
    /// something. This allows you to have a generic collision handler and attach a collision
    /// forwarder to your child objects. In addition, you also get access to the game object that is
    /// colliding, along with the object being collided into, which is helpful.
    /// </summary>
    ///
 

    public class FireCollisionForwardScript : MonoBehaviour
    {
        /// <summary>   The collision handler. </summary>
        public ICollisionHandler CollisionHandler;

        /// <summary>   Executes the collision enter action. </summary>
        ///
     
        ///
        /// <param name="col">  The col. </param>

        public void OnCollisionEnter(Collision col)
        {
            CollisionHandler.HandleCollision(gameObject, col);
        }
    }
}
