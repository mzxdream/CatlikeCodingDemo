using UnityEngine;

public class Tower : GameTileContent
{
    [SerializeField, Range(1.5f, 10.5f)]
    float targetingRange = 1.5f;
    TargetPoint target;
    static Collider[] targetsBuffer = new Collider[1];
    public override void GameUpdate()
    {
        base.GameUpdate();
        //Debug.Log("Searching for target...");
        if (TrackTarget() || AcquireTarget())
        {
            Debug.Log("Locked on target!");
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 position = transform.localPosition;
        position.y += 0.01f;
        Gizmos.DrawWireSphere(position, targetingRange);
        if (target != null)
        {
            Gizmos.DrawLine(position, target.Position);
        }
    }
    bool AcquireTarget()
    {
        Vector3 a = transform.localPosition;
        Vector3 b = a;
        b.y += 3f;
        const int enemyLayerMask = 1 << 9;
        int hits = Physics.OverlapCapsuleNonAlloc(a, b, targetingRange, targetsBuffer, enemyLayerMask);
        if (hits > 0)
        {
            target = targetsBuffer[0].GetComponent<TargetPoint>();
            Debug.Assert(target != null, "Targeted non-enemy!", targetsBuffer[0]);
            return true;
        }
        target = null;
        return false;
    }
    bool TrackTarget()
    {
        if (target == null)
        {
            return false;
        }
        Vector3 a = transform.localPosition;
        Vector3 b = target.Position;
        float x = a.x - b.x;
        float z = a.z - b.z;
        float r = targetingRange + 0.125f * target.Enemy.Scale;
        if (x * x + z * z > r * r)
        {
            target = null;
            return false;
        }
        return true;
    }
}