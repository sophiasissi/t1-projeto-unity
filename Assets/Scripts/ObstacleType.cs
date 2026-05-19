using UnityEngine;

public class ObstacleType : MonoBehaviour
{
    public enum RequiredAction
    {
        None,
        Jump
    }

    [Header("Ação necessária para passar pelo obstáculo")]
    public RequiredAction requiredAction = RequiredAction.None;
}