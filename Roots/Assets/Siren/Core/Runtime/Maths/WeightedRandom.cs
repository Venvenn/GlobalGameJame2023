namespace Siren
{
    public static class WeightedRandom
    {
        public static int Random(int[] weights)
        {
            float total = 0;

            for (var i = 0; i < weights.Length; i++)
            {
                total += weights[i];
            }

            var random = UnityEngine.Random.Range(0, total);

            for (var i = 0; i < weights.Length; i++)
            {
                if (random < weights[i])
                {
                    return i;
                }

                random -= weights[i];
            }

            return -1;
        }
    }
}