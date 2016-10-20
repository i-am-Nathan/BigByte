// file:	Assets\DownloadedContent\PyroParticles\Prefab\Script\FireProjectileScript.cs
//
// summary:	Implements the fire projectile script class

using UnityEngine;
using System.Collections;

namespace DigitalRuby.PyroParticles
{
    /// <summary>   Handle collision of a fire projectile. </summary>
    ///
 
    ///
    /// <param name="script">   Script. </param>
    /// <param name="pos">      Position. </param>

    public delegate void FireProjectileCollisionDelegate(FireProjectileScript script, Vector3 pos);

    /// <summary>   This script handles a projectile such as a fire ball. </summary>
    ///
 

    public class FireProjectileScript : FireBaseScript, ICollisionHandler
    {
        /// <summary>   The projectile collider object. </summary>
        [Tooltip("The collider object to use for collision and physics.")]
        public GameObject ProjectileColliderObject;

        /// <summary>   The projectile collision sound. </summary>
        [Tooltip("The sound to play upon collision.")]
        public AudioSource ProjectileCollisionSound;

        /// <summary>   The projectile explosion particle system. </summary>
        [Tooltip("The particle system to play upon collision.")]
        public ParticleSystem ProjectileExplosionParticleSystem;

        /// <summary>   The projectile explosion radius. </summary>
        [Tooltip("The radius of the explosion upon collision.")]
        public float ProjectileExplosionRadius = 50.0f;

        /// <summary>   The projectile explosion force. </summary>
        [Tooltip("The force of the explosion upon collision.")]
        public float ProjectileExplosionForce = 50.0f;

        /// <summary>   The projectile collider delay. </summary>
        [Tooltip("An optional delay before the collider is sent off, in case the effect has a pre fire animation.")]
        public float ProjectileColliderDelay = 0.0f;

        /// <summary>   The projectile collider speed. </summary>
        [Tooltip("The speed of the collider.")]
        public float ProjectileColliderSpeed = 450.0f;

        /// <summary>   The projectile direction. </summary>
        [Tooltip("The direction that the collider will go. For example, flame strike goes down, and fireball goes forward.")]
        public Vector3 ProjectileDirection = Vector3.forward;

        /// <summary>   The projectile collision layers. </summary>
        [Tooltip("What layers the collider can collide with.")]
        public LayerMask ProjectileCollisionLayers = Physics.AllLayers;

        /// <summary>   The projectile destroy particle systems on collision. </summary>
        [Tooltip("Particle systems to destroy upon collision.")]
        public ParticleSystem[] ProjectileDestroyParticleSystemsOnCollision;

        /// <summary>   The collision delegate. </summary>
        [HideInInspector]
        public FireProjectileCollisionDelegate CollisionDelegate;

        /// <summary>   True if collided. </summary>
        private bool collided;

        /// <summary>   Sends the collision after delay. </summary>
        ///
     
        ///
        /// <returns>   An IEnumerator. </returns>

        private IEnumerator SendCollisionAfterDelay()
        {
            yield return new WaitForSeconds(ProjectileColliderDelay);

            Vector3 dir = ProjectileDirection * ProjectileColliderSpeed;
            dir = ProjectileColliderObject.transform.rotation * dir;
            ProjectileColliderObject.GetComponent<Rigidbody>().velocity = dir;
        }

        /// <summary>   Starts this object. </summary>
        ///
     

        protected override void Start()
        {
            base.Start();

            StartCoroutine(SendCollisionAfterDelay());
        }

        /// <summary>   Handles the collision. </summary>
        ///
     
        ///
        /// <param name="obj">  The object. </param>
        /// <param name="c">    The Collision to process. </param>

        public void HandleCollision(GameObject obj, Collision c)
        {
            if (collided)
            {
                // already collided, don't do anything
                return;
            }

            // stop the projectile
            collided = true;
            Stop();

            // destroy particle systems after a slight delay
            if (ProjectileDestroyParticleSystemsOnCollision != null)
            {
                foreach (ParticleSystem p in ProjectileDestroyParticleSystemsOnCollision)
                {
                    GameObject.Destroy(p, 0.1f);
                }
            }

            // play collision sound
            if (ProjectileCollisionSound != null)
            {
                ProjectileCollisionSound.Play();
            }

            // if we have contacts, play the collision particle system and call the delegate
            if (c.contacts.Length != 0)
            {
                ProjectileExplosionParticleSystem.transform.position = c.contacts[0].point;
                ProjectileExplosionParticleSystem.Play();
                FireBaseScript.CreateExplosion(c.contacts[0].point, ProjectileExplosionRadius, ProjectileExplosionForce);
                if (CollisionDelegate != null)
                {
                    CollisionDelegate(this, c.contacts[0].point);
                }
            }
        }
    }
}