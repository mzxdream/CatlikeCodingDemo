using UnityEngine;

[CreateAssetMenu]
public class EnemyAnimationConfig : ScriptableObject
{
    [SerializeField]
    AnimationClip move = default;
    public AnimationClip Move => move;
}