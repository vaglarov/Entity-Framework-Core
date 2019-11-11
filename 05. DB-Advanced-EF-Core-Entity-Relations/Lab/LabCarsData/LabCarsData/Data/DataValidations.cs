namespace LabCarsData.Data
{
    public static class DataValidations
    {
        public static class Make
        {
            public const int MaxNameLength = 30;
        }

        public static class Model
        {
            public const int MaxNameLength = 30;
            public const int MaxModificationLength = 30;
        }

        public static class Car
        {
            public const int MaxVINLength = 17;
            public const int MaxColorLength = 30;
        }
        
        public static class Customer
        {
            public const int MaxFirstNameLength = 30;
            public const int MaxLastNameLength = 30;
        }

        public static class Address
        {
            public const int MaxTextLength = 50;
            public const int MaxTownLength = 30;
        }
    }
}
