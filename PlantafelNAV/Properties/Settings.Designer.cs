﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PlantafelNAV.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.7.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://win-nav16:7047/DynamicsNAV90/WS/Carpart%20GmbH/Page/ws_mitarbeiter")]
        public string PlantafelNAV_ws_mitarbeiterservice_ws_mitarbeiter_Service {
            get {
                return ((string)(this["PlantafelNAV_ws_mitarbeiterservice_ws_mitarbeiter_Service"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://win-nav16:7047/DynamicsNAV90/WS/Carpart%20GmbH/Page/WS_Production")]
        public string PlantafelNAV_ws_production_service_2_WS_Production_Service {
            get {
                return ((string)(this["PlantafelNAV_ws_production_service_2_WS_Production_Service"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://win-nav16:7047/DynamicsNAV90/WS/Carpart%20GmbH/Page/WS_Arbeitzplatz")]
        public string PlantafelNAV_ws_arbeitsplatzservice_WS_Arbeitzplatz_Service {
            get {
                return ((string)(this["PlantafelNAV_ws_arbeitsplatzservice_WS_Arbeitzplatz_Service"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://win-nav16:7047/DynamicsNAV90/WS/Carpart%20GmbH/Page/WS_Auf_Arb_Nav")]
        public string PlantafelNAV_ws_aufarbservice_WS_Auf_Arb_Nav_Service {
            get {
                return ((string)(this["PlantafelNAV_ws_aufarbservice_WS_Auf_Arb_Nav_Service"]));
            }
        }
    }
}