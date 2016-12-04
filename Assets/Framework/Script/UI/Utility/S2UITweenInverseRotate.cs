using UnityEngine;
using System.Collections;

public class S2UITweenInverseRotate : MonoBehaviour 
{
    public Transform m_trTarget = null;
    private Transform m_trMine = null;

	void Start() 
    {
        m_trMine = this.transform;
	}
	
    void Update()
    {
        if (null == m_trTarget || null == m_trMine)
            return;

        m_trMine.localRotation = Quaternion.Inverse(m_trTarget.localRotation);
    }
}
