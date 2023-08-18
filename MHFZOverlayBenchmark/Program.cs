// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Running;
using System.Management;
using MHFZOverlayBenchmark.Comparisons;

var summary = BenchmarkRunner.Run<TimerComparison>();

