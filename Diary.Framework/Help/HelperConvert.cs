namespace Diary.Framework.Help
{
    public static class HelperConvert
    {
        public static bool? StringSexToBoolConvert(string sex)
        {
            if (sex == "Male")
                return true;
            return false;
        }

        public static string BoolToSexStringConvert(bool? sex)
        {
            if (sex == true)
                return "Male";
            return "Female";
        }
    }
}