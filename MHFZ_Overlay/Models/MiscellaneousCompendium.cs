// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models;

// TODO: ORM
public sealed class MiscellaneousCompendium
{
    public long TotalOverlaySessions { get; set; }

    public double HighestSessionDuration { get; set; }

    public double LowestSessionDuration { get; set; }

    public double AverageSessionDuration { get; set; }

    public double MedianSessionDuration { get; set; }
}
