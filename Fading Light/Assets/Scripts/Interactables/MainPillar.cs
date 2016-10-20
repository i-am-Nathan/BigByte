using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MainPillar : MonoBehaviour {

    private List<PillarPressurePlate> _activePillars = new List<PillarPressurePlate>(2);
    public GameObject arcGroup;
    public GameObject prefab;

    void Update()
    {
        if (_activePillars.Count == 2 && !_activePillars[0]._onCooldown && !_activePillars[1]._onCooldown)
        {
            arc();
        }
    }

    public void activatePillar(PillarPressurePlate pillar)
    {
        _activePillars.Add(pillar);

    }
    
    public void deactivatePillar(PillarPressurePlate pillar)
    {
        _activePillars.Remove(pillar);

    }

    public void arc()
    {

        Transform arc = arcGroup.transform.Find(_activePillars[0].pillarNumber.ToString() + _activePillars[1].pillarNumber.ToString());
        if (arc != null)
        {
            arc.GetComponent<ParticleSystem>().Play();
            arcGroup.transform.Find(_activePillars[1].pillarNumber.ToString() + _activePillars[0].pillarNumber.ToString()).GetComponent<ParticleSystem>().Play();

            Instantiate(prefab, _activePillars[0].transform.position, angleBetweenPillars(_activePillars[0].transform.position, _activePillars[1].transform.position));

            _activePillars[1].putOnCoolDown();
            _activePillars[0].putOnCoolDown();
        }   

    }

    private Quaternion angleBetweenPillars(Vector3 pillar1, Vector3 pillar2)
    {
        int offset = 0;
        if (pillar1.z> pillar2.z)
        {
            offset = 180;
        }

        double angle = (Math.Atan((pillar2.x - pillar1.x) / (pillar2.z - pillar1.z)))*180/Math.PI;
        angle = angle + offset;


        return Quaternion.Euler(new Vector3(0, (float)angle, 0));
    }

}
