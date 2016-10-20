// file:	Assets\Scripts\Mobs\Fireball.cs
//
// summary:	Implements the fireball class

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Mobs
{
    /// <summary>   A fireball. </summary>
    ///
 

    class Fireball:MonoBehaviour
    {
        /// <summary>   The speed. </summary>
        float speed = 10;
        /// <summary>   The damage. </summary>
        float damage = 8;

        /// <summary>   The lifetime. </summary>
        float lifetime = 3;
        /// <summary>   True to debug. </summary>
        private bool DEBUG = false;
        /// <summary>   True if this object is exploded. </summary>
        private bool _isExploded = false;

        /// <summary>   Target for the. </summary>
        Player target;
        /// <summary>   The torch controller. </summary>
        TorchFuelController TorchController;
        
        /// <summary>   The fireball created. </summary>
        public AudioClip FireballCreated;
        /// <summary>   The fireball explodes. </summary>
        public AudioClip FireballExplodes;
        /// <summary>   Source for the. </summary>
        private AudioSource _source;

        /// <summary>   Query if this object is exploded. </summary>
        ///
     
        ///
        /// <returns>   True if exploded, false if not. </returns>

        public bool isExploded()
        {
            return _isExploded;
        }

        /// <summary>   Starts this object. </summary>
        ///
     

        void Start()
        {
            _source = GetComponent<AudioSource>();
            _source.PlayOneShot(FireballCreated);
            //Destroy(gameObject, lifetime);
            if (DEBUG) Debug.Log("Starting fireball!");
            //Find player with the torch and set them as the target for this fireball
            TorchController = GameObject.FindGameObjectWithTag("TorchFuelController").transform.GetComponent<TorchFuelController>();
            if (TorchController.TorchWithPlayer1())
            {
                target = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<Player>();
            } else
            {
                target = GameObject.FindGameObjectWithTag("Player2").transform.GetComponent<Player>();
            }         
        }      

        /// <summary>   Updates this object. </summary>
        ///
     

        void Update()
        {
            float moveDistance = speed * Time.deltaTime;
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
        }       

        /// <summary>   Executes the trigger enter action. </summary>
        ///
     
        ///
        /// <param name="other">    The other. </param>

        void OnTriggerEnter(Collider other)
        {
            if (DEBUG) Debug.Log("Collison detected");
            if (other.tag == "Player" || other.tag == "Player2")
            {
                if (DEBUG) Debug.Log("Fireball collision: Player");

                other.GetComponent<Player>().Damage(damage, this.transform.root);

                _source.PlayOneShot(FireballExplodes);

                _isExploded = true;

                if (DEBUG) Debug.Log("Creating fireball explosion");
                GameObject newFireball = (GameObject)Instantiate(Resources.Load("Explosion"));
                Vector3 newPos = new Vector3(this.transform.position.x, 6, this.transform.position.z);
                newFireball.transform.position = newPos;
                GameObject.Destroy(gameObject);
            }            
        }

        /// <summary>   Sets a speed. </summary>
        ///
     
        ///
        /// <param name="newSpeed"> The new speed. </param>

        public void SetSpeed(float newSpeed)
        {
            speed = newSpeed;
        }
    }
}
