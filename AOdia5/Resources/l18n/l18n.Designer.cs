﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.42000
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

namespace AOdia5.Resources.l18n {
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
    internal class L18N {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal L18N() {
        }
        
        /// <summary>
        ///   このクラスで使用されているキャッシュされた ResourceManager インスタンスを返します。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("AOdia5.Resources.l18n.L18N", typeof(L18N).Assembly);
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
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Add from station LIST に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string ADD_STATION_FROM_LIST {
            get {
                return ResourceManager.GetString("ADD_STATION_FROM_LIST", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Add from MAP に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string ADD_STATION_FROM_MAP {
            get {
                return ResourceManager.GetString("ADD_STATION_FROM_MAP", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Add station to route に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string ADD_STATION_MODAL_TITLE {
            get {
                return ResourceManager.GetString("ADD_STATION_MODAL_TITLE", resourceCulture);
            }
        }
        
        /// <summary>
        ///   New route に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string NEW_ROUTE {
            get {
                return ResourceManager.GetString("NEW_ROUTE", resourceCulture);
            }
        }
    }
}
