using UnityEngine;

namespace WooshiiAttributes
{
    public class ExamplePropertyScript : MonoBehaviour
    {
        [ClassProperty] public bool BooleanProperty { get; set; }
        [ClassProperty] public string StringProperty { get; private set; } = "Hello World;";

        [ClassProperty] public int IntegerProperty { get; private set; } = 4444;
        [ClassProperty] public float FloatProperty { get; private set; } = 4444f;
        [ClassProperty] public double DoubleProperty { get; private set; } = 4444d;
        [ClassProperty] public long LongProperty { get; private set; } = 4444L;

        [ClassProperty] public DisplayMode EnumProperty { get; private set; } = DisplayMode.EDITOR;

        [ClassProperty] public Object ObjectProperty { get; private set; }

        [ClassProperty] public Vector2 Vector2Property { get; private set; } = Vector2.one;
        [ClassProperty] public Vector3 Vector3Property { get; private set; } = Vector3.one;
        [ClassProperty] public Vector4 Vector4Property { get; private set; } = Vector4.one;

        [ClassProperty] public Vector2Int Vector2IntProperty { get; private set; } = Vector2Int.one;
        [ClassProperty] public Vector3Int Vector3IntProperty { get; private set; } = Vector3Int.one;

        [ClassProperty] public Color ColorProperty { get; private set; } = Color.red;
        [ClassProperty] public Gradient GradientProperty { get; private set; } = new Gradient ();

        [ClassProperty] public LayerMask LayerProperty { get; private set; } = ~1;
        [ClassProperty] public AnimationCurve CurveProperty { get; private set; } = new AnimationCurve ()
        {
            keys = new Keyframe[]
            {
                new Keyframe(1f, 1f),
                new Keyframe(0.5f, 0.5f),
                new Keyframe(1f, 1f),
            }
        };

        [ClassProperty] public Rect RectProperty { get; set; }
        [ClassProperty] public RectInt RectIntProperty { get; set; }

        [ClassProperty] public Bounds boundsa { get; set; }
        [ClassProperty] public BoundsInt boundsb { get; set; }

        public void Update()
        {
            float delta = Time.deltaTime;

            IntegerProperty++;

            FloatProperty += delta;
            DoubleProperty += delta;

            Vector3Property += Vector3.up;
        }
    }
}
