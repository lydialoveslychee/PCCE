namespace PCCE
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(ItemPage), typeof(ItemPage));
            Routing.RegisterRoute(nameof(LookupPage), typeof(LookupPage));
        }
    }
}
