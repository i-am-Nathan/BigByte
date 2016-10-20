// file:	Assets\DownloadedContent\PyroParticles\Prefab\Script\FireConstantBaseScript.cs
//
// summary:	Implements the fire constant base script class

using UnityEngine;
using System.Collections;

namespace DigitalRuby.PyroParticles
{
    /// <summary>
    /// Provides an easy wrapper to looping audio sources with nice transitions for volume when
    /// starting and stopping.
    /// </summary>
    ///
 

    public class LoopingAudioSource
    {
        /// <summary>   Gets or sets the audio source. </summary>
        ///
        /// <value> The audio source. </value>

        public AudioSource AudioSource { get; private set; }

        /// <summary>   Gets or sets target volume. </summary>
        ///
        /// <value> The target volume. </value>

        public float TargetVolume { get; private set; }

        /// <summary>   The start multiplier. </summary>
        private float startMultiplier;
        /// <summary>   The stop multiplier. </summary>
        private float stopMultiplier;
        /// <summary>   The current multiplier. </summary>
        private float currentMultiplier;

        /// <summary>   Constructor. </summary>
        ///
     
        ///
        /// <param name="script">           The script. </param>
        /// <param name="audioSource">      The audio source. </param>
        /// <param name="startMultiplier">  The start multiplier. </param>
        /// <param name="stopMultiplier">   The stop multiplier. </param>

        public LoopingAudioSource(MonoBehaviour script, AudioSource audioSource, float startMultiplier, float stopMultiplier)
        {
            AudioSource = audioSource;
            if (audioSource != null)
            {
                AudioSource.loop = true;
                AudioSource.volume = 0.0f;
                AudioSource.Stop();
            }

            TargetVolume = 1.0f;

            this.startMultiplier = currentMultiplier = startMultiplier;
            this.stopMultiplier = stopMultiplier;
        }

        /// <summary>   Plays. </summary>
        ///
     

        public void Play()
        {
            Play(TargetVolume);
        }

        /// <summary>   Plays. </summary>
        ///
     
        ///
        /// <param name="targetVolume"> Target volume. </param>

        public void Play(float targetVolume)
        {
            if (AudioSource != null && !AudioSource.isPlaying)
            {
                AudioSource.volume = 0.0f;
                AudioSource.Play();
                currentMultiplier = startMultiplier;
            }
            TargetVolume = targetVolume;
        }

        /// <summary>   Stops this object. </summary>
        ///
     

        public void Stop()
        {
            if (AudioSource != null && AudioSource.isPlaying)
            {
                TargetVolume = 0.0f;
                currentMultiplier = stopMultiplier;
            }
        }

        /// <summary>   Updates this object. </summary>
        ///
     

        public void Update()
        {
            if (AudioSource != null && AudioSource.isPlaying &&
                (AudioSource.volume = Mathf.Lerp(AudioSource.volume, TargetVolume, Time.deltaTime / currentMultiplier)) == 0.0f)
            {
                AudioSource.Stop();
            }
        }
    }

    /// <summary>
    /// Script for objects such as wall of fire that never expire unless manually stopped.
    /// </summary>
    ///
 

    public class FireConstantBaseScript : FireBaseScript
    {
        /// <summary>   The looping audio source. </summary>
        [HideInInspector]
        public LoopingAudioSource LoopingAudioSource;

        /// <summary>   Awakes this object. </summary>
        ///
     

        protected override void Awake()
        {
            base.Awake();

            // constant effect, so set the duration really high and add an infinite looping sound
            LoopingAudioSource = new LoopingAudioSource(this, AudioSource, StartTime, StopTime);
            Duration = 999999999;
        }

        /// <summary>   Updates this object. </summary>
        ///
     

        protected override void Update()
        {
            base.Update();

            LoopingAudioSource.Update();
        }

        /// <summary>   Starts this object. </summary>
        ///
     

        protected override void Start()
        {
            base.Start();

            LoopingAudioSource.Play();
        }

        /// <summary>   Stops this object. </summary>
        ///
     

        public override void Stop()
        {
            LoopingAudioSource.Stop();

            base.Stop();
        }
    }
}