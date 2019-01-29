using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (MapGenerator))] // 이 에디터 스크립트가 어떤 클래스나 스크립트를 다루는지 명시 
public class NewBehaviourScript : Editor {

    public override void OnInspectorGUI()
    {

        MapGenerator map = target as MapGenerator; // CustomEditor 키워드로 이 에디터 스크립트가 다룰것이라 선언한 오브젝트는 target으로 접근할 수 있도록 자동 설정

        if (DrawDefaultInspector())
        {
            map.GenerateMap();
        }
        if(GUILayout.Button("Generate Map"))
        {
            map.GenerateMap();
        }
    }
}
