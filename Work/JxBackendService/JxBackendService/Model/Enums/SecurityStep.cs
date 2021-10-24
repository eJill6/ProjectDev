namespace JxBackendService.Model.Enums
{
    public enum TokenType
    {
        SecurityInitialize = 1,
        UpdateBankCard = 2,
        UnbindUserAuthenticator = 3,
    }

    public enum SecurityStep
    {
        SaveNickname = 1,
        SaveSecuritySetting = 2
    }
}
