using System;
using UnityEngine;

namespace Shared.Editor
{
    /// <summary>
    ///     Add this property tag to a variable
    ///     to create a dependency chain.
    /// </summary>
    /// <remarks>
    ///     Note: This does not work on Lists and Arrays
    /// </remarks>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
    AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
    public sealed class DependsUponAttribute : PropertyAttribute
    {
        public readonly string[] attributes;
        public readonly bool[] predicates;
        public readonly object[] objs;
        
        /// <summary>
        ///     Attribute Dependencies
        /// </summary>
        /// <param name="attributes">Attributes</param>
        public DependsUponAttribute(params string[] attributes)
        {
            this.attributes = attributes;
            this.predicates = new bool[] { };
            this.objs = new object[] { };
        }

        /// <summary>
        ///     Attribute Dependencies
        /// </summary>
        /// <param name="predicates">Predicates</param>
        public DependsUponAttribute(params bool[] predicates)
        {
            this.attributes = new string[] { };
            this.predicates = predicates;
            this.objs = new object[] { };
        }

        /// <summary>
        ///     Attribute Dependencies
        /// </summary>
        /// <param name="attributes">Array of Attributes</param>
        /// <param name="predicates">Predicates</param>
        public DependsUponAttribute(string[] attributes, params bool[] predicates)
        {
            this.attributes = attributes;
            this.predicates = predicates;
            this.objs = new object[] { };
        }

        /// <summary>
        ///     Attribute Dependencies
        /// </summary>
        /// <param name="predicates">Array of Predicates</param>
        /// <param name="attributes">Attributes</param>
        public DependsUponAttribute(bool[] predicates, params string[] attributes)
        {
            this.attributes = attributes;
            this.predicates = predicates;
            this.objs = new object[] { };
        }

        /// <summary>
        ///     Attribute Dependencies
        /// </summary>
        /// <param name="variable">Name of Variable</param>
        /// <param name="expected">Expected Outcome of Variable</param>
        public DependsUponAttribute(string variable, object expected)
        {
            this.attributes = new string []{ variable };
            this.predicates = new bool[] { };
            this.objs = new object[] { expected };
        }

        /// <summary>
        ///     Attribute Dependencies
        /// </summary>
        /// <param name="variable">Name of the first variable.</param>
        /// <param name="expected">Expected outcome of the first variable.</param>
        /// <param name="variable2">Name of the second variable.</param>
        /// <param name="expected2">Expected outcome of the second variable.</param>
        public DependsUponAttribute(string variable, object expected, string variable2, object expected2)
        {
            this.attributes = new string[] { variable, variable2 };
            this.predicates = new bool[] { };
            this.objs = new object[] { expected, expected2 };
        }

        /// <summary>
        ///     Attribute Dependencies
        /// </summary>
        /// <param name="variable">Name of the first variable.</param>
        /// <param name="expected">Expected outcome of the first variable.</param>
        /// <param name="variable2">Name of the second variable.</param>
        /// <param name="expected2">Expected outcome of the second variable.</param>
        /// <param name="variable3">Name of the third variable.</param>
        /// <param name="expected3">Expected outcome of the third variable.</param>
        public DependsUponAttribute(string variable, object expected, string variable2, object expected2, string variable3, object expected3)
        {
            this.attributes = new string[] { variable, variable2, variable3 };
            this.predicates = new bool[] { };
            this.objs = new object[] { expected, expected2, expected3 };
        }
    }
}