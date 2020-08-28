using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace BlazorMovies.Client.Auth
{
    public class TokenRenewer : IDisposable
    {
        Timer timer;
        private readonly ILoginService loginService;

        public TokenRenewer(ILoginService loginService)
        {
            this.loginService = loginService;
        }

        public void Initiate()
        {
            timer = new Timer();
            timer.Interval = 1000 * 60 * 4; // 4 minutes
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }
        public void Dispose()
        {
            timer?.Dispose();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            loginService.TryRenewToken();
        }
    }
}
