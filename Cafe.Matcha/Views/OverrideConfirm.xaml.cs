// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.Views
{
    using System.Windows;
    using Cafe.Matcha.Utils;

    public partial class OverrideConfirm : Window
    {
        public bool All { get; set; } = false;

        public OverrideConfirm()
        {
            InitializeComponent();
        }

        public OverrideConfirm(string content, string title) : this()
        {
            this.Title = title;
            lMain.Content = content;
        }

        private void Close(bool dialogResult)
        {
            DialogResult = dialogResult;
            Close();
        }

        private void BYes_Click(object sender, RoutedEventArgs e)
        {
            Close(true);
        }

        private void BNo_Click(object sender, RoutedEventArgs e)
        {
            Close(false);
        }

        private void Window_SourceInitialized(object sender, System.EventArgs e)
        {
            Helper.SetDialog(this);
        }
    }
}
