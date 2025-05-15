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
        }

        public static class PT
        {
            public const string PTEndPoint = ApiEndpoint + "/pt";
            public const string CreateNewPT = PTEndPoint;
            public const string GetAllPT = PTEndPoint;
        }

        public static class Admin
        {
            public const string AdminEndPoint = ApiEndpoint + "/admin";
            public const string CreateAdmin = AdminEndPoint;
            public const string GetAllPT = AdminEndPoint + "/get-pt";
        }

        public static class GymCourse
        {
            public const string GymCourseEndPoint = ApiEndpoint + "/course";
            public const string CreateGymCourse = GymCourseEndPoint;
            public const string GetAllGymCourse = GymCourseEndPoint;
        }

        public static class Slot
        {
            public const string SlotEndPoint = ApiEndpoint + "/slot";
            public const string CreateSlot = SlotEndPoint;
            public const string GetAllSlot = SlotEndPoint;
            public const string GetSlot = SlotEndPoint + "/{id}";
        }

    }
}
