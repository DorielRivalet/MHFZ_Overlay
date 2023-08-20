// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZOverlayBenchmark.Comparisons;

using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

[RPlotExporter]
[MedianColumn, MinColumn, MaxColumn]
[UnicodeConsoleLogger]
public class TimerComparison
{
    [Params(0, 216_000)] // 2 hours at 30fps
    public int TimeDefInt { get; set; }

    [Params(0, 216_000)]
    public int TimeInt { get; set; }

    [Params(true, false)] // Arguments can be combined with Params
    public bool TimePercentShown;

    [Benchmark]
    [Arguments(100, 10)]
    [Arguments(100, 20)]
    [Arguments(200, 10)]
    [Arguments(200, 20)]
    public void Benchmark(int a, int b)
    {
        if (TimePercentShown)
            Thread.Sleep(a + b + 5);
        else
            Thread.Sleep(a + b);
    }

    private const int N = 10000;
    private readonly byte[] data;

    private readonly SHA256 sha256 = SHA256.Create();
    private readonly MD5 md5 = MD5.Create();

    public TimerComparison()
    {
        data = new byte[N];
        new Random(42).NextBytes(data);
    }

    [Benchmark]
    public byte[] Sha256() => sha256.ComputeHash(data);

    [Benchmark]
    public byte[] Md5() => md5.ComputeHash(data);
}
