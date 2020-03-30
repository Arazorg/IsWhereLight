using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using CodeMonkey.Utils;

public class UnitMoveOrderSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Entities.ForEach((Entity entity, ref Translation translation) => {

                EntityManager.AddComponentData(entity, new PathfindingParams
                {
                    startPosition = new int2(4, 0),
                    endPosition = new int2(20, 10)
                });
            });
        }
    }
}
