namespace Common.Results;

public static class ErrorCodes
{
    public static class Admin
    {
        public const string GetUsers = "AdminService.GetUsers";
        public const string AddUserToPermission = "AdminService.AddUserToPermission";
        public const string DeleteUserFromPermission = "AdminService.DeleteUserFromPermission";
        public const string GetUsersPermissions = "AdminService.GetUsersPermissions";
    }
    public static class Image
    {
        public const string AddImage = "ImageService.AddImage";
        public const string AddImages = "ImageService.AddImages";
    }
    public static class Moderator
    {
        public const string GetModeratorOverviewStatusByName = "ModeratorService.GetModeratorOverviewStatusByName";
        public const string UpdateModeratorOverviewStatus = "ModeratorService.UpdateModeratorOverviewStatus";
        public const string GetTransportAdvertisementByStatusId =
            "ModeratorService.GetTransportAdvertisementByStatusId";
        public const string UpdateTransportAdvertisementVerificationStatus = "ModeratorService.UpdateTransportAdvertisementVerificationStatus";
    }
    public static class Resource
    {
        public const string GetTransportTypes = "TransportService.GetTransportTypes";
        public const string GetTransportMakesByTransportTypeId = "TransportService.GetTransportMakesByTransportTypeId";
        public const string GetTransportBodyTypesByTransportTypeId =
            "TransportService.GetTransportBodyTypesByTransportTypeId";
        public const string GetTransportModelsByTransportMakeId =
            "TransportService.GetTransportModelsByTransportMakeId";
        public const string GetModeratorOverviewStatuses = "ModeratorService.GetModeratorOverviewStatuses";
    }
    public static class TransportAdvertisement
    {
        public const string GetTransportAdvertisements = "AdvertisementService.GetTransportAdvertisements";
        public const string CreateTransportAdvertisement = "TransportAdvertisementService.CreateTransportAdvertisement";
        public const string GetTransportAdvertisementById = "AdvertisementService.GetTransportAdvertisementById";
    }
    public static class User
    {
        public const string Register = "UserService.Register";
        public const string Login = "UserService.Login";
        public const string GetUser = "UserService.GetUser";
    }
    public static class UserValidation
    {
        public const string ValidateRegisterModel = "UserValidationService.ValidateRegisterModel";
        public const string ValidateLoginModel = "UserValidationService.ValidateLoginModel";
    }
}