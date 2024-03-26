// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("M7C+sYEzsLuzM7CwsXqht7+rTvj2U0cYFHVYqc8hBWqHtKshbC13vB/kr2tSjQDTmlG+Lve6TTyE5ieggTOwk4G8t7ibN/k3RrywsLC0sbJ+yV0u1nXJfyhvfY9iwOWPT5T4Eu8yxEw2OyvIcW385XZ2gJ+7TeO/UE8iW10cZN5bMCAi7kkLaEeAArjvU+AdoPRDQCT5I9EJ8VucE8TuaDkQd6xgHOMcpy7O26opEwRx5mnIbpVvT6ejnALQZM6e7IapxtMvbXy3IpXQitWYGHEdWGs+4sI9nIOpSkwUug77oMHllTFEItBh8fZBfvR1tem3FVnTfwsu6YoPa5ld8mK8nfNn/CMSXUnK/yz33Gz1vw1HcVnn+rDE81Q22LmYJrOysLGw");
        private static int[] order = new int[] { 1,9,2,9,6,6,8,11,10,11,11,13,13,13,14 };
        private static int key = 177;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
