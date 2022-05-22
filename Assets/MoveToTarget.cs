using UnityEngine;

public class MoveToTarget : MonoBehaviour
{
    private Transform m_target;
    private float m_speed =2;

    public void FindNearestTarget(Transform[] _players)
    {
        var closestDistance = 4000f;
        var possibleTarget = _players[0];
        foreach (var target in _players)
        {
            var currentDistance = Vector3.Distance(target.position, transform.position);
            if (currentDistance < closestDistance)
            {
                possibleTarget = target;
                closestDistance = currentDistance;
            }

            m_target = possibleTarget;

        }
    }

    private void Update()
    {
        var direction = Vector3.Normalize(-transform.position + m_target.position);
        transform.position += direction*m_speed*Time.deltaTime;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = rotation;
    }
}