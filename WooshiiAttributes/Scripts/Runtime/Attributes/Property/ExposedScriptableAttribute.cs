using System;
using UnityEngine;

namespace WooshiiAttributes
{
    public class ExposedScriptableAttribute : PropertyAttribute
    {
        public readonly bool Foldout;

        public ExposedScriptableAttribute(bool foldout = true)
        {
            Foldout= foldout;
        }
    }
}
