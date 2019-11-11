namespace P01_StudentSystem.Data
{
    public static class DataValidations
    {
        public static class Student
        {
            public const int MaxNameLength = 100;
            public const int PhoneNumberLength = 100;
        }

        public static class Course
        {
            public const int MaxNameLength = 80;
        }

        public static class Resource
        {
            public const int MaxNameLength = 50;
        }
    }
}
