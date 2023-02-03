using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class EffectSpawner
{
    private static readonly Dictionary<string, GameObject> effects = new Dictionary<string, GameObject>();

    public static GameObject SpawnFightEffect(Vector3 position)
    {
        if (!effects.Keys.Contains("FightEffect"))
            effects.Add("FightEffect", Resources.Load<GameObject>("Effect/FightEffect"));

        return Object.Instantiate(effects["FightEffect"], position, Quaternion.identity);
    }
}