﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataLayer.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.8.1.0")]
    public sealed partial class DBLayer : global::System.Configuration.ApplicationSettingsBase {
        
        private static DBLayer defaultInstance = ((DBLayer)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new DBLayer())));
        
        public static DBLayer Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Michal\\PujcovnaAutom" +
            "obiluIS.mdf;database=PujcovnaAutomobiluIS;Trusted_Connection=True;")]
        public string ConnString {
            get {
                return ((string)(this["ConnString"]));
            }
        }
    }
}
