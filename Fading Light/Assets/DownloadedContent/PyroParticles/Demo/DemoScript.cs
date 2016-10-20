// file:	Assets\DownloadedContent\PyroParticles\Demo\DemoScript.cs
//
// summary:	Implements the demo script class

using UnityEngine;
using System.Collections;

namespace DigitalRuby.PyroParticles
{
    /// <summary>   A demo script. </summary>
    ///
 

    public class DemoScript : MonoBehaviour
    {
        /// <summary>   The prefabs. </summary>
        public GameObject[] Prefabs;
        /// <summary>   The sun. </summary>
        public Light Sun;
        /// <summary>   The side camera. </summary>
        public Camera SideCamera;
        /// <summary>   The time of day slider. </summary>
        public UnityEngine.UI.Slider TimeOfDaySlider;
        /// <summary>   The mouse look toggle. </summary>
        public UnityEngine.UI.Toggle MouseLookToggle;
        /// <summary>   The current item text. </summary>
        public UnityEngine.UI.Text CurrentItemText;

        /// <summary>   The current prefab object. </summary>
        private GameObject currentPrefabObject;
        /// <summary>   The current prefab script. </summary>
        private FireBaseScript currentPrefabScript;
        /// <summary>   The current prefab index. </summary>
        private int currentPrefabIndex;

        /// <summary>   Values that represent rotation axes. </summary>
        ///
     

        private enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
        /// <summary>   The axes. </summary>
        private RotationAxes axes = RotationAxes.MouseXAndY;
        /// <summary>   The sensitivity x coordinate. </summary>
        private float sensitivityX = 15F;
        /// <summary>   The sensitivity y coordinate. </summary>
        private float sensitivityY = 15F;
        /// <summary>   The minimum x coordinate. </summary>
        private float minimumX = -360F;
        /// <summary>   The maximum x coordinate. </summary>
        private float maximumX = 360F;
        /// <summary>   The minimum y coordinate. </summary>
        private float minimumY = -60F;
        /// <summary>   The maximum y coordinate. </summary>
        private float maximumY = 60F;
        /// <summary>   The rotation x coordinate. </summary>
        private float rotationX = 0F;
        /// <summary>   The rotation y coordinate. </summary>
        private float rotationY = 0F;
        /// <summary>   The original rotation. </summary>
        private Quaternion originalRotation;

        /// <summary>   Updates the user interface. </summary>
        ///
     

        private void UpdateUI()
        {
            Sun.transform.rotation = Quaternion.Euler(TimeOfDaySlider.value, 60.0f, 0.0f);
            CurrentItemText.text = Prefabs[currentPrefabIndex].name;
        }

        /// <summary>   Updates the movement. </summary>
        ///
     

        private void UpdateMovement()
        {
            float speed = 5.0f * Time.deltaTime;

            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(0.0f, 0.0f, speed);
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(0.0f, 0.0f, -speed);
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(-speed, 0.0f, 0.0f);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(speed, 0.0f, 0.0f);
            }

            if (Input.GetKeyDown(KeyCode.M))
            {
                MouseLookToggle.isOn = !MouseLookToggle.isOn;
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
            }
            else if (Input.GetKeyDown(KeyCode.O))
            {
                if (SideCamera.enabled = !SideCamera.enabled)
                {
                    Camera.main.rect = new Rect(0.0f, 0.0f, 0.5f, 1.0f);
                }
                else
                {
                    Camera.main.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
                }                
            }
        }

        /// <summary>   Updates the mouse look. </summary>
        ///
     

        private void UpdateMouseLook()
        {
            if (!MouseLookToggle.isOn)
            {
                return;
            }
            else if (axes == RotationAxes.MouseXAndY)
            {
                // Read the mouse input axis
                rotationX += Input.GetAxis("Mouse X") * sensitivityX;
                rotationY += Input.GetAxis("Mouse Y") * sensitivityY;

                rotationX = ClampAngle(rotationX, minimumX, maximumX);
                rotationY = ClampAngle(rotationY, minimumY, maximumY);

                Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
                Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, -Vector3.right);

                transform.localRotation = originalRotation * xQuaternion * yQuaternion;
            }
            else if (axes == RotationAxes.MouseX)
            {
                rotationX += Input.GetAxis("Mouse X") * sensitivityX;
                rotationX = ClampAngle(rotationX, minimumX, maximumX);

                Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
                transform.localRotation = originalRotation * xQuaternion;
            }
            else
            {
                rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
                rotationY = ClampAngle(rotationY, minimumY, maximumY);

                Quaternion yQuaternion = Quaternion.AngleAxis(-rotationY, Vector3.right);
                transform.localRotation = originalRotation * yQuaternion;
            }
        }

        /// <summary>   Updates the effect. </summary>
        ///
     

        private void UpdateEffect()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCurrent();
            }
            else if (Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                NextPrefab();
            }
            else if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus))
            {
                PreviousPrefab();
            }
        }

        /// <summary>   Clamp angle. </summary>
        ///
     
        ///
        /// <param name="angle">    The angle. </param>
        /// <param name="min">      The minimum. </param>
        /// <param name="max">      The maximum. </param>
        ///
        /// <returns>   A float. </returns>

        private static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360F)
            {
                angle += 360F;
            }
            if (angle > 360F)
            {
                angle -= 360F;
            }

            return Mathf.Clamp(angle, min, max);
        }

        /// <summary>   Begins an effect. </summary>
        ///
     

        private void BeginEffect()
        {
            Vector3 pos;
            float yRot = transform.rotation.eulerAngles.y;
            Vector3 forwardY = Quaternion.Euler(0.0f, yRot, 0.0f) * Vector3.forward;
            Vector3 forward = transform.forward;
            Vector3 right = transform.right;
            Vector3 up = transform.up;
            Quaternion rotation = Quaternion.identity;
            currentPrefabObject = GameObject.Instantiate(Prefabs[currentPrefabIndex]);
            currentPrefabScript = currentPrefabObject.GetComponent<FireConstantBaseScript>();

            if (currentPrefabScript == null)
            {
                // temporary effect, like a fireball
                currentPrefabScript = currentPrefabObject.GetComponent<FireBaseScript>();
                if (currentPrefabScript.IsProjectile)
                {
                    // set the start point near the player
                    rotation = transform.rotation;
                    pos = transform.position + forward + right + up;
                }
                else
                {
                    // set the start point in front of the player a ways
                    pos = transform.position + (forwardY * 10.0f);
                }
            }
            else
            {
                // set the start point in front of the player a ways, rotated the same way as the player
                pos = transform.position + (forwardY * 5.0f);
                rotation = transform.rotation;
                pos.y = 0.0f;
            }

            FireProjectileScript projectileScript = currentPrefabObject.GetComponentInChildren<FireProjectileScript>();
            if (projectileScript != null)
            {
                // make sure we don't collide with other friendly layers
                projectileScript.ProjectileCollisionLayers &= (~UnityEngine.LayerMask.NameToLayer("FriendlyLayer"));
            }

            currentPrefabObject.transform.position = pos;
            currentPrefabObject.transform.rotation = rotation;
        }

        /// <summary>   Starts a current. </summary>
        ///
     

        public void StartCurrent()
        {
            StopCurrent();
            BeginEffect();
        }

        /// <summary>   Stops a current. </summary>
        ///
     

        private void StopCurrent()
        {
            // if we are running a constant effect like wall of fire, stop it now
            if (currentPrefabScript != null && currentPrefabScript.Duration > 10000)
            {
                currentPrefabScript.Stop();
            }
            currentPrefabObject = null;
            currentPrefabScript = null;
        }

        /// <summary>   Next prefab. </summary>
        ///
     

        public void NextPrefab()
        {
            currentPrefabIndex++;
            if (currentPrefabIndex == Prefabs.Length)
            {
                currentPrefabIndex = 0;
            }
            UpdateUI();
        }

        /// <summary>   Previous prefab. </summary>
        ///
     

        public void PreviousPrefab()
        {
            currentPrefabIndex--;
            if (currentPrefabIndex == -1)
            {
                currentPrefabIndex = Prefabs.Length - 1;
            }
            UpdateUI();
        }

        /// <summary>   Starts this object. </summary>
        ///
     

        private void Start()
        {
            originalRotation = transform.localRotation;
            UpdateUI();
        }

        /// <summary>   Updates this object. </summary>
        ///
     

        private void Update()
        {
            UpdateMovement();
            UpdateMouseLook();
            UpdateEffect();
        }

        /// <summary>   Slider changed. </summary>
        ///
     
        ///
        /// <param name="value">    The value. </param>

        public void SliderChanged(float value)
        {
            UpdateUI();
        }
    }
}