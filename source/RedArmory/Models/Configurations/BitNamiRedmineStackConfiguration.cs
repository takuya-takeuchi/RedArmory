﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:2.0.50727.5485
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

// 
// このソース コードは xsd によって自動生成されました。Version=2.0.50727.3038 です。
// 

using System.Xml.Serialization;

namespace RedArmory.Models.Configurations {
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlRoot(Namespace="", IsNullable=false)]
    public partial class ApplicationConfiguration {
        
        private BitNamiRedmineStackConfiguration[] bitNamiRedmineStackConfigurationField;
        
        /// <remarks/>
        [XmlElement("BitNamiRedmineStackConfiguration")]
        public BitNamiRedmineStackConfiguration[] BitNamiRedmineStackConfiguration {
            get {
                return this.bitNamiRedmineStackConfigurationField;
            }
            set {
                this.bitNamiRedmineStackConfigurationField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class BitNamiRedmineStackConfiguration {
        
        private string versionField;
        
        private string defaultDestionationField;
        
        private string defaultSourceField;
        
        /// <remarks/>
        [XmlAttribute()]
        public string Version {
            get {
                return this.versionField;
            }
            set {
                this.versionField = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public string DefaultDestionation {
            get {
                return this.defaultDestionationField;
            }
            set {
                this.defaultDestionationField = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public string DefaultSource {
            get {
                return this.defaultSourceField;
            }
            set {
                this.defaultSourceField = value;
            }
        }
    }
}
