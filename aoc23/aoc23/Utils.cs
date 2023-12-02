public static class Utils
{
    public static string ReverseString(string value)
    {
        char[] charArray = value.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }
}