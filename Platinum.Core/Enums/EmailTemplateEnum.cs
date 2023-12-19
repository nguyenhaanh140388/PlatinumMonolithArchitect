namespace Platinum.Core.Enums
{
    public enum EmailTemplateEnum
    {
        [EnumContent(key: EmailTemplateProperties.FileName, value: "ConfirmAccountEmail.cshtml")]
        [EnumContent(key: EmailTemplateProperties.EmailTemplateName, value: "Confirm Account Email")]
        [EnumContent(key: EmailTemplateProperties.EmailSubject, value: "[AppName] - Confirm Account Email")]
        ConfirmRegister = 1,
    }
}
