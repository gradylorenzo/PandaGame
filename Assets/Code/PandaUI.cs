using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PandaUI : MonoBehaviour
{
    private void Awake()
    {
        PandaEvents.pandaHungry += pandaHungry;
        PandaEvents.pandaThirsty += pandaThirsty;
        PandaEvents.pandaHurting += pandaHurting;
    }

    public void pandaHungry(PandaScript ps)
    {
        print(ps.pandaName + " is hungry");
    }

    public void pandaThirsty(PandaScript ps)
    {
        print(ps.pandaName + " is thirsty");
    }

    public void pandaHurting(PandaScript ps)
    {
        print(ps.pandaName + " is hurting");
    }
}
