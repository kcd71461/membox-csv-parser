using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CsvHelper;

namespace MemboxCsvParser.Data
{
    public class Template8
    {
        public string RightTitle { get; set; }
        public string History { get; set; }
        public string Question { get; set; }
        public string QuestionPassage { get; set; }
        public string Answer { get; set; }
        public string Solution { get; set; }
        public string SolutionPassage { get; set; }
        public bool OxQuiz { get; set; }

        public static Card Parse(CsvReader csv, string imageUrl)
        {
            try
            {
                var card = new Card()
                {
                    TemplateId = 8,
                    Data = new Template8()
                    {
                        RightTitle = csv.GetField<string>("인덱스"),
                        History = csv.GetField<string>("출제"),
                        Question = escapeString(csv.GetField<string>("문제"), imageUrl),
                        QuestionPassage = escapeString(csv.GetField<string>("문제_지문"), imageUrl),
                        Answer = escapeString(csv.GetField<string>("정답"), imageUrl),
                        Solution = escapeString(csv.GetField<string>("해설"), imageUrl),
                        SolutionPassage = escapeString(csv.GetField<string>("해설_지문"), imageUrl),
                        OxQuiz = csv.GetField<string>("유형") == "0",
                    }
                };
                return card;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return null;
        }

        private static string escapeString(string input, string imageUrl = "")
        {
            input = input.Replace("\n", "<br/>");
            var regex = new Regex("ㅋ(.+?)ㅋ");
            var result = regex.Replace(input, $"<img src=\"{imageUrl.TrimEnd('/')}/$1\"/>");
            return result;
        }
    }
}