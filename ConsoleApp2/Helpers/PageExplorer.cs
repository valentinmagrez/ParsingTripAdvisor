using System;
using System.Threading.Tasks;
using PuppeteerSharp;

namespace ConsoleApp2.Helpers
{
    public class PageExplorer
    {
        private Browser _browser;

        public PageExplorer(Browser browser)
        {
            _browser = browser;
        }

        public async Task<Page> LoadPage(string url)
        {
            try
            {
                var page = await _browser.NewPageAsync();
                await page.GoToAsync(url);
                return page;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}
