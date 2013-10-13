using Nancy.Authentication.Basic;
using Nancy.Security;

namespace PSWFWeb
{
    using Nancy;

    class PSWFWebUserIdentity : IUserIdentity
    {
        private string userName;
        public PSWFWebUserIdentity(string userName)
        {
            this.userName = userName;
        }

        public System.Collections.Generic.IEnumerable<string> Claims
        {
            get { return new string[] {"Admin"}; }
        }

        public string UserName
        {
            get { return userName; }
        }
    }

    class PSWFWebUserValidator : IUserValidator
    {
        public Nancy.Security.IUserIdentity Validate(string username, string password)
        {
            string passwordHash = PasswordUtil.GeneratePasswordHash(password);

            if (username == PSWFWebConfig.Current.Username &&
                passwordHash == PSWFWebConfig.Current.PasswordHash)
            {
                return new PSWFWebUserIdentity(username);
            }

            return null;
        }
    }

    public class Bootstrapper : DefaultNancyBootstrapper
    {
        // The bootstrapper enables you to reconfigure the composition of the framework,
        // by overriding the various methods and properties.
        // For more information https://github.com/NancyFx/Nancy/wiki/Bootstrapper

        protected override void RequestStartup(Nancy.TinyIoc.TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines, NancyContext context)
        {
            var authConfig = new BasicAuthenticationConfiguration(new PSWFWebUserValidator(), "PSWFWeb");
            BasicAuthentication.Enable(pipelines, authConfig);

            base.RequestStartup(container, pipelines, context);
        }
    }
}