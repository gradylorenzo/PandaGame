using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PandaScript : MonoBehaviour
{
    public string pandaName = "Pumphrey";
    public int thirst = 1000;
    public int hunger = 1000;
    public int health = 60;

    public Canvas meterCanvas;
    public Image thirstMeter;
    public Image hungerMeter;
    public Image healthMeter;

    public float lastTick = 0.0f;

    private void Update()
    {
        doMeterUpdate();
    }

    private void doMeterUpdate()
    {
        thirstMeter.fillAmount = thirst / 1000.0f;
        hungerMeter.fillAmount = hunger / 1000.0f;
        healthMeter.fillAmount = health / 60.0f;
    }

    private void FixedUpdate()
    {
        doTick();
    }

    private void doTick()
    {
        if (lastTick + 1 < Time.time)
        {
            lastTick = Time.time;
            hunger = Mathf.Clamp(hunger - 1, 0, 1000);
            thirst = Mathf.Clamp(thirst - 1, 0, 1000);

            if(hunger < 1 || thirst < 1)
            {
                health = Mathf.Clamp(health - 1, 0, 60);
            }
            
            if(hunger > 250 && thirst > 250)
            {
                health = Mathf.Clamp(health + 1, 0, 60);
            }
        }
    }

    private void OnMouseOver()
    {
        meterCanvas.enabled = true;
    }
}
