// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.ViewModels
{
    using System.Windows;
    using Cafe.Matcha.Utils;

    internal class TelemetrySetting : BindingTarget
    {
        public bool Enabled { get; set; } = true;
        public bool IsInit { get; set; } = false;

        public Visibility CheckboxVisibility
        {
            get
            {
                return IsInit ? Visibility.Hidden : Visibility.Visible;
            }
        }

        public string OkText
        {
            get
            {
                return IsInit ? "同意" : "确认";
            }
        }

        public string CancelText
        {
            get
            {
                return IsInit ? "拒绝" : "取消";
            }
        }
    }
}
