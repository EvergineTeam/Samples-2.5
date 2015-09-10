#region File Description
//-----------------------------------------------------------------------------
// NativeClass
//
// Copyright © 2015 Wave Corporation
// Use is subject to license terms.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Security;
#endregion

namespace WaveGTK.WaveIntegration
{
    #region Generated Code

    /// <summary>
    /// Native Class allow native mac instances
    /// </summary>    
    static class NativeClass
    {
        /// <summary>
        /// Native mac library
        /// </summary>
        const string mac_objc_name = "/usr/lib/libobjc.dylib";

        [DllImport(mac_objc_name)]
        extern static IntPtr class_getName(IntPtr handle);

        [DllImport(mac_objc_name)]
        extern static bool class_addMethod(IntPtr classHandle, IntPtr selector, IntPtr method, string types);

        [DllImport(mac_objc_name)]
        extern static IntPtr objc_getClass(string name);

        [DllImport(mac_objc_name)]
        extern static IntPtr objc_allocateClassPair(IntPtr parentClass, string name, int extraBytes);

        [DllImport(mac_objc_name)]
        extern static void objc_registerClassPair(IntPtr classToRegister);

        [SuppressUnmanagedCodeSecurity, DllImport(mac_objc_name)]
        extern static IntPtr sel_registerName(string name);

        public static IntPtr Get(string name)
        {
            var id = objc_getClass(name);
            if (id == IntPtr.Zero)
            {
                throw new ArgumentException("Unknown class: " + name);
            }
            return id;
        }

        public static IntPtr AllocateClass(string className, string parentClass)
        {
            return objc_allocateClassPair(Get(parentClass), className, 0);
        }

        public static void RegisterClass(IntPtr handle)
        {
            objc_registerClassPair(handle);
        }

        static List<Delegate> storedDelegates = new List<Delegate>();

        public static void RegisterMethod(IntPtr handle, Delegate d, string selector, string typeString)
        {
            // TypeString info:
            // https://developer.apple.com/library/mac/documentation/Cocoa/Conceptual/ObjCRuntimeGuide/Articles/ocrtTypeEncodings.html

            IntPtr p = Marshal.GetFunctionPointerForDelegate(d);
            bool r = class_addMethod(handle, sel_registerName(selector), p, typeString);

            if (!r)
            {
                throw new ArgumentException("Could not register method " + d + " in class + " + class_getName(handle));
            }

            storedDelegates.Add(d); // Don't let the garbage collector eat our delegates.
        }
    }
    #endregion
}
