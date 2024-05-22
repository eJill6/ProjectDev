using Microsoft.AspNetCore.StaticFiles;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

var provider = new FileExtensionContentTypeProvider();
provider.Mappings.Add(".aes", "application/octet-stream");

app.UseStaticFiles(new StaticFileOptions()
{
    ContentTypeProvider = provider
});

app.Run();
