namespace Platinum.Identity.Core.ViewModels
{
    public class ConfirmAccountEmailViewModel
    {
        public string ConfirmEmailUrl { get; set; }
        public ConfirmAccountEmailViewModel(string confirmEmailUrl)
        {
            ConfirmEmailUrl = confirmEmailUrl;
        }
    }
}
