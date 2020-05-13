namespace Tutorial.Web
{
    public class WelcomeServie : IWelcomeService
    {
        public string getMessage()
        {
            return "Hello from IWelcome service";
        }
    }
}