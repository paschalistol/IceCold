// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("JQ8y/G+4pNd7hx1UBiPZR0uv0rcbI0g5msBaYQrvbMriKbPfuSG2pTFDIdNKC1G9H1xIms4Wbb/1hxAZHknm/G50Z5J8jmtUDoOZzRTmc+JdfR/oZpnFKAhDP5MTN1HsfhjJ1Vh4Rw9a9z4jfKBeRJwGacbl57/5huedH4OJfdeE6x0WWTve2Pvj3S5l5ujn12Xm7eVl5ubnFqR1mJqY92it7AYLRiyM+7CeUSntrMHIsS53hRxC80vrsC1MUu0fyYJyK/Ms1V3XZebF1+rh7s1hr2EQ6ubm5uLn5PHaU3qD4L6yJzUX6fKS9LX9BnwPmwfJBctK9vM83Ks3IVF8Mp7Wif9f7I7kix6+fl9bPHl58FjIViryKmiPJ4bgtjD0BuXk5ufm");
        private static int[] order = new int[] { 2,12,10,6,6,9,8,12,13,13,10,12,13,13,14 };
        private static int key = 231;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
