using BankCheckBot.Interfaces;
using BankCheckBot.SamanBank;
using BankCheckBot.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddScoped<ISmsVerificationCodeService, SmsVerificationCodeService>();
        services.AddScoped<ILoginService, LoginService>();

        services.AddScoped<ISamanLoginPage, SamanLoginPage>();
        services.AddScoped<ISamanVerificationCodePage, SamanVerificationCodePage>();
        services.AddScoped<ISamanHomePage, SamanHomePage>();
        services.AddScoped<IUserActivitiesHistoryPage, UserActivitiesHistoryPage>();

        services.AddScoped<IManagementSamanBank, ManagementSamanBank>();
    }).Build();

var managementSamanBank = host.Services.GetRequiredService<IManagementSamanBank>();

await managementSamanBank.StartAsync();