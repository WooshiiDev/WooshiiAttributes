
<h1 align="center">WooshiiAttributes</h1>
<h4 align="center">A growing collection of flexible, powerful Unity attributes</h4>
<p align="center">
 <a href="https://unity3d.com">
  <img src="https://img.shields.io/badge/Made%20with-Unity-grey.svg?style=for-the-badge&logo=unity" alt="Unity Link">
 <a href="https://unity3d.com/get-unity/download">
  <img src="https://img.shields.io/badge/Supported-2018.4+-grey.svg?style=for-the-badge&logo=unity" alt="Unity Download Link">
 <a href="https://github.com/WooshiiDev/WooshiiAttributes/blob/main/LICENSE">
 <img src="https://img.shields.io/badge/License-MIT-brightgreen.svg?style=for-the-badge" alt="License MIT">
</p>

<p align="center"> 
  <a href="#about">About</a> •
  <a href="#installation">Installation</a> •
  <a href="#attributes">Attributes</a>
</p>

<p align="center"> 
 <img src="https://github.com/WooshiiDev/WooshiiAttributes/blob/readme-assets/.github/README_Attr_Full_Example.png" alt="Example Image">
</p>

## About

<p>
 WooshiiAttributes is a collection of attributes to flexibility and better visualisation in the Unity Inspector.
</p>

<p>
 Not only does WooshiiAttribute add general attributes for headers, dropdowns or for providing extra information, but also allows attributes to manipulate how information is displayed; local and global groups, and also attributes to modify how arrays show rather than the elements themselves. See <a href="#attributes">attributes</a> for examples,
</p>

<p>Features, systems and new attribute types will be added to this repository as development continues.</p>
 
## Installation
 
<p>
 For now you can either download using the zip or check <a href="https://github.com/WooshiiDev/WooshiiAttributes/releases">releases</a> for all Unity Packages.
</p>

## Attributes

<p align="center"> 
  <a href="#properties">Properties</a> •
  <a href="#decorators">Decorators</a> •
  <a href="#groups">Groups</a> •
  <a href="#globals">Globals</a> •
  <a href="#arrays">Arrays</a> •
  <a href="#methods">Methods</a>
</p>

### Properties

#### Basics

Vector3 and Vector2 fields can be clamped within a range.
Integers and floats can also be clamped, but also have a slider attribute if preferred.

```cs
[IntClamp (0, 10, true)] public int clampedInteger;
[FloatClamp (0, 10, true)] public float clampedFloat;

[IntSlider (0, 10)] public int integerSlider;
[FloatSlider (0, 10)] public float floatSlider;

[Vector2Clamp (0, 10)] public Vector2 clampedVector2;
[Vector3Clamp (0, 10)] public Vector3 clampedVector3;
```

<a>
 <img src="https://github.com/WooshiiDev/WooshiiAttributes/blob/readme-assets/.github/PropertyDrawers/README_Attr_Basics_Example.png">
</a>

#### ReadOnly 

Readonly restricts field editing, and can be set to hide depending on the current state of the editor.

```cs
[ReadOnly (DisplayMode.BOTH)] public string readOnlyAll = "Can see me at all times. Can't edit me though.";
[ReadOnly (DisplayMode.EDITOR)] public string readOnlyEditor = "Can see me in the Editor when not playing only.";
[ReadOnly (DisplayMode.PLAYING)] public string readOnlyPlay = "Can see me when Playing only.";
```

<a>
 <img src="https://github.com/WooshiiDev/WooshiiAttributes/blob/readme-assets/.github/PropertyDrawers/README_Attr_ReadOnly_Example.png">
</a>

#### Array Elements 

Array Elements will add direct removal or addition buttons to each element of a collection in the inspector.

```cs
[ArrayElements] public float[] arrayElements;
```

<a>
 <img src="https://github.com/WooshiiDev/WooshiiAttributes/blob/readme-assets/.github/PropertyDrawers/README_Attr_ArrayElements_Example.png">
</a>

### Decorators

#### HeaderLine 

Header Lines appear like normal headers, but keep the header capitalized and underlined to show a more clear, sharp visualisation between sections.

```cs
[HeaderLine ("ReadOnly")]
[ReadOnly (DisplayMode.BOTH)] public string readOnlyAll = "Can see me at all times. Can't edit me though.";
```

<a>
 <img src="https://github.com/WooshiiDev/WooshiiAttributes/blob/readme-assets/.github/DecoratorDrawers/README_Attr_HeaderLine_Example.png.png">
</a>

### Paragraph

Paragraph's will display text contained in a box above a field. The background colour, text colour and text alignment can be set in the attribute too.
Please note that the colours defined are hexadecimal.

```cs
[Paragraph ("This be a string with a paragraph. Go ahead. Type stuff. Yes.", "#D2D2D2", "#1000FF")]
public string stringWithParagraph;
```

<a>
 <img src="https://github.com/WooshiiDev/WooshiiAttributes/blob/readme-assets/.github/DecoratorDrawers/README_Attr_Paragraph_Example.png">
</a>

#### Comment

Comments draw helpboxes to the inspector, allowing information to be provided about fields with an icon for the message type.

```cs
[Comment ("This is an integer.\nAmazing. Easy. Simple.", CommentAttribute.MessageType.INFO)]
public int intValue;

[Comment ("This is a string.\nCareful - can disguise itself with ToString()", CommentAttribute.MessageType.WARNING)]
public string stringValue;

[Comment ("Toggle value.\nTake caution when editing. Can be indecisive. Also likes to bite.", CommentAttribute.MessageType.ERROR)]
public bool boolValue;
```

<a>
 <img src="https://github.com/WooshiiDev/WooshiiAttributes/blob/readme-assets/.github/DecoratorDrawers/README_Attr_Comment_Example.png">
</a>

### Groups

Local Groups can be created using attributes that are named with `BeginGroup` and `EndGroup` respectively to begin and end a group. `BeginGroup` has optional parameters to set capitalization, whether the title is underlined, and if the title is within the group box or not.

```cs
[BeginGroup ("Group of stuff", true, true, true)]
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

[EndGroup()]
public int k;
```
 
<a>
 <img src="https://github.com/WooshiiDev/WooshiiAttributes/blob/readme-assets/.github/Groups/README_Attr_LocalGroup_Example.png">
</a>

### Globals

Globals are attributes that affect a group of members together no matter where they are in the script. Any field that uses a global attribute, will be displayed at the bottom of the script.

Globals need to be applied to each field manually however, which may be slower. If you're specifically using globals for groups, see <a href="#groups">Groups</a> for local versions.

#### HeaderGroup

Groups all fields under a header. 

```cs
[HeaderGroup ("Header Group Stats")] public int otherHealth, otherSpeed, otherDamage;
```

<a>
 <img src="https://github.com/WooshiiDev/WooshiiAttributes/blob/readme-assets/.github/GlobalDrawers/README_Attr_GlobHeader_Example.png">
</a>

#### HeaderLineGroup

Groups all fields under a header but formatted like the `HeaderLine` attribute.

```cs
[HeaderLineGroupAttribute ("Header Line Group Stats")] public int health, speed, damage;
```

<a>
 <img src="https://github.com/WooshiiDev/WooshiiAttributes/blob/readme-assets/.github/GlobalDrawers/README_Attr_GlobHeaderLine_Example.png">
</a>

#### ContainedGroup

Contains the fields within a box.

```cs
[ContainedGroup ("Contained Group Stats")] public int containedHealth, containedSpeed, containedDamage;
```

<a>
 <img src="https://github.com/WooshiiDev/WooshiiAttributes/blob/readme-assets/.github/GlobalDrawers/README_Attr_GlobContained_Example.png">
</a>

#### FoldoutGroup

Group that has a foldout to hide or show the fields when required.

```cs
[FoldoutGroup ("Foldout Group Stats")] public int foldedHealth, foldedSpeed, foldedDamage;
```

<a>
 <img src="https://github.com/WooshiiDev/WooshiiAttributes/blob/readme-assets/.github/GlobalDrawers/README_Attr_GlobFoldout_Example.png">
</a>

### Arrays

Array drawers unlike property drawers work on the array itself rather than the elements that are in the array.

#### Reorderable

Allows for reorderable lists and arrays in the inspector.

```cs
[Reorderable] public ExampleData[] childClassArray;
```

<a>
 <img src="https://github.com/WooshiiDev/WooshiiAttributes/blob/readme-assets/.github/ArrayDrawers/README_Attr_Reorderable_Example.png">
</a>

### Methods

#### MethodButton

Method buttons will display a button for each method given a `MethodButton`. These can be private or public buttons too. The attribute has an optional string parameter allowing you to name the button that will appear in the inspector. By default it will take the method name.

```cs
[MethodButton ()]
public void ExampleMethod()
{
    Debug.Log ("Example Method");
}
```

<a>
 <img src="https://github.com/WooshiiDev/WooshiiAttributes/blob/readme-assets/.github/Methods/README_Attr_Button_Example.png">
</a>
