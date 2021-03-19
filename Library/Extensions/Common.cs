using System;
namespace Api.Library.Extensions
{
    public static class MyExtensions
    {
        public static int GetAge(this DateTime dob)
        {
            var today = DateTime.Today;
            var age = today.Year - dob.Year;

            if (dob.Date > today.AddYears(-age))
                age--;

            return age;
        }
    }
}
