// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.Views
{
    using System;
    using System.Diagnostics;
    using System.Windows;
    using Cafe.Matcha.Utils;

    public partial class License : Window
    {
        public License()
        {
            InitializeComponent();
            Title = "授权提示 - " + Data.Title;
        }

        private void BOK_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            Helper.SetDialog(this);
        }
    }
}
