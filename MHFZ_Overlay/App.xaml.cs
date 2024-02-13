// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay;

using System;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using MHFZ_Overlay.Models.Constant;
using MHFZ_Overlay.Services;

/// <summary>
/// Interaction logic for App.xaml.
/// </summary>
public partial class App : Application
{
    private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

    /// <inheritdoc/>
    protected override void OnStartup(StartupEventArgs e)
    {
        Logger.Info("Starting up App");
        this.DispatcherUnhandledException += App_DispatcherUnhandledException;

        base.OnStartup(e);
    }

    private static void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e) =>

    // Log/inspect the inspection here
    Logger.Error(CultureInfo.InvariantCulture, "Unhandled exception\n\nMessage: {0}\n\nStack Trace: {1}\n\nHelp Link: {2}\n\nHResult: {3}\n\nSource: {4}\n\nTarget Site: {5}", e.Exception.Message, e.Exception.StackTrace, e.Exception.HelpLink, e.Exception.HResult, e.Exception.Source, e.Exception.TargetSite);
}
