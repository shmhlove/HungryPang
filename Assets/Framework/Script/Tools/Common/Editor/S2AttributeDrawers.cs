/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 05월 29일
★ E-Mail ☞ shmhlove@neowiz.com
★ Desc   ☞ 이 클래스는 MonoBehaviour를 커스텀하여 유니티 인스펙터를 제어할 수 있게 합니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

// 인스펙터에 함수이름을 버튼형식으로 노출시킵니다.
[CanEditMultipleObjects]
[CustomEditor(typeof(MonoBehaviour), true)]
public class S2DrawerToShowFunc : Editor
{
    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();

        InspectorGUIToFunctionButton();
    }

    void InspectorGUIToFunctionButton()
    {
        Type pType              = target.GetType();
        MethodInfo[] pMethods   = pType.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        for (int iLoop = 0; iLoop < pMethods.Length; ++iLoop)
        {
            MethodInfo pMethod  = pMethods[iLoop];
            object[] pAttribute = pMethod.GetCustomAttributes(typeof(S2AttributeToShowFunc), true);
            if (0 >= pAttribute.Length)
                continue;

            if (true == GUILayout.Button(pMethod.Name))
            {
                ((Component)target).SendMessage(pMethod.Name, SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}

// 인스펙터에 몬스터 상태를 콤보박스 형태로 노출시킵니다.
[CustomPropertyDrawer(typeof(S2AttributeToMonsterState))]
public class S2DrawerToMonsterState : PropertyDrawer
{
    S2MonsterInspector m_pMonster = null;

    public override void OnGUI(Rect pRect, SerializedProperty pProperty, GUIContent pLabel)
    {
        if (SerializedPropertyType.String != pProperty.propertyType)
        {
            EditorGUI.LabelField(pRect, "ERROR:", "May only apply to type string");
            return;
        }

        if (pProperty.serializedObject.targetObject is Component)
        {
            Component pComponent = pProperty.serializedObject.targetObject as Component;
            m_pMonster = pComponent.GetComponent<S2MonsterInspector>();
        }

        if (null == m_pMonster)
        {
            EditorGUI.LabelField(pRect, "ERROR:", "Must have reference to a SkeletonDataAsset");
            return;
        }

        string strButtonName = pProperty.stringValue;
        if (true == string.IsNullOrEmpty(strButtonName))
            strButtonName = "Select To DevPlayState Type";

        pRect = EditorGUI.PrefixLabel(pRect, pLabel);
        if (GUI.Button(pRect, strButtonName, EditorStyles.popup))
        {
            Selector(pProperty);
        }
    }

    void Selector(SerializedProperty property)
    {
        if (null == m_pMonster)
            return;

        GenericMenu pMenu   = new GenericMenu();
        List<string> pState = m_pMonster.GetState();
        foreach(string strMenu in pState)
        {
            pMenu.AddItem(new GUIContent(strMenu), strMenu == property.stringValue, HandleSelect, strMenu);
        }
        
        pMenu.ShowAsContext();
    }

    void HandleSelect(object val)
    {
        m_pMonster.m_eDevStateID = (string)val;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 18;
    }
}