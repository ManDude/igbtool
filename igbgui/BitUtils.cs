namespace igbgui
{
    public static class BitUtils
    {
        public static int Align(int val, int n)
        {
            int res = val + (n - 1);
            return res - res % n;
        }
    }
}
