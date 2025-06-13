using System.Runtime.CompilerServices;

namespace GymRadar.API.Constant
{
    public static class ApiEndPointConstant
    {
        static ApiEndPointConstant()
        {
        }

        public const string RootEndPoint = "/api";
        public const string ApiVersion = "/v1";
        public const string ApiEndpoint = RootEndPoint + ApiVersion;

        public static class Account
        {
            public const string AccountEndPoint = ApiEndpoint + "/account";
            public const string RegisterAccount = AccountEndPoint;
            public const string GetProfile = AccountEndPoint + "/profile";
        }

        public static class Authentication
        {
            public const string AuthEndPoint = ApiEndpoint + "/auth";
            public const string Auth = AuthEndPoint;
        }

        public static class Gym
        {
            public const string GymEndPoint = ApiEndpoint + "/gym";
            public const string CreateNewGym = GymEndPoint;
            public const string GetAllGym = GymEndPoint;
            public const string GetGym = GymEndPoint + "/{id}";
            public const string GetAllPT = GymEndPoint + "/{id}/pts";
            public const string GetAllCourse = GymEndPoint + "/{id}/courses";
            public const string DeleteGym = GymEndPoint + "/{id}";
        }

        public static class PT
        {
            public const string PTEndPoint = ApiEndpoint + "/pt";
            public const string CreateNewPT = PTEndPoint;
            public const string GetAllPT = PTEndPoint;
            public const string DeletePT = PTEndPoint + "/{id}";
            public const string UpdatePT = PTEndPoint;
            public const string GetPT = PTEndPoint + "/{id}";
        }

        public static class Admin
        {
            public const string AdminEndPoint = ApiEndpoint + "/admin";
            public const string CreateAdmin = AdminEndPoint;
            public const string GetAllPT = AdminEndPoint + "/get-pt";
            public const string GetAllBooking = AdminEndPoint + "/booking";
            public const string GetAllTransaction = AdminEndPoint + "/transaction";
        }

        public static class GymCourse
        {
            public const string GymCourseEndPoint = ApiEndpoint + "/course";
            public const string CreateGymCourse = GymCourseEndPoint;
            public const string GetAllGymCourse = GymCourseEndPoint;
            public const string GetAllPTForGymCourse = GymCourseEndPoint + "/{id}/pts";
        }

        public static class Slot
        {
            public const string SlotEndPoint = ApiEndpoint + "/slot";
            public const string CreateSlot = SlotEndPoint;
            public const string GetAllSlot = SlotEndPoint;
            public const string GetSlot = SlotEndPoint + "/{id}";
            public const string DeleteSlot = SlotEndPoint + "/{id}";
        }

        public static class User
        {
            public const string UserEndPoint = ApiEndpoint + "/user";
            public const string UpdateUser = UserEndPoint;
        }

        public static class PTSlot
        {
            public const string PTSlotEndPoint = ApiEndpoint + "/pt-slot";
            public const string CreatePTSlot = PTSlotEndPoint;
            public const string GetAllPTSlot = PTSlotEndPoint;
            public const string GetAllPTSlotForUser = PTSlotEndPoint + "/{id}/user";
            public const string ActivePTSlot = PTSlotEndPoint + "/{id}/active";
            public const string UnActivePTSlot = PTSlotEndPoint + "/{id}/un-active";
        }

        public static class GymCoursePT
        {
            public const string GymCoursePTEndPoint = ApiEndpoint + "/course-pt";
            public const string CreateGymCoursePT = GymCoursePTEndPoint;
            public const string GetGymCoursePT = GymCoursePTEndPoint + "/{id}";
            public const string GetAllGymCoursePT = GymCoursePTEndPoint + "/{id}/gym-course";
        }

        public static class Cart
        {
            public const string CartEndPoint = ApiEndpoint + "/cart";
            public const string CreateQR = CartEndPoint;
            public const string CreateQRNotPT = CartEndPoint + "/gym-course";
            public const string CreateQRPremium = CartEndPoint + "/premium";
            public const string ReturnUrl = CartEndPoint + "/return";
            public const string GetPaymentStatus = CartEndPoint + "/status";
        }

        public static class Booking
        {
            public const string BookingEndPoint = ApiEndpoint + "/booking";
            public const string CreateBooking = BookingEndPoint;
            public const string GetBookingForUser = BookingEndPoint + "/user";
            public const string GetBookingForPT = BookingEndPoint + "/pt";
            public const string UpdateBooking = BookingEndPoint + "/{id}";
            public const string GetBooking = BookingEndPoint + "/{id}";
        }

        public static class Transaction
        {
            public const string TransactionEndPoint = ApiEndpoint + "/transaction";
            public const string GetAllTransaction = TransactionEndPoint;
            public const string GetAllTransactionForGym = TransactionEndPoint + "/gym";
        }

        public static class Upload
        {
            public const string UploadEndPoint = ApiEndpoint + "/upload";
            public const string UploadImage = UploadEndPoint;
        }

        public static class Premium
        {
            public const string PremiumEndPoint = ApiEndpoint + "/premium";
            public const string CreatePremium = PremiumEndPoint;
            public const string GetAllPremium = PremiumEndPoint;
            public const string GetPremiumById = PremiumEndPoint + "/{id}";
        }
    }
}
