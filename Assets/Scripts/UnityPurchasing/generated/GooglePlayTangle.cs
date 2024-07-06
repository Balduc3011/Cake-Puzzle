// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("xKMcsgbJ2OVUmSTCvgcTtCGeV2GJ/5mT8+tJO/aZ9n2RgfSRvMp62Gc3LGuoAEsoL/6jnUu7RReBOoB1662n/lcgLVc97NqpoBcSzy01X1VDJL8JL5qMRBntBzOp0XFA5bM/F8chagfyN6Oh+I4mof0zRRQHPkFJdQ9WvrUxMYc9xFb9XF5qR0v8CksOG5lhAHYH9v7C+vaaMMklp6tSQGabxse1CxKpMJTyve1oqj58Fqmm6FrZ+ujV3tHyXpBeL9XZ2dnd2Nta2dfY6FrZ0tpa2dnYRoDinDKKpmgJCdjfdle4MAvIzu83xcA0tALDqXtQU6fXe+u5J0E3rrdR3MUcO1yRq9O1BEqA7JAR3cHEuPAu1ZtS65pLjXN8EkxFr9rb2djZ");
        private static int[] order = new int[] { 13,3,5,6,5,8,9,7,8,13,13,11,12,13,14 };
        private static int key = 216;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
