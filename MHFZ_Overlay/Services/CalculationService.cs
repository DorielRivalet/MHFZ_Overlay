// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Services;

using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CsvHelper;
using MHFZ_Overlay.Models;
using MHFZ_Overlay.Models.Constant;
using Newtonsoft.Json;
using Wpf.Ui;
using Wpf.Ui.Controls;
using Application = System.Windows.Application;
using Clipboard = System.Windows.Clipboard;
using MessageBox = System.Windows.MessageBox;
using MessageBoxButton = System.Windows.MessageBoxButton;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;
using TextBlock = System.Windows.Controls.TextBlock;
using MathNet.Numerics.Distributions;

/// <summary>
/// Handles all sorts of calculations.
/// </summary>
public sealed class CalculationService
{
    private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

    public static int CalculateAttemptsForSuccess(double successChancePercentage, double confidenceLevelPercentage = 95)
    {
        double p = successChancePercentage / 100; // Convert percentage to a decimal
        double confidenceLevel = confidenceLevelPercentage / 100;
        int k = 1; // Start counting trials from  1

        // Iteratively check the CDF until we reach the desired confidence level
        while ((1 - Math.Pow(1 - p, k)) < confidenceLevel)
        {
            k++;
        }

        return k;
    }
}
