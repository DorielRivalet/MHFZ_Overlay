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

public interface IDatabase
{
    // TODO i would like to return the data that was set
    /// <summary>
    /// Returns: null if the data failed to set, the value set if the object was successfully set.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    object? SetData(object data, object value);

    /// <summary>
    /// Gets an object from the database.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="mode"></param>
    /// <returns></returns>
    object? GetData(object data, GetterMode mode = GetterMode.Single);

    void SetUpDatabase();
}
