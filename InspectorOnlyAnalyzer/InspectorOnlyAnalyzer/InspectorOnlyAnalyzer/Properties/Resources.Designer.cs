﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.42000
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

namespace InspectorOnlyAnalyzer.Properties {
    using System;
    
    
    /// <summary>
    ///   ローカライズされた文字列などを検索するための、厳密に型指定されたリソース クラスです。
    /// </summary>
    // このクラスは StronglyTypedResourceBuilder クラスが ResGen
    // または Visual Studio のようなツールを使用して自動生成されました。
    // メンバーを追加または削除するには、.ResX ファイルを編集して、/str オプションと共に
    // ResGen を実行し直すか、または VS プロジェクトをビルドし直します。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   このクラスで使用されているキャッシュされた ResourceManager インスタンスを返します。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("InspectorOnlyAnalyzer.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   すべてについて、現在のスレッドの CurrentUICulture プロパティをオーバーライドします
        ///   現在のスレッドの CurrentUICulture プロパティをオーバーライドします。
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
        ///   The assignment to &apos;{0}&apos; is prohibited by the InspectorOnly attribute に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string MessageFormat001 {
            get {
                return ResourceManager.GetString("MessageFormat001", resourceCulture);
            }
        }
        
        /// <summary>
        ///   The InspectorOnly attribute must be assigned to a serializable field に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string MessageFormat002 {
            get {
                return ResourceManager.GetString("MessageFormat002", resourceCulture);
            }
        }
        
        /// <summary>
        ///   InspectorOnly attribute cannot be used with NonSerialized attribute に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string MessageFormat003 {
            get {
                return ResourceManager.GetString("MessageFormat003", resourceCulture);
            }
        }
        
        /// <summary>
        ///   The InspectorOnly attribute must be assigned to fields of serializable class or struct に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string MessageFormat004 {
            get {
                return ResourceManager.GetString("MessageFormat004", resourceCulture);
            }
        }
    }
}