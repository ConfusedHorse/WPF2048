﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WPF2048.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("WPF2048.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There are no possible moves left!
        ///
        ///Do you want to start a new game?.
        /// </summary>
        public static string DefeatBody {
            get {
                return ResourceManager.GetString("DefeatBody", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Defeat!.
        /// </summary>
        public static string DefeatHeader {
            get {
                return ResourceManager.GetString("DefeatHeader", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Moves.
        /// </summary>
        public static string Moves {
            get {
                return ResourceManager.GetString("Moves", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Do you want to start a new game?.
        /// </summary>
        public static string ResetBody {
            get {
                return ResourceManager.GetString("ResetBody", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Restart?.
        /// </summary>
        public static string ResetHeader {
            get {
                return ResourceManager.GetString("ResetHeader", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Score.
        /// </summary>
        public static string Score {
            get {
                return ResourceManager.GetString("Score", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Congratulations! You won the game!
        ///
        ///Do you want to start a new game?.
        /// </summary>
        public static string WinBody {
            get {
                return ResourceManager.GetString("WinBody", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Success!.
        /// </summary>
        public static string WinHeader {
            get {
                return ResourceManager.GetString("WinHeader", resourceCulture);
            }
        }
    }
}
