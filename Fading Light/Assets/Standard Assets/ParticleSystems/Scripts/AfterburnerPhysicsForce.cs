// file:	Assets\Standard Assets\ParticleSystems\Scripts\AfterburnerPhysicsForce.cs
//
// summary:	Implements the afterburner physics force class

using System;
using UnityEngine;

namespace UnityStandardAssets.Effects
{
    /// <summary>   An afterburner physics force. </summary>
    ///
 

    [RequireComponent(typeof (SphereCollider))]
    public class AfterburnerPhysicsForce : MonoBehaviour
    {
        /// <summary>   The effect angle. </summary>
        public float effectAngle = 15;
        /// <summary>   Width of the effect. </summary>
        public float effectWidth = 1;
        /// <summary>   The effect distance. </summary>
        public float effectDistance = 10;
        /// <summary>   The force. </summary>
        public float force = 10;

        /// <summary>   The cols. </summary>
        private Collider[] m_Cols;
        /// <summary>   The sphere. </summary>
        private SphereCollider m_Sphere;

        /// <summary>   Executes the enable action. </summary>
        ///
     

        private void OnEnable()
        {
            m_Sphere = (GetComponent<Collider>() as SphereCollider);
        }

        /// <summary>   Fixed update. </summary>
        ///
     

        private void FixedUpdate()
        {
            m_Cols = Physics.OverlapSphere(transform.position + m_Sphere.center, m_Sphere.radius);
            for (int n = 0; n < m_Cols.Length; ++n)
            {
                if (m_Cols[n].attachedRigidbody != null)
                {
                    Vector3 localPos = transform.InverseTransformPoint(m_Cols[n].transform.position);
                    localPos = Vector3.MoveTowards(localPos, new Vector3(0, 0, localPos.z), effectWidth*0.5f);
                    float angle = Mathf.Abs(Mathf.Atan2(localPos.x, localPos.z)*Mathf.Rad2Deg);
                    float falloff = Mathf.InverseLerp(effectDistance, 0, localPos.magnitude);
                    falloff *= Mathf.InverseLerp(effectAngle, 0, angle);
                    Vector3 delta = m_Cols[n].transform.position - transform.position;
                    m_Cols[n].attachedRigidbody.AddForceAtPosition(delta.normalized*force*falloff,
                                                                 Vector3.Lerp(m_Cols[n].transform.position,
                                                                              transform.TransformPoint(0, 0, localPos.z),
                                                                              0.1f));
                }
            }
        }

        /// <summary>   Executes the draw gizmos selected action. </summary>
        ///
     

        private void OnDrawGizmosSelected()
        {
            //check for editor time simulation to avoid null ref
            if(m_Sphere == null)
                m_Sphere = (GetComponent<Collider>() as SphereCollider);

            m_Sphere.radius = effectDistance*.5f;
            m_Sphere.center = new Vector3(0, 0, effectDistance*.5f);
            var directions = new Vector3[] {Vector3.up, -Vector3.up, Vector3.right, -Vector3.right};
            var perpDirections = new Vector3[] {-Vector3.right, Vector3.right, Vector3.up, -Vector3.up};
            Gizmos.color = new Color(0, 1, 0, 0.5f);
            for (int n = 0; n < 4; ++n)
            {
                Vector3 origin = transform.position + transform.rotation*directions[n]*effectWidth*0.5f;

                Vector3 direction =
                    transform.TransformDirection(Quaternion.AngleAxis(effectAngle, perpDirections[n])*Vector3.forward);

                Gizmos.DrawLine(origin, origin + direction*m_Sphere.radius*2);
            }
        }
    }
}
