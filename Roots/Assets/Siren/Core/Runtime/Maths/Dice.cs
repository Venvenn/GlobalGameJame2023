using UnityEngine;

namespace Siren
{
    public enum DiceType
    {
        D2,
        D4,
        D6,
        D8,
        D10,
        D12,
        D20,
        D100
    }

    public static class Dice
    {
        public static int D2()
        {
            return Random.Range(1, 3);
        }

        public static int D4()
        {
            return Random.Range(1, 5);
        }

        public static int D6()
        {
            return Random.Range(1, 7);
        }

        public static int D8()
        {
            return Random.Range(1, 9);
        }

        public static int D10()
        {
            return Random.Range(1, 11);
        }

        public static int D12()
        {
            return Random.Range(1, 13);
        }

        public static int D20()
        {
            return Random.Range(1, 21);
        }

        public static int D100()
        {
            return Random.Range(1, 101);
        }

        public static int Custom(int size)
        {
            return Random.Range(1, size + 1);
        }

        public static int Multiple(int number, DiceType type)
        {
            var result = 0;
            switch (type)
            {
                case DiceType.D2:
                    for (var i = 0; i < number; i++)
                    {
                        result += D2();
                    }

                    break;
                case DiceType.D4:
                    for (var i = 0; i < number; i++)
                    {
                        result += D4();
                    }

                    break;
                case DiceType.D6:
                    for (var i = 0; i < number; i++)
                    {
                        result += D6();
                    }

                    break;
                case DiceType.D8:
                    for (var i = 0; i < number; i++)
                    {
                        result += D8();
                    }

                    break;
                case DiceType.D10:
                    for (var i = 0; i < number; i++)
                    {
                        result += D10();
                    }

                    break;
                case DiceType.D12:
                    for (var i = 0; i < number; i++)
                    {
                        result += D12();
                    }

                    break;
                case DiceType.D20:
                    for (var i = 0; i < number; i++)
                    {
                        result += D20();
                    }

                    break;
                case DiceType.D100:
                    for (var i = 0; i < number; i++)
                    {
                        result += D100();
                    }

                    break;
            }

            return result;
        }
    }
}