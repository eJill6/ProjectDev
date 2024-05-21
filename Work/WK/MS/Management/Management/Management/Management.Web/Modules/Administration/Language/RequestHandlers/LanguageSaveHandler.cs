using Serenity.Services;
using MyRequest = Serenity.Services.SaveRequest<Management.Administration.LanguageRow>;
using MyResponse = Serenity.Services.SaveResponse;
using MyRow = Management.Administration.LanguageRow;


namespace Management.Administration
{
    public interface ILanguageSaveHandler : ISaveHandler<MyRow, MyRequest, MyResponse> { }
    public class LanguageSaveHandler : SaveRequestHandler<MyRow, MyRequest, MyResponse>, ILanguageSaveHandler
    {
        public LanguageSaveHandler(IRequestContext context)
             : base(context)
        {
        }
    }
}