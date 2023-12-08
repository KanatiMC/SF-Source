namespace lol
{
    using System;
    using UnityEngine;

    public static class game
    {
        public static float KHOCKABHDPO(float NCKMKLNALMF, float GNLEKCONDBB, float MHMNHMPNFAC)
        {
            while (true)
            {
                if (NCKMKLNALMF < -360f)
                {
                    NCKMKLNALMF += 360f;
                }
                if (NCKMKLNALMF > 360f)
                {
                    NCKMKLNALMF -= 360f;
                }
                if ((NCKMKLNALMF >= -360f) && (NCKMKLNALMF <= 360f))
                {
                    return Mathf.Clamp(NCKMKLNALMF, GNLEKCONDBB, MHMNHMPNFAC);
                }
            }
        }
    }
}

