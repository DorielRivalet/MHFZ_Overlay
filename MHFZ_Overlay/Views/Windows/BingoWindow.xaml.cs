// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Views.Windows;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MHFZ_Overlay.Services;
using Wpf.Ui.Controls;

/// <summary>
/// Interaction logic for Window1.xaml
/// </summary>
public partial class BingoWindow : FluentWindow
{
    public BingoWindow()
    {
        InitializeComponent();
    }

    private void BingoWindowObject_Closed(object sender, EventArgs e)
    {
        ChallengeServiceInstance.State = Models.Structures.ChallengeState.Idle;
    }

    private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

    private static readonly ChallengeService ChallengeServiceInstance = ChallengeService.GetInstance();
}
