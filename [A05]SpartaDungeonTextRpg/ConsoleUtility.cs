using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace _A05_SpartaDungeonTextRpg
{
    internal class ConsoleUtility
    {
        // 플레이어의 선택 입력 시 입력값을 받는 함수입니다
        public static int PromptMenuChoice(int min, int max)
        {
            while (true)
            {
                Console.WriteLine("원하시는 번호를 입력해주세요.");
                Console.Write(">> ");
                if (int.TryParse(Console.ReadLine(), out int choice) && choice >= min && choice <= max)
                {
                    return choice;
                }
                Console.WriteLine("잘못된 입력입니다. 다시 시도해주세요.\n");
            }
        }

        // 타이틀 글씨 색상 변경(DarkYellow)
        internal static void ShowTitle(string title)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(title);
            Console.ResetColor();
        }

        // 바꾸고 싶은 색을 s0에 ConsoleColor로 입력하면 s2의 색깔이 지정한 색깔로 변경됨
      
        public static void PrintTextHighlights(ConsoleColor s0, string s1, string s2, string s3 = "")
        {
            Console.Write(s1);
            Console.ForegroundColor = s0;
            Console.Write(s2);
            Console.ResetColor();
            Console.WriteLine(s3);
        }

        public static int GetPrintableLength(string str)
        {
            int length = 0;
            foreach (char c in str)
            {
                if (char.GetUnicodeCategory(c) == System.Globalization.UnicodeCategory.OtherLetter)
                {
                    length += 2; // 한글과 같은 넓은 문자에 대해 길이를 2로 취급
                }
                else
                {
                    length += 1; // 나머지 문자에 대해 길이를 1로 취급
                }
            }

            return length;
        }

        public static string PadRightForMixedText(string str, int totalLength)
        {
            // 가나다
            // 111111
            int currentLength = GetPrintableLength(str);
            int padding = totalLength - currentLength;
            return str.PadRight(str.Length + padding);
        }



    }
}
