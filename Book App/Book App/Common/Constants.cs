namespace Book_App.Common
{
    public static class Constants
    {
        public static class MediaConstants
        {
            public const int TitleMinLength = 2;
            public const int TitleMaxLength = 25;

            public const int DescriptionMinLength = 50;
            public const int DescriptionMaxLength = 500;

            public const int AuthorNameMinLength = 2;
            public const int AuthorNameMaxLength = 20;

            public const int DirectorNameMinLength = 2;
            public const int DirectorNameMaxLength = 20;

            public const int ImgUrlMaxLength = 2048;

            public const int GenerMinLength = 5;
            public const int GenerMaxLength = 15;

            public const int YearPublishedMinValue = 1000;
            public const int YearPublishedMaxValue = 2024;

            public const double PriceMinValue = 1.00;
            public const double PriceMaxValue = 99.99;

            public const int PagesMinLength = 50;
            public const int PagesMaxLength = 1999;

            public const int MovieDuration = 0;
        }

        public static class UserConstants
        {
            public const int FirstNameMinLength = 2;
            public const int FirstNameMaxLength = 25;

            public const int LastNameMinLength = 2;
            public const int LastNameMaxLength = 25;

            public const int EmailMinLength = 7;
            public const int EmailMaxLength = 35;
        }
    }
}
