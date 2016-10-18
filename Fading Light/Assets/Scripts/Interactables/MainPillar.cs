using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainPillar : MonoBehaviour {

    private List<PillarPressurePlate> _activePillars = new List<PillarPressurePlate>(2);
    public GameObject arcGroup;

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
        Debug.Log("arc: " + _activePillars[0].pillarNumber + " " + _activePillars[1].pillarNumber);
        Transform arc = arcGroup.transform.Find(_activePillars[0].pillarNumber.ToString() + _activePillars[1].pillarNumber.ToString());
        if (arc != null)
        {
            arc.GetComponent<ParticleSystem>().Play();
            arcGroup.transform.Find(_activePillars[1].pillarNumber.ToString() + _activePillars[0].pillarNumber.ToString()).GetComponent<ParticleSystem>().Play();
            _activePillars[1].putOnCoolDown();
            _activePillars[0].putOnCoolDown();
        }   

    }

}
