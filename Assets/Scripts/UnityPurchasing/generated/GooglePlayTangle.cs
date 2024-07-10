// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("UVadUHycS66i0ddsQQHgSls9WEVlD/JKc0DpFJ+mncuav4MyjHR7c9Lmi5FtVBe7vvOha+NG79ujRDN/l1FsxgTAKOs7I14tUnQAlrcLjlUsr6GuniyvpKwsr6+uYuWihlMczXY0ebsdl80EEt5WrmxTN9l11cRnk+DAxLs4WzpebfyOCvM8h4VQVMdNtWznw42mAoOxIELdtSHzXCkgwMiJ03Ixz9eBrPD5DdQj9N7DbOeV/cbN/O/OByilKKC+DmFkW7JWOXvn7j+YapV8SIq2TtKOpIXLATdG+Z4sr4yeo6inhCjmKFmjr6+vq66tKw8Jqv+laepq0popnM2a4N7gmTn9odAOKvRPmOCHW35OcJjCzgwrVEvGXoJncVnNkaytr66v");
        private static int[] order = new int[] { 9,4,11,6,4,10,12,12,13,10,12,12,13,13,14 };
        private static int key = 174;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
