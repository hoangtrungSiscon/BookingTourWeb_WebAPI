namespace BookingTourWeb_WebAPI.Models.InputModels
{
    public class ChangePasswordRequestModel
    {
        public long accountId { get; set; }
        public string oldPassword { get; set; }
        public string newPassword { get; set; }
    }
}
