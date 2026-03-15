using IndeedClone.Emails.Contracts;
using IndeedClone.Emails.Queue;
using IndeedClone.Emails.Services;
using IndeedClone.Modules.IndeedClone.RepoContracts;
using IndeedClone.Modules.IndeedClone.Repositories;
using IndeedClone.Modules.IndeedClone.ServiceContracts;
using IndeedClone.Modules.IndeedClone.Services;
using IndeedClone.Modules.JobDashboard.RepoContracts;
using IndeedClone.Modules.JobDashboard.Repositories;
using IndeedClone.Modules.JobDashboard.ServiceContracts;
using IndeedClone.Modules.JobDashboard.Services;
using IndeedClone.Modules.Shared.Configuration;
using IndeedClone.Modules.Shared.Data;
using IndeedClone.Modules.Shared.Extensions;
using IndeedClone.Modules.SubModules.Auth.Emails;
using IndeedClone.Modules.SubModules.Auth.Models;
using IndeedClone.Modules.SubModules.Auth.OTP;
using IndeedClone.Modules.SubModules.Auth.RepoContracts;
using IndeedClone.Modules.SubModules.Auth.Repositories;
using IndeedClone.Modules.SubModules.Auth.ServiceContracts;
using IndeedClone.Modules.SubModules.Auth.Services;
using IndeedClone.Modules.SubModules.JobApplication.RepoContracts;
using IndeedClone.Modules.SubModules.JobApplication.Repository;
using IndeedClone.Modules.SubModules.JobApplication.Service;
using IndeedClone.Modules.SubModules.JobApplication.ServiceContracts;
using IndeedClone.Modules.SubModules.JobPost.RepoContracts;
using IndeedClone.Modules.SubModules.JobPost.Repositories;
using IndeedClone.Modules.SubModules.JobPost.Service;
using IndeedClone.Modules.SubModules.JobPost.ServiceContracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;


namespace IndeedClone
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            /* ===============================
             * MVC + Modular View Resolution
             * =============================== */
             builder.Services.AddControllersWithViews()
                .AddApplicationPart(typeof(IndeedClone.Modules.IndeedClone.Controllers.IndeedCloneController).Assembly)
                .AddApplicationPart(typeof(IndeedClone.Modules.JobDashboard.Controllers.JobDashboardController).Assembly)
                .AddApplicationPart(typeof(IndeedClone.Modules.SubModules.Auth.Controllers.AuthController).Assembly)
                .AddApplicationPart(typeof(IndeedClone.Modules.SubModules.JobPost.Controllers.JobPostController).Assembly)
                .AddApplicationPart(typeof(IndeedClone.Modules.SubModules.JobPost.Controllers.JobDetailsController).Assembly)
                .AddApplicationPart(typeof(IndeedClone.Modules.SubModules.JobPost.Controllers.JobReviewController).Assembly)
                .AddApplicationPart(typeof(IndeedClone.Modules.SubModules.JobApplication.Controllers.JobApplicationController).Assembly)
                .AddRazorOptions(options =>
                {
                    options.ViewLocationFormats.Clear();
                    options.ViewLocationFormats.Add("/Modules/IndeedClone/Views/Home/{0}.cshtml");
                    options.ViewLocationFormats.Add("/Modules/IndeedClone/Views/Footer/{0}.cshtml");
                    options.ViewLocationFormats.Add("/Modules/JobDashboard/Views/Dashboard/{0}.cshtml");
                    options.ViewLocationFormats.Add("/Modules/SubModules/Auth/Views/AuthLogin/{0}.cshtml");
                    options.ViewLocationFormats.Add("/Modules/SubModules/JobPost/Views/JobPost/{0}.cshtml");
                    options.ViewLocationFormats.Add("/Modules/SubModules/JobApplication/Views/Apply/{0}.cshtml");
                    options.ViewLocationFormats.Add("/Views/Shared/{0}.cshtml");
                });

            /* ===============================
            * Modular Scoped Layouts
            * =============================== */
            builder.Services.AddControllersWithViews().AddRazorOptions(options => {options.ViewLocationExpanders.Add(new ModuleViewLocationExpander());});

            /* ===============================
             * Database
             * =============================== */
            builder.Services.AddDbContext<AuthDbContext>(options =>options.UseSqlServer( builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddDbContext<JobPostDbContext>(options =>options.UseSqlServer( builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddDbContext<JobApplicationDbContext>(options =>options.UseSqlServer( builder.Configuration.GetConnectionString("DefaultConnection")));
            //builder.Services.AddDbContext<JobApplicationDbContext>(options =>options.UseSqlServer(builder.Configuration.GetConnectionString("JobApplicationConnection")));
            builder.Services.AddScoped<IPasswordHasher<RegisterModel>, PasswordHasher<RegisterModel>>();
            builder.Services.AddScoped<IPasswordHasher<string>, PasswordHasher<string>>();

            /* ===============================
             * Repositories
             * =============================== */
            // # App
            builder.Services.AddScoped<IIndeedCloneHomeRepository, IndeedCloneHomeRepository>();
            builder.Services.AddScoped<IIndeedCloneJobDescriptionRepository, IndeedCloneJobDescriptionRepository>();

         // # Dashboard Module
            builder.Services.AddScoped<IRecruiterOverviewRepository, RecruiterOverviewRepository>();

        // # Auth SubModule
            builder.Services.AddScoped<IRegisterRepository, RegisterRepository>();
            builder.Services.AddScoped<IGoogleAuthRepository, GoogleAuthRepository>();
            builder.Services.AddScoped<IResetPasswordRepository, ResetPasswordRepository>();

         // # JobPost SubModule
            builder.Services.AddScoped<IJobOrganizationRepository, JobOrganizationRepository>();
            builder.Services.AddScoped<IJobBasicsRepository, JobBasicsRepository>();
            builder.Services.AddScoped<IJobDetailsRepository, JobDetailsRepository>();
            builder.Services.AddScoped<IJobPayBenefitsRepository, JobPayBenefitsRepository>();
            builder.Services.AddScoped<IJobDescriptionRepository, JobDescriptionRepository>();
            builder.Services.AddScoped<IJobPreferencesRepository, JobPreferencesRepository>();
            builder.Services.AddScoped<IJobQualificationsRepository, JobQualificationsRepository>();
        
         // # JobApplication SubModule
            builder.Services.AddScoped<IJobApplicationRepository, JobApplicationRepository>();
            builder.Services.AddScoped<IJobApplicationCVRepository, JobApplicationCVRepository>();
            builder.Services.AddScoped<IRelExperienceRepository, RelExperienceRepository>();
            builder.Services.AddScoped<IScreenerQuestionsRepository, ScreenerQuestionsRepository>();



            /* ===============================
             * Services
             * =============================== */
         // # App
            builder.Services.AddScoped<IIndeedCloneHomeService, IndeedCloneHomeService>();
            builder.Services.AddScoped<IIndeedCloneJobDescriptionService, IndeedCloneJobDescriptionService>();

         // # Dashboard Module
            builder.Services.AddScoped<IRecruiterOverviewService, RecruiterOverviewService>();

         // # Emails
            builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("Smtp"));
            builder.Services.AddSingleton<IEmailSender, SmtpEmailSender>();
            builder.Services.AddSingleton<IEmailQueue, EmailQueue>();
            builder.Services.AddHostedService<EmailQueueWorker>();

         // # Auth SubModule
            builder.Services.AddScoped<IRegisterService, RegisterService>();
            builder.Services.AddScoped<IOTPService, OTPService>();
            builder.Services.AddScoped<IGoogleAuthService, GoogleAuthService>();
            builder.Services.AddScoped<ILoginService, LoginService>();
            builder.Services.AddScoped<IResetPasswordService, ResetPasswordService>();
            builder.Services.AddScoped<IAuthEmailService, AuthEmailService>();


         // # JobPost SubModule
            builder.Services.AddScoped<IJobOrganizationService, JobOrganizationService>();
            builder.Services.AddScoped<IJobBasicService, JobBasicService>();
            builder.Services.AddScoped<IJobDetailsService, JobDetailsService>();
            builder.Services.AddScoped<IJobPayBenefitsService, JobPayBenefitsService>();
            builder.Services.AddScoped<IJobDescriptionService, JobDescriptionService>();
            builder.Services.AddScoped<IJobPreferencesService, JobPreferencesService>();
            builder.Services.AddScoped<IJobReviewService, JobReviewService>();
            builder.Services.AddScoped<IJobQualificationsService, JobQualificationsService>();
            builder.Services.AddScoped<IJobProgressService, JobProgressService>();
            builder.Services.AddScoped<IJobActivateService, JobActivateService>();

         // # JobApplication SubModule
            builder.Services.AddScoped<IJobApplicationService, JobApplicationService>();
            builder.Services.AddScoped<IRelExperienceService, RelExperienceService>();
            builder.Services.AddScoped<IScreenerQuestionsService, ScreenerQuestionsService>();
            builder.Services.AddScoped<IJobApplicationReviewService, JobApplicationReviewService>();
            builder.Services.AddScoped<IJobApplicationProgressService, JobApplicationProgressService>();
            builder.Services.AddScoped<IJobApplicationActivateService, JobApplicationActivateService>();


            /* ===============================
             * Helpers / Filters
             * =============================== */

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddSingleton<IUrlHelper>(sp =>
            {
                var httpContext = sp.GetRequiredService<IHttpContextAccessor>().HttpContext!;
                var actionContext = new ActionContext(
                    httpContext,
                    new Microsoft.AspNetCore.Routing.RouteData(),
                    new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor()
                );

                return new UrlHelper(actionContext);
            });

            /* ===============================
             * Authentication (Cookie)
             * =============================== */
             builder.Services
            .AddAuthentication("AuthCookie")
            .AddCookie("AuthCookie", options =>
            {
                options.LoginPath = "/Auth/Login";
                options.LogoutPath = "/Auth/Logout";
                options.AccessDeniedPath = "/Auth/Login";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30); 
                options.SlidingExpiration = true;
            });


            builder.Services.AddAuthorization();

          
            var app = builder.Build();

            /* =============================== 
             * Middleware Pipeline
             * =============================== */
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseHomepageRedirect(app.Configuration);
            app.UseStaticFiles();
            app.UseRouting();        
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
            );

            app.Run();
        }
    }
}
