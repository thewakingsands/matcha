// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.Views
{
    using System;
    using System.Diagnostics;
    using System.Windows;
    using Cafe.Matcha.Utils;

    public partial class TelemetrySetting : Window
    {
        private ViewModels.TelemetrySetting _viewModel = null;
        private ViewModels.TelemetrySetting ViewModel
        {
            get
            {
                if (_viewModel == null)
                {
                    _viewModel = (ViewModels.TelemetrySetting)DataContext;
                }

                return _viewModel;
            }
        }

        public TelemetrySetting(bool shouldUpdate = false)
        {
            InitializeComponent();
            Title = "公共数据汇报设置 - " + Data.Title;

            ViewModel.IsInit = shouldUpdate;
            if (!ViewModel.IsInit)
            {
                ViewModel.Enabled = Telemetry.Instance.Enabled;
            }
            else
            {
                ViewModel.Enabled = true;
            }
        }

        private void BOK_Click(object sender, RoutedEventArgs e)
        {
            Telemetry.Instance.Enabled = ViewModel.Enabled;
            Close();
        }

        private void BCancel_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.IsInit)
            {
                Telemetry.Instance.Enabled = false;
            }

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
