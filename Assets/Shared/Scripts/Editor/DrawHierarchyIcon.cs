/*The MIT License (MIT)
Copyright (c) 2016 Edward Rowe (@edwardlrowe)
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

#if UNITY_EDITOR
using Shared.Helpers.Extensions;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Editor.Tools
{
    /// <summary>
    ///     Draws an Icon on GameObjects in the Hierarchy that contain a MonoBehaviour.
    /// </summary>
    [InitializeOnLoad]
    public class DrawHierarchyIcon
    {
        static DrawHierarchyIcon() =>
            EditorApplication.hierarchyWindowItemOnGUI += DrawIconOnWindowItem;

        private static void DrawIconOnWindowItem(int instanceID, Rect rect)
        {
            GameObject gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

            if (gameObject == null) return;

            //Get the behaviour
            var behaviours = new List<MonoBehaviour>(gameObject.GetComponents<MonoBehaviour>());
            if (behaviours == null) return;

            //Gets the behaviour icon
            Texture2D Icon = null;
            while(behaviours.Count > 0 && Icon == null)
            {
                MonoBehaviour mb = behaviours.Where(x => x is Shared.Behaviour).FirstOrDefault();
                if (mb == null)
                    mb = behaviours[0];
                var icon = mb.GetEditorIcon();
                if(icon != null && icon.name != "d_cs Script Icon") //Not using the default icon
                    Icon = icon;
                behaviours.RemoveAt(0);
            }

            //Gets the component icon if no behaviour exists
            if (Icon == null)
            {
                //Get the component
                var components = new List<Component>(gameObject.GetComponents<Component>());
                if (components == null) return;
                components = components.Where(x => !(x is Transform)).ToList(); //Remove transforms

                //Gets the component icon
                while (components.Count > 0 && Icon == null)
                {
                    var icon = components[0].GetEditorIcon();
                    if(icon != null && icon.name != "d_cs Script Icon") //Not using the default icon
                        Icon = icon;
                    components.RemoveAt(0);
                }
            }

            //Gets the GameObject icon if no component exists
            if(Icon == null)
            {
                var icon = EditorGUIUtility.ObjectContent(gameObject, gameObject.GetType()).image as Texture2D;
                if (icon != null && icon.name != "d_GameObject Icon" && icon.name != "d_Prefab Icon")
                    Icon = icon;
            }

            if (Icon != null)
            {
                //Create the icon
                float iconWidth = 15;
                EditorGUIUtility.SetIconSize(new Vector2(iconWidth, iconWidth));
                var padding = new Vector2(5, 0);
                var iconDrawRect = new Rect(
                                       rect.xMax - (iconWidth + padding.x),
                                       rect.yMin,
                                       rect.width,
                                       rect.height);
                var iconGUIContent = new GUIContent(Icon);
                EditorGUI.LabelField(iconDrawRect, iconGUIContent);
                EditorGUIUtility.SetIconSize(Vector2.zero);
            }
        }
    }
}
#endif