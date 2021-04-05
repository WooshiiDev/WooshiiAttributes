
<h1 align="center">WooshiiAttributes</h1>
<h4 align="center">My growing collection of Unity attributes used in projects.</h4>
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
 <img src="https://i.imgur.com/P9QFjAA.png?1" alt="Example Image">
</p>

## About

<p>
 WooshiiAttributes is my collection of attributes that I've either used in previous projects or have wanted to add myself to extend Unity's Inspector functionality.
</p>

<p>
 Not only does WooshiiAttribute add general attributes for headers, dropdowns or for providing extra information, but also allows attributes to manipulate how information is displayed; local and global groups, and also attributes to modify how arrays show rather than the elements themselves. See <a href="#attributes">attributes</a> for examples,
</p>

<p>This will grow as new features and attributes are added to it. Pretty much will just keep adding as long as there's a reason to add new attributes.</p>
 
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

#### ReadOnly 

<a>
 <img src="https://github.com/WooshiiDev/WooshiiAttributes/blob/readme-assets/.github/README_Attr_Comment_Example.png">
</a>

### Decorators

#### HeaderLine 

Header Lines appear like normal headers, but keep the header capitalized and underlined to show a more clear, sharp visualisation between sections.

#### Comment

Comments draw helpboxes to the inspector, allowing information to be provided about fields with an icon for the message type.

<a>
 <img src="https://github.com/WooshiiDev/WooshiiAttributes/blob/readme-assets/.github/README_Attr_Comment_Example.png">
</a>

### Groups

Local Groups can be created using any attribute that has `BeginGroup` and `EndGroup` naming. Groups will be created in the structure they're stated in the script. All groups have optional parameters:
 - `groupedTitle`: Is the title shown inside or outside of the group container
 - `upperTitle`: Is the title fully capitalized 
 - `underlineTitle` : Is the title underlined. This appears just like `HeaderLine` or `HeaderLineGroup` attributes.
<a>
 <img src="https://github.com/WooshiiDev/WooshiiAttributes/blob/readme-assets/.github/README_Attr_LocalGroup_Example.png">
</a>

### Globals

Please note that some of the global attributes defined here are named with "group". This is just because the following are "global groups" where attributes that provide the same group name will be drawn together.

The Global Group Attributes differ to the Local Group Attributes in functionality. Name changing in the future may happen.

#### HeaderGroup

Will display all fields with the provided `HeaderGroup` attribute together with a header. 

<a>
 <img src="https://github.com/WooshiiDev/WooshiiAttributes/blob/readme-assets/.github/README_Attr_GlobHeader_Example.png">
</a>

#### HeaderLineGroup

The same as the `HeaderGroup` attribute but with an underline.

<a>
 <img src="https://github.com/WooshiiDev/WooshiiAttributes/blob/readme-assets/.github/README_Attr_GlobHeaderLine_Example.png">
</a>

#### ContainedGroup

Contains the group within a box.

<a>
 <img src="https://github.com/WooshiiDev/WooshiiAttributes/blob/readme-assets/.github/README_Attr_GlobContained_Example.png">
</a>

#### FoldoutGroup

Group that has a foldout to hide or show the fields when required.
<a>
 <img src="https://github.com/WooshiiDev/WooshiiAttributes/blob/readme-assets/.github/README_Attr_GlobFoldout_Example.png">
</a>

### Arrays

#### Reorderable

<a>
 <img src="https://github.com/WooshiiDev/WooshiiAttributes/blob/readme-assets/.github/README_Attr_Reorderable_Example.png">
</a>

### Methods

#### MethodButton

Method buttons will display a button for each method given a `MethodButton`. These can be private or public buttons too. The attribute has an optional string parameter allowing you to name the button that will appear in the inspector. By default it will take the method name.

<Todo>
