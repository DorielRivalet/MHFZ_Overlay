// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models;

using System;

// TODO: ORM
public sealed class AmmoPouch
{
    public DateTime CreatedAt { get; set; }

    public string? CreatedBy { get; set; } = string.Empty;

    public long AmmoPouchID { get; set; }

    public long RunID { get; set; }

    public long Item1ID { get; set; }

    public long Item1Quantity { get; set; }

    public long Item2ID { get; set; }

    public long Item2Quantity { get; set; }

    public long Item3ID { get; set; }

    public long Item3Quantity { get; set; }

    public long Item4ID { get; set; }

    public long Item4Quantity { get; set; }

    public long Item5ID { get; set; }

    public long Item5Quantity { get; set; }

    public long Item6ID { get; set; }

    public long Item6Quantity { get; set; }

    public long Item7ID { get; set; }

    public long Item7Quantity { get; set; }

    public long Item8ID { get; set; }

    public long Item8Quantity { get; set; }

    public long Item9ID { get; set; }

    public long Item9Quantity { get; set; }

    public long Item10ID { get; set; }

    public long Item10Quantity { get; set; }
}
