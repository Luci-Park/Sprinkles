using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team { strawberry, mint, none};

public static class TeamController
{
    static Color[] teamColors = { Color.magenta, new Color(148f / 255f, 253f/255f, 234f/255f), Color.white };

    public static Color GetTeamColor(Team color)
    {
        return teamColors[(int)color];
    }
}
