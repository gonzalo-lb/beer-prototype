using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AguaCanilla_ParticleSystem : MonoBehaviour
{

    [SerializeField] ParticleSystem particleSystem_AguaCanilla;
    ParticleSystem.EmissionModule emissionModule;

    float timer;

    // Start is called before the first frame update
    void Start()
    {
        emissionModule = particleSystem_AguaCanilla.emission;
        emissionModule.enabled = false;
        particleSystem_AguaCanilla.Play();
    }

    private void Update()
    {
        timer += Time.deltaTime;
    }

    public void _EnableEmissionParticleSystem()
    {
        if(timer < 1) { return; }

        timer = 0;
        emissionModule.enabled = true;
    }

    public void _DisableEmissionParticleSystem()
    {
        if (timer < 1) { return; }

        timer = 0;        
        emissionModule.enabled = false;
    }

    public bool _IsParticleSystemEmissionEnabled()
    {
        //return particleSystem_AguaCanilla.isPlaying;
        //var emission = particleSystem_AguaCanilla.emission;
        return emissionModule.enabled;
    }
}
