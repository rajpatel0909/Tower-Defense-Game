
using System.Collections;

public static class Utility {

	public static T[] ShuffleArray<T>(T[] array)
    {
        System.Random prng = new System.Random();

        for(int i = 0; i < array.Length - 1; i++)
        {
            int randomIndex = prng.Next(i, array.Length);
            T tempItem = array[randomIndex];
            array[randomIndex] = array[i];
            array[i] = tempItem;
        }

        return array;
    }

    public static int PickRandom(int min, int max, int seed)
    {
        System.Random prng = new System.Random(seed);
        return prng.Next(min, max);
    }

    //0>Nothing 1>Straight 2> side 3>upleft 4>upright 5>leftup 6>rightup 7>tower 8>Barricade 9>Don't use
    //0>upleft 90>rightup 180>leftup 270>upright
    //LU>u ur>r ul>l ru>l
}
