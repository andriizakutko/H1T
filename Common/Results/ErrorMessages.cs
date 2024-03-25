namespace Common.Results;

public static class ErrorMessages
{
    public static class Admin
    {
        public const string PermissionHasAlreadyAdded = "Permission has already added";
        public const string UserNotHavePermissionToDelete = "User doesn't have this permission to delete";
        public const string DeleteUserFromPermissionFailed = "Delete user permission is failed";
    }
    public static class TransportAdvertisement
    {
        public const string CreatorNotFound = "Creator was not found";
    }
    public static class User
    {
        public const string UserNotCreated = "User was not created";
        public const string UserNotFound = "User was not found";
        public const string UserNotExists = "User doesn't exist";
        public const string UserAlreadyExist = "User already exists with this email";
        public const string UserNotActive = "User is not active";
    }
    public static class UserValidation
    {
        public const string IncorrectCredentials = "Incorrect credentials";
    }
    public static class Image
    {
        public const string ImageWasNotAdded = "Image was not added";
        public const string ImagesWereNotAdded = "Images were not added";
    }
    public static class Moderator
    {
        public const string UpdateModeratorOverviewStatusFailed = "Moderator overview status was not updated";
        public const string UpdateTransportAdvertisementVerificationStatusFailed =
            "Transport advertisement verification status was not updated";
    }

    public const string ServiceError = "Service error!";
}