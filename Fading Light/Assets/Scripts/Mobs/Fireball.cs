using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Mobs
{
    class Fireball:MonoBehaviour
    {
        float speed = 15;
        float damage = 1;

        float lifetime = 3;
        private bool DEBUG = true;
        private bool _isExploded = false;

        Player target;
        TorchFuelController TorchController;

        public bool isExploded()
        {
            return _isExploded;
        }

        void Start()
        {
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

        void Update()
        {
            float moveDistance = speed * Time.deltaTime;
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
        }       

        void OnTriggerEnter(Collider other)
        {
            if (DEBUG) Debug.Log("Collison detected");
            if (other.tag == "Player" || other.tag == "Player2")
            {
                if (DEBUG) Debug.Log("Fireball collision: Player");
                target.Damage(damage, this.transform.root);

                _isExploded = true;

                if (DEBUG) Debug.Log("Creating fireball explosion");
                GameObject newFireball = (GameObject)Instantiate(Resources.Load("Explosion"));
                Vector3 newPos = new Vector3(this.transform.position.x, 6, this.transform.position.z);
                newFireball.transform.position = newPos;
                GameObject.Destroy(gameObject);
            }            
        }

        public void SetSpeed(float newSpeed)
        {
            speed = newSpeed;
        }
    }
}
