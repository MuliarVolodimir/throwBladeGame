// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("CO9lllna22RwCGWey8VVOxLeuIhOrEnwFfwR0Qq7vWZbEp5zIAnTkWzD7utoz6wpilhlhPwLe+wLqDmCkqG7AGGm+NcNrtL0VaqqrgYe7hvD9ss4yL3+L7U9GTH3IFgu6N2e5FOtPP+7moy2CSBFWiBev6HsMnQ/vq5bNSATvSLp/F9kf61GHFelPqyctJtEA3HxPjr6KEyQXZDSKEv/IMj4tH4hBdRExlhCUOhhKz+h3BYw83B+cUHzcHtz83BwccPSr0K9PpxB83BTQXx3eFv3OfeGfHBwcHRxclRm/rdR255QXb1MmSVdlFT7mGDKvbMDm2XBMcyzsYw2YaYR48fEA25GyNATT4SWTSe8gE2n62kx/5O9cVJ9bIpYhS2J3nNycHFw");
        private static int[] order = new int[] { 7,5,11,12,12,6,11,8,13,11,13,11,12,13,14 };
        private static int key = 113;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
