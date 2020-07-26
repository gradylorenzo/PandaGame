using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PandaEvents
{
    public delegate void PandaEvent(PandaScript ps);
    public static PandaEvent pandaHungry;
    public static PandaEvent pandaThirsty;
    public static PandaEvent pandaHurting;
    public static PandaEvent changeActivePanda;

    public delegate void WorldPointClicked(Vector3 pos);
    public static WorldPointClicked worldPointClicked;
}
