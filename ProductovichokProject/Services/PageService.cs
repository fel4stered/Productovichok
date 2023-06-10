using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductovichokProject.Services
{
    public class PageService
    {
        public async Task GoToPageAsync(string page)
        {
            Task.Delay(200).Wait();
            await Shell.Current.GoToAsync(page);
        }
    }
}
