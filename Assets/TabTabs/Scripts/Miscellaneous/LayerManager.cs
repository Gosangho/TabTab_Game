using UnityEngine;

public static class LayerManager
{
    public static LayerMask PlayerLayerMask => 1 << LayerMask.NameToLayer("Player");
    public static LayerMask MonsterLayerMask => 1 << LayerMask.NameToLayer("Monster");
}