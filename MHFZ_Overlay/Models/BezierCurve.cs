// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Cubic bezier curve with four control points.
/// </summary>
public class BezierCurve
{
    public Vector2 P0, P1, P2, P3;

    public BezierCurve(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
    {
        this.P0 = p0;
        this.P1 = p1;
        this.P2 = p2;
        this.P3 = p3;
    }

    public Vector2 Evaluate(float t)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector2 B = new Vector2();
        B = uuu * P0; // (1-t) * (1-t) * (1-t) * P0
        B += 3 * uu * t * P1; // 3 * (1-t) * (1-t) * t * P1
        B += 3 * u * tt * P2; // 3 * (1-t) * t * t * P2
        B += ttt * P3; // t * t * t * P3

        return B;
    }
}

