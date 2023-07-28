// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models;

using System.Collections.Generic;

// Create a class to store ItemIDs for each row in PlayerInventory
public sealed class PlayerInventoryItemIds
{
    public long PlayerInventoryID { get; set; }

    public List<long>? ItemIds { get; set; }
}
