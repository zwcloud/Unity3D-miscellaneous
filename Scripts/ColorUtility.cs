using UnityEngine;

public static class ColorUtility
{
    public static uint ToUint(Color32 color)
    {
        return (uint)((color.a << 24) | (color.r << 16) | (color.g << 8) | (color.b << 0));
    }

    public static Color32 FromUint(uint color)
    {
        return new Color32((byte)(color >> 16), (byte)(color >> 8), (byte)color, (byte)(color >> 24));
    }

    public static float[] ToHSV(Color32 color)
    {
        var r = color.r / 255f;
        var g = color.g / 255f;
        var b = color.b / 255f;

        var min = Mathf.Min(r, g, b);
        var max = Mathf.Max(r, g, b);
        var delta = max - min;

        var h = 0f;
        var s = 0f;
        var v = max;

        if (delta.Equals(0f))
        {
            return new[] { h, s, v };
        }

        s = delta / max;

        var dR = ((max - r) / 6f + delta / 2f) / delta;
        var dG = ((max - g) / 6f + delta / 2f) / delta;
        var dB = ((max - b) / 6f + delta / 2f) / delta;

        if (r.Equals(max))
        {
            h = dB - dG;
        }
        else if (g.Equals(max))
        {
            h = 1f / 3f + dR - dB;
        }
        else if (b.Equals(max))
        {
            h = 2f / 3f + dG - dR;
        }

        if (h < 0)
        {
            h += 1;
        }
        else if (h > 1)
        {
            h -= 1;
        }

        return new[] { h, s, v };
    }

    public static Color32 FromHSV(float h, float s, float v)
    {
        if (s.Equals(0f))
        {
            var normV = (byte)(v * 255f);
            return new Color32(normV, normV, normV, 255);
        }

        h = h.Equals(1f) ? 0f : h * 6f;

        var i = (int)h;

        var r = v;
        var g = v;
        var b = v;

        switch (i)
        {
            case 0:
                g = v * (1f - s * (1f - (h - i)));
                b = v * (1f - s);
                break;
            case 1:
                r = v * (1f - s * (h - i));
                b = v * (1f - s);
                break;
            case 2:
                r = v * (1f - s);
                b = v * (1f - s * (1f - (h - i)));
                break;
            case 3:
                r = v * (1f - s);
                g = v * (1f - s * (h - i));
                break;
            case 4:
                r = v * (1f - s * (1f - (h - i)));
                g = v * (1f - s);
                break;
            case 5:
                g = v * (1f - s);
                b = v * (1f - s * (h - i));
                break;
        }

        return new Color32((byte)(r * 255f), (byte)(g * 255f), (byte)(b * 255f), 255);
    }
}