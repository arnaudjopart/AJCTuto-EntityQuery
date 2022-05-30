using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Transform m_target;
    private float m_speed = 10;

    public void SetTarget(Transform _variable)
    {
        m_target = _variable;
    }

    private void Update()
    {
        var direction = Vector3.Normalize(m_target.position - transform.position);
        transform.position += direction * (m_speed * Time.deltaTime);

        if (Vector3.Distance(m_target.position, transform.position) < .1f)
        {
            m_target.gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}