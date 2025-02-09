using Amazon.SQS;

namespace BackgroundWorker.AWSExtentions
{
    public static class AWSExtensions
	{
		public static IServiceCollection ConfigureAWSServices(this IServiceCollection services, IConfiguration configuration)
		{
            /*
            * GetAWSOptions Looks at the "AWS section of appsettings and loads in:
            * profile - Profile name specificed in .aws credentials file
            * profilesLocation - Not provided, it will look in default home folder for mac 
            * region - which region we're pointing to/infrastrucure is hosted on
            * 
            * Order to which aws sdk loads credentials .net
            * AwsOptions.Credentials -> AWSOptions.Profile (which we do) -> Profile Store Chain -> ENV Variables (i.e docker) -> EC2/ECS Task profile
            * Ref: https://coderjony.com/blogs/understanding-credential-loading-in-aws-sdk-for-net?source=stackoverflow
            */
            var options = configuration.GetAWSOptions();
            services.AddDefaultAWSOptions(options);

            //Add your service for DI 
            services.AddAWSService<IAmazonSQS>();
            return services;
		}
	}
}

