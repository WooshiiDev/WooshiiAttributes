using UnityEngine;

namespace WooshiiAttributes
{
    public class ExamplePropertyScript : MonoBehaviour
    {
        [NativeProperty] public bool BooleanProperty { get; set; }
        [NativeProperty] public string StringProperty { get; private set; } = "Hello World;";

        [NativeProperty] public int IntegerProperty { get; private set; } = 4444;
        [NativeProperty] public float FloatProperty { get; private set; } = 4444f;
        [NativeProperty] public double DoubleProperty { get; private set; } = 4444d;
        [NativeProperty] public long LongProperty { get; private set; } = 4444L;

        [NativeProperty] public DisplayMode EnumProperty { get; private set; } = DisplayMode.EDITOR;

        [NativeProperty] public Object ObjectProperty { get; private set; }

        [NativeProperty] public Vector2 Vector2Property { get; private set; } = Vector2.one;
        [NativeProperty] public Vector3 Vector3Property { get; private set; } = Vector3.one;
        [NativeProperty] public Vector4 Vector4Property { get; private set; } = Vector4.one;

        [NativeProperty] public Vector2Int Vector2IntProperty { get; private set; } = Vector2Int.one;
        [NativeProperty] public Vector3Int Vector3IntProperty { get; private set; } = Vector3Int.one;

        [NativeProperty] public Color ColorProperty { get; private set; } = Color.red;
        [NativeProperty] public Gradient GradientProperty { get; private set; } = new Gradient ();

        [NativeProperty] public LayerMask LayerProperty { get; private set; } = ~1;
        [NativeProperty] public AnimationCurve CurveProperty { get; private set; } = new AnimationCurve ()
        {
            keys = new Keyframe[]
            {
                new Keyframe(1f, 1f),
                new Keyframe(0.5f, 0.5f),
                new Keyframe(1f, 1f),
            }
        };

        [NativeProperty] public Rect RectProperty { get; set; }
        [NativeProperty] public RectInt RectIntProperty { get; set; }

        [NativeProperty] public Bounds boundsa { get; set; }
        [NativeProperty] public BoundsInt boundsb { get; set; }

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
