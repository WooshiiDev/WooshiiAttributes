﻿using System;

namespace WooshiiAttributes
{
    [AttributeUsage (AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class EndGroupAttribute : GlobalAttribute
    {
        public EndGroupAttribute()
        {
        }
    }
}