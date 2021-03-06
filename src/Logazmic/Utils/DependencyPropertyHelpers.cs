﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logazmic.Utils
{
    using System.Windows;
    using System.Windows.Media;

    public static class DependencyPropertyHelpers
    {
        public static T FindParent<T>(this DependencyObject start) where T : DependencyObject
        {
            var parentObject = VisualTreeHelper.GetParent(start);
            if (parentObject == null)
            {
                return null;
            }

            var parent = parentObject as T;
            return parent ?? FindParent<T>(parentObject);
        }

        public static TChildItem FindVisualChild<TChildItem>(this DependencyObject obj) where TChildItem : DependencyObject
        {
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is TChildItem)
                {
                    return (TChildItem)child;
                }
                var childOfChild = FindVisualChild<TChildItem>(child);
                if (childOfChild != null)
                {
                    return childOfChild;
                }
            }
            return null;
        }
    }
}
