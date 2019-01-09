using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (MapGenerator))] // 이 에디터 스크립트가 어떤 클래스나 스크립트를 다루는지 명시 
public class NewBehaviourScript : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MapGenerator map = target as MapGenerator; // CustomEditor 키워드로 이 에디터 스크립트가 다룰것이라 선언한 오브젝트는 target으로 접근할 수 있도록 자동 설정

        map.GenerateMap(); // GUI가 그려지는 매 프레임마다 map.GenerateMap() 호출 
    }
}
