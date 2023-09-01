// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Services.Contracts;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MHFZ_Overlay.Models.Structures;

public interface ITimeService
{
    /// <summary>
    /// Gets the elapsed time in the desired format.
    /// </summary>
    /// <param name="frames"></param>
    /// <returns></returns>
    string GetMinutesSecondsFromSeconds(double seconds);

    /// <summary>
    /// Gets the elapsed time in the desired format.
    /// </summary>
    /// <param name="frames"></param>
    /// <returns></returns>
    string GetMinutesSecondsFromFrames(double frames);

    /// <summary>
    /// Gets the elapsed time in the desired format.
    /// </summary>
    /// <param name="frames"></param>
    /// <returns></returns>
    string GetMinutesSecondsMillisecondsFromFrames(double frames);

    /// <summary>
    /// Gets the elapsed time in the desired format.
    /// </summary>
    /// <param name="frames"></param>
    /// <returns></returns>
    string GetMinutesSecondsMillisecondsFromFrames(long frames);
}
