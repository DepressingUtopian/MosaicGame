using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotController : MonoBehaviour
{
    public float gizmoSize = 0.75f;
    public Color gizmoColor = Color.yellow;
    // Start is called before the first frame update
    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position,gizmoSize);
    }
}
