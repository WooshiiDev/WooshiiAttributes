using UnityEngine;

#pragma warning disable CS0649

namespace WooshiiAttributes
{
    internal class ExampleScript : MonoBehaviour
    {
        [System.Serializable]
        public class ExampleData
        {
            public string name;
            public int intVal;
            public bool boolVal;

            public ExampleData childData;
        }

        // ============ ReadOnly Examples ============
        [HeaderLine ("ReadOnly")]
        [ReadOnly (DisplayMode.BOTH)] public string readOnlyAll = "Can see me at all times. Can't edit me though.";
        [ReadOnly (DisplayMode.EDITOR)] public string readOnlyEditor = "Can see me in the Editor when not playing only.";
        [ReadOnly (DisplayMode.PLAYING)] public string readOnlyPlay = "Can see me when Playing only.";

        // ============ Group Examples ============

        [HeaderLineGroup ("Header Line Group Stats")] public int health, speed, damage;
        [HeaderGroup ("Header Group Stats")] public int otherHealth, otherSpeed, otherDamage;
        [ContainedGroup ("Contained Group Stats")] public int containedHealth, containedSpeed, containedDamage;
        [FoldoutGroup ("Foldout Group Stats")] public int foldedHealth, foldedSpeed, foldedDamage;

        [HeaderLine ("Basics Types")]
        [Vector2Clamp (0, 10)] public Vector2 clampedVector2;

        [Vector3Clamp (0, 10)] public Vector3 clampedVector3;

        // ============ Comment Examples ============
        [Comment ("This is an integer.\nAmazing. Easy. Simple.", CommentAttribute.MessageType.INFO)]
        [HeaderLine ("Comments")]
        public int intValue;

        [Comment ("This is a string.\nCareful - can disguise itself with ToString()", CommentAttribute.MessageType.WARNING)]
        public string stringValue;

        [Comment ("Toggle value.\nTake caution when editing. Can be indecisive. Also likes to bite.", CommentAttribute.MessageType.ERROR)]
        public bool boolValue;

        // ============ Array Based Examples ============
        [HeaderLine ("Array Based")]
        [ArrayElements] public float[] arrayElements;

        [Reorderable] public ExampleData[] childClassArray;
        [Reorderable] public int[] intArray;

        [MethodButton()]
        public void ExampleMethod()
        {
            Debug.Log ("Example Method");
        }
    }
}