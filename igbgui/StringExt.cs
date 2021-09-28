namespace igbgui
{
    public static class StringExt
    {
        public static string TrimNull(this string str)
        {
            return str.Remove(str.IndexOf('\0'));
        }
    }
}
