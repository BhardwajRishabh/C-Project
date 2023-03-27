using System;
using System.IO;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using System.Drawing;
using System.Threading.Tasks;
namespace test
{
    class Program
    {
        static Dictionary<string, byte[]> image_data = new Dictionary<string, byte[]>();

        static async Task Main(string[] args)
        {
            List<string> URLList = await urlload();
            downloadim(URLList);
            string search_word = enterword();
            await findimg(URLList, search_word);
            Console.WriteLine("IMAGE IS STORED IN YOUR DESIRED PATH");
            Console.ReadLine();

        }
        static string enterword()
        {
            Console.WriteLine("Enter a search word:");
            string search_word = Console.ReadLine();
            return search_word;

        }

        static async Task save_image(string url, byte[] image_bytes, string file_path)
        {
            using (MemoryStream streamm = new MemoryStream(image_bytes))
            {
                Image img = Image.FromStream(streamm);
                await Task.Run(() => img.Save(file_path + ".jpg"));
            }
            Console.WriteLine(image_bytes.GetHashCode());
        }

        static async Task downloadim(List<string> URLList)
        {
            using (WebClient client = new WebClient())
            {
                foreach (string url in URLList)
                {
                    byte[] image_bytes = await client.DownloadDataTaskAsync(url);
                    image_data[url] = image_bytes;
                }
            }
            Console.WriteLine("Images downloaded successfully.");
        }

        static async Task findimg(List<string> URLList, string search_word)
        {
            foreach (string imageurl in URLList)
            {
                if (imageurl.Contains(search_word))
                {
                    Console.WriteLine("Image is found and the URL : " + imageurl);
                    Console.WriteLine("Enter the path where you want to save the image:");
                    string save_path = Console.ReadLine();
                    await save_image(imageurl, image_data[imageurl], save_path);
                    return;
                }
            }
            Console.WriteLine("Image not found");
            return;
        }

        static async Task<List<string>> urlload()
        {
            List<string> URLList = new List<string>();
            string jsonString = await Task.Run(()=> File.ReadAllText("C:\\Users\\SHIRO\\Downloads\\IMAGE.json"));
            dynamic[] objects = JsonConvert.DeserializeObject<dynamic[]>(jsonString);
            foreach (dynamic obj in objects)
            {
                string link = obj.link;
                URLList.Add(link);
            }
            Console.WriteLine("Image URLs loaded successfully.");
            return URLList;
        }
    }

}