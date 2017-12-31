using System;
using UnityEngine;

public static class LayerMaskExtensions {

    public static int ToSingleLayer(LayerMask mask) {
        int bitmask = mask.value;

        if (!Mathf.IsPowerOfTwo(bitmask))
            throw new Exception("Mask is not a one layer");

        int result = bitmask > 0 ? 0 : 31;
        while (bitmask > 1) {
            bitmask = bitmask >> 1;
            result++;
        }
        return result;
    }

}
