// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("SxEVMz/ooYNWq913mA6Hdbc1O1GvLCItHa8sJy+vLCwtvTM1BzNpC5lBClZw5JAGc7S/dkEEgCblUJEQCI7RBb03yzg0PBQrofEsIpcuPpbiZboFnTOG7Mn8+DBWnDSfLmvftil94pOzsi7jEu+lj1ydi8xkGsMxqYjpsvSzy5koip6Oqanit2nydnOM+3FqragSZKWYZ2PicbShpKJa7B9eCx2Of947mgGZkFlX+08FQjS38tMwPae2BUMOLUt0wzburt1ein6YDQ8e7rJdvmuuHjdhiJq1lXfSJB2vLA8dICskB6tlq9ogLCwsKC0u0gba1cuorFeGAGcY8illf5PDcn3bmuv1YqvNNzeKsIlRN/JYbxQavSQMkyMHDESXri8uLC0s");
        private static int[] order = new int[] { 3,1,5,6,9,5,11,9,13,12,12,11,13,13,14 };
        private static int key = 45;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
