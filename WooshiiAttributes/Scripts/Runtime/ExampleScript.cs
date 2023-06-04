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

        // ============ Groups ============
        [GlobalGroup ("Header Line Group Stats", true, false, false, false)] public int health, speed, damage;
        [GlobalGroup ("Header Group Stats", true, true)] public int otherHealth, otherSpeed, otherDamage;
        [GlobalGroup ("Contained Group Stats", true, true, true)] public int containedHealth, containedSpeed, containedDamage;
        [GlobalGroup ("Foldout Group Stats", true, true, true, true)] public int foldedHealth, foldedSpeed, foldedDamage;
        [GlobalGroup ("Foldout Group Stats")] public ExampleData data;

        // ============ ReadOnly ============
        [Group("ReadOnly", true, true, true)]
        [ReadOnly (DisplayMode.BOTH)] public string readOnlyAll = "Can see me at all times. Can't edit me though.";
        [ReadOnly (DisplayMode.EDITOR)] public string readOnlyEditor = "Can see me in the Editor when not playing only.";
        [EndGroup][ReadOnly (DisplayMode.PLAYING)] public string readOnlyPlay = "Can see me when Playing only.";

        [Group ("Group of stuff", true, true, true)]
        public int a;
        public int b;
        public int c;
        public int d;
        public int e;
        public int f;
        public int g;
        public int h;
        public int i;

        public ExampleData[] j;

        [EndGroup ()] public int k;

        // ============ Basic Data Types ============

        [Group ("Basic Types", true, true, true)]
        [IntClamp (0, 10, true)] public int clampedInteger;
        [FloatClamp (0, 10, true)] public float clampedFloat;

        [IntSlider (0, 10)] public int integerSlider;
        [FloatSlider (0, 10)] public float floatSlider;
        
        [Vector2Clamp (0, 10)] public Vector2 clampedVector2;
        [EndGroup][Vector3Clamp (0, 10)] public Vector3 clampedVector3;
        

        // ============ Comment Examples ============

        [Group ("Comment Examples", true, true, true)]
        [Paragraph("This be a string with a paragraph. Go ahead. Type stuff. Yes.", "#D2D2D2", "#1000FF")]
        public string stringWithParagraph;

        [Comment ("Get yer 2D space here!", CommentAttribute.MessageType.NONE)]
        [HeaderLine ("Comments")]
        public Vector2 vectorValue;

        [Comment ("This is an integer.\nAmazing. Easy. Simple.", CommentAttribute.MessageType.INFO)]
        public int intValue;

        [Comment ("This is a string.\nCareful - can disguise itself with ToString()", CommentAttribute.MessageType.WARNING)]
        public string stringValue;

        [Comment ("Toggle value.\nTake caution when editing. Can be indecisive. Also likes to bite.", CommentAttribute.MessageType.ERROR)]
        [EndGroup ()] public bool boolValue;

        // ============ Array Based Examples ============

        [Group("Collections", true, true, true)]
        [ArrayElements] public float[] arrayElements;

        [Reorderable] public ExampleData[] childClassArray;
        [Reorderable] public int[] intArray;

        // ============ Methods ============

        [MethodButton ()]
        public void ExampleMethod()
        {
            Debug.Log ("Example Method");
        }

        [MethodButton ()]
        public void ExampleMethod(int a, int b)
        {
            Debug.Log (string.Format("{0} + {1} = {2}", a, b, a + b));
        }

        [MethodButton ("Example Method With Parameters", 10, "Hello World", true)]
        public void AnotherExampleMethod(int intParameter, string stringParameter, bool booleanParameter)
        {
            Debug.Log (intParameter);
            Debug.Log (stringParameter);
            Debug.Log (booleanParameter);
        }

        [MethodButton ("Example Method With Complex Parameters", 10, "Hello World", true)]
        public void AComplexMethod(float evalTime, Gradient gradient, AnimationCurve curve)
        {
            string format = string.Format (
                "Gradient Colour - {0}, Animation Curve Value - {1}", gradient.Evaluate (evalTime), curve.Evaluate (evalTime));

            Debug.Log (format);

            transform.position += Vector3.up * 10;
        }
    }
}
