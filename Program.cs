/*
Name: Lotto Generator
Course: PROG1783-18W-Sec1-IT Support Programming Fundamentals
Professor: Scanlan, H.
Number Student ID: 7679566
Student: Daniel Brazil
Date: March, 27, 2018
*/

using System;

namespace LottoGenerator
{
    /*
    1. Generate multiple tickets
    2. Incorporate one or more lucky numbers into the number  
    a.Incorporate multi-digit numbers into the number
    3.Print out to a file
    4.Write code that will make another random ticket (the “draw”) and compare if the ticket they picked won. 
   */

    class Program
    {
        struct Ticket
        {
            public int number;
        }

        struct Purchase
        {
            public Ticket[] tkt;
        }

        public static string sDraw = string.Empty;

        static void Main(string[] args)
        {
            string sAnswer = "Y";
            while (sAnswer.Trim().ToUpper() == "Y")
            {
                Execute();
                Console.WriteLine("Would you like to execute again? (Press [Y]es or Any thing to Finish)");
                sAnswer = Console.ReadLine();
            }
            TryToMatch();

            Console.ReadLine();
        }

        private static void TryToMatch()
        {
            string sTicket = string.Empty;
            string sAnswer;
            int iCount = 0;
            DateTime timeStart;
            DateTime timeEnd;

            sAnswer = string.Empty;
            while (sAnswer.Trim().ToUpper() != "Y" && sAnswer.Trim().ToUpper() != "N")
            {
                Console.Clear();
                Console.WriteLine("Would you like to know how many attempts to match last Draw ?");
                Console.WriteLine("The last Draw is {0} - Press [Y]es or Press [N]o !", sDraw);
                sAnswer = Console.ReadLine();
               
            }

            if (sAnswer.Trim().ToUpper() == "Y")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("If the system spends more than 1 minute to return a result, the system will terminate.");
                Console.ResetColor();
                timeStart = DateTime.Now.AddSeconds(60);

                while (ValidateTicketWin(sDraw, sTicket) != 8)
                {
                    sTicket = CreateDraw();
                    iCount++;
                    timeEnd = DateTime.Now;
                    if (timeStart <= timeEnd)
                        break;
                }


                if (ValidateTicketWin(sDraw, sTicket) == 8)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("The system tried {0} to match!", iCount);
                    Console.WriteLine(sDraw);
                    Console.WriteLine(sTicket);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("The system tried {0} to match and do not it can!", iCount);
                }
            }
            else
                Console.WriteLine("Press ENTER to finish.");

        }

        private static void Execute()
        {
            int iQtyTickets;
            int iPositionStruct = 0;
            int iNumber;

            iQtyTickets = QuestionHowManyTickets();

            Purchase[] pc = new Purchase[iQtyTickets];

            for (int i = 0; i < iQtyTickets; i++)
            {
                Ticket[] tk = new Ticket[8];
                iNumber = 0;
                iPositionStruct = 0;
                while (iPositionStruct < 8)
                {
                    while (ValidateIfNumberExist(iNumber, tk))
                        iNumber = GenerateNumberOfTicket();
                    tk[iPositionStruct].number = iNumber;
                    iPositionStruct++;
                }
                pc[i].tkt = tk;
            }
            ShowResults(pc);

        }

        private static string CreateDraw()
        {
            string sResult = string.Empty;
            int iNumber;
            int iPositionStruct = 0;

            Ticket[] tk = new Ticket[8];
            iNumber = 0;
            while (iPositionStruct < 8)
            {
                while (ValidateIfNumberExist(iNumber, tk))
                    iNumber = GenerateNumberOfTicket();
                tk[iPositionStruct].number = iNumber;
                sResult += " - " + iNumber.ToString("00");
                iPositionStruct++;
            }
            sResult = sResult.Remove(0, 3);
            return sResult;
        }

        private static void ShowResults(Purchase[] pc)
        {
            int iCounter = 1;
            string sResult = string.Empty;
            
            Console.WriteLine("==============================================================================");
            sDraw = CreateDraw();
            Console.WriteLine("The “Draw”  is {0}", sDraw);
            Console.WriteLine("==============================================================================");
            iCounter = 1;
            foreach (Purchase pcResult in pc)
            {
                Console.WriteLine("Ticket {0}", iCounter);
                Console.WriteLine("========================================");
                sResult = string.Empty;
                foreach (Ticket tk in pcResult.tkt)
                {
                    sResult += " - " + tk.number.ToString("00");
                }
                sResult = sResult.Remove(0, 3);
                if (ValidateTicketWin(sDraw, sResult) == 8)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(sResult + "      ===>  YOU ARE WIN!!! CONGRATULATIONS");
                    Console.ResetColor();
                }
                else if ((ValidateTicketWin(sDraw, sResult) == 0))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(sResult + "      ===>  You lose!!!");
                    Console.ResetColor();
                }
                else if ((ValidateTicketWin(sDraw, sResult) > 0))
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(sResult + "      ===>  You have {0} hits!!!", ValidateTicketWin(sDraw, sResult));
                    Console.ResetColor();
                }
                    
                Console.WriteLine("========================================");
                iCounter++;
            }

        }

        private static int ValidateTicketWin(string sDraw, string sTicket)
        {
            int iCount = 0;
            sDraw = sDraw.Replace(" ", "").Replace("-", ";");
            sTicket = sTicket.Replace(" ", "").Replace("-", ";");
            foreach (string sNumber in sTicket.Split(';'))
            {
                if (sDraw.Contains(sNumber))
                    iCount++;
            }

            return iCount;
        }

        private static bool ValidateIfNumberExist(int iNumber, Ticket[] tk)
        {
            bool bResult = false;

            foreach (Ticket tkFind in tk)
            {
                if (tkFind.number == iNumber)
                {
                    bResult = true;
                }
            }

            return bResult;
        }

        private static int GenerateNumberOfTicket()
        {
            int iResult;
            Random rdm = new Random();

            iResult = rdm.Next(1, 49);

            return iResult;
        }

        private static int QuestionHowManyTickets()
        {
            string sAnswer = string.Empty;
            int iAnswer = 0;
            Console.SetCursorPosition(0, 2);
            while (!int.TryParse(sAnswer, out iAnswer))
            {
                WelcomeScreen();
                Console.WriteLine("How many tickets you need? [1 to 99]: [  ]");
                Console.SetCursorPosition(39, 2);
                sAnswer = Console.ReadLine();
                if (int.Parse(sAnswer) >= 100 || int.Parse(sAnswer) <= 0)
                    sAnswer = "?";
            }

            return int.Parse(sAnswer);
        }

        private static void WelcomeScreen()
        {
            Console.Clear();
            Console.WriteLine("Welcome to Lotto Generator");
            Console.WriteLine("==============================================================================");


        }
    }
}
