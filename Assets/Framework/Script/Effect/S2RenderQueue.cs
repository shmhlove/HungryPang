using UnityEngine;
using System.Collections;

public class S2RenderQueue : MonoBehaviour
{	
	public int m_RenderQueue = 3000;
    	
	void Start()
    {
        SetQueue();
    }

    [S2AttributeToShowFunc]
    void SetQueue()
    {
        Transform[] pTransform = transform.GetComponentsInChildren<Transform>();
        foreach (Transform tr in pTransform)
        {
            MeshRenderer pRenderer = tr.gameObject.GetComponent<MeshRenderer>();
            if (null == pRenderer)
                continue;

            if (null == pRenderer.sharedMaterial)
                continue;
            
            pRenderer.sharedMaterial.renderQueue = m_RenderQueue;
        }
    }
}
