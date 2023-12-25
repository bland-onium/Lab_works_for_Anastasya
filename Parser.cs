

using System.Diagnostics.Tracing;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Ast
{
    public class Parser
    {
        // Список проверенных ссылок
        static private readonly HashSet<Uri> urls = new HashSet<Uri>();
        // хз зочем
        static public readonly WebClient webClient = new WebClient();
        // Стартовая ссылка
        static private Uri domain;
        // Список данных, которые удалось достать
        static public List<string> datalist = new List<string>();
        
        static public void ParseSite(Uri firstPage, int pagecnt = 1)
        { 
            if (pagecnt < 0) { pagecnt *= -1; }
            urls.Clear();
            domain = firstPage;
            CheckSite(firstPage, pagecnt);
        }


        // Рекурсивная функция проверки сайта на существование
        static private void CheckSite(Uri page, int depth)
        {
            // page - Нынешняя страница
            // depth - оставшаяся глубина поиска
            if (urls.Contains(page)) { return;}

            if (depth >= 0) 
            {
                urls.Add(page);
                string link = "";
                try { link = webClient.DownloadString(page); }
                catch { Console.WriteLine($"Link '{page}' is broken"); }
                if (link == "")
                {
                    try
                    {
                        Uri page2 = new Uri(Convert.ToString(page) + "/", UriKind.Absolute);
                        link = webClient.DownloadString(page2);
                    }
                    catch { Console.WriteLine($"Link '{page}' is broken"); return; }
                }

                string title = SeekTitle(link);
                Parse(page, link, title);
                
                // Если глубина больше нуля – то запустится рекурсия для проверки ссылок в зависимости от глубины
                if (depth > 0)
                {
                    // Список существующих на сайте ссылок
                    var hrefs = (from href in Regex.Matches(link, @"href=""[\/\w-\.:]+""").Cast<Match>()
                                 let url = href.Value.Replace("href=", "").Trim('"')
                                 let loc = url.StartsWith("/")
                                 select new
                                 {
                                     Ref = loc ? $"{domain}{url}" : url,
                                     IsLocal = loc || url.StartsWith(Convert.ToString(domain))
                                 }
                                 ).ToList();
                    // Список локальных(дочерних) ссылок
                    var locals = (from href in hrefs where href.IsLocal select href.Ref).ToList();
                    for (int i = 0; i < locals.Count; i++)
                    {
                        var href = locals[i];
                        // Поиск рабочих ссылок с откидыванием лишнего(файлы (.png и т.п.))
                        string extenshion = Path.GetExtension(href).ToLower();
                        if (extenshion != "") { continue; }
                        // Создается некая ссылка, которая потом будет проверяться
                        Uri URL = new Uri(href);

                        if (urls.Contains(URL) == false)
                        {
                            depth--; // Убираем одно значение, так как опустились на 1 уровень
                            CheckSite(URL, depth);
                            depth++; 
                            // Рекурсия в шарпе работает через ж,
                            // поэтому при возвращении с дочерней ссылки она не возвращает значение depth
                        }
                    }
                }
                else { return; }
            }

        }

        // Отдельная функция нахождения титульника
        private static string SeekTitle(string text)
        {
            string title = "";
            int h2 = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '<')
                {
                    // Поиск Титульника по ключевому слову <h1>
                    if (text[i + 1] == 'h' && text[i + 2] == '1' && text[i + 3] == '>')
                    {
                        for (int j = i + 4; text[j] != '<'; j++)
                        {
                            title += text[j];
                        }
                        break;
                    }
                    // Поиск по ключевому слову <h2>
                    else if (text[i + 1] == 'h' && text[i + 2] == '2' && text[i + 3] == '>')
                    {
                        if (h2 >= 1) { title = ""; break; }
                        for (int j = i + 4; text[j] != '<'; j++)
                        {
                            title += text[j];
                        }
                        h2 += 1;
                        break;
                    }
                }
            }
            // ну если прям совсем всё плохо
            if (title == "") { return ("No title"); }
            return title;
        }

        // Функция нахождения номеров или адресов
        private static void Parse(Uri page, string link, string title)
        {
            // Парсинг номеров телефона
            {
                // Создание нескольких регулярных вырашений, по которым будем искать
                Regex regex1 = new Regex(@"(351)\d{3}-\d{2}-\d{2}");
                Regex regex2 = new Regex(@"\d{11}");
                Regex regex3 = new Regex(@"\d{3}-\d{3}-\d{4}");
                Regex regex4 = new Regex(@"\d{3}-\d{2}-\d{2}");

                MatchCollection matches1 = regex1.Matches(link);
                MatchCollection matches2 = regex2.Matches(link);
                MatchCollection matches3 = regex3.Matches(link);
                MatchCollection matches4 = regex4.Matches(link);

                // Внесение найденных данных в список данных datalist
                foreach (Match match in matches1) { datalist.Add(Convert.ToString(match)); }
                foreach (Match match in matches2)
                {
                    string newmt = AddSeven(Convert.ToString(match), link);
                    datalist.Add(Convert.ToString(newmt));
                }
                foreach (Match match in matches3) { datalist.Add(Convert.ToString(match)); }
                foreach (Match match in matches4) { datalist.Add("+7(351)" + Convert.ToString(match)); }
            }
            
            // Парсинг адресов
            {
               // Так как с адресами вариативность раз в 50 больше - то проще их парсить через обычный цикл
               for(int i = 1; i < link.Length-5; i++)
                {
                    if ( ( (link[i-1] == 'у' && link[i] == 'л' && link[i+1] == 'и' && link[i+2] == 'ц' && link[i+3] == 'а') || 
                           (link[i-1] == 'п' && link[i] == 'р' && link[i+1] == 'о' && link[i+2] == 'с' && link[i+3] == 'п') ) &&
                           (link[i+4] == ' ') ) // поиск по "улица" и "проспект"
                    {
                        string buff = "";
                        for(int j = i-1; link[j] != '<' && link[j] != '"' && link[j] != '>'; j++)
                        {
                            buff += link[j];
                        }
                        i += buff.Length;
                        datalist.Add(buff);
                    }
                    else if ( ((link[i + 4] == 'у' && link[i + 5] == 'л') || (link[i + 4] == 'п' && link[i + 5] == 'р') ) && 
                        link[i+6] == '.') // поиск по "ул." и "пр."
                    {
                        string buff = "";
                        for(int j = i + 4; link[j]!='<' && link[j] != '"' && link[j] != '>'; j++)
                        {
                            buff += link[j];
                        }
                        datalist.Add(buff);
                        i = i + 4 + buff.Length;
                    }
                    
                }
            }
            // После сбора данных со страницы записываем данные в файл
             FillCSV("C:\\Users\\NITRO\\Downloads\\Lab_4_Nastya\\data.csv", page, title);
        }

        // Функция для проверки номера из разряда +7351124180
        private static string AddSeven(string dat, string text)
        {
            for (int i = 1; i < text.Length; i++)
            {
                if (text[i - 1] == '+' && text[i] == '7')
                {
                    // Ищет схождения индексов между text[i->i+9] и dat[] = 9, где dat - проверяемый номер
                    if (text[i] == dat[0] && text[i + 1] == dat[1] && text[i + 2] == dat[2] && text[i + 3] == dat[3] &&
                        text[i+4] == dat[4] && text[i + 5] == dat[5] && text[i + 6] == dat[6] && text[i + 7] == dat[7] &&
                        text[i+8] == dat[8] && text[i+9] == dat[9])
                    {
                        return "+" + dat;
                    }
                }
            }
            return null;
        }

        // Функция записи в CSV файл
        internal static void FillCSV(string pathfile, Uri link, string title)
        {
            // Проверка существования
            if (File.Exists(pathfile) == false) { File.Create(pathfile); }
            var csv = new StringBuilder();
            int backstep = 0;
            // Создание строчного массива данных, который потом будет внесён в файл
            for (int i = 0; i < datalist.Count(); i++)
            {   
                if (datalist[i] == null) { backstep++; continue; }
                var line = string.Format("{0}, {1}, {2}", title, Convert.ToString(link), datalist[i-backstep]);
                Console.WriteLine($"{link}, {datalist[i]}");
                csv.AppendLine(line);
            }
            // С кодировкой все хорошо, поэтому можно открывать даже в excel
            File.WriteAllText(pathfile, csv.ToString(), Encoding.UTF8);
        }
    }
       
}