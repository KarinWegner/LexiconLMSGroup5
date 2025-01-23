namespace LMS.Blazor.Client.Utilities
{
    public static class Validators
    {
        public static bool IsValid<T>(this IEnumerable<T> data)
        {
            if(data != null && data.Count() != 0)
                return true;
            return false;
        }
    }
}
